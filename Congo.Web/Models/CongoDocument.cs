using Newtonsoft.Json; 

namespace Congo.Web.Models
{
    /// <summary>
    /// this is the parent class for all of our DocumentDB documents. 
    /// Since we're storing documents with different schemas, the DocType property
    /// will let us specify the document type. In this project, that will be either be 
    /// "product", "review", or "category". 
    /// </summary>
    public class CongoDocument
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("doctype")]
        public string DocType { get; set; }
    }
}
