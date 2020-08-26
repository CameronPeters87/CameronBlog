using CameronBlog.Entities;
using CameronBlog.Extensions;
using CameronBlog.Models;
using CameronBlog.Models.ViewModels;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CameronBlog.Controllers
{
    public class PostsController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();
        // GET: Posts
        public ActionResult Index()
        {
            return View();
        }

        // Get: Writer/Posts/Preview/id
        [ActionName("preview")]
        public async Task<ActionResult> Preview(string slug)
        {
            if (slug == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.FindPostBySlug(slug);
            ViewBag.PageId = await (from p in db.Pages
                                    where p.PostId == post.Id
                                    select p.Id).FirstOrDefaultAsync();
            if(User.IsInRole("Writer"))
            {
                ViewBag.IsWriter = true;
            }
            else
                ViewBag.IsWriter = false;

            if (post == null)
            {
                return HttpNotFound();
            }
            var model = new PostPreviewViewModel(post);

            model.Comments = await db.Comments.Where(c => c.PostId.Equals(model.PostId))
                .ToListAsync();

            return View("Preview", model);
        }

        [HttpPost]
        public async Task<ActionResult> WriteComment(PostPreviewViewModel model)
        {
            if (ModelState.IsValid)
            {
                db.Comments.Add(new Comment
                {
                    Name = model.CommentName,
                    Content = model.CommentContent,
                    PostId = model.PostId,
                    DatePosted = DateTime.Now
                });

                await db.SaveChangesAsync();

                return Redirect("/Posts/Preview/" + model.PostId);
            }
            return View(model);
        }
    }
}