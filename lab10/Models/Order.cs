using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace lab10.Models
{
    public class Order
    {
        public List<CartItem> CartItems { get; set; }
        
        [Required]
        public OrderPerson Person { get; set; }

        [Required]
        public OrderAddress Address { get; set; }

        [Required]
        public int PaymentMethod { get; set; }
    }
}