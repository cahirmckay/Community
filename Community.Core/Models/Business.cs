using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Community.Data.Validators;

namespace Community.Core.Models
{
    
    public class Business
    {
                
        public int Id { get; set; }
        
        // name of buiness
        [Required]
        public string Title { get; set; }

        // type of business
        [Required]        
        public string Type { get; set; }

        [Required]        
        public string Address { get; set; }

        // the general description of the business (up to 500 chars)
        [Required]
        [StringLength(500, MinimumLength = 10)]
        public string Description { get; set; }

        [UrlResource]
        public string PosterUrl { get; set; } 

        //EF forgien key
        [Required]
        public int CommunityId {get; set;}
        
        // ReadOnly Property - Calculates Rating % based on average of all reviews
        public int Rating
        {
            get
            {
                var count = Reviews.Count > 0 ? Reviews.Count : 1;
                return Reviews.AsEnumerable().Sum(r => r.Rating) / count * 10;
            }
        }

        // EF Relationship - a Business can have many reviews 
        public IList<Review> Reviews { get; set; } = new List<Review>();

    }
}