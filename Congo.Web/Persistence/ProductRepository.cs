namespace Congo.Web.Persistence
{
    using System;
    using System.Collections.Generic;
    using Congo.Web.Models;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using System.Threading.Tasks;
    using Microsoft.Azure.Documents.Linq;

    public class ProductRepository
    {
        //a single DocumentClient instance that is shared across all instances of ProductRepository.
        //We can set up the DocumentClient once and reuse since this is a static member variable. 
        private static DocumentClient _client;
        private static Uri _collectionUri;
        private static string _databaseId;
        private static string _collectionId; 

        /// <summary>
        /// Should be called before any other methods. We'll call this method in our Global.asax when 
        /// the app launches. This will set up our static member variables. 
        /// </summary>
        public static void Initialize(string databaseId, string collectionId, string endPoint, string primaryKey)
        {
            if (_client == null)
            {
                _client = new DocumentClient(new Uri(endPoint), primaryKey);
            }
            //all of our documents for this application live in the same collection. We'll go ahead and 
            //create the collection's Uri here and hang on to it for all future requests. 
            _collectionUri = UriFactory.CreateDocumentCollectionUri(databaseId, collectionId);
            _databaseId = databaseId;
            _collectionId = collectionId; 
        }

        /// <summary>
        /// Searches the product name field and returns all matching products. If the categoryId is null, 
        /// we'll include all categories.  
        /// Note that we're using a parameterized query to product against injection attacks since we're working
        /// with strings provided by users. 
        /// </summary>
        /// <param name="categoryId">The ID of the category where we should limit our product search. If null, 
        /// search all categories.</param>
        /// <param name="productName">The name of the product to search for.</param>
        /// <returns></returns>
        public async Task<List<Product>> SearchForProducts(string categoryId, string productName)
        {
            SqlParameterCollection sqlParams = new SqlParameterCollection(); 
            //construct our SQL statement. Note the inclusion of the partition key. If you fail to include
            //the partition key in a query against a partitioned collection, you will receive an error. 
            string sqlQuery = $"select * from c where c.doctype = 'product' and c.partitionkey = '{Constants.PARTITIONKEY_PRODUCT}'";
            if (! string.IsNullOrEmpty(categoryId))
            {
                sqlQuery += " and (c.categoryid = @categoryId)";
                sqlParams.Add(new SqlParameter("@categoryId", categoryId));
            }
            if (! string.IsNullOrEmpty(productName))
            {
                sqlQuery += " and contains(lower(c.productname), lower(@productName))";
                sqlParams.Add(new SqlParameter("@productName", productName));
            }

            //create our SqlQuerySpec parameterized query
            SqlQuerySpec querySpec = new SqlQuerySpec(sqlQuery, sqlParams);
         
            IDocumentQuery<Product> productsQuery = _client.CreateDocumentQuery<Product>(
                   _collectionUri, querySpec,
                    new FeedOptions { MaxItemCount = -1 }).AsDocumentQuery();

            List<Product> results = new List<Product>();
            while (productsQuery.HasMoreResults)
            {
                results.AddRange(await productsQuery.ExecuteNextAsync<Product>());
            }

            return results;
        }

        /// <summary>
        /// Retrieves a product document by id. 
        /// </summary>
        public async Task<Product> GetProductById(string productId)
        {
            //If you need a single document and you have the id, you don't have to perform a query. Instead, you 
            //can read it directly via its unique URI. Note that since this is a partitioned collection, 
            //we still have to include the partition key value in the RequestOptions. 
            Uri productDocumentUri = UriFactory.CreateDocumentUri(_databaseId, _collectionId, productId);
            Document productDocument = await _client.ReadDocumentAsync(productDocumentUri, new RequestOptions { PartitionKey = new PartitionKey(Constants.PARTITIONKEY_PRODUCT) }); 
            if (productDocument != null)
            {
                return (Product)(dynamic)productDocument; 
            }
            return null; 
        }

        ///// <summary>
        ///// Writes changes to a product back to the Collection. 
        ///// </summary>
        ///// <param name="updatedProduct"></param>
        ///// <returns></returns>
        //public async Task UpdateProduct(Product updatedProduct)
        //{
        //    //replace the product document in our collection. 
        //    await UpdateItemAsync(updatedProduct.Id, Constants.PARTITIONKEY_PRODUCT, updatedProduct);
        //}

        /// <summary>
        /// There is no concept of updating an existing document. Instead, we replace documents. 
        /// This method replaces the document with the specified id. 
        /// </summary>
        public async Task<Document> UpdateItemAsync<T>(string documentId, string partitionKey, T item)
        {
            Uri documentUri = UriFactory.CreateDocumentUri(_databaseId, _collectionId, documentId);
            return await _client.ReplaceDocumentAsync(documentUri, item, new RequestOptions { PartitionKey = new PartitionKey(partitionKey) });
        }

        /// <summary>
        /// Queries Cosmos DB for all reviews for a specific product. 
        /// </summary>
        public async Task<List<ProductReview>> GetReviewsForProduct(string productId)
        {
            string sqlQuery = $"select * from c where c.doctype = 'product_review' and c.productid = '{productId}' and c.partitionkey = 'review-{productId}'";

            IDocumentQuery<ProductReview> reviewsQuery = _client.CreateDocumentQuery<ProductReview>(
                   _collectionUri, sqlQuery,
                    new FeedOptions { MaxItemCount = -1 }).AsDocumentQuery();
            List<ProductReview> results = new List<ProductReview>();
            while (reviewsQuery.HasMoreResults)
            {
                results.AddRange(await reviewsQuery.ExecuteNextAsync<ProductReview>());
            }
            return results;
        }

        /// <summary>
        /// writes the supplied document to the database
        /// </summary>
        public async Task<Document> CreateDocumentAsync<T>(T document)
        {
            return await _client.CreateDocumentAsync(_collectionUri, document);
        }

        /// <summary>
        /// Removes a document from the collection. 
        /// </summary>
        /// <returns></returns>
        public async Task DeleteDocumentAsync(string documentId, string partitionKey)
        {
            Uri documentUri = UriFactory.CreateDocumentUri(_databaseId, _collectionId, documentId);
            await _client.DeleteDocumentAsync(documentUri,
                new RequestOptions { PartitionKey = new PartitionKey(partitionKey) });
        }

        /// <summary>
        /// Returns all product categories defined in the system. We do not have to worry about injection 
        /// attacks with this query since we aren't accepting input from the user. 
        /// </summary>
        public async Task<List<ProductCategory>> GetAllCategories()
        {
            IDocumentQuery<ProductCategory> categoriesQuery = _client.CreateDocumentQuery<ProductCategory>(
                   _collectionUri, "select * from c where c.doctype = 'category' and c.partitionkey = 'category'",
                    new FeedOptions { MaxItemCount = -1 }).AsDocumentQuery();

            List<ProductCategory> results = new List<ProductCategory>();
            while (categoriesQuery.HasMoreResults)
            {
                results.AddRange(await categoriesQuery.ExecuteNextAsync<ProductCategory>());
            }

            return results;
        }
    }
}