using System.Collections.Generic;
using System.Globalization;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using TodoApiNet.Contexts;
using TodoApiNet.Extensions;
using TodoApiNet.Repositories;

namespace TodoApiNet
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration) => Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddTokenAuthentication(Configuration);
            services.AddAutoMapper(typeof(Startup));
            services.AddControllers().AddNewtonsoftJson().AddDataAnnotationsLocalization(options =>
            {
                options.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(SharedResources));
            });
            services.Configure<MongoDBSettings>(Configuration.GetSection(nameof(MongoDBSettings)));
            services.AddSingleton<IMongoDBSettings>(sp => sp.GetRequiredService<IOptions<MongoDBSettings>>().Value);
            services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
            services.AddSingleton<IStringLocalizer, JsonStringLocalizer>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ITodoRepository, TodoRepository>();
            services.AddTransient<IAccessTokenRepository, AccessTokenRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) { app.UseDeveloperExceptionPage(); }

            var cultures = new List<CultureInfo>
            {
                new CultureInfo("en-US"),
                new CultureInfo("es-MX")
            };

            app.UseRequestLocalization(options => 
            {
                options.DefaultRequestCulture = new RequestCulture("es-MX");
                options.SupportedCultures = cultures;
                options.SupportedUICultures = cultures;
            });
            
            app.UseAuthentication();
            app.UseUpdateDate();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
