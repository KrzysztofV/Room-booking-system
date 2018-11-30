using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RezerwacjaSal.Data;
using RezerwacjaSal.Models;

namespace RezerwacjaSal.Pages.AppUsers
{
    public class DetailsModel : PageModel
    {
        private readonly RezerwacjaSal.Data.RezerwacjaSalContext _context;

        public DetailsModel(RezerwacjaSal.Data.RezerwacjaSalContext context)
        {
            _context = context;
        }

        public ApplicationUser ApplicationUser { get; set; }
        public string SortOrderRoute { get; set; }
        public string CurrentFilterRoute { get; set; }
        public string SearchStringRoute { get; set; }
        public int? PageIndexRoute { get; set; }
        public int? PageSizeRoute { get; set; }

        public async Task<IActionResult> OnGetAsync(string id, string sortOrder, string currentFilter, string searchString, int? pageIndex, int? pageSize)
        {
            if (id == null)
                return NotFound();

            SortOrderRoute = sortOrder;
            CurrentFilterRoute = currentFilter;
            SearchStringRoute = searchString;
            PageIndexRoute = pageIndex;
            PageSizeRoute = pageSize;

            ApplicationUser = await _context.AppUsers  
                .Include(e => e.Department) 
                .AsNoTracking()                 
                .FirstOrDefaultAsync(m => m.Id == id);  // domyślne

            if (ApplicationUser == null)
                return NotFound();

            return Page();
        }
    }
}
