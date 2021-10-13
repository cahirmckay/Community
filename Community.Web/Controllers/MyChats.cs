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
//MISC
namespace Community.Web.Controllers
{
    [Authorize]
    public class MyChatsController : BaseController
    {
        private IPostService svc;
        private IUserService userService;
        
        public MyChatsController(IPostService ps, IUserService us)
        {
            svc = ps;
            userService = us;
        }

        public IActionResult Index()
        {
            var user = userService.GetUser(GetSignedInUserId());
            var p = svc.GetAllPosts(user);
            return View(p);
        }

        public IActionResult Details(int id)
        {
            var post= svc.GetPost(id);
            
            if (post == null)
            {
                Alert("Post does not exist", AlertType.warning);
                //return to index to look for Post
                return RedirectToAction(nameof(Index));
            }
         
            return View(post);
        } 

        //GET
        public IActionResult Edit(int id)
        {
            var post = svc.GetPost(id);

            if (post == null)
            {
                Alert("Post does not exist", AlertType.warning);
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        [HttpPost]
        public IActionResult Edit(int id, Post p)
        {
            if (ModelState.IsValid)
            {
                svc.UpdatePost(p);
                Alert("Your post has been updated ", AlertType.info);
                return RedirectToAction(nameof(Index));
            }

            return View(p);
        }

        //GET
        public IActionResult Create()
        {
            var p =  new Post();
            
            return View(p);
        }

        [HttpPost]
        public IActionResult Create(Post p)
        {
            var user = userService.GetUser(GetSignedInUserId());
            p.Name= user.Name;
            p.CommunityId = user.CommunityId;
            p.CreatedOn = DateTime.Now;
            if (ModelState.IsValid)
            {
                var added = svc.AddPost(p);
                if (added != null)
                {
                    Alert("Your post has been successfully added", AlertType.success);
                    //Redirects to see the newly added post in the index page
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(p);
        }

        //GET
        public IActionResult Delete(int id)
        {
            var p = svc.GetPost(id);
            

            if (p == null)
            {
                Alert("Post does not exist", AlertType.warning);
                return RedirectToAction(nameof(Index));
            }

            return View(p);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirm(int id)
        {
            var post = svc.GetPost(id);
            svc.DeletePost(id);
            Alert("Post has been deleted", AlertType.success);
            return RedirectToAction(nameof(Index));
        }

        //========Comment Operations===================================
        //-----Comment Creation from posts view
        //GET
        public IActionResult AddComment(int id)
        {
            var post = svc.GetPost(id);

            if (post == null)
            {
                Alert("Postdoes not exist", AlertType.warning);
                return RedirectToAction(nameof(Index));
            }

            var user = userService.GetUser(GetSignedInUserId());
            var comment = new Comment
            {
                PostId = id, 
                CreatedOn = DateTime.Now,
                Name = user.Name,
            };

            return View("AddComment", comment);
        }

        //post /posts/addcomment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddComment(Comment c)
        {
            var post = svc.GetPost(c.PostId);
            if (post == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                svc.AddComment(c);
                Alert($"Comment by {c.Name} has been added", AlertType.success);
                return RedirectToAction("Details", new{ Id = c.PostId});
            }

            return View("AddComment", c);
        }

        //GET
        public IActionResult DeleteComment(int id)
        {
            var comment = svc.GetCommentById(id);

            if (comment == null)
            {
                return NotFound();
            }
            //view delete comment page
            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmComment(int id)
        {
            var comment = svc.GetCommentById(id);
            var post = svc.GetPost(comment.PostId);
            svc.DeleteComment(id);
            Alert($"Comment by {comment.Name} has deleted successfully", AlertType.success);
            //after deleting a review return to the assoiated business so the user can see it's been deleted
            return RedirectToAction("Details", new{ Id = comment.PostId});
        }
    }

}