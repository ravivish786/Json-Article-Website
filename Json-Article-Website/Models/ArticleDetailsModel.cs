
using Microsoft.Build.Framework;

namespace Json_Article_Website.Models
{
    public class ArticleDetailsModel
    {
        public int Id { get; set; }
 
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.Today;
    }
}
