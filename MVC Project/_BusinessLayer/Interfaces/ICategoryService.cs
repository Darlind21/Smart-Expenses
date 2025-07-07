using MVC_Project._DataLayer.Models;
using Type = MVC_Project._DataLayer.Models.Type;

namespace MVC_Project._BusinessLayer.Interfaces
{
    public interface ICategoryService
    {
        public Task<Category> CreateCategoryAsync(string name, Type type, string? description = null);
        public Task<Category> GetCategoryByIdAsync(int id);
        public Task<List<Category>> GetAllCategoriesAsync(Type? type = null, bool? isActive = null);
        public Task<bool> ModifyCategoryAsync(Category payload);
        public Task<bool> DeleteCategoryAsync(int id);
    }
}
