using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WeatherServiceApp.Startup))]
namespace WeatherServiceApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
