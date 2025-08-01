using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFirstApi.Data;
using MyFirstApi.Model;
using Microsoft.EntityFrameworkCore;

namespace MyFirstApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get() =>
        
            await _db.Products.ToListAsync();

    }
}
