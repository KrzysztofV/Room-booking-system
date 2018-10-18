
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RezerwacjaSal.Models
{
    public class Reservation
    {
        public int ReservationID { get; set; }

        [Required(ErrorMessage = "Wymagany jest pokój.")]
        public int RoomID { get; set; }

        [Required(ErrorMessage = "Wymagana jest osoba rezerwująca.")]
        public int PearsonID { get; set; }

        [Required(ErrorMessage = "Data rezerwacji jest wymagana.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Czas rozpoczęcia rezerwacji jest wymagany.")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "Czas zakończenia rezerwacji jest wymagany.")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime EndTime { get; set; }

        [StringLength(200, ErrorMessage = "Notatka nie może być dłuższa niż 200 znaków.")]
        public string Note { get; set; }

        public Pearson Pearson { get; set; }
        public Room Room { get; set; }

    }
}
