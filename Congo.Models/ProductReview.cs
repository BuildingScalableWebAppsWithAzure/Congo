using System;
using Newtonsoft.Json; 

namespace Congo.Models
{
    public class ProductReview : CongoDocument
    {
        [JsonProperty("reviewername")]
        public string ReviewerName { get; set; }

        [JsonProperty("rating")]
        public decimal Rating { get; set; }

        [JsonProperty("review")]
        public string Review { get; set; }

        [JsonProperty("createdat")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("productid")]
        public string ProductId { get; set; }
    }
}
