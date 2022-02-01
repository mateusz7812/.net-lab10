using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace lab10.Models
{
    public class OrderAddress
    {
        [Required]
        [MaxLength(10)]
        public string HouseNumber { get; set; }

        [Required]
        [MaxLength(40)]
        public string Street { get; set; }

        [Required]
        [MaxLength(30)]
        public string City { get; set; }

        [Required]
        [MaxLength(10)]
        public string Postcode { get; set; }
    }
}