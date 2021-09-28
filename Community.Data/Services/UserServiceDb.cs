using System;
using System.Linq;
using System.Collections.Generic;

using Community.Core.Models;
using Community.Core.Security;
using Community.Data.Repositories;
namespace Community.Data.Services
{
    public class UserServiceDb : IUserService
    {
        private readonly DatabaseContext  ctx;

        public UserServiceDb(DatabaseContext db)
        {
            ctx = db; 
        }

        public void Initialise()
        {
           ctx.Initialise(); 
        }

        // ------------------ User Related Operations ------------------------

        // retrieve list of Users
        public IList<User> GetUsers()
        {
            return ctx.Users.ToList();
        }

        // Retrive User by Id 
        public User GetUser(int id)
        {
            return ctx.Users.FirstOrDefault(s => s.Id == id);
        }

        // Add a new User checking a User with same email does not exist
        public User AddUser(string name, string email, int age, string gender, int communityId, string password, Role role)
        {     
            var existing = GetUserByEmail(email);
            if (existing != null)
            {
                return null;
            } 

            var user = new User
            {            
                Name = name,
                Email = email,
                Age = age,
                Gender = gender,
                CommunityId = communityId,
                Password = Hasher.CalculateHash(password), // can hash if required 
                Role = role              
            };
            ctx.Users.Add(user);
            ctx.SaveChanges();
            return user; // return newly added User
        }

        // Delete the User identified by Id returning true if deleted and false if not found
        public bool DeleteUser(int id)
        {
            var s = GetUser(id);
            if (s == null)
            {
                return false;
            }
            ctx.Users.Remove(s);
            ctx.SaveChanges();
            return true;
        }

        // Update the User with the details in updated 
        public User UpdateUser(User updated)
        {
            // verify the User exists
            var User = GetUser(updated.Id);
            if (User == null)
            {
                return null;
            }
            // update the details of the User retrieved and save
            User.Name = updated.Name;
            User.Email = updated.Email;
            User.Age = updated.Age;
            User.Gender = updated.Gender;
            User.CommunityId = updated.CommunityId;
            User.Password = Hasher.CalculateHash(updated.Password);  
            User.Role = updated.Role; 

            ctx.SaveChanges();          
            return User;
        }

        public User GetUserByEmail(string email, int? id=null)
        {
            return ctx.Users.FirstOrDefault(u => u.Email == email && ( id == null || u.Id != id));
        }

        public IList<User> GetUsersQuery(Func<User,bool> q)
        {
            return ctx.Users.Where(q).ToList();
        }

        public User Authenticate(string email, string password)
        {
            // retrieve the user based on the EmailAddress (assumes EmailAddress is unique)
            var user = GetUserByEmail(email);

            // Verify the user exists and Hashed User password matches the password provided
            return (user != null && Hasher.ValidateHash(user.Password, password)) ? user : null;
            //return (user != null && user.Password == password ) ? user: null;
        }


        // //Myplaces related options
        // public IList<Business> GetAllBusiness()
        // {

        //     return ctx.Businesses.ToList();
        // }

        // public Business GetBusiness(int id)
        // {
        //     return ctx.Businesses
        //              .Include(b => b.Reviews)
        //              .FirstOrDefault(b => b.Id == id);
        // }
        

        // public bool DeleteBusiness(int id)
        // {
        //     var b = GetBusiness(id);
        //     if (b == null)
        //     {
        //         return false;
        //     }
        //     ctx.Businesses.Remove(b);
        //     ctx.SaveChanges();
        //     return true;
        // }

        // public Business AddBusiness(Business b)
        // {
        //     ctx.Businesses.Add(b);
        //     ctx.SaveChanges();
        //     return b;
        // }

        // public bool UpdateBusiness(Business b)
        // {
        //     var business = GetBusiness(b.Id);
        //     if (business == null)
        //     {
        //         return false;
        //     }
        //     business.Id = b.Id;
        //     business.Title = b.Title;
        //     business.Type = b.Type;
        //     business.Address = b.Address;
        //     business.Description = b.Description;
        //     business.PosterUrl = b.PosterUrl;
            
            
        //     ctx.SaveChanges();
        //     return true;
        // }

        // //REVIEW OPERATIONS

        // public Review GetReviewById(int id)
        // {
        //     return ctx.Reviews
        //              .Include(r => r.Business)
        //              .FirstOrDefault(r => r.Id == id);
        // }

        // public Review AddReview(Review r)
        // {
        //     //Creates a new instance of a review so more than can be created for each movie
        //     var review = new Review
        //     {
        //         // Id created by Database 
        //         Name = r.Name,              
        //         Comment = r.Comment,      
        //         BusinessId = r.BusinessId,
        //         Rating = r.Rating,
        //         CreatedOn = DateTime.Now                
        //     };


        //     ctx.Reviews.Add(review);
        //     ctx.SaveChanges();
        //     return r;
        // }

        // public bool DeleteReview(int id)
        // {
        //     var review = GetReviewById(id);
        //     if (review == null)
        //     {
        //         return false;
        //     }
            
        //     //deletes one review
        //     var result = review.Business.Reviews.Remove(review);
        //     ctx.SaveChanges();
        //     return result;
        // }
    }
}