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
            var p = svc.GetAllIssues();
            
            return View(p);
        }

        public IActionResult AdminIndex()
        {
            var p = svc.GetAllIssues();
            
            return View(p);
        }

        public IActionResult Edit(int id)
        {
            var issue = svc.GetIssue(id);

            if (issue == null)
            {
                Alert("Issue does not exist", AlertType.warning);
                //return to index to look for Issue
                return RedirectToAction(nameof(Index));
            }
         
            return View(issue);
        } 

        [HttpPost]
        public IActionResult Edit(int id, Issue issue )
        {
            if (ModelState.IsValid)
            {
                svc.UpdateIssue(issue);
                Alert("Your issue has been updated ", AlertType.info);
                return RedirectToAction(nameof(Index));
            }

            return View(issue);
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
            if (ModelState.IsValid)
            {
                var added = svc.AddIssue(issue);
                if (added != null)
                {
                    Alert("Your issue has been successfully added", AlertType.success);
                    //Redirects to see the newly added issue in the index page
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(issue);
            
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
            
            var issue = svc.GetIssue(id);
            svc.DeleteIssue(id);
            Alert("Issue has been deleted", AlertType.success);
            return RedirectToAction(nameof(Index));
        }
    }
}