using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RezerwacjaSal.Data;
using RezerwacjaSal.Models;

namespace RezerwacjaSal.Pages.Buildings
{
    [Authorize(Roles = "administrator")]
    public class EditModel : PageModel
    {
        private readonly RezerwacjaSal.Data.RezerwacjaSalContext _context;

        public EditModel(RezerwacjaSal.Data.RezerwacjaSalContext context)
        {
            _context = context;
        }

        public int DepartmentIdRoute { get; private set; }
        [BindProperty]
        public Building Building { get; set; }
        public IEnumerable<string> AllOtherBuildingsNames { get; set; }
        public string DuplicateNameExistError { get; private set; }

        public async Task<IActionResult> OnGetAsync(int buildingid, int departmentid)
        {
            DepartmentIdRoute = departmentid;

            Building = await _context.Buildings
                .Include(b => b.Department)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.BuildingID == buildingid);

            if (Building == null)
                 return NotFound();

           ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "Name");

           return Page();
        }

        public async Task<IActionResult> OnPostAsync(int buildingid, int departmentid)
        {
            DepartmentIdRoute = departmentid;


            ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "Name");

            if (!ModelState.IsValid)
                 return Page();

            AllOtherBuildingsNames = await _context.Buildings
                .Where(r => r.Name != _context.Buildings.SingleOrDefault(m => m.BuildingID == buildingid).Name)
                .Select(r => r.Name)
                .AsNoTracking()
                .ToListAsync();

            if (AllOtherBuildingsNames.Contains(Building.Name))
            {
                DuplicateNameExistError = "Już istnieje budynek o tej nazwie.";
                return Page();
            }

            var buildingToUpdate = await _context.Buildings.FindAsync(buildingid);

            if (await TryUpdateModelAsync<Building>(
            buildingToUpdate,
            "Building",
            s => s.Name, s => s.Address, s => s.DepartmentID, s => s.GPS_E, s => s.GPS_N))
            {
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index", new { departmentid = DepartmentIdRoute });

        }


    }
}
