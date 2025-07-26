namespace Json_Article_Website.Models
{
    public struct ArticleMetaConst
    {
        public const int MaxRowsPerIndexFile = 15;

        public ArticleMetaConst()
        {
        }
    }
    public sealed class ArticleFileMetadata
    {
        public int LastIndexFile { get; set; } = 1;
        public int LastArticleId { get; set; } = 0;
    }
     
}
