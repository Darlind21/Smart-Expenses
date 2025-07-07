using Microsoft.EntityFrameworkCore;
using MVC_Project._BusinessLayer.Interfaces;
using MVC_Project._DataLayer.DTOs;
using MVC_Project._DataLayer.Models;

namespace MVC_Project._BusinessLayer.Services
{
    public class IncomeService(AppDbContext context) : IIncomeService
    {
        public async Task<Income> CreateIncomeAsync(IncomeDTO income)
        {
            if (income.Id != 0) throw new ArgumentException("You cannot create id when creating income");
            if (income.Value <= 0) throw new ArgumentException("Value cannot be negative or 0");
            if (income.Name == null) throw new ArgumentException("Name cannot be null");
            if (income.IncomeDate == null) throw new ArgumentException("Income date cannot be null");

            var tenYearsAgo = DateOnly.FromDateTime(DateTime.Now).AddYears(-10);
            var tenYearsAfter = DateOnly.FromDateTime(DateTime.Now).AddYears(10);

            if (income.IncomeDate < tenYearsAgo) throw new ArgumentException("Incomes cannot be more than 10 years ago");
            if (income.IncomeDate > tenYearsAfter) throw new ArgumentException("Incomes cannot be added more than 10 years in the future");

            var createdIncome = new Income
            {
                Name = income.Name,
                Description = income.Description,
                Value = income.Value!.Value,
                IncomeDate = income.IncomeDate.Value,
                CategoryId = income.CategoryId
            };

            await context.Incomes.AddAsync(createdIncome);
            await context.SaveChangesAsync();

            return createdIncome;
        }

        public async Task<bool> DeleteIncomeAsync(int id)
        {
            var income = await context.Incomes.FindAsync(id);

            if (income == null) throw new ArgumentException("Income not found");

            context.Remove(income);
            return await context.SaveChangesAsync() > 0; //Save changes returns an int(number of rows affected)
                                                         //if any changes have been made it will return true;
        }

        public async Task<List<Income>> GetAllIncomesAsync(int? categoryId = null, decimal? minValue = null, decimal? maxValue = null, string? description = null)
        {
            var query = context.Incomes.AsQueryable();

            if (categoryId != null)
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

        public async Task<Income> GetIncomeByIdAsync(int id)
        {
            var result = await context.Incomes
                .Include(i => i.Category)
                .SingleOrDefaultAsync(i => i.Id ==id);

            return result == null ? throw new ArgumentException("Cannot find income") : result;
        }

        public async Task<decimal> GetTotalIncomesAsync()
        {
            var total = await context.Incomes
                .SumAsync(x => x.Value);

            return total;
        }

        public async Task<bool> ModifyIncomeAsync(IncomeDTO modifiedIncome, int incomeId)
        {
            var income = await context.Incomes.FindAsync(incomeId);

            if (income == null) throw new ArgumentException("Could not find income");

            income = MapIncomeDTOtoIncome(modifiedIncome, income);
            income.ModifiedOn = DateTime.UtcNow;

            return await context.SaveChangesAsync() > 0;
        }

        private Income MapIncomeDTOtoIncome(IncomeDTO incomeDTO, Income income)
        {
            if (incomeDTO.Name != null) income.Name = incomeDTO.Name;

            if (incomeDTO.Value <= 0) throw new ArgumentException("Value cannot be negative");
            if (incomeDTO.Value != null) income.Value = incomeDTO.Value.Value;
            if (incomeDTO.Description != null) income.Description = incomeDTO.Description;


            var tenYearsAgo = DateOnly.FromDateTime(DateTime.Now).AddYears(-10);
            var tenYearsAfter = DateOnly.FromDateTime(DateTime.Now).AddYears(10);

            if (income.IncomeDate < tenYearsAgo) throw new ArgumentException("Incomes cannot be more than 10 years ago");
            if (income.IncomeDate > tenYearsAfter) throw new ArgumentException("Incomes cannot be added more than 10 years in the future");


            if (incomeDTO.IncomeDate != null) income.IncomeDate = incomeDTO.IncomeDate.Value;
            income.CategoryId = incomeDTO.CategoryId;

            return income;
        }
    }
}
