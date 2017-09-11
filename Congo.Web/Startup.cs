using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Congo.Web.Startup))]
namespace Congo.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
