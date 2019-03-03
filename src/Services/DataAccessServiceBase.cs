using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Threading.Tasks;
using Utilities.EFCore;
using Utilities.SeedWork;

namespace Utilities.Services
{
    [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
    public abstract class DataAccessServiceBase<TEntity, TDbContext>
        where TEntity : IAggregateRoot
        where TDbContext : DbContext, IUserDbContext
    {
        protected TDbContext Context { get; }
        protected IAsyncRepository<TEntity> Repository { get; }

        protected DataAccessServiceBase(IAsyncRepository<TEntity> repository, TDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        protected ClaimsPrincipal User
        {
            set
            {
                Context.User = value;
                Repository.User = value;
            }
        }

        public async Task<IEnumerable<TEnumeration>> GetEnumerationListAsync<TEnumeration>()
            where TEnumeration : Enumeration
        {
            return await Context.Set<TEnumeration>().ToListAsync();
        }
    }
}
