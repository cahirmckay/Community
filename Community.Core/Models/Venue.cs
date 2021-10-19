using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using Community.Data.Validators;

namespace Community.Core.Models
{
    
    public class Venue
    {
        public int Id { get; set; }
        
        // name of venue
        [Required]
        public string Name { get; set; }

        [Required]        
        public string Address { get; set; }

        public string Description { get; set; }

        public int SocialDistance {get; set;}

        public int OriginalCapacity {get; set;}

        public int Capacity 
        {
            get
            {
                const int constant = 2;
                int socialDistance = SocialDistance * constant;
                
                int cap = OriginalCapacity;

                var newCap =  cap/socialDistance;
                if(newCap==0)
                {
                    newCap = OriginalCapacity;
                }

                return newCap;
            }
        }

        //EF forgien key
        [Required]
        public int CommunityId {get; set;}

        // EF Relationship - a venue can have many event
        public IList<Event> Events { get; set; } = new List<Event>();
    }
}