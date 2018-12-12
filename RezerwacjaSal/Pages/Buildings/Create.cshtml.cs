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
    public class CreateModel : PageModel
    {
        private readonly RezerwacjaSal.Data.RezerwacjaSalContext _context;
        public int DepartmentIdRoute { get; set; }
        [BindProperty]
        public Building Building { get; set; }
        public IEnumerable<string> AllBuildingsNames { get; set; }
        public string DuplicateNameExistError { get; private set; }

        public CreateModel(RezerwacjaSal.Data.RezerwacjaSalContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int departmentid)
        {
            DepartmentIdRoute = departmentid;

            ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "Name", departmentid);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int departmentid)
        {
            if (!ModelState.IsValid)
                return Page();

            AllBuildingsNames = await _context.Buildings
                .Select(r => r.Name)
                .AsNoTracking()
                .ToListAsync();

            if (AllBuildingsNames.Contains(Building.Name))
            {
                DuplicateNameExistError = "Już istnieje budynek o tej nazwie.";
                return Page();
            }

            var emptyBuilding = new Building();

            if (await TryUpdateModelAsync<Building>(
            emptyBuilding,
            "Building",
            s => s.Name, s => s.Address, s => s.DepartmentID, s => s.GPS_E, s => s.GPS_N)) 
            {
                _context.Buildings.Add(emptyBuilding);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index", new { departmentid = departmentid });
        }
    }
}