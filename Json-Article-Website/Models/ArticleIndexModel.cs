namespace Json_Article_Website.Models
{
    public class ArticleIndexModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public DateTime PublishedDate { get; set; } = DateTime.Now;
        public string ImageUrl { get; set; } = string.Empty;
        
    }
}
