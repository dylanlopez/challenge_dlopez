using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Commands
{
	public class PermissionCreateCommand : IRequest<int>
    {
		public string EmployeeForename { get; set; }
		public string EmployeeSurname { get; set; }
		public int PermissionType { get; set; }
		public DateTime PermissionDate { get; private set; }

		public PermissionCreateCommand()
		{
			PermissionDate = DateTime.Now;
		}

        public class PermissionCreateCommandHandler : IRequestHandler<PermissionCreateCommand, int>
        {
            private readonly IPermissionRepository _repository;
            private readonly IElasticSearchService _elasticSearchService;
            private readonly IMapper _mapper;
            private readonly ILogger<PermissionCreateCommandHandler> _logger;

            public PermissionCreateCommandHandler(IPermissionRepository repository, IElasticSearchService elasticSearchService, IMapper mapper, ILogger<PermissionCreateCommandHandler> logger)
            {
                _repository = repository;
                _elasticSearchService = elasticSearchService;
                _mapper = mapper;
                _logger = logger;
            }

            public async Task<int> Handle(PermissionCreateCommand request, CancellationToken cancellationToken)
            {
				int result;
                _logger.LogInformation("Inicializando CreatePermission");
                try
                {
                    var entity = _mapper.Map<Permission>(request);
                    result = await _repository.Create(entity);
                    _logger.LogInformation("Resultado: ", result);
                    if (result > 0)
                    {
                        var esRequest = new EsPermissionOp()
                        {
                            Name = "CreatePermission",
                            Description = String.Format("Se ejecuto el metodo CreatePermission a las {0} enviando EmployeeSurname: {1}, EmployeeForename: {2}, PermissionType: {3} y PermissionDate: {4}", DateTime.Now.ToString(), request.EmployeeSurname, request.EmployeeForename, request.PermissionType, request.PermissionDate)
                        };
                        var newIndex = (_elasticSearchService.Count()) + 1;
                        _logger.LogInformation("ElasticSearch New Index: newIndex", newIndex);
                        _elasticSearchService.Call(esRequest, newIndex);
                        _logger.LogInformation("Save ElasticSearch");
                    }
                }
                catch (Exception ex)
                {
                    result = -1;
                }
                _logger.LogInformation("Finalizando CreatePermission");
                return result;
            }
        }
    }
}
