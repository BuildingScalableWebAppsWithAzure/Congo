using System;

namespace Congo.Web.ViewModels
{
    public class ProductReviewVM
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string ReviewerName { get; set; }
        public decimal Rating { get; set; }
        public string Review { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}