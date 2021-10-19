using System;
using System.Collections.Generic;

using Community.Core.Models;

namespace Community.Data.Services
{
    // This interface describes the operations that a BookingService class should implement.
    public interface IBookingService
    {
        // Initialise the repository - only to be used during development 
        void Initialise();

    
        IList<Venue> GetAllVenues(User u);
        Venue GetVenue(int id);
        Venue AddVenue(Venue v);
        bool DeleteVenue(int id);
        bool UpdateVenue(Venue v);
        Event GetEventById(int id);
        Event AddEvent(Event e);
        bool DeleteEvent(int id);
        bool UpdateEvent(Event e);
    }
}
