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

namespace RezerwacjaSal.Pages.Rooms
{
    [Authorize(Roles = "administrator")]
    public class CreateModel : PageModel
    {
        private readonly RezerwacjaSal.Data.RezerwacjaSalContext _context;

        public CreateModel(RezerwacjaSal.Data.RezerwacjaSalContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int buildingid, int departmentid)
        {
            DepartmentIdRoute = departmentid;
            BuildingIdRoute = buildingid;

            ViewData["Building"] = new SelectList(_context.Buildings, "BuildingID", "Name", buildingid);
            return Page();
        }

        [BindProperty]
        public Room Room { get; set; }
        public int DepartmentIdRoute { get; private set; }
        public int BuildingIdRoute { get; private set; }
        public List<int> AllRoomNumbers { get; private set; }

        public string SameNumberError { get; set; }

        public async Task<IActionResult> OnPostAsync(int buildingid, int departmentid)
        {
            BuildingIdRoute = buildingid;
            DepartmentIdRoute = departmentid;

            ViewData["Building"] = new SelectList(_context.Buildings, "BuildingID", "Name", buildingid);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            AllRoomNumbers = await _context.Rooms
                .Where(r => r.BuildingID == buildingid)
                .Select(i => i.Number)
                .ToListAsync();

            if (AllRoomNumbers.Contains(Room.Number))
            {
                SameNumberError = "Już istnieje pokój o tym numerze.";
               
                return Page();
            }

            _context.Rooms.Add(Room);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index",new { buildingid = BuildingIdRoute, departmentid = DepartmentIdRoute });
        }
    }
}