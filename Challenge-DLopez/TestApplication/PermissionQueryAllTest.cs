using Application.Dtos;
using Application.Handlers.Queries;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Infraestructure.Persistence;
using Infraestructure.Repositories;
using Infraestructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading;
using Xunit;
using static Application.Handlers.Queries.PermissionQueryAll;

namespace TestApplication
{
	public class PermissionQueryAllTest
	{

		public PermissionQueryAllTest()
		{
			
		}

		[Fact]
		public void TestHandleOk()
		{
			//var dbSetPermissionMock = new Mock<DbSet<Permission>>();
			//var dbExamContextMock = new Mock<DBExamContext>();
			////dbExamContextMock.Setup(s => s.Set<Permission>()).Returns(dbSetPermissionMock.Object);
			//dbExamContextMock.Setup(s => s.Set<Permission>()).Returns(dbSetPermissionMock.Object);
			////var repository = new Mock<PermissionRepository>(dbExamContextMock);

			//var httpClientFactory = new Mock<IHttpClientFactory>();
			//var configuration = new Mock<IConfiguration>();
			//var elasticSearchService = new Mock<ElasticSearchService>(httpClientFactory, configuration);

			//var mapper = new Mock<IMapper>();
			//var logger = new Mock<ILogger<PermissionQueryAllHandler>>();

			////_permissionQueryAll = new PermissionQueryAllHandler(repository.Object, elasticSearchService.Object, mapper.Object, logger.Object);
			////var requestMock = new Mock<PermissionQueryAll>();
			////var cancellationTokenMock = new Mock<CancellationToken>();
			////var cancellationTokenMock = new CancellationToken();
			////var permissions = _permissionQueryAll.Handle(requestMock.Object, cancellationTokenMock);

			//var listPermissions = new List<Permission>();
			//listPermissions.Add(new Permission() { Id = 1, EmployeeForename = "Dylan", EmployeeSurname = "Lopez", PermissionType = 1, PermissionDate = DateTime.Now });
			////var queryable = listPermissions.Select(s => new { s.Id, s.EmployeeForename, s.EmployeeSurname, s.PermissionType, s.PermissionDate }).ToList();
			//var repository = new Mock<PermissionRepository>();
			//repository.Setup(p => p.GetAllBy(q => q.Id != 0))
			//	.Returns(listPermissions.AsQueryable());
			//_permissionQueryAll = new PermissionQueryAllHandler(repository.Object, elasticSearchService.Object, mapper.Object, logger.Object);

			var requestMock = new Mock<PermissionQueryAll>();
			////var cancellationTokenMock = new Mock<CancellationToken>();
			var cancellationTokenMock = new CancellationToken();
			//var permissions = _permissionQueryAll.Handle(requestMock.Object, cancellationTokenMock);

			var listPermissionsDto = new List<PermissionDto>();
			listPermissionsDto.Add(new PermissionDto() { Id = 1, EmployeeForename = "Dylan", EmployeeSurname = "Lopez", PermissionType = 1, PermissionDate = DateTime.Now });

			var permissionQueryAll = new Mock<PermissionQueryAllHandler>();
			permissionQueryAll.Setup(s => s.Handle(It.IsAny<PermissionQueryAll>(), It.IsAny<CancellationToken>())).ReturnsAsync(listPermissionsDto);
			var permissions2 = permissionQueryAll.Object.Handle(requestMock.Object, cancellationTokenMock);
			
			//permissionQueryAll.
			Assert.NotNull(permissions2);
		}
	}
}