using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ANONYMOUS_SURVEY.Data;
using ANONYMOUS_SURVEY.DTOs;
using ANONYMOUS_SURVEY.Models;
using ANONYMOUS_SURVEY.Repositories.Interfaces;
using ANONYMOUS_SURVEY.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Anonymous_Survey.Services
{
    public class AuthService(IAdminRepository adminRepository, ApplicationDbContext context, IConfiguration configuration) : IAuthService
    {
        public async Task<TokenResponseDto> LoginRequestAsync(AdminForLoginDto adminForLogin)
        {
            var admin = await context.Admins.FirstOrDefaultAsync(a => a.Email == adminForLogin.Email);
            if (admin is null)
            {
                return null;
            }
            if (new PasswordHasher<Admin>().VerifyHashedPassword(admin, admin.PasswordHash, adminForLogin.Password)
            == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return await CreateTokenResponse(admin);
        }


        public async Task<Admin?> RegisterAsync(AdminForRegistrationDto adminForRegistration)
        {
            var adminExists = await adminRepository.GetByEmailAsync(adminForRegistration.Email);
            if (adminExists != null)
            {
                throw new Exception("Admin with the same email already exits");
            }
            if (await context.Admins.AnyAsync(u => u.AdminName == adminForRegistration.AdminName))
            {
                throw new Exception("Admin with the same Admin name already exits");
            }
            if (!await context.Subjects.AnyAsync(s => s.SubjectId == adminForRegistration.SubjectId))
            {
                throw new Exception("No subject found with the subject id " + adminForRegistration.SubjectId);
            }
            var admin = new Admin();
            var passwordHash = new PasswordHasher<Admin>().
            HashPassword(admin, adminForRegistration.Password);

            admin.AdminName = adminForRegistration.AdminName;
            admin.Email = adminForRegistration.Email;
            admin.PasswordHash = passwordHash;
            admin.SubjectId = adminForRegistration.SubjectId;
            admin.CreatedAt = DateTime.UtcNow;

            context.Admins.Add(admin);
            await context.SaveChangesAsync();
            return admin;
        }

        public async Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request)
        {
            var admin = await ValidateRefreshTokenAsync(request.AdminId, request.RefreshToken);
            if (admin is null)
            {
                return null;
            }
            return await CreateTokenResponse(admin);

        }
        private async Task<TokenResponseDto> CreateTokenResponse(Admin admin)
        {
            return new TokenResponseDto
            {
                AccessToken = CreateToken(admin),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(admin)
            };
        }
        private async Task<Admin?> ValidateRefreshTokenAsync(int adminId, string refreshToken)
        {
            var admin = await context.Admins.FindAsync(adminId);
            if (admin is null || admin.RefreshToken != refreshToken || admin.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return null;
            }
            return admin;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<string> GenerateAndSaveRefreshTokenAsync(Admin admin)
        {
            var refreshToken = GenerateRefreshToken();
            admin.RefreshToken = refreshToken;
            admin.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(2);
            await context.SaveChangesAsync();
            return refreshToken;
        }

        private string CreateToken(Admin admin)
        {
            var claims = new List<Claim>{
                new Claim(ClaimTypes.Name, admin.AdminName),
                new Claim(ClaimTypes.NameIdentifier, admin.AdminId.ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("AppSettings:Issuer"),
                audience: configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

    }
}