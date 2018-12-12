using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RezerwacjaSal.Data;
using RezerwacjaSal.Models;


namespace RezerwacjaSal.Pages.Messages
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly RezerwacjaSalContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        public CreateModel(
            RezerwacjaSal.Data.RezerwacjaSalContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Message Message { get; set; }

        [BindProperty]
        public int Number { get; set; }
        public string AppUserNotFoundError { get; private set; }
        public string EmailNotFoundError { get; private set; }
        public string Info { get; private set; }

        private List<int> AllNumbers { get; set; }
        public ApplicationUser CurrentUser { get; private set; }

        public async Task<IActionResult> OnPostAsync()
        {
            CurrentUser = await _userManager.GetUserAsync(base.User);
            if (CurrentUser == null)
            {
                return base.NotFound($"Unable to load user with ID '{_userManager.GetUserId(base.User)}'.");
            }

            if (!ModelState.IsValid)
                return Page();

            AllNumbers = await _context.AppUsers
                    .Select(i => i.Number)
                    .ToListAsync();

            var newMessage = new Message();


            newMessage.Name = CurrentUser.FirstName + " " + CurrentUser.LastName;
            newMessage.Date = DateTime.Now;
            newMessage.Id = CurrentUser.Id;
            newMessage.Email = CurrentUser.Email;

            if (await TryUpdateModelAsync<Message>(
                newMessage,
                "Message", 
                 s => s.MessageSubject, s => s.MessageContent, s => s.IP))
            {
                _context.Messages.Add(newMessage);
                await _context.SaveChangesAsync();
            }

            var userMassage = newMessage.MessageContent;
            userMassage = userMassage + "\n \n \n Wiadomość wygenerowana automatycznie. Prosimy na nią nie odpowiadać.";
            userMassage = userMassage + "\n System rezerwacji sal - Bulbulatron 3000.";

            var userSubject = " - Potwierdzenie wysłania wiadomości";
            userSubject = newMessage.MessageSubject + userSubject;

            if (newMessage.Email != null )
            {
                await _emailSender.SendEmailAsync(newMessage.Email, userSubject, userMassage);

                var adminSubject = newMessage.Name + " - Wysyła: " + newMessage.MessageSubject;
                var adminMassage = "Email: " + newMessage.Email + "\n " + "Imię i nazwisko: " + newMessage.Name + "\n " + "Numer użytkownika: " +  CurrentUser.Number + "\n " + "Treść wiadomości: " + newMessage.MessageContent;
                await _emailSender.SendEmailAsync("webapp0@outlook.com", adminSubject, adminMassage);
            }

            Info = "Wiadomość wysłano.";

            return Page();
        }
    }
}