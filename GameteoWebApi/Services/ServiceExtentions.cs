using ItZnak.Infrastruction.Services;
using GameteoDTO.Services;

namespace ItZnak.WebApi.Services{
    /* методы расширения для регистрации IDbContextService и INationalCurrencyDictionaryService в коллекции DI */
     public static class ServiceExtentions
    {
        public static void AddDbContextService(this IServiceCollection services)
        {
            services.AddTransient<IDbContextService,SqlLiteContextService>();
        }
        public static void  AddCurrencyDictionaryService(this IServiceCollection services)
        {
            services.AddSingleton<INationalCurrencyDictionaryService,CurrencyDictionaryService>();
        }
    }
}