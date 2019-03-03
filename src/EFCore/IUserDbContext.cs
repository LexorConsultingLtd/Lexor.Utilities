using System.Security.Claims;

namespace Utilities.EFCore
{
    public interface IUserDbContext
    {
        ClaimsPrincipal User { set; }
    }
}
