using ANONYMOUS_SURVEY.DTOs;
using ANONYMOUS_SURVEY.Models;

namespace ANONYMOUS_SURVEY.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Admin?> RegisterAsync(AdminForRegistrationDto adminForRegistration);
        Task<TokenResponseDto?> LoginRequestAsync(AdminForLoginDto adminForLogin);
        Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request);
    } 
}    
