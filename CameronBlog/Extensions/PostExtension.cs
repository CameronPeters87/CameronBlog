using CameronBlog.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CameronBlog.Extensions
{
    public static class PostExtension
    {
        public static Post FindPostBySlug(this IDbSet<Post> posts, string slug)
        {
            return posts.Where(p => p.Slug == slug).FirstOrDefault();
        }
    }
}