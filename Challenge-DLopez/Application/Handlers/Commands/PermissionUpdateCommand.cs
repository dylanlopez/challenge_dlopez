using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Commands
{
	public class PermissionUpdateCommand : IRequest<string>
	{
		public int Id { get; set; }
		public string EmployeeForename { get; set; }
		public string EmployeeSurname { get; set; }
		public int? PermissionType { get; set; }
		public DateTime? PermissionDate { get; set; }

        public class PermissionUpdateCommandHandler : IRequestHandler<PermissionUpdateCommand, string>
        {
            private readonly IPermissionRepository _repository;
            private readonly IElasticSearchService _elasticSearchService;
            private readonly IMapper _mapper;
            private readonly ILogger<PermissionUpdateCommandHandler> _logger;

            public PermissionUpdateCommandHandler(IPermissionRepository repository, IElasticSearchService elasticSearchService, IMapper mapper, ILogger<PermissionUpdateCommandHandler> logger)
            {
                _repository = repository;
                _elasticSearchService = elasticSearchService;
                _mapper = mapper;
                _logger = logger;
            }

            public async Task<string> Handle(PermissionUpdateCommand request, CancellationToken cancellationToken)
            {
                string result;
                _logger.LogInformation("Inicializando CreatePermission");
                try
                {
                    var entity = _repository.GetAllBy(q => q.Id == request.Id).FirstOrDefault();
                    if (!entity.EmployeeForename.Equals(request.EmployeeForename))
					{
                        entity.EmployeeForename = request.EmployeeForename;
                    }
                    if (!entity.EmployeeSurname.Equals(request.EmployeeSurname))
                    {
                        entity.EmployeeSurname = request.EmployeeSurname;
                    }
                    if (request.PermissionType != null)
					{
                        if (entity.PermissionType != request.PermissionType)
                        {
                            entity.PermissionType = (int) request.PermissionType;
                        }
                    }
                    if (request.PermissionDate != null)
					{
                        if (!entity.PermissionDate.Equals(request.PermissionDate))
                        {
                            entity.PermissionDate = (DateTime) request.PermissionDate;
                        }
                    }
                    await _repository.Update(entity);
                    result = "Ok";
                    _logger.LogInformation("Resultado: ", result);
                    if (result.Equals("Ok"))
                    {
                        var description = String.Format("Se ejecuto el metodo ModifyPermission a las {0} enviando EmployeeSurname: {1}, EmployeeForename: {2}", DateTime.Now.ToString(), request.EmployeeSurname, request.EmployeeForename);
                        if (request.PermissionType != null)
						{
                            description = description + String.Format(" PermissionType: {0}", request.PermissionType);
                        }
                        if (request.PermissionDate != null)
                        {
                            description = description + String.Format(" y PermissionDate: {0}", request.PermissionDate);
                        }
                        var esRequest = new EsPermissionOp()
                        {
                            Name = "ModifyPermission",
                            Description = description
                        };
                        var newIndex = (_elasticSearchService.Count()) + 1;
                        _logger.LogInformation("ElasticSearch New Index: newIndex", newIndex);
                        _elasticSearchService.Call(esRequest, newIndex);
                        _logger.LogInformation("Save ElasticSearch");
                    }
                }
                catch (Exception ex)
                {
                    result = "Error";
                }
                _logger.LogInformation("Finalizando CreatePermission");
                return result;
            }
        }
    }
}
