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
    public class MessageController : ControllerBase
    {
        private readonly AutoPartsDBContext _context;

        public MessageController(AutoPartsDBContext context)
        {
            _context = context;
        }

        // GET: api/Message
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageModel>>> GetMessageModel()
        {
            return await _context.MessageModel.Select(x => new MessageModel()
            {

                IdMessage = x.IdMessage,
                FullName = x.FullName,
                PhoneNumber = x.PhoneNumber,
                Subject = x.Subject,
                Email = x.Email,
                Message = x.Message,
                Date = x.Date,
                Read = x.Read
            }).ToListAsync();
        }

        // GET: api/Message/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MessageModel>> GetMessageModel(int id)
        {
            var messageModel = await _context.MessageModel.FindAsync(id);

            if (messageModel == null)
            {
                return NotFound();
            }

            return messageModel;
        }

        // PUT: api/Message/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMessageModel(int id, MessageModel messageModel)
        {
            if (id != messageModel.IdMessage)
            {
                return BadRequest();
            }

            _context.Entry(messageModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessageModelExists(id))
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

        // POST: api/Message
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<MessageModel>> PostMessageModel(MessageModel messageModel)
        {
            _context.MessageModel.Add(messageModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMessageModel", new { id = messageModel.IdMessage }, messageModel);
        }

        // DELETE: api/Message/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<MessageModel>> DeleteMessageModel(int id)
        {
            var messageModel = await _context.MessageModel.FindAsync(id);
            if (messageModel == null)
            {
                return NotFound();
            }

            _context.MessageModel.Remove(messageModel);
            await _context.SaveChangesAsync();

            return messageModel;
        }

        private bool MessageModelExists(int id)
        {
            return _context.MessageModel.Any(e => e.IdMessage == id);
        }
    }
}
