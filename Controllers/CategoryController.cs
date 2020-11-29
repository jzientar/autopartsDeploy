using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoPartsCompany.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace AutoPartsCompany.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly AutoPartsDBContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public CategoryController(AutoPartsDBContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        // GET: api/Category
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryModel>>> GetCategoryModel()
        {
            return await _context.CategoryModel.Select(x => new CategoryModel()
            {
                IdCategory = x.IdCategory,
                IdSpare = x.IdSpare,
                Name = x.Name,
                Description = x.Description,
                ImageName = x.ImageName,
                ImageSrc = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.ImageName)
            }).ToListAsync();
        }

        // GET: api/Category/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryModel>> GetCategoryModel(int id)
        {
            var categoryModel = await _context.CategoryModel.FindAsync(id);

            if (categoryModel == null)
            {
                return NotFound();
            }

            return categoryModel;
        }


        // PUT: api/Category/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoryModel(int id, [FromForm] CategoryModel categoryModel)
        {
            if (id != categoryModel.IdCategory)
            {
                return BadRequest();
            }
            if (categoryModel.ImageFile != null)
            {
                DeleteImage(categoryModel.ImageName);
                categoryModel.ImageName = await SaveImage(categoryModel.ImageFile);
            }
            _context.Entry(categoryModel).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        // GET: api/Category/5/AutoParts
        [HttpGet("{id}/AutoParts")]
        public async Task<ActionResult<IEnumerable<AutoPartsModel>>> GetAutoPartsModelCategory(int id)
        {
            IQueryable<AutoPartsModel> query = _context.AutoPartsModel.Where(x => x.IdCategory == id);
            query = query.AsNoTracking().Where(x => x.IdCategory == id);
            return await query.Select(x => new AutoPartsModel()
            {
                IdSpare = x.IdSpare,
                IdCategory = x.IdCategory,
                Name = x.Name,
                MarkSpare = x.MarkSpare,
                Description = x.Description,
                Price=x.Price,
                TypeVehicle = x.TypeVehicle,
                MarkVehicle = x.MarkVehicle,
                ModelVehicle = x.ModelVehicle,
                ImageName = x.ImageName,
                ImageSrc = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.ImageName)
            }).ToListAsync();
        }
        // GET: api/Category/5/AutoParts/Brands
        [HttpGet("{id}/AutoParts/Brand")]
        public async Task<ActionResult<IEnumerable<String>>> GetAutoPartsBrands(int id)
        {
            IQueryable<String> query = _context.AutoPartsModel.Where(x => x.IdCategory==id).Select(x=>x.MarkVehicle).Distinct();
            return await query.ToListAsync();
        }
        // GET: api/Category/5/AutoParts/Toyota/
        [HttpGet("{id}/AutoParts/{Brand}/")]
        public async Task<ActionResult<IEnumerable<String>>> GetAutoPartsBrandsModels(int id, string brand)
        {
            IQueryable<String> query = _context.AutoPartsModel.Where(x => x.IdCategory == id&&x.MarkVehicle==brand).Select(x => x.ModelVehicle).Distinct();
            return await query.ToListAsync();
        }
   
        [HttpPost]
        public async Task<ActionResult<CategoryModel>> PostCategoryModel([FromForm] CategoryModel categoryModel)
        {
            categoryModel.ImageName = await SaveImage(categoryModel.ImageFile);
            _context.CategoryModel.Add(categoryModel);
            await _context.SaveChangesAsync();
            return StatusCode(201);
            //return CreatedAtAction("GetCategoryModel", new { id = categoryModel.IdCategory }, categoryModel);
        }

        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CategoryModel>> DeleteCategoryModel(int id)
        {
            var categoryModel = await _context.CategoryModel.FindAsync(id);
            if (categoryModel == null)
            {
                return NotFound();
            }
            DeleteImage(categoryModel.ImageName);
            _context.CategoryModel.Remove(categoryModel);
            await _context.SaveChangesAsync();

            return categoryModel;
        }

        private bool CategoryModelExists(int id)
        {
            return _context.CategoryModel.Any(e => e.IdCategory == id);
        }

        [NonAction]
        public async Task<string> SaveImage(IFormFile imageFile)
        {
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
            imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", imageName);
            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }
            return imageName;
        }

        [NonAction]
        public void DeleteImage(string imageName)
        {
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", imageName);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);
        }
    }
}
