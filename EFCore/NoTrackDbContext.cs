using Lexor.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;

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
