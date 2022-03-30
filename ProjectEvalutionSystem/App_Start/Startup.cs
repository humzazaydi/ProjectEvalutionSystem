using Microsoft.Owin;
using Owin;
using System;
using System.Runtime.Hosting;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;

[assembly: OwinStartup(typeof(ProjectEvalutionSystem.App_Start.Startup))]

namespace ProjectEvalutionSystem.App_Start
{
    public class Startup
    {
        public static void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            app.Map("/signalr", map =>
            {
                map.UseCors(CorsOptions.AllowAll);
                var hubConfiguration = new HubConfiguration
                {
                    EnableJavaScriptProxies = false,
                    EnableDetailedErrors = true
                };
                map.RunSignalR(hubConfiguration);
            });
        }
    }
}
