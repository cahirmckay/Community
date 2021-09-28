using System;
using System.ComponentModel.DataAnnotations;

using System.Text.Json.Serialization;

namespace Community.Core.Models
{
    public class Review
    {     

        public int Id { get; set; }      

        // name of reviewer
        [Required]
        public string Name { get; set; }   

        // date review was made        
        public DateTime CreatedOn { get; set; }

        // reviewer comments
        //not required as some users would rather simply just leave a rating
        [StringLength(200, MinimumLength = 5)]
        public string Comment { get; set; }

        // value between 1-10
        [Required]
        [Range(0,10)]
        public int Rating { get; set; }
    
        // EF Dependant Relationship Review belongs to a Business(foreign key)
        public int BusinessId { get; set; }

        // Navigation property
        [JsonIgnore]
        public Business Business { get; set; }
 
    }
}