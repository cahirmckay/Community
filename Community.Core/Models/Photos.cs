using System;
using System.ComponentModel.DataAnnotations;

using System.Text.Json.Serialization;

namespace Community.Core.Models
{
    public class Photo
    {
        public int PhotoId{get; set;}
        
        public string PhotoTitle{get; set;}

        public string PhotoPath{get; set;}

        public string PhotoDescription{get; set;}

        //EF forgien key
        [Required]
        public int CommunityId {get; set;}

    }
}