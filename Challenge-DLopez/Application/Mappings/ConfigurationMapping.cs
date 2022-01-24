using Application.Dtos;
using Application.Handlers.Commands;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
	public class ConfigurationMapping : Profile
	{
        public ConfigurationMapping()
        {
            CreateMap<Permission, PermissionDto>();
            CreateMap<PermissionCreateCommand, Permission>();
            CreateMap<PermissionUpdateCommand, Permission>();
        }
    }
}
