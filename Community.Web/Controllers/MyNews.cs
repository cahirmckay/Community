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
    public class MyNewsController : BaseController
    {
        private INewsService svc;
        private IUserService userService;
        
        public MyNewsController(INewsService ns, IUserService us)
        {
            svc = ns;
            userService = us;
        }

        public IActionResult Index()
        {
            var user = userService.GetUser(GetSignedInUserId());
            var n = svc.GetAllNewsArticles(user);
            return View(n);
        }

        public IActionResult Details(int id)
        {
            var newsArticle= svc.GetNewsArticle(id);
            
            if (newsArticle == null)
            {
                Alert("Article does not exist", AlertType.warning);
                //return to index to look for article
                return RedirectToAction(nameof(Index));
            }
         
            return View(newsArticle);
        } 

        //GET
        public IActionResult Edit(int id)
        {
            var newsArticle = svc.GetNewsArticle(id);

            if (newsArticle == null)
            {
                Alert("Article does not exist", AlertType.warning);
                return RedirectToAction(nameof(Index));
            }
            return View(newsArticle);
        }

        [HttpPost]
        public IActionResult Edit(int id, NewsArticle n)
        {
            if (ModelState.IsValid)
            {
                svc.UpdateNewsArticle(n);
                Alert("This article has been updated ", AlertType.info);
                return RedirectToAction(nameof(Index));
            }

            return View(n);
        }

        //GET
        public IActionResult Create()
        {
            var n =  new NewsArticle();
            
            return View(n);
        }

        [HttpPost]
        public IActionResult Create(NewsArticle n)
        {
            var user = userService.GetUser(GetSignedInUserId());
            n.CommunityId = user.CommunityId;
            n.CreatedOn = DateTime.Now;
            if (ModelState.IsValid)
            {
                var added = svc.AddNewsArticle(n);
                if (added != null)
                {
                    Alert("This article has been successfully added", AlertType.success);
                    //Redirects to see the newly added article in the index page
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(n);
        }

        //GET
        public IActionResult Delete(int id)
        {
            var n = svc.GetNewsArticle(id);
            

            if (n == null)
            {
                Alert("Article does not exist", AlertType.warning);
                return RedirectToAction(nameof(Index));
            }

            return View(n);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirm(int id)
        {
            
            svc.DeleteNewsArticle(id);
            Alert("Article has been deleted", AlertType.success);
            return RedirectToAction(nameof(Index));
        }
    }
}