﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BulkyBookweb.Models
{
    public class Category
    {
        [Key]
        public int id { get; set; }
        [Required(ErrorMessage ="Please Enter Name")]
        public string Name { get; set; }
        [DisplayName("Display Order")]
        [Required(ErrorMessage = "Please Enter Display Ordder")]
        [Range(1,100,ErrorMessage ="Display Ordder must be between 1 and 100 only!!")]
        public int DisplayOrder { get; set; }

        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
