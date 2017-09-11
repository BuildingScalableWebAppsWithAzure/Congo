namespace Congo.Web.ViewModels
{
    using System.Collections.Generic;

    /// <summary>
    /// Holds a list of all products returned by a query. 
    /// </summary>
    public class ProductSearchResultsVM
    {
        public List<ProductVM> ProductResults { get; set; }
    }
}