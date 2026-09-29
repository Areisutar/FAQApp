namespace src.Models;

public sealed record AuthLoginResult(AuthUserResponse? User, string? ErrorMessage);
