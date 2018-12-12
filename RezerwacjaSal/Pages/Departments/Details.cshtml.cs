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

namespace RezerwacjaSal.Pages.Departments
{
    [Authorize]

    public class DetailsModel : PageModel
    {
        private readonly RezerwacjaSal.Data.RezerwacjaSalContext _context;

        public DetailsModel(RezerwacjaSal.Data.RezerwacjaSalContext context)
        {
            _context = context;
        }

        public Department Department { get; set; }
        public IEnumerable<ApplicationUser> AppUsers { get; set; }
        public int DepartmentIdRoute { get; private set; }

        public async Task<IActionResult> OnGetAsync(int? departmentid)
        {
            DepartmentIdRoute = departmentid ?? 0;

            if (departmentid == null)
                return NotFound();

            Department = await _context.Departments
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.DepartmentID == departmentid);

            AppUsers = await _context.AppUsers
                .AsNoTracking()
                .ToListAsync();

            if (Department == null)
                return NotFound();

            return Page();
        }
    }
}
