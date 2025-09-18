using VShop.ProductApi.DTOs;
using VShop.ProductApi.Models;

namespace VShop.ProductApi.Services;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetCategories();
    Task<IEnumerable<CategoryDto>> GetCategoriesProducts();
    Task<CategoryDto> GetCategoryById(int id);
    Task AddCategory(CategoryDto categoryDto);
    Task UpdateCategory(CategoryDto categoryDto);
    Task DeleteCategory(int id);
}