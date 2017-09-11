namespace Congo.Web.Models
{
    using System;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Models a single product review. 
    /// </summary>
    public class ProductReview : CongoDocument
    {
        [Required]
        [JsonProperty("reviewername")]
        public string ReviewerName { get; set; }

        [Required]
        [JsonProperty("rating")]
        public decimal Rating { get; set; }

        [Required]
        [JsonProperty("review")]
        public string Review { get; set; }

        [JsonProperty("createdat")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("productid")]
        public string ProductId { get; set; }
    }
}
