using Microsoft.EntityFrameworkCore;
using MVC_Project._BusinessLayer.Interfaces;
using MVC_Project._DataLayer.Models;

namespace MVC_Project._BusinessLayer.Services
{
    public class CategoryService(AppDbContext context) : ICategoryService
    {
        public async Task<Category> CreateCategoryAsync(string name, _DataLayer.Models.Type type, string? description = null)
        {
            var created = new Category
            {
                Name = name,
                Type = type,
                Description = description
            };

            if (type == _DataLayer.Models.Type.Income)
            {
                created.Incomes = [];
            }
            else
            {
                created.Expenses = [];
            }

            await context.Categories.AddAsync(created);
            await context.SaveChangesAsync();

            return created;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await context.Categories.FindAsync(id);

            if (category == null) throw new Exception("Category not found");

            context.Categories.Remove(category);

            return await context.SaveChangesAsync() > 0;

        }

        public async Task<List<Category>> GetAllCategoriesAsync(_DataLayer.Models.Type? type = null, bool? isActive = null)
        {
            var query = context.Categories.AsQueryable();

            if (type != null)
            {
                query = query
                    .Where(x => x.Type == type);
            }

            if(isActive != null)
            {
                query = query
                    .Where(x => x.IsActive == true);
            }

            var data = await query.ToListAsync();
            return data;
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            var category = await context.Categories.FindAsync(id);

            if (category == null) throw new Exception("Category not found");

            return category;
        }

        public async Task<bool> ModifyCategoryAsync(Category payload)
        {
            if (payload.Name == null) throw new ArgumentException("Category name cannot be null");

            var category = await context.Categories.FindAsync(payload.Id);

            if (category == null) throw new Exception("Category not found");

            category.Name = payload.Name;
            category.Description = payload.Description;
            category.Type = payload.Type;

            if (payload.Type != category.Type)
            {
                if (payload.Type == _DataLayer.Models.Type.Income)
                {
                    category.Incomes = [];
                    category.Expenses = null;
                }
                else
                {
                    category.Expenses = [];
                    category.Incomes = null;
                }
            }

            if(payload.IsActive != category.IsActive)
            {
                category.IsActive = payload.IsActive;
            }

            return await context.SaveChangesAsync() > 0;
        }
    }
}
