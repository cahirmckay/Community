using System;
using System.ComponentModel.DataAnnotations;

using System.Text.Json.Serialization;
using System.Collections.Generic;


using Community.Data.Validators;
namespace Community.Core.Models
{
    public enum PostType
    {
        General, Help, Resolved
    }

    public class Post
    {
        
        public string Name{get; set;}
        
        public DateTime CreatedOn { get; set; }

        public int Id{get; set;}
        
        [Required]
        public string PostText{get; set;}

        [Required]
        public PostType PostType{get; set;}

        //EF forgien key
        
        public int CommunityId {get; set;}

        // EF Relationship - a Post can have many Comments
        
        public IList<Comment> Comments { get; set; } = new List<Comment>();
        

    }
}