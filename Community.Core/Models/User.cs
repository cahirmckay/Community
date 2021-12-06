using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
namespace Community.Core.Models
{
    
    public enum Role { Admin, Manager, Guest }

    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        public string Gender { get; set;}

        // EF Dependant Relationship User belongs to a Community(foreign key)
        [Required]
        public int CommunityId { get; set; }

        // User role within application
        [Required]
        public Role Role { get; set; }

        // used to store jwt auth token 
        public string Token { get; set; }
        
    }
}
