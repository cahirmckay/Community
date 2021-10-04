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
        

        public PhotoServiceDb(DatabaseContext db)
        {
            ctx = db; 
        }

        public void Initialise()
        {
           ctx.Initialise(); 
        }

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

        // public bool DeletePhoto(int id)
        // {

        // }

        // public bool UpdatePhoto(Photo p)
        // {

        // }
      
    }
}
