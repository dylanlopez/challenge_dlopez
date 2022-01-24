using Application.Interfaces.Repositories;
using Domain.Entities;
using Infraestructure.Persistence;
using System.Data.Entity;
using System.Linq.Expressions;

namespace Infraestructure.Repositories
{
	public class PermissionRepository : IPermissionRepository
	{
		private readonly DBExamContext _context;

		public PermissionRepository(DBExamContext context)
		{
			_context = context;
		}

        public async Task<int> Create(Permission entity)
        {
            _context.Permissions.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }
        public IQueryable<Permission> GetAllBy(Expression<Func<Permission, bool>> predicate, bool asNoTracking = false)
        {
            IQueryable<Permission> result;
            if (predicate == null)
            {
                result = Query(asNoTracking)
                    .Include(i1 => i1.PermissionTypeNavigation);
            }
            else
            {
                result = Query(asNoTracking)
                    .Where(predicate)
                    .Include(i1 => i1.PermissionTypeNavigation);
            }
            return result;
        }
        public IQueryable<Permission> Query(bool asNoTracking = true)
        {
            return asNoTracking ? _context.Set<Permission>().AsNoTracking() : _context.Set<Permission>();
        }
        public async Task Update(Permission entity)
        {
            _context.Permissions.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
