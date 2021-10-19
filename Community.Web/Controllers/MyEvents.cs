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
    }
}