using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using Community.Data.Validators;

namespace Community.Core.Models
{
    
    public class NewsArticle
    {
        public int Id { get; set; }
        
        // name of news article
        [Required]
        public string Title { get; set; }

        // type of business
        [Required]        
        public string Type { get; set; }

        [Required]        
        public string Author { get; set; }

        public DateTime CreatedOn { get; set; }

        // article text
        [Required]
        [StringLength(1000, MinimumLength = 10)]
        public string Newstext { get; set; }

        //EF forgien key
        [Required]
        public int CommunityId {get; set;}

    }
}