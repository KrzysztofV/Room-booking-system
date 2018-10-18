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

namespace RezerwacjaSal.Pages.People
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
        public PaginatedList<Pearson> Pearson { get; set; }
        public IList<int> PageSizesList { get; set; }
        public string SortOrderRoute { get; set; }
        public string CurrentFilterRoute { get; set; }
        public string SearchStringRoute { get; set; }
        public int? PageIndexRoute { get; set; }
        public int? PageSizeRoute { get; set; }

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

            IQueryable<Pearson> pearsonIQ = from s in _context.People
                                            select s;

            // wyszukiwanie
            if (!String.IsNullOrEmpty(searchString))
            {
                int Number;
                var searchStringIsNumber= int.TryParse(searchString, out Number);
                pearsonIQ = pearsonIQ.Where(s => s.LastName.Contains(searchString)
                                       || s.FirstName.Contains(searchString)
                                       || s.PearsonNumber.Equals(Number));
            }

            // przełączanie sortowania
            switch (sortOrder)
            {
                case "first_name_desc":
                    pearsonIQ = pearsonIQ.OrderByDescending(s => s.FirstName);
                    break;
                case "first_name_ascen":
                    pearsonIQ = pearsonIQ.OrderBy(s => s.FirstName);
                    break;
                case "last_name_ascen":
                    pearsonIQ = pearsonIQ.OrderBy(s => s.LastName);
                    break;
                case "last_name_desc":
                    pearsonIQ = pearsonIQ.OrderByDescending(s => s.LastName);
                    break;
                case "number_desc":
                    pearsonIQ = pearsonIQ.OrderByDescending(s => s.PearsonNumber);
                    break;
                default:
                    pearsonIQ = pearsonIQ.OrderBy(s => s.PearsonNumber);
                    break;
            }

            PageSize = pageSize ?? 15;

            Pearson = await PaginatedList<Pearson>.CreateAsync(
                    pearsonIQ.AsNoTracking(), pageIndex ?? 1, PageSize);
        }
    }
}
