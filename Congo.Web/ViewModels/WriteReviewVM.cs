namespace Congo.Web.ViewModels
{
    /// <summary>
    /// Used for submitting information for a new review. 
    /// </summary>
    public class WriteReviewVM
    {
        public ProductReviewVM Review { get; set; }
        public string ProductName { get; set; }
        public string ProductId { get; set; }
        public string WriteReviewResult { get; set; }
    }
}