using Microsoft.AspNetCore.Mvc;
using Shop_Backend.StoreServices;
using Shop_Backend.DTOs;
using Microsoft.AspNetCore.Authorization;


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
        [Authorize]
        public async Task<IActionResult> Create(CreateStoreRequest request)
        {
            var store = await _service.CreateStoreAsync(request);
            return CreatedAtAction(nameof(GetById), new {id = store.Id}, store);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, CreateStoreRequest request)
        {
            var result = await _service.UpdateStoreAsync(id, request);
            if (!result) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteStoreAsync(id);
            if (!result) return NotFound();

            return NoContent();
        }
    }
}