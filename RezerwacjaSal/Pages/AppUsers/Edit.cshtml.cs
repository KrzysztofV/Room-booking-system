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

namespace RezerwacjaSal.Pages.AppUsers
{
    public class EditModel : PageModel
    {
        private readonly RezerwacjaSal.Data.RezerwacjaSalContext _context;

        public EditModel(RezerwacjaSal.Data.RezerwacjaSalContext context)
        {
            _context = context;
        }

        public IEnumerable<Employment> Employments { get; set; }
        [BindProperty]
        public Employment FirstEmployment { get; set; }
        [BindProperty]
        public Employment SecondEmployment { get; set; }
        [BindProperty]
        public ApplicationUser ApplicationUser { get; set; }
        public string SortOrderRoute { get; set; }
        public string CurrentFilterRoute { get; set; }
        public string SearchStringRoute { get; set; }
        public int? PageIndexRoute { get; set; }
        public int? PageSizeRoute { get; set; }

        [BindProperty]
        public bool SecondEmploymentChecked { get; set; }
        public string ErrorSameNumber { get; set; }
        private List<int> AllOthersNumbers;
        public async Task<IActionResult> OnGet(int id, string sortOrder, string currentFilter, string searchString, int? pageIndex, int? pageSize)
        {

            SortOrderRoute = sortOrder;
            CurrentFilterRoute = currentFilter;
            SearchStringRoute = searchString;
            PageIndexRoute = pageIndex;
            PageSizeRoute = pageSize;

            ApplicationUser = await _context.AppUsers
                .Include(s => s.Employments)      // wczytuje naviagtion properties z Employment
                .ThenInclude(e => e.Department) // wczytuje naviagtion properties z Department
                .AsNoTracking()                 // poprawia wydajność w przypadku gdy wczytane encje nie są modyfikowane w tej stronie
                .FirstOrDefaultAsync(m => Int32.Parse(m.Id) == id);  // dla danego ID

            Employments = await _context.Employments
                .Where(s => s.Id == id)
                .AsNoTracking()
                .ToListAsync();

            ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "Name");

            FirstEmployment = Employments.FirstOrDefault();

            if (Employments.Count() > 1)
            {
                SecondEmployment = Employments.Last();
                SecondEmploymentChecked = true;
            }
            else
                SecondEmploymentChecked = false;

            if (ApplicationUser == null)
                return NotFound();

            return Page();
        }


        public async Task<IActionResult> OnPostAsync(int id, string sortOrder, string currentFilter, string searchString, int? pageIndex, int? pageSize)
        {
            SortOrderRoute = sortOrder;
            CurrentFilterRoute = currentFilter;
            SearchStringRoute = searchString;
            PageIndexRoute = pageIndex;
            PageSizeRoute = pageSize;

            AllOthersNumbers = await _context.AppUsers
                .Where(i => Int32.Parse(i.Id) != id)
                .Select(i => i.Number)
                .ToListAsync();

            if (AllOthersNumbers.Contains(ApplicationUser.Number))      // własna validacja numeru osoby
            {
                ErrorSameNumber = "Doopanuj się! Ten numer jest już zajęty.";
                return Page();
            }

            // aktualizacja osoby
            var appUserToUpdate = await _context.AppUsers.FindAsync(id);

            if (await TryUpdateModelAsync<ApplicationUser>(
                appUserToUpdate,
                "ApplicationUser",   
                 s => s.Number, s => s.FirstName, s => s.LastName, s => s.Employee, s => s.Email, s => s.Phone, s => s.Note))
            {
                await _context.SaveChangesAsync();
            }

            // zatrudnienia dla danej osoby
            Employments = await _context.Employments
                .Where(s => s.Id == id)
                .AsNoTracking()
                .ToListAsync();

            // jeśli zaznaczono "pracownik"
            if (appUserToUpdate.Employee)
            {
                if (Employments.Any())
                    FirstEmployment = Employments.FirstOrDefault(); // jeśli osoba miała pierwszy etat -> aktualizacja
                else
                    FirstEmployment = new Employment(); // osoba nie miała pierwszego etatu -> nowy pierwszy etat

                FirstEmployment.Id = Int32.Parse(appUserToUpdate.Id); // przekazanie ID z osoby do zatrudnienia

                if (await TryUpdateModelAsync<Employment>(
                    FirstEmployment,
                    "FirstEmployment",
                    s => s.DepartmentID, s => s.Position))
                {
                    if (!Employments.Any()) _context.Employments.Add(FirstEmployment);  //ewentualne dodanie zatrudnienia
                    await _context.SaveChangesAsync();
                }

                // jeśli zaznaczono drugi etat
                if (SecondEmploymentChecked)
                {
                    if (Employments.Count() > 1)
                        SecondEmployment = Employments.Last(); // osoba miała drugi etat -> aktualizacja
                    else
                        SecondEmployment = new Employment();  // osoba nie miała drugiego etatu -> nowy drugi etat

                    SecondEmployment.Id = Int32.Parse(appUserToUpdate.Id); // przekazanie ID z osoby do zatrudnienia

                    if (await TryUpdateModelAsync<Employment>(
                                        SecondEmployment,
                                        "SecondEmployment",
                                        s => s.DepartmentID, s => s.Position))
                    {
                        if (Employments.Count() < 2) _context.Employments.Add(SecondEmployment);  //ewentualne dodanie zatrudnienia
                        await _context.SaveChangesAsync();
                    }
                }
                else // jeśli nie zaznaczono drugi etat
                {
                    if (Employments.Count() > 1) // jeśli osoba miała drugi etat -> usuń
                    {
                        SecondEmployment = Employments.Last();
                        try
                        {
                            _context.Employments.Remove(SecondEmployment);
                            await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateException /* ex */)
                        {
                            //Log the error (uncomment ex variable name and write a log.)
                            return RedirectToAction("./Index",
                                                 new { id = id, saveChangesError = true });
                        }
                    }
                }
            }

            else // jeśli nie zaznaczono pracownik
            {
                if (Employments.Any()) // jeśli miała pierwsze zatrudnienie -> usuń pierwsze
                {
                    FirstEmployment = Employments.FirstOrDefault();

                    try
                    {
                        _context.Employments.Remove(FirstEmployment);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateException /* ex */)
                    {
                        //Log the error (uncomment ex variable name and write a log.)
                        return RedirectToAction("./Index",
                                             new { id = id, saveChangesError = true });
                    }
                }
                if (Employments.Count() > 1) // jeśli miała drugie zatrudnienie -> usuń drugie
                {
                    SecondEmployment = Employments.Last();

                    try
                    {
                        _context.Employments.Remove(SecondEmployment);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateException /* ex */)
                    {
                        //Log the error (uncomment ex variable name and write a log.)
                        return RedirectToAction("./Index",
                                             new { id = id, saveChangesError = true });
                    }
                }
            }
            return RedirectToPage("./Index");
        }

        private bool AppUserExists(int id)
        {
            return _context.AppUsers.Any(e => Int32.Parse(e.Id) == id);
        }
    }
}
