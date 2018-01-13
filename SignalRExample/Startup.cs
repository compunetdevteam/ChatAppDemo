using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SignalRExample.Startup))]
namespace SignalRExample
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
