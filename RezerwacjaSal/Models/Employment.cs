using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RezerwacjaSal.Models
{
    // encja zatrudnienie - tabela pośrednia w relacji wiele do wielu pomiędzy Department<->ApplicationUser
    public class Employment
    {
        // EF samo generuje sobie liczbę ID - nie muszę jej znać ani ustawiać 
        [Key]
        public int EmploymentID { get; set; }
        
        public string Id { get; set; } 
        public int? DepartmentID { get; set; }

        [StringLength(50, ErrorMessage = "Opis pozycji nie może być dłuższy niż 50 znaków.")]
        public string Position { get; set; }

        // property jest rozpoznawane jako klucz obcy kiedy się nazywa <navigation property name><primary key property name>
        public ApplicationUser ApplicationUser { get; set; }
        public Department Department { get; set; }

    }
}
