using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCard.Model.ViewModels
{
    public class ProductItemVM
    {
        public ProductItem ProductItem { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> ItemTypeList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> OccasionList { get; set; }
    }
}
