namespace Congo.Web.ViewModels
{
    /// <summary>
    /// Our view model for a single product category. 
    /// </summary>
    public class CategoryVM
    {
        public CategoryVM() { }

        /// <summary>
        /// Convenience method for copying from a ProductCategory model to a CategoryVM. 
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="categoryDescription"></param>
        public CategoryVM(string id, string categoryName, string categoryDescription)
        {
            this.Id = id;
            this.CategoryName = categoryName; 
            this.CategoryDescription = categoryDescription; 
        }

        public string Id { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
    }
}