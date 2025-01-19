using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Application.Interfaces;
using AuthenticationApi.Application.Responses;
using AuthenticationApi.Domain.Entities;
using AuthenticationApi.Infrastructure.Data;
using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationApi.Infrastructure.Repositories
{
    internal class AuthenticationRepository(AuthenticationDbContext context, IConfiguration config) : IUser
    {
        public async Task<UserEntity> GetUserByEmail(string email)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Email == email);
            return user is null ? null! : user!;
        }

        public async Task<ApiResponse> EditUserById(UserDTO appUserDTO)
        {
            var user = await context.Users.FindAsync(appUserDTO.Id);
            if (user is null)
                return new ApiResponse(false, "User not found");

            user.Name = appUserDTO.Name;
            user.TelephoneNumber = appUserDTO.TelephoneNumber;
            user.Adress = appUserDTO.Address;
            user.Email = appUserDTO.Email;
            user.Role = appUserDTO.Role;
            if (!string.IsNullOrEmpty(appUserDTO.Password))
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(appUserDTO.Password);
            }

            context.Users.Update(user);
            await context.SaveChangesAsync();

            return new ApiResponse(true, "User updated successfully");
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsers()
        {
            var users = await context.Users.ToListAsync();
            return users.Select(user => new UserDTO(
                    user.Id, user.Name!, user.TelephoneNumber!, user.Adress!, user.Email!, user.Password!, user.Role!
                ));
        }

        public async Task<UserDTO> GetUser(int userId)
        {
            var user = await context.Users.FindAsync(userId);

            if (user == null) return null!;

            return new UserDTO(user.Id, user.Name!, user.TelephoneNumber!, user.Adress!, user.Email!, user.Password!, user.Role!);
;

        }

        private string GenerateToken(UserEntity user)
        {
            var key = Encoding.UTF8.GetBytes(config.GetSection("Authentication:Key").Value!);
            var securityKey = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Name!),
                new(ClaimTypes.Email, user.Email!),
                new("UserId", user.Id.ToString()) // Agrega el userId como claim
            };

            if (!string.IsNullOrEmpty(user.Role) && !Equals("string", user.Role))
                claims.Add(new(ClaimTypes.Role, user.Role!));

            var token = new JwtSecurityToken(
                issuer: config["Authentication:Issuer"],
                audience: config["Authentication:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1), 
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<ApiResponse> Login(LoginDTO loginDTO)
        {
            var getUser = await GetUserByEmail(loginDTO.Email);
            if (getUser is null)
                return new ApiResponse(false, "invalid credentials");

            bool verifyPassword = BCrypt.Net.BCrypt.Verify(loginDTO.Password, getUser.Password);
            if (!verifyPassword)
                return new ApiResponse(false, "invalid credentials");

            string token = GenerateToken(getUser);
            return new ApiResponse(true, token);
        }

        public async Task<ApiResponse> Register(UserDTO appUserDTO)
        {
            var getUser = await GetUserByEmail(appUserDTO.Email);

            if (getUser is not null)
                return new ApiResponse(false, $"invalid email for registration");

            var result = context.Users.Add(new UserEntity()
            {
                Name = appUserDTO.Name,
                Email = appUserDTO.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(appUserDTO.Password),
                TelephoneNumber = appUserDTO.TelephoneNumber,
                Adress = appUserDTO.Address,
                Role = appUserDTO.Role
            });

            await context.SaveChangesAsync();
            return result.Entity.Id > 0 ? new ApiResponse(true, "User registered") :
                new ApiResponse(false, "Invalid data");
        }
    }
}
