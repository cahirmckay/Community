using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Community.Core.Models;
using Community.Core.Security;
using Community.Data.Repositories;
namespace Community.Data.Services
{
    public class BusinessServiceDb : IBusinessService
    {
        private readonly DatabaseContext ctx;
        
        public BusinessServiceDb(DatabaseContext db)
        {
            ctx = db; 
        }

        public void Initialise()
        {
           ctx.Initialise(); 
        }

        //-------------MyPlaces related options---------------------------
        public IList<Business> GetAllBusiness(User u)
        {
            return ctx.Businesses
                        .Where(b => b.CommunityId == u.CommunityId)
                        .ToList();            
        }

        public Business GetBusiness(int id)
        {
            return ctx.Businesses
                     .Include(b => b.Reviews)
                     .FirstOrDefault(b => b.Id == id);
        }
        
        public bool DeleteBusiness(int id)
        {
            var b = GetBusiness(id);
            if (b == null)
            {
                return false;
            }
            ctx.Businesses.Remove(b);
            ctx.SaveChanges();
            return true;
        }

        public Business AddBusiness(Business b)
        {
            ctx.Businesses.Add(b);
            ctx.SaveChanges();
            return b;
        }

        public bool UpdateBusiness(Business b)
        {
            var business = GetBusiness(b.Id);
            if (business == null)
            {
                return false;
            }
            
            business.Title = b.Title;
            business.Type = b.Type;
            business.Address = b.Address;
            business.Description = b.Description;
            business.PosterUrl = b.PosterUrl;
            ctx.SaveChanges();
            return true;
        }

        //REVIEW OPERATIONS

        public Review GetReviewById(int id)
        {
            return ctx.Reviews
                     .Include(r => r.Business)
                     .FirstOrDefault(r => r.Id == id);
        }

        public Review AddReview(Review r)
        {
            //Creates a new instance of a review so more than can be created for each business
            var review = new Review
            {
                // Id created by Database 
                Name = r.Name,              
                Comment = r.Comment,      
                BusinessId = r.BusinessId,
                Rating = r.Rating,
                CreatedOn = DateTime.Now                
            };


            ctx.Reviews.Add(review);
            ctx.SaveChanges();
            return r;
        }

        public bool DeleteReview(int id)
        {
            var review = GetReviewById(id);
            if (review == null)
            {
                return false;
            }
            
            //deletes one review
            var result = review.Business.Reviews.Remove(review);
            ctx.SaveChanges();
            return result;
        }

    }
}
