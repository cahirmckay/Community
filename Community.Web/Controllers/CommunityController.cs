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
    public class CommunityController : BaseController
    {
        private ILocationService svc;
        private IUserService userService;
        
        public CommunityController(ILocationService ls, IUserService us)
        {
            svc = ls;
            userService = us;
        }

        public IActionResult Index()
        {
            var c = svc.GetAllLocations();
            return View(c);
        }

        //GET
        public IActionResult Edit(int id)
        {
            var community = svc.GetLocation(id);

            if (community== null)
            {
                Alert("Community does not exist", AlertType.warning);
                return RedirectToAction(nameof(Index));
            }
            return View(community);
        }

        [HttpPost]
        public IActionResult Edit(int id, Location community)
        {
            if (ModelState.IsValid)
            {
                svc.UpdateLocation(community);
                Alert($"{community.Name} has been updated ", AlertType.info);
                return RedirectToAction(nameof(Index));
            }

            return View(community);
        }

        //GET
        public IActionResult Create()
        {
            var l =  new Location();
            
            return View(l);
        }

        [HttpPost]
        public IActionResult Create(Location l)
        {
            
            if (ModelState.IsValid)
            {
                var added = svc.AddLocation(l);
                if (added != null)
                {
                    Alert($"{l.Name} has been successfully added", AlertType.success);
                    //Redirects to see the newly added community in the index page
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(l);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirm(int id)
        {
            var community = svc.GetLocation(id);
            svc.DeleteLocation(id);
            Alert($"{@community.Name} has been deleted", AlertType.success);
            return RedirectToAction(nameof(Index));
        }
    }
}