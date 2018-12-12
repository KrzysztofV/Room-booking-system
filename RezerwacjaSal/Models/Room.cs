using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RezerwacjaSal.Models
{
    public class Room
    {
        public int RoomID { get; set; }

        [Required(ErrorMessage = "Numer pokoju jest wymagany.")]
        public int Number { get; set; }

        [StringLength(50, ErrorMessage = "Opis nie może być dłuższy niż 50 znaków")]
        public string Type { get; set; }

        [StringLength(200, ErrorMessage = "Opis wyposażenia nie może być dłuższy niż 200 znaków.")]
        public string Equipment { get; set; }

        [Range(1, 100000, ErrorMessage = "Tylko liczby.")]
        public int? Spots { get; set; }

        [Required(ErrorMessage = "Wymagany jest budynek.")]
        public int BuildingID { get; set; }

        public Building Building { get; set; }

        public ICollection<Reservation> Reservations { get; set; }
    }
}
