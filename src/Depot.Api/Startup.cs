using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Depot.Api.Framework;
using Depot.Api.Handlers;
using Depot.Api.Repositories;
using Depot.Messages.Events;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RawRabbit;
using RawRabbit.vNext;

namespace Depot.Api
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        public IContainer Container { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc()
                    .AddJsonOptions(x => x.SerializerSettings.Formatting = Formatting.Indented);
            services.AddScoped<ILogRepository, LogRepository>();
            ConfigureRabbitMqServices(services);

            var builder = new ContainerBuilder();
            builder.Populate(services);
            Container = builder.Build();

            return new AutofacServiceProvider(Container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            ConfigureRabbitMqSubscriptions(app);
            app.UseErrorHandler();
            app.UseMvc();
        }

        private void ConfigureRabbitMqServices(IServiceCollection services)
        {
            var rabbitMqOptions = new RabbitMqOptions();
            var rabbitMqOptionsSection = Configuration.GetSection("rabbitmq");
            rabbitMqOptionsSection.Bind(rabbitMqOptions);

            var rabbitMqClient = BusClientFactory.CreateDefault(rabbitMqOptions);
            services.Configure<RabbitMqOptions>(rabbitMqOptionsSection);
            services.AddSingleton<IBusClient>(_ => rabbitMqClient);
            services.AddScoped<IEventHandler<EntryCreated>, EntryCreatedHandler>();
            services.AddScoped<IEventHandler<CreateEntryRejected>, CreateEntryRejectedHandler>();
        }

        private void ConfigureRabbitMqSubscriptions(IApplicationBuilder app)
        {
            var rabbitMqClient = app.ApplicationServices.GetService<IBusClient>();
            var entryCreatedHandler = app.ApplicationServices.GetService<IEventHandler<EntryCreated>>();
            var createEntryRejectedHandler = app.ApplicationServices.GetService<IEventHandler<CreateEntryRejected>>();
            rabbitMqClient.SubscribeAsync<EntryCreated>(async (msg, context) 
                => await entryCreatedHandler.HandleAsync(msg));   
            rabbitMqClient.SubscribeAsync<CreateEntryRejected>(async (msg, context) 
                => await createEntryRejectedHandler.HandleAsync(msg));   
        }
    }
}
