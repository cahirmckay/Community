using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Community.Data.Validators;

namespace Community.Core.Models
{ 
    public class Location
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description {get; set;}
        
    }
}