using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebModelDto;
using WebModelDto.Request;
using WebWorkersLogic;
using WebWorkersValidation;

namespace Lanit_HW5_Client
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            //using (WorkerDbContext context = new WorkerDbContext())
            //{
            //    context.Database.Migrate();
            //}
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ICreateWorkersCommand, CreateWorkersCommand>();
            services.AddTransient<IGetWorkersCommand, GetWorkersCommand>();            
            services.AddTransient<IUpdateWorkersCommand, UpdateWorkersCommand>();
            services.AddTransient<IDeleteWorkersCommand, DeleteWorkersCommand>();
            services.AddTransient<ICreateWorkersRequestValidator, CreateWorkersRequestValidator>();
            services.AddTransient<IUpdateWorkersRequestValidator, UpdateWorkersRequestValidator>();
            services.AddTransient<IDeleteWorkersRequestValidator, DeleteWorkersRequestValidator>();

            services.AddControllers();

            services.AddMassTransit(mt =>
            {

                mt.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost", "/", host =>
                    {
                        host.Username("guest");
                        host.Password("guest");
                    });

                });
                mt.AddRequestClient<CreateWorkersRequest>(new Uri("rabbitmq://localhost/post"));
                mt.AddRequestClient<GetWorkersRequest>(new Uri("rabbitmq://localhost/get"));
                mt.AddRequestClient<UpdateWorkersRequest>(new Uri("rabbitmq://localhost/update"));
                mt.AddRequestClient<DeleteWorkersRequest>(new Uri("rabbitmq://localhost/delete"));

            });
            services.AddMassTransitHostedService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
