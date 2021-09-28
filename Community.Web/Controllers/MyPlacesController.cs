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
    public class MyPlacesController : BaseController
    {
        private IBusinessService svc;
        private IUserService userService;

        // Configured via DI
        public MyPlacesController(IBusinessService bs, IUserService us)
        {
            svc = bs;
            userService = us;
        }


        public IActionResult Index()
        {
            var user = userService.GetUser(GetSignedInUserId());
            var p = svc.GetAllBusiness(user);
            return View(p);
        }

        public IActionResult Details(int id)
        {
            var business = svc.GetBusiness(id);
            
            if (business == null)
            {
                Alert("Business does not exist", AlertType.warning);
                //return to index to look for Business
                return RedirectToAction(nameof(Index));
                
            }
         
            return View(business);
        } 


        public IActionResult Edit(int id)
        {
            var business = svc.GetBusiness(id);

            if (business == null)
            {
                Alert("business does not exist", AlertType.warning);
                return RedirectToAction(nameof(Index));
            }
            return View(business);
        }

        [HttpPost]
        public IActionResult Edit(int id, Business b)
        {
            if (ModelState.IsValid)
            {
                svc.UpdateBusiness(b);
                Alert($"{b.Title} has updated ", AlertType.info);
                return RedirectToAction(nameof(Index));
            }

            return View(b);

        }
// 
        //GET.
        public IActionResult Create()
        {
            var b =  new Business();
            ;
            return View(b);
        }

        [HttpPost]
        public IActionResult Create(Business b)
        {
            if (ModelState.IsValid)
            {
                var added = svc.AddBusiness(b);
                if (added != null)
                {
                    Alert($"{b.Title} has been successfully added", AlertType.success);
                    //Redirects to see the newly added movie in the index page
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(b);
        }

        //GET
        public IActionResult Delete(int id)
        {
            var b = svc.GetBusiness(id);
            

            if (b == null)
            {
                Alert("Business does not exist", AlertType.warning);
                return RedirectToAction(nameof(Index));
            }

            return View(b);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirm(int id)
        {
            var business = svc.GetBusiness(id);
            svc.DeleteBusiness(id);
            Alert($"{business.Title} has been deleted", AlertType.success);
            return RedirectToAction(nameof(Index));
        }


        //-----Review Creation from movie view
        //GET/ movie/addmovie
        public IActionResult AddReview(int id)
        {
            var movie = svc.GetBusiness(id);

            if (movie == null)
            {
                Alert("Business does not exist", AlertType.warning);
                return RedirectToAction(nameof(Index));
            }

            var review = new Review
            {
                BusinessId = id, 
                CreatedOn = DateTime.Now,

            };

            return View("AddReview", review);
        }

        //post /movie/addreview
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddReview(Review r)
        {
            var business = svc.GetBusiness(r.BusinessId);
            if (business == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                svc.AddReview(r);
                Alert($"Review of {business.Title} has been added", AlertType.success);
                return RedirectToAction("Details", new{ Id = r.BusinessId});
            }

            return View("AddReview", r);
        }

        //GET
        public IActionResult DeleteReview(int id)
        {
            
            var review = svc.GetReviewById(id);

            if (review == null)
            {
                return NotFound();
            }
            //view delete review page
            return View(review);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmReview(int id)
        {
            var review = svc.GetReviewById(id);
            var movie = svc.GetBusiness(review.BusinessId);
            svc.DeleteReview(id);
            Alert($"Review by {review.Name} has deleted successfully", AlertType.success);
            //after deleting a review return to the assoiated business so the user can see it's been deleted
            return RedirectToAction("Details", new{ Id = review.BusinessId});
        }


    }//class
}//namespace
