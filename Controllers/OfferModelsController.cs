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
    public class OfferController : ControllerBase
    {
        private readonly AutoPartsDBContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public OfferController(AutoPartsDBContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        // GET: api/OfferModels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OfferModel>>> GetOfferModel()
        {
            //return await _context.OfferModel.ToListAsync();
            return await _context.OfferModel.Select(x => new OfferModel()
            {
                IdOffer = x.IdOffer,
                IdSpare = x.IdSpare,
                Name = x.Name,
                Description = x.Description,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ImageName = x.ImageName,
                ImageSrc = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.ImageName)
            }).ToListAsync();
        }

        // GET: api/OfferModels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OfferModel>> GetOfferModel(int id)
        {
            var offerModel = await _context.OfferModel.FindAsync(id);

            if (offerModel == null)
            {
                return NotFound();
            }
            offerModel.ImageSrc = string.Format("{0}://{1}{2}/Images{3}", Request.Scheme, Request.Host, Request.PathBase, offerModel.ImageName);
            return offerModel;
        }

        // PUT: api/OfferModels/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOfferModel(int id, [FromForm] OfferModel offerModel)
        {
            if (id != offerModel.IdOffer)
            {
                return BadRequest();
            }

            if (offerModel.ImageFile != null)
            {
                DeleteImage(offerModel.ImageName);
                offerModel.ImageName = await SaveImage(offerModel.ImageFile);
            }

            _context.Entry(offerModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OfferModelExists(id))
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

        // POST: api/OfferModels
        [HttpPost]
        public async Task<ActionResult<OfferModel>> PostOfferModel([FromForm] OfferModel offerModel)
        {
            offerModel.ImageName = await SaveImage(offerModel.ImageFile);
            _context.OfferModel.Add(offerModel);
            await _context.SaveChangesAsync();
            return StatusCode(201);
            //return CreatedAtAction("GetOfferModel", new { id = offerModel.IdOffer }, offerModel);
        }

        // DELETE: api/OfferModels/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<OfferModel>> DeleteOfferModel(int id)
        {
            var offerModel = await _context.OfferModel.FindAsync(id);
            if (offerModel == null)
            {
                return NotFound();
            }

            DeleteImage(offerModel.ImageName);
            _context.OfferModel.Remove(offerModel);
            await _context.SaveChangesAsync();

            return offerModel;
        }

        private bool OfferModelExists(int id)
        {
            return _context.OfferModel.Any(e => e.IdOffer == id);
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