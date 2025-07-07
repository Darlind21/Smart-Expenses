using MVC_Project._DataLayer.DTOs;
using MVC_Project._DataLayer.Models;

namespace MVC_Project._BusinessLayer.Interfaces
{
    public interface IIncomeService
    {
        Task<Income> CreateIncomeAsync(IncomeDTO incomeDTO);
        Task<Income> GetIncomeByIdAsync(int id);
        Task<List<Income>> GetAllIncomesAsync(int? categoryId = null, decimal? minValue = null, decimal? maxValue = null, string? description = null);
        Task<bool> ModifyIncomeAsync(IncomeDTO modifiedIncome, int expenseId);
        Task<bool> DeleteIncomeAsync(int id);
        Task<decimal> GetTotalIncomesAsync();
    }
}
