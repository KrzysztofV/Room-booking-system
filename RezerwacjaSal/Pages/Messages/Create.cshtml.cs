using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RezerwacjaSal.Data;
using RezerwacjaSal.Models;


namespace RezerwacjaSal.Pages.Messages
{
    public class CreateModel : PageModel
    {
        private readonly RezerwacjaSal.Data.RezerwacjaSalContext _context;

        public CreateModel(RezerwacjaSal.Data.RezerwacjaSalContext context)
        {
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
        public string EmailSendError { get; private set; }
        public string EmailSendStatus { get; private set; }

        private List<int> AllNumbers { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            AllNumbers = await _context.AppUsers
                    .Select(i => i.Number)
                    .ToListAsync();

            var newMessage = new Message();

            DateTime Now = DateTime.Now;
            newMessage.Date = Now;

            if (Number != 0)
                if (AllNumbers.Contains(Number))
                {
                    newMessage.Id = await _context.AppUsers
                        .Where(r => r.Number == Number)
                        .Select(r => Int32.Parse(r.Id))
                        .FirstOrDefaultAsync();

                    newMessage.Email = await _context.AppUsers
                        .Where(r => r.Number == Number)
                        .Select(r => r.Email)
                        .FirstOrDefaultAsync();

                    if(newMessage.Email==null)
                    {
                        EmailNotFoundError = "Ta osoba nie ma zapisanego emaila.";
                        return Page();
                    }
                }
                else
                {
                    AppUserNotFoundError = "Nie znaleziono takiej osoby.";
                    return Page();
                }

            if (await TryUpdateModelAsync<Message>(
                newMessage,
                "Message", 
                 s => s.Name, s => s.MessageSubject, s => s.MessageContent, s => s.IP))
            {
                _context.Messages.Add(newMessage);
                await _context.SaveChangesAsync();
            }

            if (Number != 0)
                Message.Email = await _context.AppUsers.Where(r => r.Number == Number)
                    .Select(r => r.Email)
                    .FirstOrDefaultAsync();

            if (Message.Email != null )
            {
                var EmailPassword = "Plazmotronix";
                var EmailLogin = "webapp0@outlook.com";

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp-mail.outlook.com";
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(EmailLogin, EmailPassword);
                smtp.EnableSsl = true;

                MailMessage mailMessage = new MailMessage();
                mailMessage.To.Add(Message.Email);
                
                var Subject = "Potwierdzenie wysłania wiadomości - system rezerwacji sal - ";
                Subject = Subject + Message.MessageSubject;
                mailMessage.Subject = Subject;

                var Body = Message.MessageContent;
                Body = Body + "\n \n \n Wiadomość wygenerowana automatycznie. Prosimy na nią nie odpowiadać.";
                Body = Body + "\n System rezerwacji sal - Bulbulatron 2000.";
                mailMessage.Body = Body;

                mailMessage.From = new MailAddress(EmailLogin);
                try
                {
                    smtp.Send(mailMessage);
                    EmailSendStatus = "Wiadomość została zapisana do systemu. Na twój email zostało wysłane potwierdzenie.";
                    return Page();
                }
                catch
                {
                    EmailSendError = "Coś poszło nie tak z wysłaniem emaila ale wiadomość została zapisana do systemu.";
                    return Page();
                }
            }

            EmailSendStatus = "Wiadomość została zapisana do systemu.";
            return Page();
        }
    }
}