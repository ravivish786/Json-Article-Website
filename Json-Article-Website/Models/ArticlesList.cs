namespace Json_Article_Website.Models
{
    public class ArticlesList
    {
        public int Page { get; set; } = 1;
        public bool HasNextPage { get; set; } = false;
        public IEnumerable<ArticleIndexModel> Articles { get; set; } = new List<ArticleIndexModel>();
    }
}
