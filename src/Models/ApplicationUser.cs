using Microsoft.AspNetCore.Identity;

namespace src.Models;

public class ApplicationUser : IdentityUser
{
    // 画面に表示するユーザー名。
    public string Name { get; set; } = string.Empty;
}
