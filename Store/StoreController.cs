using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shop_Backend.StoreServices;
using Shop_Backend.DTOs;

namespace Shop_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoreController : ControllerBase
    {
        private readonly IStoreService _service;

        public StoreController(IStoreService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stores = await _service.GetAllStoresAsync();
            return Ok(stores);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var store = await _service.GetStoreByIdAsync(id);
            if (store == null) return NotFound();

            return Ok(store);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateStoreRequest request)
        {
            var store = await _service.CreateStoreAsync(request);
            return CreatedAtAction(nameof(GetById), new {id = store.Id}, store);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateStoreRequest request)
        {
            var result = await _service.UpdateStoreAsync(id, request);
            if (!result) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteStoreAsync(id);
            if (!result) return NotFound();

            return NoContent();
        }
    }
}