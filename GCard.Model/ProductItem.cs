using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCard.Model
{
    public class ProductItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        [ValidateNever]
        public string? Image { get; set; }

        [Range(0, Double.PositiveInfinity)]
        public decimal? Price { get; set; }

        [Display(Name = "Wholesale Price")]
        [Range(0, Double.PositiveInfinity)]
        public decimal? WholesalePrice { get; set; }

        [Display(Name = "Type")]
        public int ItemTypeId { get; set; }
        [ForeignKey(nameof(ItemTypeId))]
        [ValidateNever]
        public ItemType ItemType { get; set; }

        [Display(Name = "Occasion/Holiday")]
        public int? OccasionId { get; set; }
        [ForeignKey(nameof(OccasionId))]
        [ValidateNever]
        public Occasion? Occasion { get; set; }
    }
}