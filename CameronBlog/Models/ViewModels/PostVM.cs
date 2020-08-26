using CameronBlog.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace CameronBlog.Models.ViewModels
{
    public class PostVM
    {
    }

    public class PostCreateViewModel
    {
        public PostCreateViewModel() { }
        public PostCreateViewModel(Post p)
        {
            Id = p.Id;
            Title = p.Title;
            Description = p.Description;
            Article = p.Article;
            AnonymousAuthor = p.AnonymousAuthor;
            BackgroundImageUrl = p.BackgroundImageUrl;
            WriterId = p.WriterId;
        }
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [MaxLength(int.MaxValue)]
        [AllowHtml]
        public string Article { get; set; }
        [DisplayName("Anonymous Author")]
        public bool AnonymousAuthor { get; set; }
        public HttpPostedFileBase BackgroundImage { get; set; }
        public string BackgroundImageUrl { get; set; }
        public string WriterId { get; set; }
    }

    public class PostListViewModel
    {
        public PostListViewModel() { }
        public PostListViewModel(Post p)
        {
            Id = p.Id;
            Title = p.Title;
            DatePosted = p.DatePosted;
            Description = p.Description;
            NumberOfComments = p.NumberOfComments;
            Likes = p.Likes;
            Slug = p.Slug;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        [DataType(DataType.Date)]
        public DateTime DatePosted { get; set; }
        [DisplayName("Comments")]
        public int NumberOfComments { get; set; }
        public int Likes { get; set; }
        public string WriterId { get; set; }
        public int CommentId { get; set; }
    }

    public class PostPreviewViewModel
    {
        public PostPreviewViewModel () { }
        public PostPreviewViewModel(Post post)
        {
            PostId = post.Id;
            Title = post.Title;
            Description = post.Description;
            BackgroundImageUrl = post.BackgroundImageUrl;
            Author = post.Author;
            DatePosted = post.DatePosted;
            Article = post.Article;
            Likes = post.Likes;
            AnonymousAuthor = post.AnonymousAuthor;
            Slug = post.Slug;
        }

        public int PostId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public string BackgroundImageUrl { get; set; }
        public string Author { get; set; }
        [DataType(DataType.Date)]
        public DateTime DatePosted { get; set; }
        public string Article { get; set; }
        public int Likes { get; set; }
        public bool AnonymousAuthor { get; set; }

        // Comments Section
        public ICollection<Comment> Comments { get; set; }
        public string CommentName { get; set; }
        public string CommentContent { get; set; }
        public int CommentId { get; set; }
    }

    public class PostThumbnailViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        [DataType(DataType.Date)]
        public DateTime DatePosted { get; set; }
        public string Link { get; set; }
        public string ProfileLink { get; set; }
        public string BackgroundImageUrl { get; set; }

    }
}