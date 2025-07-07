using Microsoft.EntityFrameworkCore;
using MVC_Project._BusinessLayer.Interfaces;
using MVC_Project._DataLayer.DTOs;
using MVC_Project._DataLayer.Models;

namespace MVC_Project._BusinessLayer.Services
{
    public class ExpenseService(AppDbContext context) : IExpenseService
    {
        public async Task<Expense> CreateExpenseAsync(ExpenseDTO expense)
        {
            if (expense.Id != 0) throw new ArgumentException("You cannot create id when creating expense");

            if (expense.Value <= 0) throw new ArgumentException("Value cannot be negative or 0");
            if (expense.Name == null) throw new ArgumentException("Name cannot be null");
            if (expense.ExpenseDate == null) throw new ArgumentException("Expense date cannot be null");

            var tenYearsAgo = DateOnly.FromDateTime(DateTime.Now).AddYears(-10);
            var tenYearsAfter = DateOnly.FromDateTime(DateTime.Now).AddYears(10);

            if (expense.ExpenseDate < tenYearsAgo) throw new ArgumentException("Expenses cannot be more than 10 years ago");
            if (expense.ExpenseDate > tenYearsAfter) throw new ArgumentException("Expenses cannot be added more than 10 years in the future");

            var createdExpense = new Expense
            {
                Name = expense.Name,
                Description = expense.Description,
                Value = expense.Value!.Value,
                ExpenseDate = expense.ExpenseDate.Value,
                CategoryId = expense.CategoryId
            };


            await context.Expenses.AddAsync(createdExpense);
            await context.SaveChangesAsync();

            return createdExpense;
        }

        public async Task<bool> DeleteExpenseAsync(int id)
        {
            var expense = await context.Expenses.FindAsync(id);

            if (expense == null) throw new ArgumentException("Expense not found");

            context.Remove(expense);
            return await context.SaveChangesAsync() > 0; //Save changes returns an int(number of rows affected)
                                                         //if any changes have been made it will return true;
        }

        public async Task<List<Expense>> GetAllExpensesAsync(int? categoryId = null, decimal? minValue = null, decimal? maxValue = null, string? description = null)
        {
            var query = context.Expenses.AsQueryable();

            if ( categoryId != null)
            {
                query = query
                    .Where(x => x.CategoryId == categoryId)
                    .Include(e => e.Category);
            }
            else if (categoryId == null)
            {
                query = query
                    .Include(e => e.Category);
            }

            if (minValue.HasValue)
            {
                query = query.Where(x => x.Value >= minValue);
            }

            if (maxValue.HasValue)
            {
                query = query.Where(x => x.Value <= maxValue);
            }

            if (!string.IsNullOrEmpty(description))
            {
                query = query.Where(x => x.Description != null && x.Description.Contains(description));
            }

            return await query.ToListAsync();
        }

        public async Task<Expense> GetExpenseByIdAsync(int id)
        {
            var result = await context.Expenses
                .Include(e => e.Category)
                .SingleOrDefaultAsync(e => e.Id == id);

            return result == null ? throw new ArgumentException("Cannot find expense") : result;
        }

        public async Task<decimal> GetTotalExpensesAsync()
        {
            var total = await context.Expenses
                .SumAsync(x => x.Value);

            return total;
        }

        public async Task<bool> ModifyExpenseAsync(ExpenseDTO modifiedExpense, int expenseId)
        {
            var expense = await context.Expenses.FindAsync(expenseId);

            if (expense == null) throw new ArgumentException("Could not find expense");

            expense = MapExpenseDTOtoExpense(modifiedExpense, expense);
            expense.ModifiedOn = DateTime.UtcNow;

            return await context.SaveChangesAsync() > 0;
        }

        private Expense MapExpenseDTOtoExpense(ExpenseDTO expenseDTO, Expense expense)
        {
            if (expenseDTO.Name != null) expense.Name = expenseDTO.Name;

            if (expenseDTO.Value <= 0) throw new ArgumentException("Value cannot be negative");
            if (expenseDTO.Value != null) expense.Value = expenseDTO.Value.Value;
            if (expenseDTO.Description != null) expense.Description = expenseDTO.Description;


            var tenYearsAgo = DateOnly.FromDateTime(DateTime.Now).AddYears(-10);
            var tenYearsAfter = DateOnly.FromDateTime(DateTime.Now).AddYears(10);

            if (expense.ExpenseDate < tenYearsAgo) throw new ArgumentException("Expenses cannot be more than 10 years ago");
            if (expense.ExpenseDate > tenYearsAfter) throw new ArgumentException("Expenses cannot be added more than 10 years in the future");


            if (expenseDTO.ExpenseDate != null) expense.ExpenseDate = expenseDTO.ExpenseDate.Value;
            expense.CategoryId = expenseDTO.CategoryId;

            return expense;
        }
    }
}
