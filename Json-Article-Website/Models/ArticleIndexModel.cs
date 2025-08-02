namespace Json_Article_Website.Models
{
    public class ArticleIndexModel
    {
        public int Id { get; set; }
        public virtual string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public DateTime? PublishedDate { get; set; } = DateTime.Now;
        public string ImageUrl { get; set; } = string.Empty;

        public bool IsPublished
        {
            get
            {
                return PublishedDate?.Date <= DateTime.Now.Date;
            }
        }

        public int Views { get; set; } = 0;
        public int Likes { get; set; } = 0;

        public int CommentsCount { get; set; } = 0;
        public int ReadingTime { get; set; } = 0;

    }
}
