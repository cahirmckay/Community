using System;
using System.Collections.Generic;

using Community.Core.Models;

namespace Community.Data.Services
{
    // This interface describes the operations that a PhotoService class should implement.
    public interface IPhotoService
    {
        // Initialise the repository - only to be used during development 
        void Initialise();

    
        IList<Photo> GetAllPhotos(User u);
        Photo GetPhoto(int id);
        Photo AddPhoto(Photo p);
        // bool DeletePhoto(int id);
        // bool UpdatePhoto(Photo p);
      
    }
    
}
