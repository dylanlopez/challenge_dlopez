using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Interfaces.Repositories
{
	public interface IPermissionRepository
	{
        Task<int> Create(Permission entity);
        IQueryable<Permission> GetAllBy(Expression<Func<Permission, bool>> predicate, bool asNoTracking = false);
        IQueryable<Permission> Query(bool asNoTracking = true);
        Task Update(Permission entity);
    }
}
