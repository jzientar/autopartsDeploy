using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoPartsCompany.Models;
using Microsoft.AspNetCore.Hosting;

namespace AutoPartsCompany.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InboxController : ControllerBase
    {
        private readonly AutoPartsDBContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public InboxController(AutoPartsDBContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        // GET: api/Inbox
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InboxModel>>> GetInboxModel()
        {
            return await _context.InboxModel.Select(x => new InboxModel()
            {
                IdInbox = x.IdInbox,
                Message = x.Message,
                Date = x.Date
                }).ToListAsync();
        }

        // GET: api/Inbox/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InboxModel>> GetInboxModel(int id)
        {
            var InboxModel = await _context.InboxModel.FindAsync(id);

            if (InboxModel == null)
            {
                return NotFound();
            }

            return InboxModel;
        }


        // PUT: api/Inbox/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInboxModel(int id, InboxModel InboxModel)
        {
            if (id != InboxModel.IdInbox)
            {
                return BadRequest();
            }

            _context.Entry(InboxModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InboxModelExists(id))
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
        

        [HttpPost]
        public async Task<ActionResult<InboxModel>> PostInboxModel(InboxModel InboxModel)
        {
            _context.InboxModel.Add(InboxModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInboxModel", new { id = InboxModel.IdInbox }, InboxModel);
        }

        // DELETE: api/Inbox/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<InboxModel>> DeleteInboxModel(int id)
        {
            var InboxModel = await _context.InboxModel.FindAsync(id);
            if (InboxModel == null)
            {
                return NotFound();
            }

            _context.InboxModel.Remove(InboxModel);
            await _context.SaveChangesAsync();

            return InboxModel;
        }

        private bool InboxModelExists(int id)
        {
            return _context.InboxModel.Any(e => e.IdInbox == id);
        }
    }
}
