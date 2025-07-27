
using Microsoft.Build.Framework;

namespace Json_Article_Website.Models
{
    public class ArticleDetailsModel
    {
        public int Id { get; set; }
        public int IndexFileName { get; set; } = 1;

        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;

        public DateTime PublishedDate { get; set; } = DateTime.Today;

        public bool IsPublished
        {
            get
            {
                return PublishedDate.Date <= DateTime.Today.Date;
            }
        }
    }
}
