using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IdentitiyEntity.Startup))]
namespace IdentitiyEntity
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
