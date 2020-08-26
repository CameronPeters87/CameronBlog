using CameronBlog.Models;
using CameronBlog.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CameronBlog.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Redirect ()
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
            else if (User.IsInRole("Writer"))
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Writer" });
            }
            else
            {
                return RedirectToAction("FrontPage", "Home");
            }
        }

        public async Task<ActionResult> FrontPage(int pageId = 3)
        {
            ViewBag.PageId = pageId;
            ViewBag.IsWriter = false;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var model = await (from p in db.Posts
                                   select new PostThumbnailViewModel
                                   {
                                       Author = p.Author,
                                       DatePosted = p.DatePosted,
                                       Description = p.Description,
                                       Link = "/Posts/preview/" + p.Slug,
                                       Title = p.Title,
                                       ProfileLink = "#",
                                       BackgroundImageUrl = p.BackgroundImageUrl
                                   }).ToListAsync();

                return View(model);
            }
        }

        public ActionResult About(int pageId = 6)
        {
            ViewBag.PageId = pageId;
            ViewBag.IsWriter = false;
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact(int pageId = 7)
        {
            ViewBag.PageId = pageId;
            ViewBag.IsWriter = false;
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}