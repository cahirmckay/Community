using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Community.Core.Models;

namespace Community.Web.ViewModels
{
    public class UserManageViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
 
        [Required]
        [EmailAddress]
        [Remote(action: "GetUserByEmailAddress", controller: "User")]
        public string Email { get; set; }

        [Required]
        public int Age { get; set;}

        [Required]
        public string Gender { get; set;}

        [Required]
        public int CommunityId { get; set;}

        [Required]
        public Role Role { get; set; }

    }
}