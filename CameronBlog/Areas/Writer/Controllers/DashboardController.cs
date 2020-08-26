using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CameronBlog.Areas.Writer.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Writer/Dashboard
        public ActionResult Index()
        {
            return View();
        }
    }
}