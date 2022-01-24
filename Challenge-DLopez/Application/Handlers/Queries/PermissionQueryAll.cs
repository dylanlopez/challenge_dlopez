using Application.Dtos;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Application.Handlers.Queries
{
	public class PermissionQueryAll : IRequest<List<PermissionDto>>
    {
        public class PermissionQueryAllHandler : IRequestHandler<PermissionQueryAll, List<PermissionDto>>
        {
            private readonly IPermissionRepository _repository;
            private readonly IElasticSearchService _elasticSearchService;
            private readonly IMapper _mapper;
            private readonly ILogger<PermissionQueryAllHandler> _logger;

            public PermissionQueryAllHandler(IPermissionRepository repository, IElasticSearchService elasticSearchService, IMapper mapper, ILogger<PermissionQueryAllHandler> logger)
            {
                _repository = repository;
                _elasticSearchService = elasticSearchService;
                _mapper = mapper;
                _logger = logger;
            }

            public Task<List<PermissionDto>> Handle(PermissionQueryAll request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Inicializando GetPermissions");
                //var response = new List<PermissionDto>();
                //var response = _repository.GetAllBy(q => q.EmployeeSurname == "Dylan").ToList();
                var response = _repository.GetAllBy(q => q.Id != 0).ToList();
                var result = _mapper.Map<List<PermissionDto>>(response);
                _logger.LogInformation("Resultado: ", JsonSerializer.Serialize<List<PermissionDto>>(result));
				if (result.Count > 0)
				{
					var esRequest = new EsPermissionOp()
					{
						Name = "GetPermissions",
						Description = String.Format("Se ejecuto el metodo GetPermissions a las {0}", DateTime.Now.ToString())
					};
					var newIndex = (_elasticSearchService.Count()) + 1;
					_logger.LogInformation("ElasticSearch New Index: newIndex", newIndex);
					_elasticSearchService.Call(esRequest, newIndex);
					_logger.LogInformation("Save ElasticSearch");
				}
				_logger.LogInformation("Finalizando GetPermissions");
                return Task.FromResult(result);
            }
        }
    }
}
