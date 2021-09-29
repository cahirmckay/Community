using System;
using Xunit;
using Community.Core.Models;
using Community.Core.Security;
using Community.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Community.Data.Services;

namespace Community.Test
{
    public class ServiceTests
    {
        private IUserService service;

        public ServiceTests()
        {
            service = new UserServiceDb( 
                  new DatabaseContext()
                );
            service.Initialise();
        }

        [Fact]
        public void EmptyDbShouldReturnNoUsers()
        {
            // act
            var users = service.GetUsers();

            // assert
            Assert.Equal(0, users.Count);
        }
        
        [Fact]
        public void AddingUsersShouldWork()
        {
            // arrange
            service.AddUser("admin", "admin@mail.com", 35, "male", 2, "admin", Role.Admin );
            service.AddUser("guest", "guest@mail.com", 63, "female", 3,"guest", Role.Guest);

            // act
            var users = service.GetUsers();

            // assert
            Assert.Equal(2, users.Count);
        }

        [Fact]
        public void UpdatingUserShouldWork()
        {
            // arrange
            var user = service.AddUser("admin", "admin@mail.com", 52, "female", 3, "admin", Role.Admin );
            
            // act
            user.Name = "administrator";
            user.Email = "admin@mail.com";            
            var updatedUser = service.UpdateUser(user);

            // assert
            Assert.Equal("administrator", user.Name);
            Assert.Equal("admin@mail.com", user.Email);
        }

        [Fact]
        public void LoginWithValidCredentialsShouldWork()
        {
            // arrange
            service.AddUser("admin", "admin@mail.com", 34, "female", 2, "admin", Role.Admin );
            
            // act            
            var user = service.Authenticate("admin@mail.com","admin");

            // assert
            Assert.NotNull(user);
           
        }

        [Fact]
        public void LoginWithInvalidCredentialsShouldNotWork()
        {
            // arrange
            service.AddUser("admin", "admin@mail.com", 25,"male", 1, "admin", Role.Admin );

            // act      
            var user = service.Authenticate("admin@mail.com","xxx");

            // assert
            Assert.Null(user);
           
        }

    }
}
