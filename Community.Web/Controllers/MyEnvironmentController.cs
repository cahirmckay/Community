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
    public class MyEnvironmentController : BaseController
    {
        private IEnvironmentService svc;
        
        
        public MyEnvironmentController(IEnvironmentService es)
        {
           svc = es;
        }

        public IActionResult Index()
        {
            
            return View();
        }

        public IActionResult Details(int id)
        {
         
            return View();
        } 


        //GET
        public IActionResult Create()
        {
            var issue =  new Issue();
            
            return View(issue);
        }

        [HttpPost]
        public IActionResult Create(Issue issue)
        {

            return View();
        }

        //GET
        public IActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirm(int id)
        {
            
           
            Alert("Issue has been deleted", AlertType.success);
            return RedirectToAction(nameof(Index));
        }
    }
}