using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFirstApi.Data;
using MyFirstApi.Model;
using Microsoft.EntityFrameworkCore;
using MyFirstApi.DTOs;

namespace MyFirstApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public ProductController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get() 
        {
            return await _db.Products.ToListAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get(int id)
        {
           return await _db.Products.FindAsync(id) is { } p ? Ok(p) : NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Post(Product product)
        {
            _db.Products.Add(product);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> Edit (int id, Product product)
        {
            if(id != product.Id)
            {
                return BadRequest("Product ID mismatch");
            }
            _db.Entry(product).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id}")] 
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            return NoContent();
        }
        [HttpGet("search/{keyword}")]
        public async Task<ActionResult<IEnumerable<Product>>> Search(string keyword) =>
            await _db.Products.Where(p=>p.Name.Contains(keyword)).ToArrayAsync();
        [HttpGet("dto")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetDto() =>
            await _db.Products.Select(p => new ProductDto(p.Id, p.Name, p.Price)).ToListAsync();
        
    }
}
