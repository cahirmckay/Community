using System;
using System.ComponentModel.DataAnnotations;

using System.Text.Json.Serialization;

namespace Community.Core.Models
{
    public class Photo
    {
        public int PhotoId{get; set;}
        
        [Required]
        public string PhotoTitle{get; set;}

        public byte[] PhotoData {get; set;}

        public string PhotoDataUrl{get; set;}

        //EF forgien key
        [Required]
        public int CommunityId {get; set;}

    }
}