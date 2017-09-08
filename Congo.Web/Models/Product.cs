using Newtonsoft.Json; 

namespace Congo.Web.Models
{
    /// <summary>
    /// Models a single product. 
    /// </summary>
    public class Product : CongoDocument
    {
        [JsonProperty("categoryid")]
        public string CategoryId { get; set; }

        [JsonProperty("productname")]
        public string ProductName { get; set; }

        [JsonProperty("attributes")]
        public ProductAttribute [] Attributes { get; set; }

        [JsonProperty("reviews")]
        public ProductReview [] TopReviews { get; set; }

        [JsonProperty("firstreview")]
        public ProductReview FirstReview { get; set; }

        [JsonProperty("averagerating")]
        public decimal AverageRating { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("partitionkey")]
        public string PartitionKey { get; set; }
    }
}
