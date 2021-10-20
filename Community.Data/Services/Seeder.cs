using System;
using System.Text;
using System.Collections.Generic;

using Community.Core.Models;

namespace Community.Data.Services
{
    public static class Seeder
    {
        // use this class to seed the database with dummy 
        // test data using an services 
         public static void Seed(IUserService svc, IBusinessService bs, IPhotoService ps,
          IPostService postService, INewsService newsService, IBookingService bookingService,
          IEnvironmentService environmentService)
        {
            svc.Initialise();
            bs.Initialise();
            ps.Initialise();
            postService.Initialise();
            newsService.Initialise();
            bookingService.Initialise();
            environmentService.Initialise();

            // add users
            svc.AddUser("Administrator", "admin@mail.com", 34, "male", 1,"admin",  Role.Admin);
            svc.AddUser("Manager", "manager@mail.com", 34, "female", 2, "manager" ,Role.Manager);
            svc.AddUser("Guest", "guest@mail.com",  62, "other", 3, "guest",  Role.Guest);    

            //add business
            var b1 = new Business{
                
                Title = "Gustys",
                Type = "Shop",
                Address = "119 Drumagarner Rd, Kilrea, Coleraine BT51 5TF",
                Description = "Small convience shop",
                PosterUrl = "data:image/png;base64,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",
                CommunityId = 1
            };
            bs.AddBusiness(b1);

            var b2 = new Business{
                
                Title = "The Manor",
                Type = "Restaurant",
                Address = "69 Bridge St, Kilrea, Coleraine BT51 5RR",
                Description = "The Manor House Kilrea is a distinguished and relaxing bar and restaurant with the ideal setting for casual drinks, family events and exquisite dining.",
                PosterUrl = "data:image/jpeg;base64,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",
                CommunityId = 1
            };
            bs.AddBusiness(b2);

            var b3 = new Business{
                
                Title = "The Lake Kilrea",
                Type = "Water Park",
                Address = "69B Bridge St, Kilrea, Coleraine BT51 5RR",
                Description = "Situated in the picture perfect heart of Northern Ireland, the Lake Kilrea is your must visit destination for thrills, spills, splashes, dashes, bombing, jumping, bouncing and pouncing. Come and spend an activity packed day with your friends, family and colleagues and re-discover what it means to have serious fun again.",
                PosterUrl = "https://thelakekilrea.com/wp-content/uploads/2021/03/photo-11-08-2020-11-30-22-scaled.jpg",
                CommunityId = 1
            };
            bs.AddBusiness(b3);

            var b4 = new Business{
                
                Title = "Stranded Burger",
                Type = "Fast Food",
                Address = " Portneal Lodge, 75 Bann Road, Kilrea, BT51 5RX",
                Description = "•Your new go-to burger joint. Smashed Burgers, fries & shakes.Breakfast, coffee & treats",
                PosterUrl = "data:image/png;base64,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",
                CommunityId = 1
            };
            bs.AddBusiness(b4);

            //---------MyPlaces Review Seeds-----------
            var r1 = new Review
            {
                Name = "Cara", 
                Comment = "Cheap convience store with friendly staff",
                BusinessId = 1,
                Rating = 8,
            };
            bs.AddReview(r1);

            var r2 = new Review
            {
                Name = "Angela", 
                Comment = "Easily accessible",
                BusinessId = 1,
                Rating = 9,
            };
            bs.AddReview(r2);

            var r3 = new Review
            {
                Name = "Pat", 
                Comment = "Too small, not alot of options",
                BusinessId = 1,
                Rating = 5,
            };
            bs.AddReview(r3);


            //Add Photo
            var p1 = new Photo
            {
                CommunityId =1,
                PhotoId = 1,
                PhotoTitle = "Test Photo 1",
                Description ="Test Photo1",
                PhotoDataUrl ="https://upload.wikimedia.org/wikipedia/commons/thumb/2/2d/Kilrea_-_geograph.org.uk_-_342208.jpg/360px-Kilrea_-_geograph.org.uk_-_342208.jpg"
            };
            ps.AddPhoto(p1);

            //Add Photo
            var p2 = new Photo
            {
                CommunityId =1,
                PhotoId = 2,
                PhotoTitle = "Test Photo 1",
                Description ="Test Photo1",
                PhotoDataUrl ="https://upload.wikimedia.org/wikipedia/commons/9/94/The_Kilrea_Bridge_-_geograph.org.uk_-_279711.jpg"
            };
            ps.AddPhoto(p2);
            //Add Photo
            var p3 = new Photo
            {
                CommunityId =1,
                PhotoId = 3,
                PhotoTitle = "Test Photo 1",
                Description ="Test Photo1",
                PhotoDataUrl ="https://upload.wikimedia.org/wikipedia/commons/8/89/Drumnagarner_RC_Church_-_geograph.org.uk_-_840133.jpg"
            };
            ps.AddPhoto(p3);
            //Add Photo
            var p4 = new Photo
            {
                CommunityId =1,
                PhotoId = 4,
                PhotoTitle = "Test Photo 1",
                Description ="Test Photo1",
                PhotoDataUrl ="https://upload.wikimedia.org/wikipedia/commons/9/9d/First_Kilrea_Presbyterian_Church_-_geograph.org.uk_-_342207.jpg"
            };
            ps.AddPhoto(p4);


            //==============MyChats Data================

            var post = new Post
            {
                Name = "James",
                PostType = PostType.General,
                Id = 1,
                PostText ="C# 9.0 is taking shape, and I’d like to share our thinking on some of the major features we’re adding to this next version of the language."+
                " With every new version of C# we strive for greater clarity and simplicity in common coding scenarios, and C# 9.0 is no exception. One particular focus this time is supporting terse and immutable representation of data shapes. Let’s dive in!",
                CommunityId = 1,
                CreatedOn = DateTime.Now
            };
            postService.AddPost(post);

            var comment = new Comment
            {
                CommentId = 1,
                Description = "I should look into this",
                PostId = 1,
            
                Name = "jake"
            };
            postService.AddComment(comment);
            var comment2 = new Comment
            {
                CommentId = 2,
                Description = "Wow thats so interesting",
                PostId = 1,
                Name = "Harry"
            };
            postService.AddComment(comment2);

            var post2 = new Post
            {
                Name = "Mary",
                PostType = PostType.Help,
             
                PostText = "I am currently Isolarting and was wondering if anyone could recomend any local business that deliever groceries?",
                CommunityId = 1,
                CreatedOn = DateTime.Now
            };
            postService.AddPost(post2);

            //=========MyNews Seed Data=====================
            
            var newsArticle = new NewsArticle
            {
                Headline = " A LOCAL politician says she has been “inundated” with complaints about traffic issues in Kilrea. Politician 'inundated' with complaints on Kilrea traffic issues",
                Source = "Derry Post",
                ArticleUrl ="https://www.derrynow.com/news/county-derry-post/493268/politician-inundated-with-complaints-on-kilrea-traffic-issues.html",
                CommunityId = 1,
                CreatedOn = DateTime.Now

            };
            newsService.AddNewsArticle(newsArticle);

            //===========MyEvents seed data========================
            var venue = new Venue
            {
                Name = "The Marian Hall",
                Address = "Marian Hall, Londonderry, Northern Ireland, BT48 8QX",
                Description ="Former Ballroom",
                SocialDistance = 1,
                OriginalCapacity = 100,
                CommunityId= 1
            };
            bookingService.AddVenue(venue);

            var ev = new Event
            {
                Name = "Christmas Party",
                // 2021 - year, 12 - month, 25 – day, 10 – hour, 30 – minute, 50 - second
                StartTime = new DateTime(2021, 12, 25, 10, 30, 00) ,
                EndTime = new DateTime(2021, 12, 25, 18, 30, 00) ,
                CreatedOn= DateTime.Today,
                Status = Status.Confirmed,
                VenueId = 1,
            };
            bookingService.AddEvent(ev);
            
        }
    }
} 
