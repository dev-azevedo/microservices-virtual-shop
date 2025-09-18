using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VShop.ProductApi.DTOs;
using VShop.ProductApi.Repositories;
using VShop.ProductApi.Services;

namespace VShop.ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> Get()
        {
            var categoriesDto = await _categoryService.GetCategories();
            
            if (categoriesDto is null)
                return NotFound("Categories not found");
            
            return Ok(categoriesDto);
        }
        
        [HttpGet("products")]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategoriesProducts()
        {
            var categoriesDto = await _categoryService.GetCategoriesProducts();
            
            if (categoriesDto is null)
                return NotFound("Categories not found");
            
            return Ok(categoriesDto);
        }
        
        [HttpGet("{id:int}", Name = "GetCategory")]
        public async Task<ActionResult<CategoryDto>> GetById(int id)
        {
            var categoryDto = await _categoryService.GetCategoryById(id);
            
            if (categoryDto is null)
                return NotFound("Categories not found");
            
            return Ok(categoryDto);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CategoryDto categoryDto)
        {
            if (categoryDto is null)
                return BadRequest("Invalid Data");

            await _categoryService.AddCategory(categoryDto);
            
            return new CreatedAtRouteResult("GetCategory", new {id = categoryDto.CategoryId}, categoryDto);
        }
        
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] CategoryDto categoryDto)
        {
            if (id != categoryDto.CategoryId)
                return BadRequest("Invalid Data");
            
            if (categoryDto is null)
                return BadRequest("Invalid Data");

            await _categoryService.UpdateCategory(categoryDto);
            
            return Ok(categoryDto);
        }
        
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var categoryDto = await _categoryService.GetCategoryById(id);
            
            if (categoryDto is null)
                return NotFound("Category not found");

            await _categoryService.DeleteCategory(id);
            
            return Ok(categoryDto);
        }
    }
}
