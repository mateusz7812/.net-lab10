using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace lab10.Models
{
    public class Article
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

        [MaxLength(40)]
        public string ImageName { get; set; }
    }
}
