using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCard.Model
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public int ProductItemId { get; set; } //именно нужно, не просто ProductItem.Id
        [ForeignKey("ProductItemId")]
        //[NotMapped] //when u want ur navigation prop-is never to be mapped and populated, Validatenever - never validate
        [ValidateNever]
        public ProductItem ProductItem { get; set; }

        [Range(1, 1000, ErrorMessage = "1-1000 only")]
        public int Count { get; set; }
        public string ApplicationUserId { get; set; } //in ASP Users - id is string so we using string
        [ForeignKey("ApplicationUserId")]
        //[NotMapped]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
