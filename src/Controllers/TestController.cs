// using Microsoft.AspNetCore.Mvc;
using src.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore.Storage.Json;
using src.Models;
// using src.Models.ViewModels;
using src.Services;

namespace src.Controllers;

[ApiController]
[Route("api/[controller]")] // これで /api/test になる
public class TestController : ControllerBase
{
    private readonly ITestService _testService; // インターフェースを使う
    public TestController(ITestService testService)
    {
        _testService = testService;
    }
    [HttpPost]
    public async Task<ActionResult> TestMethod([FromBody] TestModel testModel)
    {
        await _testService.SaveData(testModel);
        return Ok(new { message = "C#からの返信だよ！", time = DateTime.Now });
    }
}