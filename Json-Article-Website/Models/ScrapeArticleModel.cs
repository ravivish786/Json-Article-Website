using Newtonsoft.Json;

namespace Json_Article_Website.Models
{
    public class ScrapeArticleModel
    {
        [JsonProperty("title")]
        public string Title { get; set; } = string.Empty;

        [JsonProperty("dDescription")]
        public string Description { get; set; } = string.Empty;
        
        [JsonProperty("image")]
        public string Image { get; set; } = string.Empty;

    }
}
