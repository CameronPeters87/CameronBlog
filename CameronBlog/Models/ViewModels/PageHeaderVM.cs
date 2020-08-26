using CameronBlog.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CameronBlog.Models.ViewModels
{
    public class PageHeaderVM
    {
        public PageHeaderVM() { }
        public PageHeaderVM(Page page)
        {
            Title = page.Title;
            Description = page.Description;
            ImageUrl = page.ImageUrl;
        }
        public PageHeaderVM(Post post)
        {
            Title = post.Title;
            Description = post.Description;
            ImageUrl = post.BackgroundImageUrl;
            Author = post.Author;
            DatePosted = post.DatePosted;
            isPost = true;
            atHome = false;
        }


        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Author { get; set; }
        public DateTime DatePosted { get; set; }
        public bool atHome { get; set; }
        public bool isPost { get; set; } 
    }
}