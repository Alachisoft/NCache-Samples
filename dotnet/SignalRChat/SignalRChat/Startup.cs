using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Alachisoft.NCache.SignalR;
using Microsoft.AspNet.SignalR;
using System.Configuration;

[assembly: OwinStartup(typeof(SignalRChat.Startup))]

namespace SignalRChat
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            string cache, eventKey, userId = null, password = null;
            

            if (ConfigurationManager.AppSettings["cache"] != null)
                cache = ConfigurationManager.AppSettings["cache"];
            else
                throw new Exception("Cache Name is required to start the application");
            if (ConfigurationManager.AppSettings["eventKey"] != null)
                eventKey = ConfigurationManager.AppSettings["eventKey"]; 
            else
                throw new Exception("EventKey is required to start the application");
            if (ConfigurationManager.AppSettings["userID"] != null)
                userId = ConfigurationManager.AppSettings["userID"];
           
            if (ConfigurationManager.AppSettings["password"] != null)
                password = ConfigurationManager.AppSettings["password"];


            GlobalHost.DependencyResolver.UseNCache(cache, eventKey,userId,password);
            app.MapSignalR();
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
        }
    }
}
