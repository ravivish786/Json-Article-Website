
using System.ComponentModel.DataAnnotations;
using AngleSharp.Html;
using Microsoft.Build.Framework;

namespace Json_Article_Website.Models
{
    public class ArticleDetailsModel : ArticleIndexModel
    {
        //public int Id { get; set; }
        public int IndexFileName { get; set; } = 1;

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Title required.")]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters.")]
        public override string Title { get; set; } = string.Empty;

        //public string Slug { get; set; } = string.Empty;

        //public string ImageUrl { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;

        //public DateTime PublishedDate { get; set; } = DateTime.Today;

        //public bool IsPublished
        //{
        //    get
        //    {
        //        return PublishedDate.Date <= DateTime.Today.Date;
        //    }
        //}

         public string Keywords { get; set; } = string.Empty;
    }
}
