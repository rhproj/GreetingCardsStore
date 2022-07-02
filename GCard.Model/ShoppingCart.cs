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
        public int ProductItemId { get; set; }
        [ForeignKey("ProductItemId")]
        [ValidateNever] //[NotMapped] //when u want ur navigation prop-is never to be mapped and populated, Validatenever - never validate
        public ProductItem ProductItem { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Positive numbers only")]
        public int Count { get; set; }

        public string ApplicationUserId { get; set; } //in ASP Users - id is string so we using string
        [ForeignKey("ApplicationUserId")]
        [ValidateNever] //[NotMapped] //- используется в ситуациях когда мы не хотим, any navigation property got populated
        public ApplicationUser ApplicationUser { get; set; }
    }
}
