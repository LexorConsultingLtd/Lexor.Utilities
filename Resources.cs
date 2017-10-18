using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lexor.Utilities
{
    public class Resources
    {
        public static async Task<string> ReadResourceAsync(string resourceName, Assembly resourceAssembly = null)
        {
            if (resourceAssembly == null) resourceAssembly = Assembly.GetEntryAssembly();
            var stream = resourceAssembly.GetManifestResourceStream(resourceName);
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}
