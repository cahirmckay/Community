using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Community.Web.ViewModels;
using Community.Core.Models;
using Community.Data.Services;
using System.Web;


namespace Community.Web.Controllers
{
    
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private ILocationService locationService;
        private IUserService userService;

        public HomeController(ILogger<HomeController> logger, ILocationService ls, IUserService us)
        {
            _logger = logger;
            locationService = ls;
            userService = us;

        }

        public IActionResult Index()
        {
            var user = userService.GetUser(GetSignedInUserId());
            if(user==null)
            {
                return View();
            }
            var userCommunity = locationService.GetLocation(user.CommunityId);
            
            return View(userCommunity);
            
        }

        [Authorize]
        public IActionResult Secure()
        {
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult CommunityIndex()
        {
            var c = locationService.GetAllLocations();
            return View(c);
        }
    
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
