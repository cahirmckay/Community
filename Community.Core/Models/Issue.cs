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

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Description { get; set; }

        public IssueType IssueType {get; set;}


    }
}


         