using src.Data;
using src.Models;
using Microsoft.EntityFrameworkCore;

namespace src.Services
{
    public interface IFormService
    {
        Task<(bool isSuccess, string ErrorMessage)> SaveData(FormModel formModel);
    }
    public class FormService : IFormService
    {
        private readonly ApplicationDbContext _context;

        public FormService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(bool isSuccess, string ErrorMessage)> SaveData(FormModel formModel)
        {
            if (String.IsNullOrWhiteSpace(formModel.Gender))
            {
                return (false, "ErrorMessage: 性別を選択してください。");
            }
            if (String.IsNullOrWhiteSpace(formModel.FavoriteSns))
            {
                return (false, "ErrorMessage: 好きなSNSを選択してください。");
            }
            try
            {
                formModel.Id = 0;
                _context.FormModel.Add(formModel);
                await _context.SaveChangesAsync();
                return (true, string.Empty);
            }
            catch (DbUpdateException)
            {
                return (isSuccess: false, ErrorMessage: "データの保存に失敗しました。");
            }
        }
    }
}
