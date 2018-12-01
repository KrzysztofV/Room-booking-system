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
            RoleManager<IdentityRole> roleManager,
            ILogger<RegisterModel> logger)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        [BindProperty]
        public ApplicationUser ApplicationUser { get; set; }

        public string SortOrderRoute { get; set; }
        public string CurrentFilterRoute { get; set; }
        public string SearchStringRoute { get; set; }
        public int? PageIndexRoute { get; set; }
        public int? PageSizeRoute { get; set; }

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
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

            [Required]
            public string RoleName { get; set; }
        }

        public async Task<IActionResult> OnGet(string id, string sortOrder, string currentFilter, string searchString, int? pageIndex, int? pageSize)
        {
            // Przekazanie parametrów URL
            SortOrderRoute = sortOrder;
            CurrentFilterRoute = currentFilter;
            SearchStringRoute = searchString;
            PageIndexRoute = pageIndex;
            PageSizeRoute = pageSize;

            // Wczytanie danego użytkownika
            ApplicationUser = await _context.AppUsers
                .Include(e => e.Department)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            // Nie ma takiego użytkownika pod danym Id
            if (ApplicationUser == null)
                return NotFound();

            // Listy wyborów
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "Name");
            var userRoles = await _userManager.GetRolesAsync(ApplicationUser);
            ViewData["RoleNames"] = new SelectList(_roleManager.Roles, "Name", "Name", userRoles.First());
            return Page();
        }


        public async Task<IActionResult> OnPostAsync(string id, string sortOrder, string currentFilter, string searchString, int? pageIndex, int? pageSize)
        {
            // Przekazanie parametrów URL
            SortOrderRoute = sortOrder;
            CurrentFilterRoute = currentFilter;
            SearchStringRoute = searchString;
            PageIndexRoute = pageIndex;
            PageSizeRoute = pageSize;

            // Aktualizacja osoby
            var appUserToUpdate = await _context.AppUsers.FindAsync(id);

            // Zmiana hasła
            if (ChangePassword)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(appUserToUpdate);
                var updatePasswordResult = await _userManager.ResetPasswordAsync(appUserToUpdate, token, Input.Password);
                if (updatePasswordResult.Succeeded) _logger.LogInformation("Zaktualizowano hasło");
                foreach (var error in updatePasswordResult.Errors) ModelState.AddModelError(string.Empty, error.Description);
            }

                var removeReoleResult = await _userManager.RemoveFromRolesAsync(appUserToUpdate, await _userManager.GetRolesAsync(appUserToUpdate));
                var updateReoleResult = await _userManager.AddToRoleAsync(appUserToUpdate, Input.RoleName);
                if (updateReoleResult.Succeeded && removeReoleResult.Succeeded) _logger.LogInformation("Zaktualizowano rolę");
                foreach (var error in updateReoleResult.Errors) ModelState.AddModelError(string.Empty, error.Description);


            // Aktualizacja pozostałych danych osoby
            if (await TryUpdateModelAsync<ApplicationUser>(
                appUserToUpdate,
                "ApplicationUser",   
                 s => s.Employment, 
                 s=> s.DepartmentID, 
                 s => s.FirstName, 
                 s => s.LastName, 
                 s => s.Email, 
                 s => s.PhoneNumber, 
                 s => s.Note))
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
