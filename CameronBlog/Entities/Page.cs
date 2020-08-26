using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CameronBlog.Entities
{
    public class Page
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool isPost { get; set; }
        public bool atHome { get; set; }
        public int PostId { get; set; }
    }
}