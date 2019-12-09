using Alachisoft.NCache.AspNetCore.SignalR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;

namespace SignalRChat
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddMvc();

            services.Configure<NCacheConfiguration>(Configuration.GetSection("NCacheConfiguration"));

            services.AddSignalR().AddNCache(ncacheOptions =>
            {
                ncacheOptions.CacheName = Configuration["NCacheConfiguration:CacheName"];
                ncacheOptions.ApplicationID = Configuration["NCacheConfiguration:ApplicationID"];

                // Uncomment in case of cache security enabled.
                //ncacheOptions.UserID = Configuration["NCacheConfiguration:UserID"];
                //ncacheOptions.Password = Configuration["NCacheConfiguration:Password"];

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseSignalR(config =>
            {
                config.MapHub<MessageHub>("/messages");
            });

            app.UseMvc();
        }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }
    }
}
