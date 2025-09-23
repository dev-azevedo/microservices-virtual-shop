using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VShop.ProductApi.DTOs;
using VShop.ProductApi.Services;

namespace VShop.ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> Get()
        {
            var productsDto = await _productService.GetProducts();
            
            if (productsDto is null)
                return NotFound("Products not found");
            
            return Ok(productsDto);
        }
        
        [HttpGet("{id:int}", Name = "GetProduct")]
        public async Task<ActionResult<ProductDto>> GetById(int id)
        {
            var productDto = await _productService.GetProductById(id);
            
            if (productDto is null)
                return NotFound("Products not found");
            
            return Ok(productDto);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProductDto productDto)
        {
            if (productDto is null)
                return BadRequest("Invalid Data");

            await _productService.AddProduct(productDto);
            
            return new CreatedAtRouteResult("GetProduct", new {id = productDto.Id}, productDto);
        }
        
        [HttpPut]
        public async Task<ActionResult> Put([FromBody] ProductDto productDto)
        {
            if (productDto is null)
                return BadRequest("Invalid Data");

            await _productService.UpdateProduct(productDto);
            
            return Ok(productDto);
        }
        
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var productDto = await _productService.GetProductById(id);
            
            if (productDto is null)
                return NotFound("Product not found");

            await _productService.DeleteProduct(id);
            
            return Ok(productDto);
        }
    }
}
