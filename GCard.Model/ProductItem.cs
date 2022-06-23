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
        public string Description { get; set; }
        public string Image { get; set; }
        [Range(0, Double.PositiveInfinity, ErrorMessage = "Out of range")]
        public decimal Price { get; set; }
        [Range(0, Double.PositiveInfinity, ErrorMessage = "Out of range")]
        [Display(Name = "Wholesale Price")]
        public decimal WholesalePrice { get; set; }

        [Display(Name = "Type")]
        public int ItemTypeId { get; set; }
        //[ValidateNever]
        [ForeignKey(nameof(ItemTypeId))]
        public ItemType ItemType { get; set; }

        [Display(Name = "Occasion/Holiday")]
        public int OccasionId { get; set; }
        //[ValidateNever]
        [ForeignKey(nameof(OccasionId))]
        public Occasion Occasion { get; set; }
    }
}