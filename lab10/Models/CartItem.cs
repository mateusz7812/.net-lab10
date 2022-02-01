using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace lab10.Models
{
    public class CartItem
    {
        public Article Article { get; set; }
        public int Quantity { get; set; }
    }
}