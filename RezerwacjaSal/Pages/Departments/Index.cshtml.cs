using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RezerwacjaSal.Data;
using RezerwacjaSal.Models;

namespace RezerwacjaSal.Pages.Departments
{
    public class IndexModel : PageModel
    {
        private readonly RezerwacjaSal.Data.RezerwacjaSalContext _context;

        public IndexModel(RezerwacjaSal.Data.RezerwacjaSalContext context)
        {
            _context = context;
        }

        public IEnumerable<Department> Departments { get; set; }
        public IEnumerable<ApplicationUser> AppUsers { get; set; }
        public async Task OnGetAsync()
        {
            Departments = await _context.Departments
                .AsNoTracking()
                .OrderBy(n=>n.Name)
                .ToListAsync();
            AppUsers = await _context.AppUsers
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
