using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace lab10.Models
{
    public class OrderPerson
    {
        [Required]
        [MaxLength(20)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(30)]
        public string LastName { get; set; }
    }
}
