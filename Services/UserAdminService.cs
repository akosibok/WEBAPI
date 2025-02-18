﻿using AutoMapper;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using WEBAPI.Authorization;
using WEBAPI.Entities;
using WEBAPI.Helpers;
using WEBAPI.Models.Users;

namespace WEBAPI.Services
{
    public interface IUserAdminService
    {
        AdminAuthenticateResponse Authenticate(AdminAuthenticateRequest model);
        IEnumerable<UserAdmin> GetAll();

        UserAdmin GetByBearerToken();
        UserAdmin GetById(int id);
        void Register(AdminRegisterRequest model);
        void Update(int id, UpdateRequest model);
        void Delete(int id);
        void Logout(int id);
    }

    public class UserAdminService : IUserAdminService
    {
        private DataContext _context;
        private IJwtUtils _jwtUtils;
        private readonly IMapper _mapper;
        private IConfiguration _config;

        public UserAdminService(
            DataContext context,
            IJwtUtils jwtUtils,
            IMapper mapper,
            IConfiguration config)
        {
            _context = context;
            _jwtUtils = jwtUtils;
            _mapper = mapper;
            _config = config;
        }

        public AdminAuthenticateResponse Authenticate(AdminAuthenticateRequest model)
        {
            var useradmin = _context.UserAdmins.SingleOrDefault(x => x.UserName == model.Username);

            // validate
            if (useradmin == null || !BCrypt.Net.BCrypt.Verify(model.Password, useradmin.PasswordHash))
                throw new AppException("Username or password is incorrect");

            // authentication successful
            //var response = _mapper.Map<AdminAuthenticateResponse>(useradmin);

            //useradmin.TokenID = _jwtUtils.GenerateTokenAdmin(useradmin);

            //CB-09302023 Update TokenID in UserTable
            //_mapper.Map(model, user);
            //useradmin.TokenID = response.Token;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, useradmin.UserName),                    
                    new Claim(ClaimTypes.Role, useradmin.Role)
                }),
                IssuedAt = DateTime.UtcNow,
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"],
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(_config["AppSettings:MinuteExpire"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            useradmin.TokenID = tokenHandler.WriteToken(token);

            _context.UserAdmins.Update(useradmin);
            _context.SaveChanges();

            AdminAuthenticateResponse response = new AdminAuthenticateResponse { 
                   Id = useradmin.Id,
                   Username = model.Username,
                   Token = useradmin.TokenID,
                   Role = useradmin.Role
                };

            return response;
        }

        public IEnumerable<UserAdmin> GetAll()
        {
            return _context.UserAdmins;
        }

        public UserAdmin GetByBearerToken()
        {
            int id = 0;
            var user = _context.UserAdmins.Find(id);
            if (user == null) throw new KeyNotFoundException("User not found");
            return user;
        }

        public UserAdmin GetById(int id)
        {
            return getUser(id);
        }

        public void Register(AdminRegisterRequest model)
        {
            // validate
            if (_context.UserAdmins.Any(x => x.UserName == model.UserName))
                throw new AppException("Username '" + model.UserName + "' is already taken");

            // map model to new user object
            //var useradmin = _mapper.Map<UserAdmin>(model);

            // hash password
            //useradmin.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

            UserAdmin useradmin = new UserAdmin
            {
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                UserName = model.UserName,
                Role = model.Role                
            };

            
            
            // save user
            _context.UserAdmins.Add(useradmin);

            _context.SaveChanges();
        }

        public void Logout(int id)
        {
            //CB-09302023 Get User via id
            var user = getUser(id);
            user.TokenID = null;
            _context.UserAdmins.Update(user);
            _context.SaveChanges();
        }

        public void Update(int id, UpdateRequest model)
        {
            var user = getUser(id);

            // validate
            if (model.Username != user.UserName && _context.UserAdmins.Any(x => x.UserName == model.Username))
                throw new AppException("Username '" + model.Username + "' is already taken");

            // hash password if it was entered
            if (!string.IsNullOrEmpty(model.Password))
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

            // copy model to user and save
            _mapper.Map(model, user);
            _context.UserAdmins.Update(user);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = getUser(id);
            _context.UserAdmins.Remove(user);
            _context.SaveChanges();
        }

        // helper methods

        private UserAdmin getUser(int id)
        {
            var user = _context.UserAdmins.Find(id);
            if (user == null) throw new KeyNotFoundException("User not found");
            return user;
        }

    }
}
