using CameronBlog.Entities;
using CameronBlog.Models;
using CameronBlog.Models.ViewModels;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CameronBlog.Areas.Writer.Controllers
{
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Writer/Posts
        [Authorize(Roles = "Writer")]
        public async Task<ActionResult> Index()
        {
            string userId = User.Identity.GetUserId();
            //var posts = await db.Posts.Where(p => p.WriterId.Equals(userId)).ToListAsync();
            var model = await (from p in db.Posts
                               where p.WriterId == userId
                               orderby p.DatePosted
                               select new PostListViewModel
                               {
                                   Id = p.Id,
                                   Title = p.Title,
                                   Description = p.Description,
                                   Likes = p.Likes,
                                   NumberOfComments = p.NumberOfComments,
                                   DatePosted = p.DatePosted,
                                   Slug = p.Slug
                               }).ToListAsync();
            return View(model);
        }

        // Get: Writer/Posts/CreatePost
        [Authorize(Roles = "Writer")]
        public ActionResult CreatePost()
        {
            var model = new PostCreateViewModel();
            return View(model);
        }
        // Post: Writer/Posts/CreatePost
        [HttpPost]
        public async Task<ActionResult> CreatePost(PostCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string slug = model.Title.Replace(" ", "-");
                slug.ToLower();
                if(db.Posts.Any(p => p.Title == model.Title))
                {
                    ViewBag.Error = "That Title already exists";
                    return View(model);
                }
                // Set Author Name
                string Author = "";
                if (model.AnonymousAuthor)
                {
                    Author = "Anonymous";
                }
                else
                {
                    Author = IdentityExtension.GetUserFirstName(User.Identity);
                }

                //Upload Background Image
                string bgImageUrl = "";
                try
                {
                    if (model.BackgroundImage == null)
                    {
                        bgImageUrl = "/Content/Theme/img/post-bg.jpg";
                    }
                    else if (model.BackgroundImage.ContentLength > 0)
                    {
                        var uploadPath = "~/UploadedImages/BackgroundImages";
                        string _FileName = Path.GetFileName(model.BackgroundImage.FileName);
                        string UniqueFileName = User.Identity.GetUserId() + 
                            "-" + _FileName.ToLower();

                        string _path = Path.Combine(Server
                            .MapPath(uploadPath), UniqueFileName);
                        model.BackgroundImage.SaveAs(_path);

                        bgImageUrl = "/UploadedImages/BackgroundImages/" + UniqueFileName;
                    }
                    else { }
                }
                catch
                {
                    ViewBag.Error = "Image upload failed!!";
                    return View(model);
                }

                // Add Post
                db.Posts.Add(new Post
                {
                    Title = model.Title,
                    Description = model.Description,
                    Article = model.Article,
                    NumberOfComments = 0,
                    Likes = 0,
                    DatePosted = DateTime.Now,
                    Author = Author,
                    AnonymousAuthor = model.AnonymousAuthor,
                    BackgroundImageUrl = bgImageUrl,
                    Slug = slug,
                    WriterId = User.Identity.GetUserId()
                });

                await db.SaveChangesAsync();

                await CreatePage(model, bgImageUrl);

                TempData["SM"] = "You have successfully created a post";

                return RedirectToAction("Index");
            }

            return View(model);
        }

        // Get: Writer/Posts/EditPost?id
        [Authorize(Roles = "Writer")]
        public async Task<ActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = await db.Posts.FindAsync(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            var model = new PostCreateViewModel(post);
            return View(model);
        }

        // Post: Writer/Posts/EditPost
        [HttpPost]
        public async Task<ActionResult> EditPost(PostCreateViewModel model)
        {
            try
            {
                if (model == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                if (ModelState.IsValid)
                {
                    if(db.Pages.Any(p => p.Title == model.Title))
                    {
                        ViewBag.Error = "That title is taken.";
                        return View(model);
                    }
                    // Check Author
                    string Author = "";
                    if (model.AnonymousAuthor)
                    {
                        Author = "Anonymous";
                    }
                    else
                    {
                        Author = IdentityExtension.GetUserFirstName(User.Identity);
                    }

                    // Change Bg image
                    string UpdatebgImageUrl = "";
                    try
                    {
                        if (model.BackgroundImage == null)
                        {
                            UpdatebgImageUrl = model.BackgroundImageUrl;
                        }
                        else if (model.BackgroundImage.ContentLength > 0)
                        {
                            var uploadPath = "~/UploadedImages/BackgroundImages";
                            string _FileName = Path.GetFileName(model.BackgroundImage.FileName);
                            string UniqueFileName = User.Identity.GetUserId() +
                                "-" + _FileName.ToLower();

                            string _path = Path.Combine(Server
                                .MapPath(uploadPath), UniqueFileName);
                            model.BackgroundImage.SaveAs(_path);

                            UpdatebgImageUrl = "/UploadedImages/BackgroundImages/" + UniqueFileName;
                        }
                        else { }
                    }
                    catch
                    {
                        ViewBag.Error = "Image upload failed!!";
                        return View(model);
                    }

                    Post post = await db.Posts.Where(p => p.Id.Equals(model.Id))
                        .FirstOrDefaultAsync();
                    Page page = await db.Pages.Where(p => p.PostId.Equals(model.Id))
                        .FirstOrDefaultAsync();

                    post.Title = model.Title;
                    post.Description = model.Description;
                    post.Article = model.Article;
                    post.AnonymousAuthor = model.AnonymousAuthor;
                    post.Author = Author;
                    post.BackgroundImageUrl = UpdatebgImageUrl;

                    page.ImageUrl = UpdatebgImageUrl;
                    page.Title = post.Title;

                    db.Entry(post).State = EntityState.Modified;
                    db.Entry(page).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    TempData["SM"] = "You have successfully edited a post";
                    return RedirectToAction("Index");
                }
            }
            catch { }

            return View(model);
        }

        // Post: Writer/Posts/RemovePost?id
        [Authorize(Roles = "Writer")]
        public async Task<ActionResult> RemovePost (int id)
        {
            try
            {
                var post = await db.Posts.Where(p => p.Id == id)
                    .FirstOrDefaultAsync();
                var page = await db.Pages.Where(p => p.PostId == id)
                    .FirstOrDefaultAsync();
                if (post == null)
                {
                    return HttpNotFound();
                }

                db.Posts.Remove(post);
                db.Pages.Remove(page);
                await db.SaveChangesAsync();

                TempData["SMRemove"] = "You removed a post";
            }
            catch { }

            return RedirectToAction("Index");
        }

        // Get: Writer/Posts/Preview/id
        [Authorize(Roles = "Writer")]
        public async Task<ActionResult> Preview(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.PageId = await (from p in db.Pages
                                    where p.PostId == id
                                    select p.Id).FirstOrDefaultAsync();
            ViewBag.IsWriter = true;

            Post post = db.Posts.Find(id);

            if (post == null)
            {
                return HttpNotFound();
            }
            var model = new PostPreviewViewModel(post);

            return View(model);
        }

        public async Task CreatePage (PostCreateViewModel model, string bgImageUrl)
        {
            int postId = await (from p in db.Posts
                                where p.Title == model.Title
                                select p.Id).FirstOrDefaultAsync();

            db.Pages.Add(new Page
            {
                PostId = postId,
                atHome = false,
                Description = model.Description,
                isPost = true,
                Title = model.Title,
                ImageUrl = bgImageUrl,
            });

            await db.SaveChangesAsync();
        }
    }
}