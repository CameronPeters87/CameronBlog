using CameronBlog.Models;
using CameronBlog.Models.ViewModels;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CameronBlog.Extensions
{
    public static class ConversionExtension
    {
        // AssignPageDetails
        public static PageHeaderVM GetPageHeaderAsync(int pageId, ApplicationDbContext db)
        {
            if (db == null) db = ApplicationDbContext.Create();

            var model = (from p in db.Pages
                         where p.Id == pageId
                         select new PageHeaderVM
                         {
                             Title = p.Title,
                             Description = p.Description,
                             ImageUrl = p.ImageUrl,
                             isPost = p.isPost,
                             atHome = p.atHome,
                         }).FirstOrDefault();

            return model;
        }
        public static PageHeaderVM GetPostPageHeaderAsync(
            int pageId, ApplicationDbContext db)
        {
            if (db == null) db = ApplicationDbContext.Create();

            var model = (from p in db.Pages
                         join po in db.Posts on p.PostId equals po.Id
                         where p.Id.Equals(pageId)
                         select new PageHeaderVM
                         {
                             Title = p.Title,
                             Description = p.Description,
                             ImageUrl = p.ImageUrl,
                             isPost = p.isPost,
                             atHome = p.atHome,
                             Author = po.Author,
                             DatePosted = po.DatePosted
                         }).FirstOrDefault();

            return model;
        }
    }
}