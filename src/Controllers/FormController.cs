using src.Models;
using Microsoft.AspNetCore.Mvc;
using src.Services;

namespace src.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FormController : ControllerBase
{
    private readonly IFormService _formService;
    public FormController(IFormService formService)
    {
        _formService = formService;
    }
    [HttpPost]
    public async Task<ActionResult> Submit([FromBody] FormModel formModel)
    {
        var result = await _formService.SaveData(formModel);
        if (result.isSuccess)
        {
            return Ok(new { message = "保存に成功しました", time = DateTime.Now });
        }
        else
        {
            return BadRequest(new { message = result.ErrorMessage, time = DateTime.Now });
        }
    }
}
