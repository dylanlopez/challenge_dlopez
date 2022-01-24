using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Infraestructure.Persistence;
using Infraestructure.Repositories;
using Infraestructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace Infraestructure
{
	public static class Extensions
	{
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration Configuration)
        {
            var cs = Configuration.GetConnectionString("DbExam");
            services.AddDbContext<DBExamContext>(options =>
                options
                    .UseSqlServer(Configuration.GetConnectionString("DbExam")
                    )
                    //.LogTo(IsDevelopment ? Console.WriteLine : x => { })
                    //.EnableSensitiveDataLogging(IsDevelopment)
                );

            services.AddHttpClient("ESService", c =>
            {
                c.BaseAddress = new Uri(Configuration["Elastic:UrlService"]);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            }).ConfigurePrimaryHttpMessageHandler(() =>
            {
                return new HttpClientHandler()
                {
                    UseDefaultCredentials = true,
                    PreAuthenticate = true,
                    Credentials = CredentialCache.DefaultCredentials
                };
            });

            //Repositories
            services.AddTransient<IPermissionRepository, PermissionRepository>();
            //services.AddTransient<ISurveyScheduleRepository, SurveyScheduleRepository>();

            //Services
            services.AddTransient<IElasticSearchService, ElasticSearchService>();
            //services.AddTransient<IFileUpload, FileUpload>();
            return services;
        }
    }
}
