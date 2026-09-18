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
        var result = await _testService.SaveData(testModel);
        if (result.isSuccess)
        {
            return Ok(new { message = "保存に成功しました", time = DateTime.Now });
        }
        else
        {
            return BadRequest(new {message = result.ErrorMessage, time = DateTime.Now});
        }
    }
}