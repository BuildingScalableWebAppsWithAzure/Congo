using System;
using System.Collections.Generic;
using Newtonsoft.Json; 

namespace Congo.Models
{
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

        [JsonProperty("averagerating")]
        public decimal AverageRating { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }
    }
}
