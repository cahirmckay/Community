using System;
using System.Collections.Generic;

using Community.Core.Models;

namespace Community.Data.Services
{
    // This interface describes the operations that a UserService class should implement
    public interface ILocationService
    {
        // Initialise the repository - only to be used during development 
        void Initialise();


        IList<Location> GetAllLocations();
        Location GetLocation(int id);
        Location AddLocation(Location l);
        bool DeleteLocation(int id);
        bool UpdateLocation(Location l);

    }
    
}