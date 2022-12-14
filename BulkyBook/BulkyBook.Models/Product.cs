using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        public string Author { get; set; }

        [Required]
        [Display(Name = "List Price")]
        [Range(1, 10000)]
        public decimal ListPrice { get; set; }

        [Required]
        [Display(Name = "Price for 1~50")]
        [Range(1, 10000)]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Price for 51~100")]
        [Range(1, 10000)]
        public decimal Price50 { get; set; }

        [Required]
        [Display(Name = "Price for 100+")]
        [Range(1, 10000)]
        public decimal Price100 { get; set; }

        [ValidateNever]
        public string ImageUrl { get; set; }
        
        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]//可加可不加，EF會自行判斷帶有Id屬性的FK
        [ValidateNever]
        public Category Category { get; set; }
       
        [Required]
        [Display(Name = "Cover Type")]
        public int CoverTypeId { get; set; }
        [ForeignKey("CoverTypeId ")]
        [ValidateNever]
        public CoverType CoverType { get; set; }
    }
}
