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
using LineBot_LieFlatMonkey.Modules.Interfaces;
using LineBot_LieFlatMonkey.Modules.Services;
using LineBot_LieFlatMonkey.WebHost.Filters;
using LineBot_LieFlatMonkey.Modules.Interfaces.Factory;
using LineBot_LieFlatMonkey.Modules.Services.Factory;
using LineBot_LieFlatMonkey.Assets.Constant;
using LineBot_LieFlatMonkey.Modules.Factory;

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

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                // �����^��Ѧ�
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                // �]�w�ɶ��榡
                options.SerializerSettings.DateFormatString = "yyyy'/'MM'/'dd HH':'mm':'ss.FFFFFFFK";
            });

            services.AddCors(options => 
                options.AddPolicy("CorsPolicy",builder => 
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                })
            );

            services.Configure<LineBotSetting>(Configuration.GetSection("LineBotSetting"));
            services.Configure<SpeechSetting>(Configuration.GetSection("SpeechSetting"));
            services.Configure<ApiDomainSetting>(Configuration.GetSection("ApiDomainSetting"));

            // ���U�ۭq�q Filter
            // �Y Controller ���ϥ� TypeFilter(�i�H�a�Ѽ�) �N���ݵ��U
            services.AddScoped<VerifySignatureFilter>();

            services.AddScoped<ITarotCardService, TarotCardService>();
            services.AddScoped<IEnglishSentenceService, EnglishSentenceService>();
            services.AddScoped<ISpeechService, SpeechService>();
            services.AddScoped<IWebHookEventService, WebHookEventService>();
            services.AddScoped<IHttpClientService, HttpClientService>();

            #region �u�t���O���U

            services.AddScoped<MessageEventService>();
            services.AddScoped<FollowEventService>();
            services.AddScoped<JoinEventService>();
            services.AddScoped<Func<string, IEventFactoryService>>(serviceProvider => type =>
            {
                switch (type)
                {
                    case EventType.Message:
                        return serviceProvider.GetService<MessageEventService>();
                    case EventType.Follow:
                        return serviceProvider.GetService<FollowEventService>();
                    case EventType.Join:
                        return serviceProvider.GetService<JoinEventService>();
                    default:
                        throw new Exception("Not valid key"); ;
                }
            });

            services.AddScoped<EventFactory>();

            #endregion

            #region Entity ���U

            services.AddDbContext<LineBotLieFlatMonkeyContext>(options => options.UseNpgsql(Configuration.GetConnectionString("LineBotNpgsql")));

            services.AddScoped<DbContext, LineBotLieFlatMonkeyContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ITrackableRepository<TarotCard>, TrackableRepository<TarotCard>>();

            services.AddScoped<ITrackableRepository<EnglishSentence>, TrackableRepository<EnglishSentence>>();

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
