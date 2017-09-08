using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Congo.Web.ViewModels
{
    public class WriteReviewVM
    {
        public ProductReviewVM Review { get; set; }
        public string ProductName { get; set; }
        public string ProductId { get; set; }
        public string WriteReviewResult { get; set; }
    }
}