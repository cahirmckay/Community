using System;
using System.ComponentModel.DataAnnotations;

using System.Text.Json.Serialization;

namespace Community.Core.Models
{
    public class Comment
    {
        public int CommentId{get; set;}

        // Text body of comment  (up to 200 chars)
        [Required]
        [StringLength(200, MinimumLength = 5)]
        public string Description{get; set;}

        [Required]
        public string Name{get; set;}
        
        public DateTime CreatedOn { get; set; }

        //EF forgien key
        [Required]
        public int PostId {get; set;}

        // Navigation property
        [JsonIgnore]
        public Post Post { get; set; }

    }
}