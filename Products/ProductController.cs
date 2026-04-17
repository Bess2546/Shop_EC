using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop_Backend.DTOs;
using Shop_Backend.ProductService;
using Shop_Backend.StoreServices;

namespace Shop_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        private readonly IStoreService _storeService;

        public ProductController(IProductService productService, IStoreService storeService)
        {
            _productService = productService;
            _storeService = storeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var product = await _productService.GetAllProductAsync();
            return Ok(product);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product is null) return NotFound();

            return Ok(product);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateProductRequest request)
        {
            var ownerId = GetCurrentUserId();
            
            var store = await _storeService.GetStoreByIdAsync(request.StoreId!.Value);
            if (store is null) return NotFound(new {messgae = "ไม่พบร้านค้า"});
            if (store.OwnerId != ownerId) return Forbid();

            var product = await _productService.CreateProductAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, CreateProductRequest request)
        {
            var ownerId = GetCurrentUserId();

            var product = await _productService.GetProductByIdAsync(id);
            if (product is null) return NotFound();

            if (!await IsStoreOwnerAsync(product.StoreId, ownerId))
                return Forbid();

            var result = await _productService.UpdateProductAsync(id, request);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var ownerId = GetCurrentUserId();

            var product = await _productService.GetProductByIdAsync(id);
            if (product is null) return NotFound();

            if (!await IsStoreOwnerAsync(product.StoreId, ownerId))
                return Forbid();

            var result = await _productService.DeleteProductAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        private async Task<bool> IsStoreOwnerAsync(int storeId, int userId)
        {
            var store = await _storeService.GetStoreByIdAsync(storeId);
            return store is not null && store.OwnerId == userId;
        }
    }
}