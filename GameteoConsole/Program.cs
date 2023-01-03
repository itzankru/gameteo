using Microsoft.Extensions.DependencyInjection;
using ItZnak.Infrastruction.Services;
using GameteoConsole.Handlers.PopulateYearHandler;
using GameteoDTO.Services;

internal class Program
{
/*==================================================================================================================== 
МОДУЛЬ: GameteoConsole- консольное приложение считывающее  курсы валют по отношению к чешской кроне и записывающее курсы в БД.

РЕАЛИЗАЦИЯ: Консольное приложение. 

URL источника: https://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/year.txt?year=

ФИЗИЧЕСКИЕ МОДУЛИ:
GameteoConsole.dll: консольное приложение считывающее  курсы валют по отношению к чешской кроне и записывающее курсы в БД. 
GameteoDTO.dll: Сервис доступа к базе данных и DTO обьекты приложения  DTO

ОСНОВНЫЕ ЭЛЕМЕНТЫ ПРИЛОЖЕНИЯ

GameteoConsole.Handlers.PopulateYarHtmlDataRequest: HTML запрос и парсинг полученных данных в целевой формат соответсвующий DTO
GameteoConsole.Handlers.PopulateYearHandler: Команда обработчик загрузки данных за хаданный год в БД 

ItZnak.Infrastruction.Services.LogService: Сервис логирования   
ItZnak.Infrastruction.Services.ConfigService: Сервис конфигурации
GameteoDTO.Services.SqlLiteContextService: Сервис доступа к данным


СЦЕНАРИЙ РАБОТЫ ЗАГРУЗКА ЗАДАННОГО ГОДА 
Консольное прложение реализовано с использованием шаблона проектирования Dependency injection
1. Зарегистрировать в провайдере IServiceProvider набор необходимых зависимостей
2. Используя провайдер IServiceProvider выбрать сервис  IPopulateYearHandler  и запустить PopulateYearDataAsync

=======================================================================================================================================================*/

 
    private static async Task Main(string[] args)
    {   
        /* initiate DI  */
        IServiceProvider serviceProvider;
        initHost(out serviceProvider);        

        /* help mode */
         if(string.Join("",args).ToUpper().Equals("H")){
            Console.WriteLine("Web Scarper www.cnb.cz");
            Console.WriteLine("URL:     https://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/year.txt?year=");
            Console.WriteLine("Usage:   DbConsoleService [param]");
            Console.WriteLine("<year>   Value between 2000 and current year");
            Console.WriteLine("<d>      Show currency dictionary");
            Console.WriteLine("<>       Observe current year");

            return;
         }

         /* currency dictionary mode */
         if(string.Join("",args).ToUpper().Equals("D")){
            Console.WriteLine("Web Scarper www.cnb.cz");
            Console.WriteLine("Currency dictionary:");
            var rs=serviceProvider.GetRequiredService<INationalCurrencyDictionaryService>().CurrencyList;
            foreach(var k in rs.Keys ){
                Console.WriteLine(k+"="+rs[k]);
            }
            return;
         }

        /* one year mode */
        if( int.TryParse( string.Join("",args), out int year)){
            await serviceProvider.GetRequiredService<IPopulateYearHandler>().PopulateYearDataAsync(year);
            return;
        }

        /* site observe mode */
        CancellationTokenSource cts= new CancellationTokenSource();
        Console.CancelKeyPress+= new ConsoleCancelEventHandler((object sender, ConsoleCancelEventArgs args)=>{
            cts.Cancel();
        });
        
        await Task.Run(async ()=>{
            var cnf=serviceProvider.GetService<IConfigService>();
            int delay=cnf.GetInt("requestLoopDelayMs");
            do{
                await serviceProvider.GetRequiredService<IPopulateYearHandler>().PopulateYearDataAsync(DateTime.Now.Year);
                await Task.Delay(delay==0?60000:delay);
            }while(true);
        },cts.Token);
    }


    static void initHost(out IServiceProvider  serviceProvider){
         IServiceCollection services = new ServiceCollection();
         services.AddSingleton <ILogService,SerilogService>();
         services.AddSingleton <IConfigService>(new ConfigService());
         services.AddTransient <IDbContextService,SqlLiteContextService>();
         services.AddTransient <IPopulateYarHtmlDataRequest,PopulateYarHtmlDataRequest>();
         services.AddTransient <IPopulateYearHandler,PopulateYearHandler>();
         services.AddSingleton <INationalCurrencyDictionaryService ,CurrencyDictionaryService>();
         
         serviceProvider = services.BuildServiceProvider();  
    }

    
}