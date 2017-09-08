using System.Web.Mvc;
using System.Threading.Tasks;
using Congo.Web.ViewModels;
using Congo.Web.Services;
using System.Collections.Generic;
using System; 

namespace Congo.Web.Controllers
{
    public class HomeController : Controller
    {
        private ProductService _productService = new ProductService(); 

        /// <summary>
        /// Returns the homepage that includes the product search fields. 
        /// </summary>
        public async Task<ActionResult> Index()
        {
            ViewBag.Categories = await _productService.GetAllCategories(); 
            return View();
        }

        /// <summary>
        /// Searches our Cosmos DB Collection for all product documents matching 
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> SearchForProducts(ProductSearchRequestVM searchRequest)
        {
            ViewBag.Categories = await _productService.GetAllCategories();

            //get our search results...
            ProductSearchResultsVM searchResults = await _productService.SearchForProducts(searchRequest.CategoryId, searchRequest.ProductName);
            ViewBag.SearchResults = searchResults; 
            return View("Index");
        }

        /// <summary>
        /// Renders a page where the user can write a review for the requested product. 
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> WriteReview(string id)
        {
            ProductReviewVM reviewVm = new ProductReviewVM();
            ProductVM product = await _productService.GetProductById(id);
            WriteReviewVM vm = new WriteReviewVM();
            vm.Review = reviewVm; 
            vm.ProductId = id;
            vm.ProductName = product.ProductName; 
            return View(vm); 
        }

        /// <summary>
        /// Submits a new review from a user. After saving the review, this method will
        /// redirect the user back to the product page. 
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> WriteReview(WriteReviewVM model)
        {
            try
            {
                model.Review.ProductId = model.ProductId; 
                await _productService.WriteReviewToDatabase(model.Review);
                model.WriteReviewResult = "Your review was successfully added.";
            }
            catch (Exception)
            {
                model.WriteReviewResult = "There was a problem adding your review.";
            }
            return View(model);
        }

        /// <summary>
        /// Shows the edit product page for the specified product. 
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> EditProduct(string id)
        {
            ProductVM product = await _productService.GetProductById(id);
            return View(product); 
        }

        /// <summary>
        /// Handles the postback that contains edits made to a product. This will
        /// write edits to our Collection, then redirect the caller back to the product details
        /// page. 
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> EditProduct(ProductVM vm)
        { 

            await _productService.UpdateProduct(vm);
            //ProductVM updatedVm = await _productService.GetProductById(vm.Id);
            return RedirectToAction("Product", new { id = vm.Id });
        }

        /// <summary>
        /// Returns a partial view populated with all reviews for a product. 
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> AllReviews(string id)
        {
            List<ProductReviewVM> allReviews = await _productService.GetAllReviewsForProduct(id);
            return View(allReviews);
        }

        /// <summary>
        /// displays the details for a single product. 
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> Product(string id)
        {
            ProductVM product = await _productService.GetProductById(id);
            return View(product); 
        }
    }
}