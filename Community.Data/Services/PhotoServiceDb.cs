using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Community.Core.Models;
using Community.Core.Security;
using Community.Data.Repositories;
namespace Community.Data.Services
{
    public class PhotoServiceDb : IPhotoService
    {
        private readonly DatabaseContext ctx;

        //Confirgured Via DI
        public PhotoServiceDb(DatabaseContext db)
        {
            ctx = db; 
        }

        public void Initialise()
        {
           ctx.Initialise(); 
        }

        //-------------MyPhotos related options------

        //Only return photos in their community
        public IList<Photo> GetAllPhotos(User u)
        {
             return ctx.Photos
                         .Where(p => p.CommunityId == u.CommunityId)
                         .ToList();  
        }

        public Photo GetPhoto(int id)
        {
            return ctx.Photos
                      .FirstOrDefault(p => p.PhotoId == id);
        }

        public Photo AddPhoto(Photo p)
        {
            ctx.Photos.Add(p);
            ctx.SaveChanges();
            return p;
        }

        public bool DeletePhoto(int id)
        {
            var photo = GetPhoto(id);
            if (photo ==null)
            {
                return false;
            }
            ctx.Photos.Remove(photo);
            ctx.SaveChanges();
            return true;
        }

        
      
    }
}
