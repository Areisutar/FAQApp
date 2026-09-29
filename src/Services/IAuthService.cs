using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using src.Models;

namespace src.Services;

public interface IAuthService
{
    Task<AuthLoginResult> LoginAsync(LoginRequest request);
    Task<AuthUserResponse?> GetCurrentUserAsync(ClaimsPrincipal principal);
    Task LogoutAsync();
}

public sealed class AuthService(
    UserManager<ApplicationUser> users,
    SignInManager<ApplicationUser> signIn) : IAuthService
{
    private const string InvalidCredentials = "メールアドレスまたはパスワードが正しくありません。";

    public async Task<AuthLoginResult> LoginAsync(LoginRequest request)
    {
        var user = await users.FindByEmailAsync(request.Email.Trim());
        if (user is null)
            return new AuthLoginResult(null, InvalidCredentials);

        var result = await signIn.PasswordSignInAsync(user, request.Password, request.RememberMe, lockoutOnFailure: true);
        if (!result.Succeeded)
            return new AuthLoginResult(null, result.IsLockedOut
                ? "ログイン試行回数の上限に達しました。5分後にもう一度お試しください。"
                : InvalidCredentials);

        return new AuthLoginResult(await CreateUserResponseAsync(user), null);
    }

    public async Task<AuthUserResponse?> GetCurrentUserAsync(ClaimsPrincipal principal)
    {
        var user = await users.GetUserAsync(principal);
        return user is null ? null : await CreateUserResponseAsync(user);
    }

    public Task LogoutAsync() => signIn.SignOutAsync();

    private async Task<AuthUserResponse> CreateUserResponseAsync(ApplicationUser user) => new()
    {
        Id = user.Id,
        Name = user.Name,
        Email = user.Email,
        UserName = user.UserName,
        Roles = await users.GetRolesAsync(user),
    };
}
