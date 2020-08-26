using CameronBlog.Entities;
using CameronBlog.Models;
using CameronBlog.Models.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CameronBlog.Areas.Writer.Controllers
{
    public class ProfilePicturesController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();
        // GET: Writer/ProfilePictures
        public async Task<ActionResult> Edit()
        {
            string userId = User.Identity.GetUserId();
            var pic = await db.ProfilePictures.Where(p => p.UserId.Equals(userId))
                .FirstOrDefaultAsync();

            if(pic == null)
            {
                return HttpNotFound();
            }

            var model = new ProfilePictureViewModel
            {
                ImageUrl = pic.ImageUrl,
                Id = pic.Id,
                UserId = pic.UserId
            };

            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(ProfilePictureViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Change Bg image
                string UpdatebgImageUrl = "";
                try
                {
                    if (model.Image == null)
                    {
                        UpdatebgImageUrl = model.ImageUrl;
                    }
                    else if (model.Image.ContentLength > 0)
                    {
                        var uploadPath = "~/UploadedImages/ProfilePictures";
                        string _FileName = Path.GetFileName(model.Image.FileName);
                        string UniqueFileName = User.Identity.GetUserId() +
                            "-" + _FileName.ToLower();

                        string _path = Path.Combine(Server
                            .MapPath(uploadPath), UniqueFileName);
                        model.Image.SaveAs(_path);

                        UpdatebgImageUrl = "/UploadedImages/ProfilePictures/" + UniqueFileName;
                    }
                    else {
                        return View(model);
                    }

                    ProfilePicture picture = await db.ProfilePictures
                        .Where(p => p.Id.Equals(model.Id)).FirstOrDefaultAsync();

                    picture.ImageUrl = UpdatebgImageUrl;

                    db.Entry(picture).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    TempData["SM"] = "Your profile picture has been updated";

                    RedirectToAction("Index", "Dashboard");
                }
                catch { }
            }

            return View(model);
        }

        public ActionResult _ProfilePicturePartial()
        {
            string userId = User.Identity.GetUserId();
            ProfilePicture picture = db.ProfilePictures.Where(
                p => p.UserId.Equals(userId)).FirstOrDefault();

            return PartialView(picture);
        }

    }
}