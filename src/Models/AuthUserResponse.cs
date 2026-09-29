namespace src.Models;

public sealed class AuthUserResponse
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public string? Email { get; init; }
    public string? UserName { get; init; }
    public required IList<string> Roles { get; init; }
}
