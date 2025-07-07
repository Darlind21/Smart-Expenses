using MVC_Project._DataLayer.DTOs;
using MVC_Project._DataLayer.Models;

namespace MVC_Project._BusinessLayer.Interfaces
{
    public interface IExpenseService
    {
        Task<Expense> CreateExpenseAsync(ExpenseDTO expense);
        Task<Expense> GetExpenseByIdAsync(int id);
        Task<List<Expense>> GetAllExpensesAsync(int? categoryId = null, decimal? minValue = null, decimal? maxValue = null, string? description = null);
        Task<bool> ModifyExpenseAsync(ExpenseDTO modifiedExpense, int expenseId);
        Task<bool> DeleteExpenseAsync(int id);
        Task<decimal> GetTotalExpensesAsync();
    }
}
