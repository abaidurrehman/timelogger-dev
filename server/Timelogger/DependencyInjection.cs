using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Timelogger.Behaviors;
using Timelogger.Data.Repositories;

namespace Timelogger
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
                //  cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<>));
            });

            services.AddScoped<IProjectRepository, ProjectRepository>();

            services.AddScoped<ITimeRegistrationRepository, TimeRegistrationRepository>();

            return services;
        }
    }
}