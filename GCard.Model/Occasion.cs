using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCard.Model
{
    public class Occasion
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Occasion")]
        public string Name { get; set; }

        [Display(Name = "Display Order")]
        [Range(1, int.MaxValue, ErrorMessage = "positive integers only")]
        public int? DisplayOrder { get; set; }
    }
}
