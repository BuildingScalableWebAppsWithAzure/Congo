using System.Collections.Generic;
using Congo.Web.ViewModels;
using System.Threading.Tasks;
using Congo.Web.Persistence;
using Congo.Web.Models;

namespace Congo.Web.Services
{
    /// <summary>
    /// Business logic for interacting with products, categories, and reviews. 
    /// </summary>
    public class ProductService
    {
        private ProductRepository _repository; 

        /// <summary>
        /// Constructor
        /// </summary>
        public ProductService()
        {
            _repository = new ProductRepository(); 
        }
        /// <summary>
        /// Returns all product categories defined in the system. 
        /// </summary>
        /// <returns></returns>
        public async Task<List<CategoryVM>> GetAllCategories()
        {
            List<ProductCategory> allCategories = await _repository.GetAllCategories();
            List<CategoryVM> results = CopyCategoriesToViewModel(allCategories);
            return results; 
        }

        /// <summary>
        /// Searches for all products whose productName property contains the value specified in productName. If
        /// a categoryId is specified, we will restrict the search to products within the specified category. 
        /// </summary>
        public async Task<ProductSearchResultsVM> SearchForProducts(string categoryId, string productName)
        {
            List<Product> retrievedProducts = await _repository.SearchForProducts(categoryId, productName);
            ProductSearchResultsVM results = CopyProductSearchResultsToViewModel(retrievedProducts);
            return results; 
        }

        /// <summary>
        /// Writes the values found in vm back to our Cosmos DB collection. 
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public async Task UpdateProduct(ProductVM vm)
        {
            Product updatedProduct = new Product();
            updatedProduct.Id = vm.Id;
            updatedProduct.ProductName = vm.ProductName;
            updatedProduct.Description = vm.Description;
            updatedProduct.Price = vm.Price;

            await _repository.UpdateProduct(updatedProduct);
        }

        /// <summary>
        /// Adds a review document to the Collection. 
        /// </summary>
        public async Task WriteReviewToDatabase(ProductReviewVM review)
        {
            ProductReview model = new ProductReview();
            model.DocType = Constants.DOCTYPE_REVIEW;
            model.PartitionKey = "review-" + review.ProductId;
            model.Rating = review.Rating;
            model.Review = review.Review;
            model.ReviewerName = review.ReviewerName;
            model.ProductId = review.ProductId; 
            await _repository.CreateItemAsync(model);
        }

        /// <summary>
        /// Retrieves all reviews for a given product. 
        /// </summary>
        public async Task<List<ProductReviewVM>> GetAllReviewsForProduct(string productId)
        {
            List<ProductReview> allReviews = await _repository.GetReviewsForProduct(productId);
            List<ProductReviewVM> allReviewsVM = new List<ProductReviewVM>();
            foreach (ProductReview pr in allReviews)
            {
                ProductReviewVM vm = CopyProductReviewToViewModel(pr);
                allReviewsVM.Add(vm); 
            }
            return allReviewsVM; 
        }

        /// <summary>
        /// Searches for a product by productId. Will return a productVM instance if found.
        /// Otherwise returns null. 
        /// </summary>
        public async Task<ProductVM> GetProductById(string productId)
        {
            Product p = await _repository.GetProductById(productId);
            if (p != null)
            {
                ProductVM result = CopyProductToViewModel(p);
                return result; 
            }
            return null; 
        }

        #region Helper Methods

        private ProductSearchResultsVM CopyProductSearchResultsToViewModel(List<Product> products)
        {
            ProductSearchResultsVM resultsVM = new ProductSearchResultsVM();
            resultsVM.ProductResults = new List<ProductVM>();
            foreach (Product p in products)
            {
                resultsVM.ProductResults.Add(CopyProductToViewModel(p));
            }
            return resultsVM; 
        }

        private ProductVM CopyProductToViewModel(Product p)
        {
            ProductVM productVm = new ProductVM()
            {
                Id = p.Id,
                ProductName = p.ProductName,
                Description = p.Description,
                Price = p.Price,
                AverageRating = p.AverageRating
            };

            foreach (ProductAttribute attribute in p.Attributes)
            {
                productVm.Attributes.Add(new AttributeVM()
                {
                    Attribute = attribute.Attribute, 
                    Value = attribute.Value
                });
            }

            //see if there's a first review. If so, add it. 
            if (p.FirstReview != null)
            {
                ProductReviewVM reviewVM = CopyProductReviewToViewModel(p.FirstReview);
                productVm.Reviews.Add(reviewVM);
            }
            return productVm; 
        }

        private ProductReviewVM CopyProductReviewToViewModel(ProductReview pr)
        {
            ProductReviewVM reviewVm = new ProductReviewVM()
            {
                Id = pr.Id, 
                ReviewerName = pr.ReviewerName, 
                Rating = pr.Rating, 
                Review = pr.Review, 
                CreatedAt = pr.CreatedAt
            };
            return reviewVm; 
        }

        private List<CategoryVM> CopyCategoriesToViewModel(List<ProductCategory> categories)
        {
            List<CategoryVM> categoriesVM = new List<CategoryVM>(); 
            foreach (ProductCategory c in categories)
            {
                categoriesVM.Add(new CategoryVM(c.Id, c.CategoryName, c.CategoryDescription));
            }
            return categoriesVM; 
        }

        #endregion 
    }
}