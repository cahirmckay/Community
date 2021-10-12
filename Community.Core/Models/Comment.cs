using System;
using System.ComponentModel.DataAnnotations;

using System.Text.Json.Serialization;

namespace Community.Core.Models
{
    public class Comment
    {
        public int CommentId{get; set;}

        public string Description{get; set;}

        public string Name{get; set;}
        
        public DateTime CreatedOn { get; set; }

        //EF forgien key
        [Required]
        public int CommunityId {get; set;}

        public int PostId {get; set;}

        // Navigation property
        [JsonIgnore]
        public Post Post { get; set; }

    }
}