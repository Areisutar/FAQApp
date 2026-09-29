using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.Models;
using src.Services;

namespace src.Controllers;

[ApiController]
[Route("api/auth")]
[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
public class AuthController(
    IAuthService authService,
    IAntiforgery antiforgery) : ControllerBase
{
    [HttpGet("csrf")]
    public IActionResult Csrf() => Ok(new CsrfTokenResponse(antiforgery.GetAndStoreTokens(HttpContext).RequestToken));

    [HttpPost("login")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var result = await authService.LoginAsync(request);
        return result.User is null
            ? Unauthorized(new AuthErrorResponse(result.ErrorMessage!))
            : Ok(result.User);
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var user = await authService.GetCurrentUserAsync(User);
        return user is null ? Unauthorized() : Ok(user);
    }

    [Authorize]
    [HttpPost("logout")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await authService.LogoutAsync();
        return NoContent();
    }
}
