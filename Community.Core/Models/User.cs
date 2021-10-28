using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
namespace Community.Core.Models
{
    // Add User roles relevant to your application
    public enum Role { Admin, Manager, Guest }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Age { get; set; }
        public string Gender { get; set;}

        // EF Dependant Relationship User belongs to a Community(foreign key)
        public int CommunityId { get; set; }

        // User role within application
        public Role Role { get; set; }

        // used to store jwt auth token 
        public string Token { get; set; }

        // Navigation property
        [JsonIgnore]
        public Location Location { get; set; }
    }
}
