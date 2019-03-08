using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Utilities.EFCore;

namespace Utilities.SeedWork
{
    public class Repository<TEntity, TDbContext> : RepositoryBase<TEntity>
        where TEntity : Entity, IAggregateRoot
        where TDbContext : DbContext, IUserDbContext
    {
        private TDbContext RepositoryContext => (TDbContext)Context;

        public Repository(TDbContext context) : base(context) { }

        protected override void SetUser(ClaimsPrincipal user) => RepositoryContext.User = user;
    }
}
