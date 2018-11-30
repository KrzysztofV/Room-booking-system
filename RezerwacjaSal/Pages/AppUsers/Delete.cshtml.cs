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
    public class DeleteModel : PageModel
    {
        private readonly RezerwacjaSal.Data.RezerwacjaSalContext _context;

        public DeleteModel(RezerwacjaSal.Data.RezerwacjaSalContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ApplicationUser ApplicationUser { get; set; }
        public string ErrorMessage { get; set; }
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
                .Include(e => e.Department) // wczytuje naviagtion properties z Department
                .AsNoTracking()                 // poprawia wydajność w przypadku gdy wczytane encje nie są modyfikowane w tej stronie
                .FirstOrDefaultAsync(m => m.Id == id);  // domyślne

            if (ApplicationUser == null)
                return NotFound();


            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id, string sortOrder, string currentFilter, string searchString, int? pageIndex, int? pageSize)
        {
            if (id == null)
                return NotFound();

            SortOrderRoute = sortOrder;
            CurrentFilterRoute = currentFilter;
            SearchStringRoute = searchString;
            PageIndexRoute = pageIndex;
            PageSizeRoute = pageSize;

            var appUser = await _context.AppUsers
                .Include(e => e.Department) // wczytuje naviagtion properties z Department
                .AsNoTracking()                 // poprawia wydajność w przypadku gdy wczytane encje nie są modyfikowane w tej stronie
                .FirstOrDefaultAsync(m => m.Id == id);  // domyślne

            if (appUser == null)
                return NotFound();

            try
            {
                _context.AppUsers.Remove(appUser);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction("./Delete",
                                     new { id = id, saveChangesError = true });
            }
        }
    }
}
