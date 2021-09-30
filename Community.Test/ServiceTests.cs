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
        private IUserService userservice;
        private IBusinessService businessService;

        public ServiceTests()
        {
            userservice = new UserServiceDb(new DatabaseContext());
            businessService = new BusinessServiceDb( new DatabaseContext());
            userservice.Initialise();
           
        }

        [Fact]
        public void EmptyDbShouldReturnNoUsers()
        {
            // act
            var users = userservice.GetUsers();

            // assert
            Assert.Equal(0, users.Count);
        }
        
        [Fact]
        public void AddingUsersShouldWork()
        {
            // arrange
            userservice.AddUser("admin", "admin@mail.com", 35, "male", 2, "admin", Role.Admin );
            userservice.AddUser("guest", "guest@mail.com", 63, "female", 3,"guest", Role.Guest);

            // act
            var users = userservice.GetUsers();

            // assert
            Assert.Equal(2, users.Count);
        }

        [Fact]
        public void UpdatingUserShouldWork()
        {
            // arrange
            var user = userservice.AddUser("admin", "admin@mail.com", 52, "female", 3, "admin", Role.Admin );
            
            // act
            user.Name = "administrator";
            user.Email = "admin@mail.com";            
            var updatedUser = userservice.UpdateUser(user);

            // assert
            Assert.Equal("administrator", user.Name);
            Assert.Equal("admin@mail.com", user.Email);
        }

        [Fact]
        public void LoginWithValidCredentialsShouldWork()
        {
            // arrange
            userservice.AddUser("admin", "admin@mail.com", 34, "female", 2, "admin", Role.Admin );
            
            // act            
            var user = userservice.Authenticate("admin@mail.com","admin");

            // assert
            Assert.NotNull(user);
           
        }

        [Fact]
        public void LoginWithInvalidCredentialsShouldNotWork()
        {
            // arrange
            userservice.AddUser("admin", "admin@mail.com", 25,"male", 1, "admin", Role.Admin );

            // act      
            var user = userservice.Authenticate("admin@mail.com","xxx");

            // assert
            Assert.Null(user);
           
        }

        //============================Business Service Tests=======================================================

        [Fact]
        public void GetAllBusiness_WhenNone_ShouldReturn0()
        {
            
            var u = userservice.AddUser("CAx", "me@mail.com", 21, "male", 1, "pwwww", Role.Admin);
            // act 
            var b = businessService.GetAllBusiness(u);
            var count = b.Count;

            // assert
            Assert.Equal(0, count);
        }

        [Fact]
        public void GetAllBusiness_WhenOne_ShouldReturn1()
        {
            //arrenge
            var bb = new Business{
            Title = "test title",
            Type = "test type",
            Address = "Test address",
            Description = "Test",
            CommunityId =1,
            PosterUrl = "http://photo.com",
            };
            businessService.AddBusiness(bb);
            
            var u = userservice.AddUser("CAx", "me@mail.com", 21, "male", 1, "pwwww", Role.Admin);
            //act

            var b = businessService.GetAllBusiness(u);
            var count = b.Count;

            // assert
            Assert.Equal(1, count);
        }

        [Fact]
        public void Business_AddBusiness_WhenUnique_ShouldSetAllProperties() {
            //arrange create test movie
            var b = new Business{
                Id = 12345234,
                Title = "test title",
                Type = "test type",
                Address = "Test address",
                Description = "Test",
                CommunityId =1,
                PosterUrl = "http://photo.com",
            };
            businessService.AddBusiness(b);
            
            //Assert not null
            Assert.NotNull(b);
            //Assert properties have been set
            Assert.Equal(12345234, b.Id);  
            Assert.Equal("test title", b.Title);
            Assert.Equal("test type", b.Type);
            Assert.Equal("Test address", b.Address);
            Assert.Equal("Test", b.Description);
            Assert.Equal(1, b.CommunityId);
            Assert.Equal("http://photo.com", b.PosterUrl);
        }

        [Fact]
        public void Business_UpdateWhenExists_ShouldUpdateAllProperties()
        {
            //arrange - create a test Business
            var b = new Business{
                Id = 12345234,
                Title = "test title",
                Type = "test type",
                Address = "Test address",
                Description = "Test",
                CommunityId =1,
                PosterUrl = "http://photo.com",
            };

            //act - update the Business
            b.Id = 12;
            b.Title = "New test title";
            b.Type = "New test type";
            b.Address = "new Test address";
            b.Description = "new Test";
            b.CommunityId =2;
            PosterUrl = "http://photo1.com";
            svc.UpdateMovie(b);

            //Assert all properties have been updates
            Assert.Equal(12, m.Id);  
            Assert.Equal("New test title", m.Title);
            Assert.Equal("New Test Director", m.Director);
            Assert.Equal(1996, m.Year);
            Assert.Equal(46, m.Duration);
            Assert.Equal(235, m.Budget);
            Assert.Equal("http://newphoto.com", m.PosterUrl);
            Assert.Equal(Genre.SciFi, m.Genre);
            Assert.Equal("New Test Actor1, Test Actor2", m.Cast);
            Assert.Equal("New Test plot", m.Plot);
        }

    }
}
