using Application.Dtos;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Application.Handlers.Queries
{
	public class PermissionQueryByForenameOrSurname : IRequest<List<PermissionDto>>
	{
		public string EmployeeForename { get; set; }
		public string EmployeeSurname { get; set; }

		public PermissionQueryByForenameOrSurname()
		{
			EmployeeForename = String.Empty;
			EmployeeSurname = String.Empty;
		}

        public class PermissionQueryByForenameOrSurnameHandler : IRequestHandler<PermissionQueryByForenameOrSurname, List<PermissionDto>>
        {
            private readonly IPermissionRepository _repository;
            private readonly IElasticSearchService _elasticSearchService;
            private readonly IMapper _mapper;
            private readonly ILogger<PermissionQueryByForenameOrSurnameHandler> _logger;

            public PermissionQueryByForenameOrSurnameHandler(IPermissionRepository repository, IElasticSearchService elasticSearchService, IMapper mapper, ILogger<PermissionQueryByForenameOrSurnameHandler> logger)
            {
                _repository = repository;
                _elasticSearchService = elasticSearchService;
                _mapper = mapper;
                _logger = logger;
            }

            public Task<List<PermissionDto>> Handle(PermissionQueryByForenameOrSurname request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Inicializando RequestPermission");
                List<Permission> response = null;
                if (request.EmployeeForename != null && request.EmployeeSurname != null)
                {
                    if (!request.EmployeeForename.Equals(String.Empty) && !request.EmployeeSurname.Equals(String.Empty))
                    {
                        response = _repository.GetAllBy(q => q.EmployeeSurname.ToLower().Contains(request.EmployeeSurname) ||
                                                        q.EmployeeForename.ToLower().Contains(request.EmployeeForename)).ToList();
                    }
                }
                if (request.EmployeeForename != null)
                {
                    if (!request.EmployeeForename.Equals(String.Empty))
                    {
                        response = _repository.GetAllBy(q => q.EmployeeForename.ToLower().Contains(request.EmployeeForename)).ToList();
                    }
                }
                if (request.EmployeeSurname != null)
				{
                    if (!request.EmployeeSurname.Equals(String.Empty))
					{
                        response = _repository.GetAllBy(q => q.EmployeeSurname.ToLower().Contains(request.EmployeeSurname)).ToList();
                    }
				}
                //var response = _repository.GetAllBy(q => q.EmployeeSurname.ToLower().Contains(request.EmployeeSurname) || 
                                                        //q.EmployeeForename.ToLower().Contains(request.EmployeeForename)).ToList();
                var result = _mapper.Map<List<PermissionDto>>(response);
                _logger.LogInformation("Resultado: ", JsonSerializer.Serialize<List<PermissionDto>>(result));
                if (result.Count > 0)
                {
                    var esRequest = new EsPermissionOp()
                    {
                        Name = "RequestPermission",
                        Description = String.Format("Se ejecuto el metodo RequestPermission a las {0} enviando EmployeeSurname: {1} y EmployeeForename: {2}", DateTime.Now.ToString(), request.EmployeeSurname, request.EmployeeForename)
                    };
                    var newIndex = (_elasticSearchService.Count()) + 1;
                    _logger.LogInformation("ElasticSearch New Index: newIndex", newIndex);
                    _elasticSearchService.Call(esRequest, newIndex);
                    _logger.LogInformation("Save ElasticSearch");
                }
                _logger.LogInformation("Finalizando RequestPermission");
                return Task.FromResult(result);
            }
        }
    }
}
