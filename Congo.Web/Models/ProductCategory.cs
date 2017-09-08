using Newtonsoft.Json;

namespace Congo.Web.Models
{
    /// <summary>
    /// Models a single product category. 
    /// </summary>
    public class ProductCategory : CongoDocument
    {
        [JsonProperty("categoryname")]
        public string CategoryName { get; set; }

        [JsonProperty("categorydescription")]
        public string CategoryDescription { get; set; }
    }
}
