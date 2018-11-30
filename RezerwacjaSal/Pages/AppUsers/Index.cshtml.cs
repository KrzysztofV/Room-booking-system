using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RezerwacjaSal.Data;
using RezerwacjaSal.Models;

namespace RezerwacjaSal.Pages.AppUsers
{
    public class IndexModel : PageModel
    {
        private readonly RezerwacjaSal.Data.RezerwacjaSalContext _context;

        public IndexModel(RezerwacjaSal.Data.RezerwacjaSalContext context)
        {
            _context = context;
        }

        public string FirstNameSort { get; set; }
        public string LastNameSort { get; set; }
        public string NumberSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }
        public int PageSize { get; set; }
        public PaginatedList<ApplicationUser> ApplicationUser { get; set; }
        public IList<int> PageSizesList { get; set; }
        public string SortOrderRoute { get; set; }
        public string CurrentFilterRoute { get; set; }
        public string SearchStringRoute { get; set; }
        public int? PageIndexRoute { get; set; }
        public int? PageSizeRoute { get; set; }
        public IList<Department> Departments { get; set; }


        public async Task OnGetAsync(string sortOrder, string currentFilter, string searchString, int? pageIndex, int? pageSize)

        {
            SortOrderRoute = sortOrder;
            CurrentFilterRoute = currentFilter;
            SearchStringRoute = searchString;
            PageIndexRoute = pageIndex;
            PageSizeRoute = pageSize;

            CurrentSort = sortOrder;

            if (String.IsNullOrEmpty(sortOrder))
            {
                FirstNameSort = "first_name_ascen";
                LastNameSort = "last_name_ascen";
            }
            if (sortOrder == "first_name_desc")
            {
                FirstNameSort = "first_name_desc";
                LastNameSort = "last_name_desc";
            }
            if (sortOrder == "first_name_ascen")
            {
                FirstNameSort = "first_name_ascen";
                LastNameSort = "last_name_ascen";
            }
            if (sortOrder == "last_name_desc")
            {
                FirstNameSort = "first_name_desc";
                LastNameSort = "last_name_desc";
            }
            if (sortOrder == "last_name_ascen")
            {
                FirstNameSort = "first_name_ascen";
                LastNameSort = "last_name_ascen";
            }
            if (sortOrder == "Numer") NumberSort = "number_desc";

            if (searchString != null)
                pageIndex = 1;
            else
                searchString = currentFilter;

            CurrentFilter = searchString;

            IQueryable<ApplicationUser> appUserIQ = from s in _context.AppUsers
                                            select s;

            // wyszukiwanie
            if (!String.IsNullOrEmpty(searchString))
            {
                int Number;
                var searchStringIsNumber= int.TryParse(searchString, out Number);
                appUserIQ = appUserIQ.Where(s => s.LastName.Contains(searchString)
                                       || s.FirstName.Contains(searchString)
                                       || s.Number.Equals(Number));
            }

            // przełączanie sortowania
            switch (sortOrder)
            {
                case "first_name_desc":
                    appUserIQ = appUserIQ.OrderByDescending(s => s.FirstName);
                    break;
                case "first_name_ascen":
                    appUserIQ = appUserIQ.OrderBy(s => s.FirstName);
                    break;
                case "last_name_ascen":
                    appUserIQ = appUserIQ.OrderBy(s => s.LastName);
                    break;
                case "last_name_desc":
                    appUserIQ = appUserIQ.OrderByDescending(s => s.LastName);
                    break;
                case "number_desc":
                    appUserIQ = appUserIQ.OrderByDescending(s => s.Number);
                    break;
                default:
                    appUserIQ = appUserIQ.OrderBy(s => s.Number);
                    break;
            }

            PageSize = pageSize ?? 15;

            ApplicationUser = await PaginatedList<ApplicationUser>.CreateAsync(
                    appUserIQ.AsNoTracking(), pageIndex ?? 1, PageSize);

            Departments = await _context.Departments
                    .AsNoTracking()
                    .ToListAsync();
        }
    }
}
