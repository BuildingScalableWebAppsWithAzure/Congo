using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Congo.Web.Persistence;
using System.Configuration; 

namespace Congo.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            string databaseId = ConfigurationManager.AppSettings["DatabaseId"];
            string collectionId = ConfigurationManager.AppSettings["CollectionId"];
            string endPoint = ConfigurationManager.AppSettings["EndPoint"];
            string primaryKey = ConfigurationManager.AppSettings["PrimaryKey"];
            ProductRepository.Initialize(databaseId, collectionId, endPoint, primaryKey);
        }
    }
}
