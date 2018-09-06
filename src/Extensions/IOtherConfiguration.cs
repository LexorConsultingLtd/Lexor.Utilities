using Microsoft.EntityFrameworkCore;

namespace Utilities.Extensions
{
    public interface IOtherConfiguration
    {
        void Configure(ModelBuilder modelBuilder);
    }
}
