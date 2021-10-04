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
            foreach (var file in files)
            {
                var img = new Photo();
                img.PhotoTitle = file.FileName;
                var user = userService.GetUser(GetSignedInUserId());
                MemoryStream ms = new MemoryStream();
                file.CopyTo(ms);
                img.PhotoData = ms.ToArray();
                img.CommunityId = user.CommunityId;
                ms.Close();
                ms.Dispose();

                svc.AddPhoto(img);
                if (img != null)
                 {
                     Alert($"{p.PhotoTitle} has been successfully added", AlertType.success);
                     //Redirects to see the newly added movie in the index page
                     return RedirectToAction(nameof(Index));
                 }
            }
            
            return View("Index");
        }

       

        // [HttpPost]
        // public IActionResult Create(Photo p)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         var added = svc.AddPhoto(p);
        //         if (added != null)
        //         {
        //             Alert($"{p.PhotoTitle} has been successfully added", AlertType.success);
        //             //Redirects to see the newly added movie in the index page
        //             return RedirectToAction(nameof(Index));
        //         }
        //     }

        //     return View(p);
        // }
        // [HttpPost]
        // public ActionResult RetrieveImage()
        // {
        //     var user = userService.GetUser(GetSignedInUserId());
        //     List<Photos> img = svc.GetAllPhotos(user);//.OrderByDescending
        //     //(i => i.Id).SingleOrDefault();
        //     string imageBase64Data = Convert.ToBase64String(img.PhotoData);
        //     string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
        //     ViewBag.ImageTitle = img.PhotoTitle;
        //     ViewBag.ImageDataUrl = imageDataURL;
        //     return View("Index");
        // }
    }

}