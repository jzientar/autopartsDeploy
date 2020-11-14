using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoPartsCompany.Models;

namespace AutoPartsCompany.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfferController : ControllerBase
    {
        private readonly AutoPartsDBContext _context;

        public OfferController(AutoPartsDBContext context)
        {
            _context = context;
        }

        // GET: api/OfferModels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OfferModel>>> GetOfferModel()
        {
            return await _context.OfferModel.ToListAsync();
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

            return offerModel;
        }

        // PUT: api/OfferModels/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOfferModel(int id, OfferModel offerModel)
        {
            if (id != offerModel.IdOffer)
            {
                return BadRequest();
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
        public async Task<ActionResult<OfferModel>> PostOfferModel(OfferModel offerModel)
        {
            _context.OfferModel.Add(offerModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOfferModel", new { id = offerModel.IdOffer }, offerModel);
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

            _context.OfferModel.Remove(offerModel);
            await _context.SaveChangesAsync();

            return offerModel;
        }

        private bool OfferModelExists(int id)
        {
            return _context.OfferModel.Any(e => e.IdOffer == id);
        }
    }
}