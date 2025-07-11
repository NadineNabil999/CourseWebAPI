using CourseWebAPI.DTOs;
using CourseWebAPI.DTOs.CategoryDTOs;
using CourseWebAPI.DTOs.ProductDTOs;
using CourseWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAll()
        {
            var categories = await _context.Categories.ToListAsync();

            var result = categories.Select(c => new CategoryDTO
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();

            return Ok(result);
        }

        
        [HttpGet("{id} without products")]
        public async Task<ActionResult<CategoryDTO>> GetById(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            var dto = new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name
            };

            return Ok(dto);
        }

        
        [HttpPost ("Create Category")]
        public async Task<ActionResult<CategoryDTO>> Create(CreateCategoryDTO dto)
        {
            var category = new Category
            {
                Name = dto.Name
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var result = new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name
            };

            return CreatedAtAction(nameof(GetById), new { id = category.Id }, result);
        }

        
        [HttpPut("{id} Update By ID")]
        public async Task<IActionResult> Update(int id, UpdateCategoryDTO dto)
        {
            if (id != dto.Id)
                return BadRequest();

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            category.Name = dto.Name;
            await _context.SaveChangesAsync();

            return Ok(new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name
            });
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            var relatedProducts = await _context.Products.Where(p => p.CategoryId == id).ToListAsync();
            foreach (var product in relatedProducts)
            {
                product.CategoryId = null;
            }

            await _context.SaveChangesAsync();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return Ok();
        }


        [HttpGet("with-products")]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAllWithProducts()
        {
            var categories = await _context.Categories
                .Include(c => c.Products) 
                .ToListAsync();

            var result = categories.Select(c => new CategoryDTO
            {
                Id = c.Id,
                Name = c.Name,
                Products = c.Products?
                    .Where(p => p.DeleteFlag == false)
                    .Select(p => new ProductDTO
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price,
                        CreateDate = p.CreateDate,
                        LastUpdateDate = p.LastUpdateDate
                    }).ToList()
            }).ToList();

            return Ok(result);
        }



        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> SearchByName([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("You must provide a search term.");

            var results = await _context.Categories
                .Where(c => c.Name.ToLower().Contains(name.ToLower()))
                .ToListAsync();

            if (!results.Any())
                return NotFound("No categories found with the given name.");

            var result = results.Select(c => new CategoryDTO
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();

            return Ok(result);
        }
    }
}
