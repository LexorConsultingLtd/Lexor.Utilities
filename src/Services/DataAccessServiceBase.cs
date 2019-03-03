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
    public abstract class DataAccessServiceBase<T> where T : IAggregateRoot
    {
        protected DbContext Context { get; }
        protected IAsyncRepository<T> Repository { get; }

        protected DataAccessServiceBase(IAsyncRepository<T> repository, DbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));

            if (UserDbContext == null)
                throw new ArgumentException($"{nameof(context)} must implement {typeof(IUserDbContext)}");
        }

        protected IUserDbContext UserDbContext => Context as IUserDbContext;

        protected ClaimsPrincipal User
        {
            set
            {
                UserDbContext.User = value;
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
