using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace API.FurnitureStore.Shared
{
    public class Client
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [MinLength(3, ErrorMessage = "Name FirstName must be at least 3 characters long")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [MinLength(3, ErrorMessage = "Name LastName must be at least 3 characters long")]
        public string LastName { get; set; }

        public DateTime? BirthDate { get; set; }

        [StringLength(20)]
        [MinLength(8, ErrorMessage = "Name Phone must be at least 8 characters long")]
        public string? Phone { get; set; }

        [StringLength(250)]
        public string? Address { get; set; }
    }
}
