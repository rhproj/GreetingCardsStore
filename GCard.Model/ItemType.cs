using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCard.Model
{
    public class ItemType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Type")]
        public string Name { get; set; }

        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; }
    }
}
