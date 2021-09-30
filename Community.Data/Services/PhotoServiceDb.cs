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

        // public IList<Photo> GetAllPhotos(User u)
        // {

        // }

        // public Photo GetPhoto(int id)
        // {

        // }

        // public Photo AddPhoto(Photo p)
        // {

        // }

        // public bool DeletePhoto(int id)
        // {

        // }

        // public bool UpdatePhoto(Photo p)
        // {

        // }
      
    }
}
