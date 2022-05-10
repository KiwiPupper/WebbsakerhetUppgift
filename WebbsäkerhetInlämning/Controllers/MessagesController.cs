using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Web;
using WebbsäkerhetInlämning.Data;
using WebbsäkerhetInlämning.Models;

namespace WebbsäkerhetInlämning.Controllers
{
    public class MessagesController : Controller
    {
        private readonly SqlContext _context;
        private string[] tags = new string[] { "<strong>", "</strong>", "<em>", "</em>" };

        public MessagesController(SqlContext context)
        {
            _context = context;
        }

        // GET: MessagesController
        public async Task<ActionResult> Index()
        {
            return View(_context.Messages.ToList());
        }

        // GET: MessagesController/Details/5
        public async Task<ActionResult> Details(Message message, Guid id)
        {
            if (message.Id == null)
            {
                return NotFound();
            }

            var getMessage = await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
            if (getMessage == null)
            {
                return NotFound();
            }
            return View(getMessage);
        }

        // GET: MessagesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MessagesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection, Message message)
        {
            if(ModelState.IsValid)
            {
                string encodedContent = HttpUtility.HtmlEncode(message.MessageContent);
                foreach (var tag in tags)
                {
                    var encodedTag = HttpUtility.HtmlEncode(tag);
                    encodedContent = encodedContent.Replace(encodedTag, tag);
                }
                message.MessageContent = encodedContent;
                message.Id = Guid.NewGuid();
                _context.Add(message);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(message);
        }

        // GET: MessagesController/Edit/5
        public async Task<ActionResult> Edit(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Messages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }
            return View(message);
        }

        // POST: MessagesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid id, Message message)
        {
            if (id != message.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string encodedContent = HttpUtility.HtmlEncode(message.MessageContent);
                    foreach (var tag in tags)
                    {
                        var encodedTag = HttpUtility.HtmlEncode(tag);
                        encodedContent = encodedContent.Replace(encodedTag, tag);
                    }
                    message.MessageContent = encodedContent;
                    _context.Update(message);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MessageExists(message.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(message);
        }

        // GET: MessagesController/Delete/5
        public async Task<ActionResult> Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Messages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // POST: MessagesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Guid id, IFormCollection collection)
        {
            var message = await _context.Messages.FindAsync(id);
            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MessageExists(Guid id)
        {
            return _context.Messages.Any(e => e.Id == id);
        }
    }
}
