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
    public class AutoPartsController : ControllerBase
    {
        private readonly AutoPartsDBContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public AutoPartsController(AutoPartsDBContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        // GET: api/AutoParts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AutoPartsModel>>> GetAutoPartsModel()
        {
            return await _context.AutoPartsModel.Select(x => new AutoPartsModel()
            {
                IdSpare = x.IdSpare,
                IdCategory = x.IdCategory,
                Name = x.Name,
                MarkSpare = x.MarkSpare,
                Description = x.Description,
                Price = x.Price,
                TypeVehicle = x.TypeVehicle,
                MarkVehicle = x.MarkVehicle,
                ModelVehicle = x.ModelVehicle,
                ImageName = x.ImageName,
                ImageSrc = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.ImageName)
            }).ToListAsync();
        }

        // GET: api/AutoParts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AutoPartsModel>> GetAutoPartsModel(int id)
        {
            var autoPartsModel = await _context.AutoPartsModel.FindAsync(id);

            if (autoPartsModel == null)
            {
                return NotFound();
            }
            autoPartsModel.ImageSrc = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, autoPartsModel.ImageName);
            return autoPartsModel;
        }
      

        // GET: api/AutoParts/perno/search
        [HttpGet("{name}/search")]
        public async Task<ActionResult<IEnumerable<AutoPartsModel>>> GetAutoPartsModelWithName(string name)
        {
            IQueryable<AutoPartsModel> query = _context.AutoPartsModel.Where(x => x.Name.Contains(name));
            query = query.AsNoTracking().Where(x => x.Name.Contains(name)); 
            return await query.ToListAsync();
        }
        //// GET: api/AutoParts/toyota/
        //[HttpGet("{name}")]
        //public async Task<ActionResult<IEnumerable<AutoPartsModel>>> GetAutoPartsModelWithBrand(string brand)
        //{
        //    IQueryable<AutoPartsModel> query = _context.AutoPartsModel.Where(x => x.MarkVehicle.Contains(brand));
        //    query = query.AsNoTracking().Where(x => x.MarkVehicle.Contains(brand));
        //    return await query.ToListAsync();
        //}
        //// GET: api/AutoParts/camry/
        //[HttpGet("{name}")]
        //public async Task<ActionResult<IEnumerable<AutoPartsModel>>> GetAutoPartsModelWithModel(string model)
        //{
        //    IQueryable<AutoPartsModel> query = _context.AutoPartsModel.Where(x => x.ModelVehicle.Contains(model));
        //    query = query.AsNoTracking().Where(x => x.ModelVehicle.Contains(model));
        //    return await query.ToListAsync();
        //}

        // PUT: api/AutoParts/5

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAutoPartsModel(int id, [FromForm] AutoPartsModel autoPartsModel)
        {
            if (id != autoPartsModel.IdSpare)
            {
                return BadRequest();
            }
            if (autoPartsModel.ImageFile != null)
            {
                DeleteImage(autoPartsModel.ImageName);
                autoPartsModel.ImageName = await SaveImage(autoPartsModel.ImageFile);
            }


            _context.Entry(autoPartsModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AutoPartsModelExists(id))
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

        // POST: api/AutoParts
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        [HttpPost]
        public async Task<ActionResult<AutoPartsModel>> PostAutoPartsModel([FromForm] AutoPartsModel autoPartsModel)
        {
            autoPartsModel.ImageName = await SaveImage(autoPartsModel.ImageFile);
            _context.AutoPartsModel.Add(autoPartsModel);
            await _context.SaveChangesAsync();
            return StatusCode(201);
            //return CreatedAtAction("GetAutoPartsModel", new { id = autoPartsModel.IdSpare }, autoPartsModel);
        }

        // DELETE: api/AutoParts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AutoPartsModel>> DeleteAutoPartsModel(int id)
        {
            var autoPartsModel = await _context.AutoPartsModel.FindAsync(id);
            if (autoPartsModel == null)
            {
                return NotFound();
            }
            DeleteImage(autoPartsModel.ImageName);
            _context.AutoPartsModel.Remove(autoPartsModel);
            await _context.SaveChangesAsync();

            return autoPartsModel;
        }

        private bool AutoPartsModelExists(int id)
        {
            return _context.AutoPartsModel.Any(e => e.IdSpare == id);
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
