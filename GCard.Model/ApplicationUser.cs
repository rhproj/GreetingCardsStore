using Microsoft.AspNetCore.Identity;
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
    public class ApplicationUser : IdentityUser //that way Appl-nUser has all the props of default IdentityUser used in Authentification
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? PostalCode { get; set; }
    }
}
