using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Community.Data.Validators;

namespace Community.Core.Models
{
    public enum IssueType
    {
        Litter, Traffic, RoadWorks, Pothole, 
    }
    public class Issue
    {
        public int Id {get; set;}

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        // the general description of the issues (up to 50 chars)
        [Required]
        [StringLength(50, MinimumLength = 10)]
        public string Description { get; set; }

        [Required]
        public IssueType IssueType {get; set;}


    }
}


         