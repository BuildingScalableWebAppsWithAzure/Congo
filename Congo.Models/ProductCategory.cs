using Newtonsoft.Json;

namespace Congo.Models
{
    public class ProductCategory : CongoDocument
    {
        [JsonProperty("categoryname")]
        public string CategoryName { get; set; }

        [JsonProperty("categorydescription")]
        public string CategoryDescription { get; set; }
    }
}
