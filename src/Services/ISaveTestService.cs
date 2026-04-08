using src.Data;    // ApplicationDbContextがある場所
using src.Models;  // TestModelがある場所
using Microsoft.EntityFrameworkCore;

namespace src.Services
{
    public interface ITestService
    {
        Task SaveData(TestModel testModel);
    }
    public class TestService:ITestService
    {
        private readonly ApplicationDbContext _context;

        // 【コンストラクタ】ここでDBを使えるように注入してもらう
        public TestService(ApplicationDbContext context)
        {
            _context = context;
        }

        // あなたが書いた保存処理
        public async Task SaveData(TestModel testmodel)
        {
            testmodel.Id = 0;
            _context.TestModel.Add(testmodel);
            await _context.SaveChangesAsync(); 
        }
    }
}