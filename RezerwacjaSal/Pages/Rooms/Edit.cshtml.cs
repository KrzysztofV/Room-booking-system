using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RezerwacjaSal.Data;
using RezerwacjaSal.Models;

namespace RezerwacjaSal.Pages.Rooms
{
    public class EditModel : PageModel
    {
        private readonly RezerwacjaSal.Data.RezerwacjaSalContext _context;

        public EditModel(RezerwacjaSal.Data.RezerwacjaSalContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Room Room { get; set; }

        public int BuildingIdRoute { get; set; }
        public int DepartmentIdRoute { get; set; }
        public List<int> AllRoomNumbers { get; private set; }
        public string SameNumberError { get; private set; }

        public async Task<IActionResult> OnGetAsync(int roomid, int buildingid, int departmentid)
        {
            BuildingIdRoute = buildingid;
            DepartmentIdRoute = departmentid;

            Room = await _context.Rooms
                .Include(r => r.Building)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.RoomID == roomid);

            if (Room == null)
                return NotFound();

            ViewData["Building"] = new SelectList(_context.Buildings, "BuildingID", "Name");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int roomid, int buildingid, int departmentid)
        {
            ViewData["Building"] = new SelectList(_context.Buildings, "BuildingID", "Name", buildingid);

            if (!ModelState.IsValid)
                return Page();

            AllRoomNumbers = await _context.Rooms
                .Select(i => i.Number)
                .ToListAsync();

            if (AllRoomNumbers.Contains(Room.Number))
            {
                SameNumberError = "Doopanuj się! Już istnieje pokój o tym numerze.";
                return Page();
            }

            var roomToUpdate = await _context.Rooms.FindAsync(roomid);
            if (await TryUpdateModelAsync<Room>(
            roomToUpdate,
            "Room",
            s => s.Number, s => s.Type, s => s.Spots, s => s.Equipment, s => s.BuildingID))
            {
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index", new { buildingid = buildingid, departmentid = departmentid });
        }
    }
}
