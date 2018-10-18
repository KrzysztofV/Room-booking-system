using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RezerwacjaSal.Data;
using RezerwacjaSal.Models;

namespace RezerwacjaSal.Pages.People
{
    public class DeleteModel : PageModel
    {
        private readonly RezerwacjaSal.Data.RezerwacjaSalContext _context;

        public DeleteModel(RezerwacjaSal.Data.RezerwacjaSalContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Pearson Pearson { get; set; }
        public string ErrorMessage { get; set; }
        public string SortOrderRoute { get; set; }
        public string CurrentFilterRoute { get; set; }
        public string SearchStringRoute { get; set; }
        public int? PageIndexRoute { get; set; }
        public int? PageSizeRoute { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, string sortOrder, string currentFilter, string searchString, int? pageIndex, int? pageSize)
        {
            if (id == null)
                return NotFound();

            SortOrderRoute = sortOrder;
            CurrentFilterRoute = currentFilter;
            SearchStringRoute = searchString;
            PageIndexRoute = pageIndex;
            PageSizeRoute = pageSize;

            Pearson = await _context.People
                .Include(s => s.Employments)      // wczytuje naviagtion properties z Employment
                .ThenInclude(e => e.Department) // wczytuje naviagtion properties z Department
                .AsNoTracking()                 // poprawia wydajność w przypadku gdy wczytane encje nie są modyfikowane w tej stronie
                .FirstOrDefaultAsync(m => m.PearsonID == id);  // domyślne

            if (Pearson == null)
                return NotFound();


            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id, string sortOrder, string currentFilter, string searchString, int? pageIndex, int? pageSize)
        {
            if (id == null)
                return NotFound();

            SortOrderRoute = sortOrder;
            CurrentFilterRoute = currentFilter;
            SearchStringRoute = searchString;
            PageIndexRoute = pageIndex;
            PageSizeRoute = pageSize;

            var pearson = await _context.People
                .Include(s => s.Employments)      // wczytuje naviagtion properties z Employment
                .ThenInclude(e => e.Department) // wczytuje naviagtion properties z Department
                .AsNoTracking()                 // poprawia wydajność w przypadku gdy wczytane encje nie są modyfikowane w tej stronie
                .FirstOrDefaultAsync(m => m.PearsonID == id);  // domyślne

            if (pearson == null)
                return NotFound();

            try
            {
                _context.People.Remove(pearson);
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
