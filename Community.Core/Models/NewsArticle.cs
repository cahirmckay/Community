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
        public string Headline { get; set; }

        //link to external article
        [Required]        
        public string Source { get; set; }

        public DateTime CreatedOn { get; set; }

        [UrlResource]
        public string ArticleUrl { get; set;}
        //EF forgien key
        [Required]
        public int CommunityId {get; set;}

    }
}