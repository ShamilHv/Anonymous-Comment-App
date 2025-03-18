using ANONYMOUS_SURVEY.Data;
using ANONYMOUS_SURVEY.DTOs;
using ANONYMOUS_SURVEY.Models;
using ANONYMOUS_SURVEY.Repositories.Interfaces;
using ANONYMOUS_SURVEY.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ANONYMOUS_SURVEY.Controllers

{
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static Admin admin = new Admin();
        private readonly ApplicationDbContext _dbContext;
        private readonly IAdminRepository _adminRepository;
        private readonly IAuthService _authService;


        public AuthController(IAuthService authService, ApplicationDbContext dbContext, IAdminRepository adminRepository)
        {
            _dbContext = dbContext;
            _adminRepository = adminRepository;
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<Admin>> Register(AdminForRegistrationDto adminForRegistration)
        {
            var admin = await _authService.RegisterAsync(adminForRegistration);
            if (admin is null)
            {
                return BadRequest("Bad Credentials");
            }
            return Ok(admin);
        }
        [HttpPost("Login")]
        public async Task<ActionResult<TokenResponseDto>> Login(AdminForLoginDto adminForLogin)
        {
            var token = await _authService.LoginRequestAsync(adminForLogin);
            if (token is null)
            {
                return BadRequest("Bad Credentials");
            }
            return Ok(token);

        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto request)
        {
            var response = await _authService.RefreshTokensAsync(request);
            if (response is null || response.RefreshToken is null || response.AccessToken is null)
            {
                return Unauthorized("Invalid Credentials");
            }
            return Ok(response);
        }

        [HttpGet("authonly")]
        [Authorize]
        public IActionResult AuthenticatedOnly()
        {
            return Ok("You are authenticated");
        }

    }
}