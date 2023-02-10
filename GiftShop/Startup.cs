using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GiftShop.Startup))]
namespace GiftShop
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
