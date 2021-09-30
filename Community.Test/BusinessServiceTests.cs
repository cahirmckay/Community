// using System;
// using Xunit;
// using Community.Core.Models;
// using Community.Core.Security;
// using Community.Data.Repositories;
// using Microsoft.EntityFrameworkCore;
// using Community.Data.Services;

// namespace Community.Test
// {
//     public class BusinessServiceTests
//     {
//         private IUserService us;
//         private IBusinessService bs;

//         public BusinessServiceTests()
//         {
//             us = new UserServiceDb();
//             bs = new BusinessServiceDb();
//             bs.Initialise();
//         }

//              //============================Business Service Tests=======================================================

//         // [Fact]
//         // public void Business_GetBusinessWhenNone_ShouldReturnNull()
//         // {
//         //     //arrange
//         //     var b = new Business{
            
//         //     Title = "test title",
//         //     Type = "test type",
//         //     Address = "Test address",
//         //     Description = "Test",
//         //     PosterUrl = "http://photo.com",
//         //     };
//         //     bs.AddBusiness(b);

//         //     var ob = bs.GetBusiness(b.Id); 

//         //     //assert
//         //     Assert.Equal(b.Id, ob.Id);
//         // }
//         [Fact]
//         public void GetAllBusiness_WhenNone_ShouldReturn0()
//         {
//             //arrenge
//             var u = new User{
                 
//                  Name= "cax",
//                  Email ="me@mail.com",
//                  Age = 21,
//                  Gender= "MAle",
//                  CommunityId=1,
//                  Password= "heloo",
//                  Role = Role.Admin
//             };
//             us.AddUser("CAx", "me@mail.com", 21, "male", 1, "pwwww", Role.Admin);
//             // act 
//             var movies = bs.GetAllBusiness(u);
//             var count = movies.Count;

//             // assert
//             Assert.Equal(0, count);
//         }

//     }
// }
