using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Community.Core.Models;
using Community.Data.Services;

namespace Community.Web.Controllers
{
    [Authorize]
    public class MyEventsController : BaseController
    {
        private IBookingService svc;
        private IUserService userService;
        
        public MyEventsController(IBookingService es, IUserService us)
        {
            svc = es;
            userService = us;
        }

        public IActionResult Index()
        {
            var user = userService.GetUser(GetSignedInUserId());
            var v = svc.GetAllVenues(user);
            return View(v);
        }

        public IActionResult Details(int id)
        {
            var venue = svc.GetVenue(id);
            
            if (venue == null)
            {
                Alert("Venue does not exist", AlertType.warning);
                //return to index to look for Venue
                return RedirectToAction(nameof(Index));
            }
         
            return View(venue);
        } 


        public IActionResult Edit(int id)
        {
            var venue = svc.GetVenue(id);

            if (venue == null)
            {
                Alert("This venue does not exist", AlertType.warning);
                return RedirectToAction(nameof(Index));
            }
            return View(venue);
        }

        [HttpPost]
        public IActionResult Edit(int id, Venue v)
        {
            if (ModelState.IsValid)
            {
                svc.UpdateVenue(v);
                Alert($"{v.Name} has updated ", AlertType.info);
                return RedirectToAction(nameof(Index));
            }

            return View(v);
        }

        //GET
        public IActionResult Create()
        {
            var v =  new Venue();
            
            return View(v);
        }

        [HttpPost]
        public IActionResult Create(Venue v)
        {
            var user = userService.GetUser(GetSignedInUserId());
            
            v.CommunityId = user.CommunityId;
            if (ModelState.IsValid)
            {
                var added = svc.AddVenue(v);
                if (added != null)
                {
                    Alert($"{v.Name} has been successfully added", AlertType.success);
                    //Redirects to see the newly added business in the index page
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(v);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirm(int id)
        {
            var venue = svc.GetVenue(id);
            svc.DeleteVenue(id);
            Alert($"{venue.Name} has been deleted", AlertType.success);
            return RedirectToAction(nameof(Index));
        }

        //-----Event Creation from venue view
        //GET
        public IActionResult AddEvent(int id)
        {
            var venue = svc.GetVenue(id);

            if (venue == null)
            {
                Alert("Venue does not exist", AlertType.warning);
                return RedirectToAction(nameof(Index));
            }

            var user = userService.GetUser(GetSignedInUserId());

            var booking = new Event
            {
                VenueId = id, 
                CreatedOn = DateTime.Now,
                Status = Status.Unconfirmed
            };

            return View("AddEvent", booking);
        }

        //post 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddEvent(Event e)
        {
            var venue = svc.GetVenue(e.VenueId);
            
            if (venue == null)
            {
                Alert($"NULL", AlertType.danger);
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                svc.AddEvent(e);
                Alert($"Booking request at {venue.Name} has been made", AlertType.success);
                return RedirectToAction("Details", new{ Id = e.VenueId});
            }

            return View("AddEvent", e);
        }

        //GET
        public IActionResult DeleteEvent(int id)
        {
            
            var e= svc.GetEventById(id);

            if (e == null)
            {
                return NotFound();
            }
            //view delete event page
            return View(e);
        }

        //GET
        public IActionResult EditEvent(int id)
        {
            var e = svc.GetEventById(id);
            if (e == null)
            {
                Alert("This booking does not exist", AlertType.warning);
                return RedirectToAction(nameof(Index));
            }
            return View(e);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditEvent(Event e)
        {
            if (ModelState.IsValid)
            {
                svc.UpdateEvent(e);
                Alert("Booking has updated", AlertType.info);
                return RedirectToAction("Details", new{ Id = e.VenueId});
            }
            return View("EditEvent", e);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmEvent(int id)
        {
            var ev = svc.GetEventById(id);
            var venue = svc.GetVenue(ev.VenueId);
            svc.DeleteEvent(id);
            Alert($"Booking request by {ev.Name} has deleted successfully", AlertType.success);
            //after deleting a booking return to the assoiated venue so the admin or mod can see it's been deleted
            return RedirectToAction("Details", new{ Id = ev.VenueId});
        }
    }
}