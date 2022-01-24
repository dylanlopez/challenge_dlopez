using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System.Reflection;

namespace Application
{
	public static class DependencyInjection
	{
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
			//services.AddSingleton<IElasticClient>();
            return services;
        }
    }
}
