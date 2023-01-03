using ItZnak.Infrastruction.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ItZnak.Infrastruction.Extentions{
     public static class ServiceExtentions{
        public static void  AddLogService(this IServiceCollection services)
        {
            services.AddSingleton<ILogService,SerilogService >();
        }

        public static void  AddConfigService(this IServiceCollection services)
        {
            services.AddSingleton<IConfigService,ConfigService >();
        }
    }
}