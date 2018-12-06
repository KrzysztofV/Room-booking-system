using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RezerwacjaSal.Models
{
    public class Message
    {
        [Key]
        public int MessageID { get; set; }

        [EmailAddress(ErrorMessage = "To nie jest poprawny adres e-mail.")]
        public string Email { get; set; }
        public string Name { get; set; }

        [Required(ErrorMessage = "Temat wiadomości jest wymagany.")]
        public string MessageSubject { get; set; }

        [Required(ErrorMessage = "Treść wiadomości jest wymagana.")]
        public string MessageContent { get; set; }
        public string IP { get; set; }
        public DateTime Date { get; set; }
        public string Id { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

    }
}
