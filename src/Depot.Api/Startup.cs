﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Depot.Api.Framework;
using Depot.Api.Handlers;
using Depot.Messages.Events;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RawRabbit;
using RawRabbit.vNext;

namespace Depot.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            ConfigureRabbitMqServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            ConfigureRabbitMqSubscriptions(app);
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
