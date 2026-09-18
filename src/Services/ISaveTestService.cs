using src.Data;    // ApplicationDbContextがある場所
using src.Models;  // TestModelがある場所
using Microsoft.EntityFrameworkCore;

namespace src.Services
{
    public interface ITestService
    {
        Task<(bool isSuccess, string ErrorMessage)> SaveData(TestModel testModel);
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
        public async Task<(bool isSuccess, string ErrorMessage)> SaveData(TestModel testmodel)
        {
            Console.WriteLine(testmodel.Text);
            if(String.IsNullOrWhiteSpace(testmodel.Text))
            {
                return (false,"ErrorMessage:送信されたテキストが無効なので、文字列を正しく送信してください。");
            }
            try
            {
                testmodel.Id = 0;
                _context.TestModel.Add(testmodel);
                await _context.SaveChangesAsync();
                return(true, string.Empty);
            }
            catch(DbUpdateException)
            {
                return (isSuccess:false, ErrorMessage: "データの保存に失敗しました。");
            }
        }
    }
}