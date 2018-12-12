using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RezerwacjaSal.Data;
using RezerwacjaSal.Models;

namespace RezerwacjaSal.Pages.Messages
{
    [Authorize(Roles = "administrator")]
    public class ListModel : PageModel
    {
        private readonly RezerwacjaSal.Data.RezerwacjaSalContext _context;

        public ListModel(RezerwacjaSal.Data.RezerwacjaSalContext context)
        {
            _context = context;
        }

        public IList<Message> Messages { get; set; }
        public Message Message { get; set; }

        public async Task OnGetAsync()
        {
            Messages = await _context.Messages
                .Include(m => m.ApplicationUser)
                .AsNoTracking().OrderByDescending(r=>r.Date)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Messages = await _context.Messages
                .AsNoTracking()
                .ToListAsync();

            foreach (var message in Messages)
            {
                _context.Messages.Remove(message);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./List");
        }

    }
}
