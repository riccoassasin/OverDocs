using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NewSecurityDemo.Startup))]
namespace NewSecurityDemo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
