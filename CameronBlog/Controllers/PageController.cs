using CameronBlog.Entities;
using CameronBlog.Extensions;
using CameronBlog.Models;
using CameronBlog.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CameronBlog.Controllers
{
    public class PageController : Controller
    {
        // GET: Page
        public ActionResult _PageHeaderPartial(int pageId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Page page = db.Pages.Find(pageId);

                if(page.isPost == false)
                {
                    var model = ConversionExtension.GetPageHeaderAsync(pageId, db);
                    return PartialView(model);
                }
                else
                {
                    var model = ConversionExtension.GetPostPageHeaderAsync(pageId, db);
                    return PartialView(model);
                }
            }
        }
    }
}