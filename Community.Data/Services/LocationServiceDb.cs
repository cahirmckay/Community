using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Community.Core.Models;
using Community.Core.Security;
using Community.Data.Repositories;
namespace Community.Data.Services
{
    public class LocationServiceDb : ILocationService
    {
        private readonly DatabaseContext ctx;
        

        public LocationServiceDb(DatabaseContext db)
        {
            ctx = db; 
        }

        public void Initialise()
        {
           ctx.Initialise(); 
        }

        //-------------MyLocation related options---------------------------
        public IList<Location> GetAllLocations()
        {

            return ctx.Locations
                        .ToList();            
        }

        public Location GetLocation(int id)
        {
            return ctx.Locations
                     .FirstOrDefault(i => i.Id == id);
        }
        
        public bool DeleteLocation(int id)
        {
            var i = GetLocation(id);
            if (i == null)
            {
                return false;
            }
            ctx.Locations.Remove(i);
            ctx.SaveChanges();
            return true;
        }

        public Location AddLocation(Location i)
        {
            ctx.Locations.Add(i);
            ctx.SaveChanges();
            return i;
        }

        public bool UpdateLocation(Location i)
        {
            var Location = GetLocation(i.Id);
            if (Location == null)
            {
                return false;
            }

            Location.Description = i.Description;
            Location.Id = i.Id;
            Location.Name = i.Name;
            
            ctx.SaveChanges();
            return true;
        }

    }
}
