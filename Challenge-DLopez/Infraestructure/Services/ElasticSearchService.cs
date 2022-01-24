using Application.Interfaces.Services;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Nest;
using Newtonsoft.Json;
using System.Text;

namespace Infraestructure.Services
{
	public class ElasticSearchService : IElasticSearchService
	{
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private ElasticClient _esClient;

        public ElasticSearchService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task Call(EsPermissionOp request, long newIndex)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ESService");
                var jsonRequest = JsonConvert.SerializeObject(request);
                HttpContent httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(_configuration["Elastic:RegPermissionOp"] + newIndex.ToString(), httpContent);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public long Count()
		{
			try
			{
                InitPermission();
                var response = _esClient.Count<EsPermissionOp>(c => c.AllIndices());
                return response.Count;
            }
            catch (Exception ex) 
            {
                throw ex;
            }
		}
        private void InitPermission()
		{
            var node = new Uri(_configuration["Elastic:UrlService"]);
            _esClient = new ElasticClient(new ConnectionSettings(node).DefaultIndex("permission"));
		}
    }
}
