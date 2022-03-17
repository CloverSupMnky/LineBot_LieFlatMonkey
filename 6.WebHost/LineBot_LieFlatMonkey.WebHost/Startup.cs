using LineBot_LieFlatMonkey.Assets.Model.AppSetting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LineBot_LieFlatMonkey.Entities.Contexts;
using Microsoft.EntityFrameworkCore;
using URF.Core.Abstractions;
using URF.Core.EF;
using URF.Core.Abstractions.Trackable;
using URF.Core.EF.Trackable;
using LineBot_LieFlatMonkey.Entities.Models;

namespace LineBot_LieFlatMonkey.WebHost
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

            services.AddControllers();

            services.AddCors(options => 
                options.AddPolicy("CorsPolicy",builder => 
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                })
            );

            services.Configure<LineBotSetting>(Configuration.GetSection("LineBotSetting"));

            #region Entity ª`¤J

            services.AddDbContext<LineBotLieFlatMonkeyContext>(options => options.UseNpgsql(Configuration.GetConnectionString("LineBotNpgsql")));

            services.AddScoped<DbContext, LineBotLieFlatMonkeyContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ITrackableRepository<TarotCard>, TrackableRepository<TarotCard>>();

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
