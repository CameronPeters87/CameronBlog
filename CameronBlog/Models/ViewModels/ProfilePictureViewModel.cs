using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CameronBlog.Models.ViewModels
{
    public class ProfilePictureViewModel
    {
        public int Id { get; set; }
        public HttpPostedFileBase Image { get; set; }
        public string ImageUrl { get; set; }
        public string UserId { get; set; }
    }
}