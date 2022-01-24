using Application.Dtos;
using Application.Handlers.Commands;
using Application.Handlers.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Api.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class PermissionController : ControllerBase
	{
		private readonly ILogger<PermissionController> _logger;
		private readonly IMediator _mediator;

		public PermissionController(ILogger<PermissionController> logger, IMediator mediator)
		{
			_logger = logger;
			_mediator = mediator;
		}

		[HttpGet("get")]
		[ProducesResponseType(typeof(List<PermissionDto>), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> GetPermissions()
		{
			var request = new PermissionQueryAll();
			var result = await _mediator.Send(request);
			return Ok(result);
		}

		//[HttpGet("request/{forename}/{surname}")]
		[HttpPost("request")]
		[ProducesResponseType(typeof(List<PermissionDto>), (int)HttpStatusCode.OK)]
		//public async Task<IActionResult> RequestPermission([FromRoute] string? forename = null, [FromRoute] string? surname = null)
		public async Task<IActionResult> RequestPermission([FromBody] PermissionQueryByForenameOrSurname request)
		{
			//var request = new PermissionQueryByForenameOrSurname();
			//request.EmployeeForename = forename;
			//request.EmployeeSurname = surname;
			var result = await _mediator.Send(request);
			return Ok(result);
		}

		[HttpPost("create")]
		[ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> CreatePermission([FromBody] PermissionCreateCommand request)
		{
			var result = await _mediator.Send(request);
			return Ok(result);
		}

		[HttpPut("modify")]
		[ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> ModifyPermission([FromBody] PermissionUpdateCommand request)
		{
			var result = await _mediator.Send(request);
			return Ok(result);
		}
	}
}
