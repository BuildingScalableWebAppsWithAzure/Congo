using Newtonsoft.Json; 

namespace Congo.Web.Models
{
    /// <summary>
    /// products have differing attributes based on the type of product. 
    /// This acts as a key value pair for a single attribute. Note that this doesn't
    /// inherit from CongoDocument because it will always be part of a Product document. 
    /// </summary>
    public class ProductAttribute
    {
        [JsonProperty("attribute")]
        public string Attribute { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
