using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Community.Core.Models;
using Community.Core.Security;
using Community.Data.Repositories;
namespace Community.Data.Services
{
    public class BookingServiceDb : IBookingService
    {
        private readonly DatabaseContext ctx;
        

        public BookingServiceDb(DatabaseContext db)
        {
            ctx = db; 
        }

        public void Initialise()
        {
           ctx.Initialise(); 
        }

        //=======Venue related operations=========
        public IList<Venue> GetAllVenues(User u)
        {
            return ctx.Venues
                        .Where(b => b.CommunityId == u.CommunityId)
                        .ToList();    
        }

        public Venue GetVenue(int id)
        {
            return ctx.Venues
                     .Include(v => v.Events)
                     .FirstOrDefault(v => v.Id == id);
        }

        public Venue AddVenue(Venue v)
        {
            ctx.Venues.Add(v);
            ctx.SaveChanges();
            return v;
        }

        public bool DeleteVenue(int id)
        {
            var v = GetVenue(id);
            if (v == null)
            {
                return false;
            }
            ctx.Venues.Remove(v);
            ctx.SaveChanges();
            return true;
        }

        public bool UpdateVenue(Venue v)
        {
            var venue = GetVenue(v.Id);
            if (venue == null)
            {
                return false;
            }
            
            venue.Name = v.Name;
            venue.Address = v.Address;
            venue.Description = v.Description;
            venue.OriginalCapacity = v.OriginalCapacity;
            venue.SocialDistance = v.SocialDistance;
            
            ctx.SaveChanges();
            return true;
        }

        //=========Events Related options============

        public Event GetEventById(int id)
        {
            return ctx.Events
                     .Include(e => e.Venue)
                     .FirstOrDefault(e => e.Id == id);
        }

        public Event AddEvent(Event e)
        {
            //Creates a new instance of a event so more than can be created for each venue
            var ev = new Event
            {
                Name = e.Name,
                StartTime = e.StartTime,
                EndTime = e.EndTime,
                CreatedOn= DateTime.Today,
                Status = e.Status,
                VenueId = e.VenueId,
                
            };

            ctx.Events.Add(ev);
            ctx.SaveChanges();
            return e;
        }

        public bool DeleteEvent(int id)
        {
            var e = GetEventById(id);
            if (e == null)
            {
                return false;
            }
            
            //deletes one event
            var result = e.Venue.Events.Remove(e);
            ctx.SaveChanges();
            return result;
        }

        public bool UpdateEvent(Event e)
        {
            var ev = GetEventById(e.Id);
            if (ev == null)
            {
                return false;
            }
            ev.Id = e.Id;
            ev.Name = e.Name;
            ev.StartTime = e.StartTime;
            ev.EndTime = e.EndTime;
            ev.Status = e.Status;
            ctx.SaveChanges();
            return true;
        }
        
    }
}
