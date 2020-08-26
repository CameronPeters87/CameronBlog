using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CameronBlog.Entities
{
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public string BackgroundImageUrl { get; set; }
        public string ThumbnailImageUrl { get; set; }
        public string Author { get; set; }
        public DateTime DatePosted { get; set; }
        public string Article { get; set; }
        public int Likes { get; set; }
        public int NumberOfComments { get; set; }
        public bool AnonymousAuthor { get; set; }
        public int CommentId { get; set; }
        public string WriterId { get; set; }
    }
}