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

        [BindProperty]
        public ApplicationUser ApplicationUser { get; set; }
        public string SortOrderRoute { get; set; }
        public string CurrentFilterRoute { get; set; }
        public string SearchStringRoute { get; set; }
        public int? PageIndexRoute { get; set; }
        public int? PageSizeRoute { get; set; }

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
                .Include(e => e.Department) // wczytuje naviagtion properties z Department
                .AsNoTracking()                 // poprawia wydajność w przypadku gdy wczytane encje nie są modyfikowane w tej stronie
                .FirstOrDefaultAsync(m => m.Id == id);  // dla danego ID



            ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "Name");


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
                 s => s.Employment, s=> s.DepartmentID, s => s.Number, s => s.FirstName, s => s.LastName, s => s.Email, s => s.PhoneNumber, s => s.Note))
            {
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }

        private bool AppUserExists(int id)
        {
            return _context.AppUsers.Any(e => Int32.Parse(e.Id) == id);
        }
    }
}
