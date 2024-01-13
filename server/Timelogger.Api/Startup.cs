using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Timelogger.Entities;

namespace Timelogger.Api
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;

        public Startup(IWebHostEnvironment env)
        {
            _environment = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<TimeloggerDbContext>(opt => opt.UseInMemoryDatabase("e-conomic interview"));
            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
            });

            services.AddMvc(options => options.EnableEndpointRouting = false);

            if (_environment.IsDevelopment()) services.AddCors();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Timer logger", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseCors(builder => builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(origin => true)
                    .AllowCredentials());

            app.UseSwagger();

            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Timer logger"); });

            app.UseMvc();


            var serviceScopeFactory = app.ApplicationServices.GetService<IServiceScopeFactory>();
            using (var scope = serviceScopeFactory.CreateScope())
            {
                SeedDatabase(scope);
            }
        }

        private static void SeedDatabase(IServiceScope scope)
        {
            var context = scope.ServiceProvider.GetService<TimeloggerDbContext>();

            var testFreelancer = new Freelancer()
            {
                Id = 1,
                Name = "Abaid",

            };

            context.Freelancers.Add(testFreelancer);

            var testProject1 = new Project
            {
                Id = 1,
                Name = "e-conomic Interview",
                Deadline = DateTime.Today.AddDays(8),
                Status = ProjectStatus.New
            };

            var testProject2 = new Project
            {
                Id = 2,
                Name = "HR App",
                Deadline = DateTime.Today.AddDays(1),
                Status = ProjectStatus.InProgress
            };

            var testProject3 = new Project
            {
                Id = 3,
                Name = "Approval Workflow",
                Deadline = DateTime.Today.AddDays(3),
                Status = ProjectStatus.InProgress
            };

            var testProject4 = new Project
            {
                Id = 4,
                Name = "Accounting System",
                Deadline = DateTime.Today.AddDays(-2),
                Status = ProjectStatus.Complete
            };


            context.Projects.Add(testProject1);
            context.Projects.Add(testProject2);
            context.Projects.Add(testProject3);
            context.Projects.Add(testProject4);

            context.SaveChanges();
        }
    }
}