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
        private IPhotoService photoService;
        private IPostService postService;
        private INewsService newsService;

        public ServiceTests()
        {
            userservice = new UserServiceDb(new DatabaseContext());
            businessService = new BusinessServiceDb(new DatabaseContext());
            photoService = new PhotoServiceDb(new DatabaseContext());
            postService = new PostServiceDb(new DatabaseContext());
            newsService = new NewsServiceDb(new DatabaseContext());
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
            userservice.AddUser("admin", "admin@mail.com", 35, "male", 2, "admin", Role.Admin);
            userservice.AddUser("guest", "guest@mail.com", 63, "female", 3, "guest", Role.Guest);

            // act
            var users = userservice.GetUsers();

            // assert
            Assert.Equal(2, users.Count);
        }

        [Fact]
        public void UpdatingUserShouldWork()
        {
            // arrange
            var user = userservice.AddUser("admin", "admin@mail.com", 52, "female", 3, "admin", Role.Admin);

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
            userservice.AddUser("admin", "admin@mail.com", 34, "female", 2, "admin", Role.Admin);

            // act            
            var user = userservice.Authenticate("admin@mail.com", "admin");

            // assert
            Assert.NotNull(user);

        }

        [Fact]
        public void LoginWithInvalidCredentialsShouldNotWork()
        {
            // arrange
            userservice.AddUser("admin", "admin@mail.com", 25, "male", 1, "admin", Role.Admin);

            // act      
            var user = userservice.Authenticate("admin@mail.com", "xxx");

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
            var bb = new Business
            {
                Title = "test title",
                Type = "test type",
                Address = "Test address",
                Description = "Test",
                CommunityId = 1,
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
        public void Business_AddBusiness_WhenUnique_ShouldSetAllProperties()
        {
            //arrange create test business
            var b = new Business
            {
                Id = 12345234,
                Title = "test title",
                Type = "test type",
                Address = "Test address",
                Description = "Test",
                CommunityId = 1,
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
            var b = new Business
            {
                Id = 12345234,
                Title = "test title",
                Type = "test type",
                Address = "Test address",
                Description = "Test",
                CommunityId = 1,
                PosterUrl = "http://photo.com",
            };

            //act - update the Business
            b.Id = 12;
            b.Title = "New test title";
            b.Type = "New test type";
            b.Address = "new Test address";
            b.Description = "new Test";
            b.CommunityId = 2;
            b.PosterUrl = "http://photo1.com";
            businessService.UpdateBusiness(b);

            //Assert all properties have been updates
            Assert.Equal(12, b.Id);
            Assert.Equal("New test title", b.Title);
            Assert.Equal("New test type", b.Type);
            Assert.Equal("new Test address", b.Address);
            Assert.Equal("new Test", b.Description);
            Assert.Equal(2, b.CommunityId);
            Assert.Equal("http://photo1.com", b.PosterUrl);

        }

        [Fact]
        public void Business_DeleteBusiness_WhenExists_ShouldReturnTrue()
        {
            //arrange - create a test Business
            var b = new Business
            {
                Id = 12345234,
                Title = "test title",
                Type = "test type",
                Address = "Test address",
                Description = "Test",
                CommunityId = 1,
                PosterUrl = "http://photo.com",
            };

            businessService.AddBusiness(b);

            //act delete business
            var deleted = businessService.DeleteBusiness(b.Id);

            //assert- business is deleted
            Assert.True(deleted);
        }

        [Fact]
        public void Business_GetBusinessByIdThatExists_ShouldReturnBusiness()
        {
            //arrange - create a test Business
            var b = new Business
            {
                Id = 12345234,
                Title = "test title",
                Type = "test type",
                Address = "Test address",
                Description = "Test",
                CommunityId = 1,
                PosterUrl = "http://photo.com",
            };
            businessService.AddBusiness(b);

            //Act- Get business by Id
            var v2 = businessService.GetBusiness(b.Id);

            //Assert equal-
            Assert.Equal(v2, b);
        }

        [Fact]
        public void Business_DeleteBusinessThatDoesntExist_ShouldReturnFalse()
        {
            //act delete business(bool) with Id of 0(doesnt exist)
            var deleted = businessService.DeleteBusiness(0);

            //Assert deleted returns false
            Assert.False(deleted);
        }

        [Fact]
        public void Business_AddBusiness_WhenCommunityIdMatchesUsersCommunityId_ShouldAddOneToBusinessCount()
        {
            //arrange - create a test Business and user
            var b = new Business
            {
                Id = 12345234,
                Title = "test title",
                Type = "test type",
                Address = "Test address",
                Description = "Test",
                CommunityId = 1,
                PosterUrl = "http://photo.com",
            };
            businessService.AddBusiness(b);
            var user = userservice.AddUser("guest", "guest@mail.com", 63, "female", 1, "guest", Role.Guest);

            //Act get businesses
            var businesses = businessService.GetAllBusiness(user);

            //Assert count is plus one
            Assert.Equal(1, businesses.Count);
        }

        [Fact]
        public void Business_AddBusiness_WhenCommunityIdDoesntMatchUsersCommunityId_ShouldntAddToBusinessCount()
        {
            //arrange - create a test Business and user
            var b = new Business
            {
                Id = 12345234,
                Title = "test title",
                Type = "test type",
                Address = "Test address",
                Description = "Test",
                CommunityId = 3,
                PosterUrl = "http://photo.com",
            };
            businessService.AddBusiness(b);
            var user = userservice.AddUser("guest", "guest@mail.com", 63, "female", 1, "guest", Role.Guest);

            //Act get businesses
            var businesses = businessService.GetAllBusiness(user);

            //Assert added business is not returned for this user
            Assert.Equal(0, businesses.Count);
            //Assert business is not null
            Assert.NotNull(b);

        }

        [Fact]
        public void Review_AddReview_ShouldAddOneToReviewCount()
        {
            //Arrange- add a test business and review
            var b = new Business
            {
                Id = 1,
                Title = "test title",
                Type = "test type",
                Address = "Test address",
                Description = "Test",
                CommunityId = 3,
                PosterUrl = "http://photo.com",
            };
            businessService.AddBusiness(b);

            var r1 = new Review
            {
                Name = "Cara",
                Comment = "Cheap convience store with friendly staff",
                BusinessId = 1,
                Rating = 8,
            };
            businessService.AddReview(r1);

            var business = businessService.GetBusiness(b.Id);

            //Assert Review was added
            Assert.Equal(1, business.Reviews.Count);
        }

        [Fact]
        public void Review_DeleteReview_WhenTwoExist_ShouldReturnOne()
        {
            //Arrenge- add a test business and review
            var b = new Business
            {
                Id = 123,
                Title = "test title",
                Type = "test type",
                Address = "Test address",
                Description = "Test",
                CommunityId = 3,
                PosterUrl = "http://photo.com",
            };
            businessService.AddBusiness(b);

            var r1 = new Review
            {
                Id = 1,
                Name = "Cara",
                Comment = "Cheap convience store with friendly staff",
                BusinessId = 123,
                Rating = 8,
            };
            businessService.AddReview(r1);
            var r2 = new Review
            {
                Id = 2,
                Name = "Cahir",
                Comment = "great shop",
                BusinessId = 123,
                Rating = 6,
            };
            businessService.AddReview(r2);

            //Act delete one review
            businessService.DeleteReview(r1.Id);

            var business = businessService.GetBusiness(b.Id);

            //Assert review was deleted
            Assert.Equal(1, business.Reviews.Count);
        }

        [Fact]
        public void Review_WhenAddingTwoRatings_ShouldReturnRatingAverageOutof100()
        {
            //Arrange- add a test business and review
            var b = new Business
            {
                Id = 1,
                Title = "test title",
                Type = "test type",
                Address = "Test address",
                Description = "Test",
                CommunityId = 3,
                PosterUrl = "http://photo.com",
            };
            businessService.AddBusiness(b);

            var r1 = new Review
            {
                Name = "Cara",
                Comment = "Cheap convience store with friendly staff",
                BusinessId = 1,
                Rating = 8,
            };
            businessService.AddReview(r1);

            var r2 = new Review
            {
                Name = "Cahir",
                Comment = "great shop",
                BusinessId = 1,
                Rating = 6,
            };
            businessService.AddReview(r2);

            var business = businessService.GetBusiness(b.Id);

            //Assert rating is average of both review
            Assert.Equal(70, business.Rating);
        }

        //============MyPhotos Tests==============================
        [Fact]
        public void GetAllPhotos_WhenOne_ShouldReturn1()
        {
            //arrenge
            var photo = new Photo
            {
                PhotoTitle = "test title",
                Description = "Test",
                CommunityId = 1,
                //PhotoData=  { 0, 100, 120, 210, 255}
            };
            photoService.AddPhoto(photo);

            var u = userservice.AddUser("CAx", "me@mail.com", 21, "male", 1, "pwwww", Role.Admin);
            //act

            var p = photoService.GetAllPhotos(u);
            var count = p.Count;

            // assert
            Assert.Equal(1, count);
        }

        [Fact]
        public void DeleteOnePhoto_WhenOne_ShouldReturn1()
        {
            //arrenge
            var photo = new Photo
            {
                PhotoTitle = "test title",
                Description = "Test",
                CommunityId = 1,
                //PhotoData=  { 0, 100, 120, 210, 255}
            };
            photoService.AddPhoto(photo);

            var photo1 = new Photo
            {
                PhotoTitle = "test title",
                Description = "Test",
                CommunityId = 1,
                //PhotoData=  { 0, 100, 120, 210, 255}
            };
            photoService.AddPhoto(photo1);

            var u = userservice.AddUser("CAx", "me@mail.com", 21, "male", 1, "pwwww", Role.Admin);
            
            //act
            photoService.DeletePhoto(photo.PhotoId);

            var p = photoService.GetAllPhotos(u);
            var count = p.Count;

            // assert
            Assert.Equal(1, count);
        }

        [Fact]
        public void Photo_DeletePhoto_WhenExists_ShouldReturnTrue()
        {
            //arrange - create a test Photo
            var p = new Photo
            {
                CommunityId = 1,
                PhotoId = 4,
                PhotoTitle = "Test Photo 1",
                Description = "Test Photo1",
                PhotoDataUrl = "data:image/png;base64,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"
            };
            photoService.AddPhoto(p);

            //act delete Photo
            var deleted = photoService.DeletePhoto(p.PhotoId);

            //assert- Photo is deleted
            Assert.True(deleted);

        }

        [Fact]
        public void Photo_GetPhotoByIdThatExists_ShouldReturnPhoto()
        {
            //arrange - create a test Photo
            var p = new Photo
            {
                CommunityId = 1,
                PhotoId = 4,
                PhotoTitle = "Test Photo 1",
                Description = "Test Photo1",
                PhotoDataUrl = "data:image/png;base64,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"
            };
            photoService.AddPhoto(p);

            //Act- Get Photo by Id
            var p2 = photoService.GetPhoto(p.PhotoId);

            //Assert equal-
            Assert.Equal(p2, p);
        }

        [Fact]
        public void Photo_DeletePhotoThatDoesntExist_ShouldReturnFalse()
        {
            //act delete photo(bool) with Id of 0(doesnt exist)
            var deleted = photoService.DeletePhoto(0);

            //Assert deleted returns false
            Assert.False(deleted);
        }

        [Fact]
        public void Photo_AddPhoto_WhenCommunityIdMatchesUsersCommunityId_ShouldAddOneToPhotoCount()
        {
            //arrange - create a test Photo and user
            var p = new Photo
            {
                CommunityId = 1,
                PhotoId = 4,
                PhotoTitle = "Test Photo 1",
                Description = "Test Photo1",
                PhotoDataUrl = "data:image/png;base64,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"
            };
            photoService.AddPhoto(p);
            var user = userservice.AddUser("guest", "guest@mail.com", 63, "female", 1, "guest", Role.Guest);

            //Act get photos
            var photos = photoService.GetAllPhotos(user);

            //Assert count is plus one
            Assert.Equal(1, photos.Count);
        }

        [Fact]
        public void Photo_AddPhoto_WhenCommunityIdDoesntMatchUsersCommunityId_ShouldntAddToPhotoCount()
        {
            //arrange - create a Photo Business and user
            var p = new Photo
            {
                CommunityId = 3,
                PhotoId = 4,
                PhotoTitle = "Test Photo 1",
                Description = "Test Photo1",
                PhotoDataUrl = "data:image/png;base64,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"
            };
            photoService.AddPhoto(p);
            var user = userservice.AddUser("guest", "guest@mail.com", 63, "female", 1, "guest", Role.Guest);

            //Act get photos
            var photos = photoService.GetAllPhotos(user);

            //Assert added business is not returned for this user
            Assert.Equal(0, photos.Count);
            //Assert business is not null
            Assert.NotNull(p);
        }

        //========MyChats Unit Tests===============
        [Fact]
        public void GetAllPosts_WhenNone_ShouldReturn0()
        {
            var u = userservice.AddUser("CAx", "me@mail.com", 21, "male", 1, "pwwww", Role.Admin);
            // act 
            var p = postService.GetAllPosts(u);
            var count = p.Count;

            // assert
            Assert.Equal(0, count);
        }
       
        [Fact]
        public void Post_DeletePost_WhenExists_ShouldReturnTrue()
        {
            //arrange - create a test Post
            var post = new Post
            {
                Name = "James",
                PostType = PostType.General,
                Id = 1,
                PostText = "C# 9.0 is taking shape, and I’d like to share our thinking on some of the major features we’re adding to this next version of the language." +
                " With every new version of C# we strive for greater clarity and simplicity in common coding scenarios, and C# 9.0 is no exception. One particular focus this time is supporting terse and immutable representation of data shapes. Let’s dive in!",
                CommunityId = 1,
                CreatedOn = DateTime.Now
            };
            postService.AddPost(post);

            //act delete Post
            var deleted = postService.DeletePost(post.Id);

            //assert- Post is deleted
            Assert.True(deleted);

        }

        [Fact]
        public void Post_GetAllPosts_WhenOne_ShouldReturnOne()
        {
            //arrange - create a test Post
            var post = new Post
            {
                Name = "James",
                PostType = PostType.General,
                Id = 1321,
                PostText = "C# 9.0 is taking shape, and I’d like to share our thinking on some of the major features we’re adding to this next version of the language." +
                " With every new version of C# we strive for greater clarity and simplicity in common coding scenarios, and C# 9.0 is no exception. One particular focus this time is supporting terse and immutable representation of data shapes. Let’s dive in!",
                CommunityId = 1,
                CreatedOn = DateTime.Now
            };
            postService.AddPost(post);

            var u = userservice.AddUser("Cahir", "me@mail.com", 21, "male", 1, "pwwww", Role.Admin);
            var posts = postService.GetAllPosts(u);
            var count = posts.Count;

            Assert.Equal(1, count);
            
        }

        [Fact]
        public void Post_AddPost_WhenUnique_ShouldSetAllProperties()
        {
            //arrange - create a test Post
            var post = new Post
            {
                Name = "James",
                PostType = PostType.General,
                Id = 1321,
                PostText = "C# 9.0 is taking shape, and I’d like to share our thinking on some of the major features we’re adding to this next version of the language." +
                " With every new version of C# we strive for greater clarity and simplicity in common coding scenarios, and C# 9.0 is no exception. One particular focus this time is supporting terse and immutable representation of data shapes. Let’s dive in!",
                CommunityId = 1,
                CreatedOn = DateTime.Now
            };
            postService.AddPost(post);

            Assert.NotNull(post);
            Assert.Equal("James", post.Name);
            Assert.Equal(PostType.General, post.PostType);
            Assert.Equal(1321, post.Id);
            Assert.Equal("C# 9.0 is taking shape, and I’d like to share our thinking on some of the major features we’re adding to this next version of the language." +
                " With every new version of C# we strive for greater clarity and simplicity in common coding scenarios, and C# 9.0 is no exception. One particular focus this time is supporting terse and immutable representation of data shapes. Let’s dive in!",
                 post.PostText);
            Assert.Equal(1, post.CommunityId);
            

        }

        [Fact]
        public void Post_UpdatePost_WhenExists_ShouldUpDateAllProperties()
        {
            var post = new Post
            {
                Name = "Cam",
                PostType = PostType.Help,
                Id = 131,
                PostText = "C# 9.0 is taking shape, and I’d like to share our thinking on some of the major features we’re adding to this next version of the language." +
                " With every new version of C# we strive for greater clarity and simplicity in common coding scenarios, and C# 9.0 is no exception. One particular focus this time is supporting terse and immutable representation of data shapes. Let’s dive in!",
                CommunityId = 2,
                CreatedOn = DateTime.Now
            };
            postService.AddPost(post);

            post.Name = "James";
            post.PostType= PostType.General;
            post.Id = 1321;
            post.PostText = "C# 9.0 is taking shape, and I’d like to share our thinking on some of the major features we’re adding to this next version of the language." +
                " With every new version of C# we strive for greater clarity and simplicity in common coding scenarios, and C# 9.0 is no exception. One particular focus this time is supporting terse and immutable representation of data shapes. Let’s dive in!";
            post.CommunityId = 1; 


            Assert.NotNull(post);
            Assert.Equal("James", post.Name);
            Assert.Equal(PostType.General, post.PostType);
            Assert.Equal(1321, post.Id);
            Assert.Equal("C# 9.0 is taking shape, and I’d like to share our thinking on some of the major features we’re adding to this next version of the language." +
                " With every new version of C# we strive for greater clarity and simplicity in common coding scenarios, and C# 9.0 is no exception. One particular focus this time is supporting terse and immutable representation of data shapes. Let’s dive in!",
                 post.PostText);
            Assert.Equal(1, post.CommunityId);
        }


        [Fact]
        public void Post_GetPostById_WhenExists_ShouldReturnPost()
        {
            var post = new Post
            {
                Name = "Cam",
                PostType = PostType.Help,
                Id = 131,
                PostText = "C# 9.0 is taking shape, and I’d like to share our thinking on some of the major features we’re adding to this next version of the language." +
                " With every new version of C# we strive for greater clarity and simplicity in common coding scenarios, and C# 9.0 is no exception. One particular focus this time is supporting terse and immutable representation of data shapes. Let’s dive in!",
                CommunityId = 2,
                CreatedOn = DateTime.Now
            };
            postService.AddPost(post);

            //Act- Get post by Id
            var v2 = postService.GetPost(post.Id);

            //Assert equal-
            Assert.Equal(v2, post);
        }

        [Fact]
        public void Post_DeletePostThatDoesntExist_ShouldReturnFalse()
        {
            //act delete post(bool) with Id of 0(doesnt exist)
            var deleted = postService.DeletePost(0);

            //Assert deleted returns false
            Assert.False(deleted);
        }

        [Fact]
        public void Post_AddPost_WhenCommunityIdMatchesUsersCommunityId_ShouldAddOneToPostCount()
        {
            var post = new Post
            {
                Name = "Cam",
                PostType = PostType.Help,
                Id = 131,
                PostText = "C# 9.0 is taking shape, and I’d like to share our thinking on some of the major features we’re adding to this next version of the language." +
                " With every new version of C# we strive for greater clarity and simplicity in common coding scenarios, and C# 9.0 is no exception. One particular focus this time is supporting terse and immutable representation of data shapes. Let’s dive in!",
                CommunityId = 1,
                CreatedOn = DateTime.Now
            };
            postService.AddPost(post);

            var user = userservice.AddUser("guest", "guest@mail.com", 63, "female", 1, "guest", Role.Guest);

            //Act get post
            var posts = postService.GetAllPosts(user);

            //Assert count is plus one
            Assert.Equal(1, posts.Count);
        }

        [Fact]
        public void Post_AddPostWhenCommunityIdDoesntMatchUserCommunityId_ShouldntAddOneToPostCount()
        {
            var post = new Post
            {
                Name = "Cam",
                PostType = PostType.Help,
                Id = 131,
                PostText = "C# 9.0 is taking shape, and I’d like to share our thinking on some of the major features we’re adding to this next version of the language." +
                " With every new version of C# we strive for greater clarity and simplicity in common coding scenarios, and C# 9.0 is no exception. One particular focus this time is supporting terse and immutable representation of data shapes. Let’s dive in!",
                CommunityId = 2,
                CreatedOn = DateTime.Now
            };
            postService.AddPost(post);

            var user = userservice.AddUser("guest", "guest@mail.com", 63, "female", 1, "guest", Role.Guest);

            //Act get post
            var posts = postService.GetAllPosts(user);

            //Assert count is plus one
            Assert.Equal(0, posts.Count);
            Assert.NotNull(post);
        }

        [Fact]
        public void Comment_AddComment_ShouldAddOneToCommentCount()
        {
            //Arrenge- add a test post and comment 
            var post = new Post
            {
                Name = "Cam",
                PostType = PostType.Help,
                Id = 131,
                PostText = "C# 9.0 is taking shape, and I’d like to share our thinking on some of the major features we’re adding to this next version of the language." +
                " With every new version of C# we strive for greater clarity and simplicity in common coding scenarios, and C# 9.0 is no exception. One particular focus this time is supporting terse and immutable representation of data shapes. Let’s dive in!",
                CommunityId = 2,
                CreatedOn = DateTime.Now
            };
            postService.AddPost(post);

            var c1 = new Comment
            {
                Name = "Cara",
                Description = "Test Comment",
                PostId = 131,
                CreatedOn = DateTime.Now,
                CommentId = 123
            };
            postService.AddComment(c1);

            var post2 = postService.GetPost(post.Id);

            //Assert Review was added
            Assert.Equal(1, post2.Comments.Count);
        }
        
        [Fact]
        public void Comment_DeleteComment_WhenTwoExist_ShouldReturnOne()
        {
            //Arrenge- add a test post and comment 
            var post = new Post
            {
                Name = "Cam",
                PostType = PostType.Help,
                Id = 131,
                PostText = "C# 9.0 is taking shape, and I’d like to share our thinking on some of the major features we’re adding to this next version of the language." +
                " With every new version of C# we strive for greater clarity and simplicity in common coding scenarios, and C# 9.0 is no exception. One particular focus this time is supporting terse and immutable representation of data shapes. Let’s dive in!",
                CommunityId = 2,
                CreatedOn = DateTime.Now
            };
            postService.AddPost(post);

            var c1 = new Comment
            {
                Name = "Cara",
                Description = "Test Comment",
                PostId = 131,
                CreatedOn = DateTime.Now,
                CommentId = 123
            };
            postService.AddComment(c1);

            var c2 = new Comment
            {
                Name = "Cahir",
                Description = "Test Comment 2",
                PostId = 131,
                CreatedOn = DateTime.Now,
                CommentId = 122
            };
            postService.AddComment(c2);

            postService.DeleteComment(c1.CommentId);

            //Assert comment was deleted
            Assert.Equal(1, post.Comments.Count);
        }

        //==========MyNews Related Tests=====================
        [Fact]
        public void News_GetAllNews_WhenNone_ShouldReturn0()
        {
            var user = userservice.AddUser("guest", "guest@mail.com", 63, "female", 1, "guest", Role.Guest);
            var news = newsService.GetAllNewsArticles(user);
            Assert.Equal(0, news.Count);
        }

        [Fact]
        public void News_GetAllNews_WhenOne_ShouldReturnOne()
        {
            var n = new NewsArticle
            {
                Id = 123,
                Headline = "Test Headline",
                Source ="Test Source",
                ArticleUrl ="www.test.com",
                CommunityId = 1
            };
            newsService.AddNewsArticle(n);

            var user = userservice.AddUser("guest", "guest@mail.com", 63, "female", 1, "guest", Role.Guest);
            var news = newsService.GetAllNewsArticles(user);


            Assert.Equal(1, news.Count);
        }

        [Fact]
        public void News_AddNews_WhenUnique_ShouldSetAllProperties()
        {
            var n = new NewsArticle
            {
                Id = 123,
                Headline = "Test Headline",
                Source ="Test Source",
                ArticleUrl ="www.test.com",
                CommunityId = 1
            };
            newsService.AddNewsArticle(n);

            Assert.Equal(123, n.Id);
            Assert.Equal("Test Headline", n.Headline);
            Assert.Equal("Test Source", n.Source);
            Assert.Equal("www.test.com", n.ArticleUrl);
            Assert.Equal(1, n.CommunityId);
        }

        [Fact]
        public void News_UpdateNews_WhenExists_ShouldUpdateAllProperties()
        {
            var n = new NewsArticle
            {
                Id = 1293,
                Headline = "ALt Test Headline",
                Source ="AltTest Source",
                ArticleUrl ="www.alttest.com",
                CommunityId = 2
            };
            newsService.AddNewsArticle(n);

            n.Id = 123;
            n.Headline = "Test Headline";
            n.Source ="Test Source";
            n.ArticleUrl ="www.test.com";
            n.CommunityId = 1;

            newsService.UpdateNewsArticle(n);

            Assert.Equal(123, n.Id);
            Assert.Equal("Test Headline", n.Headline);
            Assert.Equal("Test Source", n.Source);
            Assert.Equal("www.test.com", n.ArticleUrl);
            Assert.Equal(1, n.CommunityId);
        }

        [Fact]
        public void News_DeleteNews_WhenExists_ShouldReturnTrue()
        {
            var n = new NewsArticle
            {
                Id = 1293,
                Headline = "ALt Test Headline",
                Source ="AltTest Source",
                ArticleUrl ="www.alttest.com",
                CommunityId = 2
            };
            newsService.AddNewsArticle(n);

            var deleted = newsService.DeleteNewsArticle(n.Id);

            Assert.True(deleted);
        }

        [Fact]
        public void News_GetNewsByIdThatExists_ShouldReturnNews()
        {
            var n = new NewsArticle
            {
                Id = 1293,
                Headline = "ALt Test Headline",
                Source ="AltTest Source",
                ArticleUrl ="www.alttest.com",
                CommunityId = 2
            };
            newsService.AddNewsArticle(n);

            var news = newsService.GetNewsArticle(n.Id);

            Assert.Equal(news, n);
        }

        [Fact]
        public void News_DeleteNewsThatDoesntExist_ShouldReturnFalse()
        {
            var deleted = newsService.DeleteNewsArticle(0);

            Assert.False(deleted);
        }

        [Fact]
        public void News_AddNews_WhenCommunityIdMatchesUserCommunityId_ShouldAddOneToNewsCount()
        {
            var n = new NewsArticle
            {
                Id = 123,
                Headline = "Test Headline",
                Source ="Test Source",
                ArticleUrl ="www.test.com",
                CommunityId = 1
            };
            newsService.AddNewsArticle(n);

            var user = userservice.AddUser("guest", "guest@mail.com", 63, "female", 1, "guest", Role.Guest);
            var news = newsService.GetAllNewsArticles(user);


            Assert.Equal(1, news.Count);
        }

        [Fact]
        public void News_AddNews_WhenCommunityIdDoesntMatchUserCommunityId_ShouldNOTAddOneToNewsCount()
        {
            var n = new NewsArticle
            {
                Id = 123,
                Headline = "Test Headline",
                Source ="Test Source",
                ArticleUrl ="www.test.com",
                CommunityId = 2
            };
            newsService.AddNewsArticle(n);

            var user = userservice.AddUser("guest", "guest@mail.com", 63, "female", 1, "guest", Role.Guest);
            var news = newsService.GetAllNewsArticles(user);


            Assert.Equal(0, news.Count);
        }
    }
}
