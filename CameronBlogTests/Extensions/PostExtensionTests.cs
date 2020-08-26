using Microsoft.VisualStudio.TestTools.UnitTesting;
using CameronBlog.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CameronBlog.Entities;
using CameronBlog.Models;

namespace CameronBlog.Extensions.Tests
{
    [TestClass()]
    public class PostExtensionTests
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [TestMethod()]
        public void FindPostBySlugTest()
        {
            var actual = db.Posts.Where(p => p.Slug == "hello").FirstOrDefault();
            var result = db.Posts.FindPostBySlug("hello");

            Assert.AreEqual(result, actual);
        }
    }
}