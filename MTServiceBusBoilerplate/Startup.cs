using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contratos;
using GreenPipes;
using MassTransit;
using MassTransit.Azure.ServiceBus.Core;
using MassTransit.Log4NetIntegration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus.Primitives;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace MTServiceBusBoilerplate
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddScoped<DoSomethingConsumer>();
            services.AddMassTransit(x =>
            {
                x.AddConsumer<DoSomethingConsumer>();

                
            }
                );
            services.AddSingleton(provider => Bus.Factory.CreateUsingAzureServiceBus(cfg =>
            {
                var host = cfg.Host("azure service bus endpoint" +
                                    ""
                    ,
                    z =>
                    {
                        z.TokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider("RootManageSharedAccessKey", "key");
                    });

                cfg.UseExtensionsLogging(provider.GetService<ILoggerFactory>());

                cfg.ReceiveEndpoint(host, "do-something-queue", e =>
                {
                    e.Consumer<DoSomethingConsumer>(provider);
                });
            }));
            
            services.AddSingleton<IPublishEndpoint>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<ISendEndpointProvider>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<IBus>(provider => provider.GetRequiredService<IBusControl>());
            services.AddScoped(provider => provider.GetRequiredService<IBus>().CreateRequestClient<DoSomething>());

            services.AddSingleton<IHostedService, BusService>();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}