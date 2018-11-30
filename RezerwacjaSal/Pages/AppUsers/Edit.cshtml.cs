using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RezerwacjaSal.Data;
using RezerwacjaSal.Models;

namespace RezerwacjaSal.Pages.AppUsers
{
    public class EditModel : PageModel
    {
        private readonly RezerwacjaSal.Data.RezerwacjaSalContext _context;

        public EditModel(
            RezerwacjaSal.Data.RezerwacjaSalContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
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

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;

        [BindProperty]
        public InputModel Input { get; set; }
        [BindProperty]
        public bool ChangePassword { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnGet(string id, string sortOrder, string currentFilter, string searchString, int? pageIndex, int? pageSize)
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
                .FirstOrDefaultAsync(m => m.Id == id);  // dla danego ID

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


        public async Task<IActionResult> OnPostAsync(string id, string sortOrder, string currentFilter, string searchString, int? pageIndex, int? pageSize)
        {
            SortOrderRoute = sortOrder;
            CurrentFilterRoute = currentFilter;
            SearchStringRoute = searchString;
            PageIndexRoute = pageIndex;
            PageSizeRoute = pageSize;

            AllOthersNumbers = await _context.AppUsers
                .Where(i => i.Id != id)
                .Select(i => i.Number)
                .ToListAsync();

            if (AllOthersNumbers.Contains(ApplicationUser.Number))      // własna validacja numeru osoby
            {
                ErrorSameNumber = "Doopanuj się! Ten numer jest już zajęty.";
                return Page();
            }

            // Aktualizacja osoby
            var appUserToUpdate = await _context.AppUsers.FindAsync(id);

            // Zmiana hasła

            if (ChangePassword)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(appUserToUpdate);
                var result = await _userManager.ResetPasswordAsync(appUserToUpdate, token, Input.Password);
                if (result.Succeeded) { }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // Aktualizacja pozostałych danych osoby

            if (await TryUpdateModelAsync<ApplicationUser>(
                appUserToUpdate,
                "ApplicationUser",   
                 s => s.Number, s => s.FirstName, s => s.LastName, s => s.Employee, s => s.Email, s => s.PhoneNumber, s => s.Note))
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

                FirstEmployment.Id = appUserToUpdate.Id; // przekazanie ID z osoby do zatrudnienia

                // TODO Nie aktualizuje position employment
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

                    SecondEmployment.Id = appUserToUpdate.Id; // przekazanie ID z osoby do zatrudnienia

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
