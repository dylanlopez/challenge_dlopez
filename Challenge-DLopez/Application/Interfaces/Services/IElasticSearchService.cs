using Domain.Models;

namespace Application.Interfaces.Services
{
	public interface IElasticSearchService
	{
		Task Call(EsPermissionOp request, long newIndex);
		long Count();
	}
}
