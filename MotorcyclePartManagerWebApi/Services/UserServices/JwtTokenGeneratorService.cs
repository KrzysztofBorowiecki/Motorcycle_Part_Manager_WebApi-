using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MotorcyclePartManagerWebApi.CustomExceptions;
using MotorcyclePartManagerWebApi.Data;
using MotorcyclePartManagerWebApi.JwtAuthentication;
using MotorcyclePartManagerWebApi.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace MotorcyclePartManagerWebApi.Services.UserServices
{
    public class JwtTokenGeneratorService : IJwtTokenGeneratorService
    {
        private readonly ProjectContext _context;
        private readonly AuthenticationSettings _authenticationSettings;
        public JwtTokenGeneratorService(ProjectContext context, AuthenticationSettings authenticationSettings)
        {
            _context = context;
            _authenticationSettings = authenticationSettings;
        }

        public string Generate(Login model)
        {
            var user = _context.Users
              .Include(u => u.Role)
              .FirstOrDefault(u => u.Email == model.Email);

            if (user is null)
            {
                throw new BadRequestException("Nie poprawna nazwa użytkownka lub hasło");
            }

            var verifyResult = BCrypt.Net.BCrypt.Verify(model.Password, user.Password);
            if (!verifyResult)
            {
                throw new BadRequestException("Nie poprawna nazwa użytkownka lub hasło");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.UserName}"),
                new Claim(ClaimTypes.Role, $"{user.Role.Name}"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(_authenticationSettings.JwtExpireMinutes);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}
