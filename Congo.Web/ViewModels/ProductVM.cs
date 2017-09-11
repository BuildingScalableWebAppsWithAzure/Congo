namespace Congo.Web.ViewModels
{
    using System.Collections.Generic;

    /// <summary>
    /// Holds information about a single product. This includes reviews and 
    /// attributes. 
    /// </summary>
    public class ProductVM
    {
        public ProductVM()
        {
            this.Reviews = new List<ProductReviewVM>();
            this.Attributes = new List<AttributeVM>(); 
        }

        public string Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal? AverageRating { get; set; }
        public List<ProductReviewVM> Reviews { get; set; }
        public List<AttributeVM> Attributes { get; set; }
    }
}