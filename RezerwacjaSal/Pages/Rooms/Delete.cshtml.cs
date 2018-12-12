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

namespace RezerwacjaSal.Pages.Rooms
{
    [Authorize(Roles = "administrator")]

    public class DeleteModel : PageModel
    {
        private readonly RezerwacjaSal.Data.RezerwacjaSalContext _context;

        public DeleteModel(RezerwacjaSal.Data.RezerwacjaSalContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Room Room { get; set; }
        public int BuildingIdRoute { get; private set; }
        public int DepartmentIdRoute { get; set; }

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

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int roomid, int buildingid, int departmentid)
        {

            Room = await _context.Rooms.FindAsync(roomid);

            if (Room != null)
            {
                _context.Rooms.Remove(Room);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index", new { buildingid = buildingid, departmentid = departmentid });
        }
    }
}
