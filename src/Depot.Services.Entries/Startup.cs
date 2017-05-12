using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Depot.Messages.Commands;
using Depot.Services.Entries.Framework;
using Depot.Services.Entries.Handlers;
using Depot.Services.Entries.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;
using RawRabbit;
using RawRabbit.vNext;

namespace Depot.Services.Entries
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
            // services.AddScoped<IEntryRepository, EntryRepository>();
            ConfigureRabbitMqServices(services);
            ConfigureRedis(services);

            var builder = new ContainerBuilder();
            builder.Populate(services);
            ConfigureMongoDb(builder);
            Container = builder.Build();

            return new AutofacServiceProvider(Container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            ConfigureRabbitMqSubscriptions(app);
            MongoConfigurator.Initialize();
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
            services.AddScoped<ICommandHandler<CreateEntry>, CreateEntryHandler>();
        }

        private void ConfigureRabbitMqSubscriptions(IApplicationBuilder app)
        {
            var rabbitMqClient = app.ApplicationServices.GetService<IBusClient>();
            var createEntryHandler = app.ApplicationServices.GetService<ICommandHandler<CreateEntry>>();
            rabbitMqClient.SubscribeAsync<CreateEntry>(async (msg, context) 
                => await createEntryHandler.HandleAsync(msg));          
        }

        private void ConfigureMongoDb(ContainerBuilder builder)
        {
            var optionsSection = Configuration.GetSection("mongo");
            var options = new MongoOptions();
            optionsSection.Bind(options);
            builder.RegisterInstance(options).SingleInstance();

            var mongoClient = new MongoClient(options.ConnectionString);
            builder.RegisterInstance(mongoClient.GetDatabase(options.Database));
            builder.RegisterType<MongoEntryRepository>()
                .As<IEntryRepository>()
                .InstancePerLifetimeScope();
        }

        private void ConfigureRedis(IServiceCollection services)
        {
            var optionsSection = Configuration.GetSection("redis");
            var options = new RedisOptions();
            optionsSection.Bind(options);
            services.Configure<RedisOptions>(optionsSection);

            services.AddDistributedRedisCache(x => 
            {
                x.Configuration = options.ConnectionString;
                x.InstanceName = options.Instance;
            });
        }
    }
}
