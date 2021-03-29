using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DGTradesIn.Startup))]
namespace DGTradesIn
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
