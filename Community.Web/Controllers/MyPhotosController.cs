using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using Community.Core.Models;
using Community.Data.Services;

//MISC
namespace Community.Web.Controllers
{
    [Authorize]
    public class MyPhotosController : BaseController
    {
        //user service passed in also to be able pass the current users CommunityId to the service layer
        private IPhotoService svc;
        private IUserService userService;

        // Configured via DI
        public MyPhotosController(IPhotoService ps, IUserService us)
        {
            svc = ps;
            userService = us;
        }

        public IActionResult Index()
        {
            var user = userService.GetUser(GetSignedInUserId());
            var gallery = svc.GetAllPhotos(user);
            return View(gallery);
        }

        ///GET
        public IActionResult Create()
        {
            var p = new Photo();

            return View(p);
        }

        [HttpPost]
        public IActionResult Create(Photo p)
        {
            var files = HttpContext.Request.Form.Files;
            //reads image from memory stream and converts to a string to store in the DB
            foreach (var file in files)
            {
                
                var img = new Photo();
                img.PhotoTitle = p.PhotoTitle;
                var user = userService.GetUser(GetSignedInUserId());
                MemoryStream ms = new MemoryStream();
                file.CopyTo(ms);
                img.PhotoData = ms.ToArray();
                img.CommunityId = user.CommunityId;
                img.Description = p.Description;
                ms.Close();
                ms.Dispose();

                string PhotoBase64Data = Convert.ToBase64String(img.PhotoData);
                string PhotoDataURL = string.Format("data:Photo/jpg;base64,{0}", PhotoBase64Data);
                
                img.PhotoDataUrl = PhotoDataURL;

                svc.AddPhoto(img);
                if (img != null)
                {
                    Alert($"{img.PhotoTitle} has been successfully added", AlertType.success);
                    //Redirects to see the newly added movie in the index page
                    return RedirectToAction(nameof(Index));
                }

            }

            return View("Index");
        }

        public IActionResult Details(int id)
        {
            var photo = svc.GetPhoto(id);
            
            if (photo == null)
            {
                Alert("Photo does not exist", AlertType.warning);
                //return to index to look for Photo
                return RedirectToAction(nameof(Index));
                
            }
         
            return View(photo);
        } 

        //get
        public IActionResult Delete(int id)
        {
            var p = svc.GetPhoto(id);

            if (p == null)
            {
                Alert("Photo does not exist", AlertType.warning);
                return RedirectToAction(nameof(Index));
            }
            return View(p);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirm(int id)
        {
            var p = svc.GetPhoto(id);
            svc.DeletePhoto(id);
            //Alert($"{p.PhotoTitle} has been deleted successfully", AlertType.success);
            return RedirectToAction(nameof(Index));
        }
    }

}