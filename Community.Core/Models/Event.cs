using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using Community.Data.Validators;

using System.Text.Json.Serialization;

namespace Community.Core.Models
{
    public enum Status
    {
        Confirmed, Unconfirmed
    }
    
    public class Event
    {
        [Required]
        public int Id { get; set; }
        
        // name of venue
        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime StartTime{get; set;}

        [Required]
        public DateTime EndTime{get; set;}

        public DateTime CreatedOn {get; set;}

        public Status Status{get; set;}

        //EF foreign Key
        [Required]
        public int VenueId {get; set;}


        // Navigation property
        [JsonIgnore]
        public Venue Venue { get; set; }
    }
}