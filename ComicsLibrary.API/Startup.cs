using System;
using ComicsLibrary.Common.Delegates;
using ComicsLibrary.Common.Interfaces;
using ComicsLibrary.Data;
using ComicsLibrary.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ComicsLibrary.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IService, Service>();
            services.AddTransient<IMapper, Mapper.Mapper>();
            services.AddTransient<IApiService, MarvelComicsApi.Service>();
            services.AddTransient<IMarvelAppKeys, AppKeys>();
            services.AddTransient<Common.Interfaces.ILogger, Logger>();
            services.AddTransient<IAsyncHelper, AsyncHelper>();

            services.AddScoped(sp => new GetCurrentDateTime(() => DateTime.Now));
            services.AddScoped<Func<IUnitOfWork>>(sp => () => new UnitOfWork(Configuration));

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
