using Microsoft.EntityFrameworkCore;
using Utilities.Extensions;

namespace Utilities.EFCore
{
    public class NoTrackDbContext : DbContext
    {
        public NoTrackDbContext(DbContextOptions options)
            : base(options)
        {
            this.DisableAutomaticEntityTracking();
        }
    }
}
