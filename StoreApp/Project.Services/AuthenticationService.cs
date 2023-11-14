using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Project.Entity.DTOs.Identity;
using Project.Entity.Exceptions;
using Project.Entity.Models;
using Project.Services.Abstract;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        private User? User;
        public AuthenticationService(ILoggerService logger, IMapper mapper, UserManager<User> userManager, IConfiguration configuration)
        {
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<TokenDto> CreateToken(bool populateExp)
        {
            var signInCredential = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signInCredential, claims);

            var refreshToken = GenerateRefreshToken();
            User.RefreshToken = refreshToken;
            if(populateExp)
                User.RefreshTokenExpireTime = DateTime.Now.AddDays(7);
            await _userManager.UpdateAsync(User);
            var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions); 

            return new TokenDto { AccessToken = accessToken, RefreshToken = refreshToken };
        }
        // access token ve refresh token yenilenmesini sağlıyor.
        public async Task<TokenDto> RefreshToken(TokenDto Model)
        {
            var principles = GetPrincipalFromExpiredToken(Model.AccessToken);
            var user = await _userManager.FindByNameAsync(principles.Identity.Name);
            if (user is null || user.RefreshToken != Model.RefreshToken || user.RefreshTokenExpireTime <= DateTime.Now)
                throw new InvalidObjectException<User>("Reason : User is null, Refresh token doesn't equal or refresh token time is expired");
            User = user;
            return await CreateToken(populateExp: false);
        }
        public async Task<IdentityResult> RegisterUser(UserForRegistrationDto Model)
        {
            var user = _mapper.Map<User>(Model);
            var result = await _userManager.CreateAsync(user, Model.Password);
            if(result.Succeeded)
                await _userManager.AddToRolesAsync(user,Model.Roles);
            return result;
        }

        public async Task<bool> ValidateUser(UserForAuthenticationDto Model)
        {
            User = await _userManager.FindByNameAsync(Model.Username);
            var result = (User != null && await _userManager.CheckPasswordAsync(User, Model.Password));
            if (!result)
                _logger.Log($"Auth failed for user : {Model.Username}", Entity.Enums.LogType.Error);
            return result;
        }
        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signInCredential, List<Claim> claims)
        {
            var jwtSeciton = _configuration.GetSection("JwtSettings");

            var tokenOptions = new JwtSecurityToken(
                issuer: jwtSeciton["validIssuer"],
                audience: jwtSeciton["validAudience"],
                expires: DateTime.Now.AddMinutes(double.Parse(jwtSeciton["expires"])),
                signingCredentials: signInCredential,
                claims : claims);

            return tokenOptions;
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, User.UserName) // kullanıcı bilgileri ve role bilgileri claim olarak eklenir.
            };
            var roles = await _userManager.GetRolesAsync(User);

            foreach (var item in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, item));
            }
            return claims; 
        }
        private SigningCredentials GetSigningCredentials()
        {
            var jwtSeciton = _configuration.GetSection("JwtSettings"); 
            var secretKey = Encoding.UTF8.GetBytes(jwtSeciton["secretKey"]);
            var secret = new SymmetricSecurityKey(secretKey);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        // süresi dolmuş olan tokendan kullanıcı bilgilerini alacağımız fonksiyon
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var jwtSeciton = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSeciton["secretKey"];
            var validIssuer = jwtSeciton["validIssuer"];
            var validAudience = jwtSeciton["validAudience"];

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = validIssuer,
                ValidAudience = validAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
            };

            var tokenHandler = new JwtSecurityTokenHandler(); 
            var principle = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
            var tokenConverted = validatedToken as JwtSecurityToken;
            if (tokenConverted is null || 
                !tokenConverted.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new InvalidObjectException<SecurityToken>("Invalid validate token"); 
            }
            return principle;
        }

    }
}
