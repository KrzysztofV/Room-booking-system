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

namespace RezerwacjaSal.Pages.Buildings
{
    [Authorize(Roles = "administrator")]
    public class DeleteModel : PageModel
    {
        private readonly RezerwacjaSal.Data.RezerwacjaSalContext _context;

        public DeleteModel(RezerwacjaSal.Data.RezerwacjaSalContext context)
        {
            _context = context;
        }
        public int DepartmentIdRoute { get; private set; }
        [BindProperty]
        public Building Building { get; set; }

        public async Task<IActionResult> OnGetAsync(int buildingid, int departmentid)
        {

            DepartmentIdRoute = departmentid;

            Building = await _context.Buildings
                .Include(b => b.Department)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.BuildingID == buildingid);

            if (Building == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int buildingid, int departmentid)
        {
            Building = await _context.Buildings.FindAsync(buildingid);

            if (Building != null)
            {
                _context.Buildings.Remove(Building);
                await _context.SaveChangesAsync();
            }
            else
                return NotFound();

            return RedirectToPage("./Index", new { departmentid = departmentid });
        }
    }
}
