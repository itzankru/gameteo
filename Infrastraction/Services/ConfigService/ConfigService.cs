
using Microsoft.Extensions.Configuration;

/* реализация  IConfigService*/
namespace ItZnak.Infrastruction.Services
{
    public class ConfigService : IConfigService
    {
        readonly IConfigurationRoot _root;
        public ConfigService()
        {
            _root=new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json")
                        .Build();
        }
        public int GetInt(string field)
        {
            return int.Parse(_root[field]??"0");
        }

        public string GetString(string field)
        {
            return _root[field]??"";
        }
    }
}