using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace lab10.Models
{
    public class ArticleViewModel
    {
        [Key]
        public int ArticleId { get; set; }

        [Required]
        [MaxLength(40)]
        public string Name { get; set; }

        [Required]
        public decimal Money { get; set; }

        [Required]
        public int CategoryId { get; set; }
        
        public Category Category { get; set; }

        [NotMapped]
        public IFormFile Image { get; set; }
    }
}
