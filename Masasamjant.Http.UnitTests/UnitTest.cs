using Microsoft.Extensions.Configuration;

namespace Masasamjant.Http
{
    public abstract class UnitTest
    {
        protected static IConfiguration GetConfiguration(Dictionary<string, string?> values)
        {
            return new ConfigurationBuilder().AddInMemoryCollection(values).Build();
        }
    }
}
