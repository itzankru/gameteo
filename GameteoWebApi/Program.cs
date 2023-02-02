 using Microsoft.Extensions.FileProviders;
using ItZnak.WebApi.Services;
using ItZnak.Infrastruction.Extentions;
using System.Net;
using ItZnak.Infrastruction.Web.Middleware;
/*=======================================================================================================================================================  
МОДУЛЬ: RestApi Server - бэкэнд+фронтэнд сервера тестового задания .

РЕАЛИЗАЦИЯ: WebApi сервер с реализацией хостинга в консольном приложении. 

URL сайта: http://20.160.63.109:8080

ФИЗИЧЕСКИЕ МОДУЛИ:
GameteoWebApi.dll: GameteoWebApi = webapi (консольное приложение) контроллеры конечных точек и обработчики операций конечных точек
Infrastruction.dll: Инфраструктура проекта сервисы логирования, конфигурации компоненты MiddleWare, базовые классы контроллеров
GameteoDTO.dll:  Сервис доступа к данным и DTO обьекты приложения  

ОСНОВНЫЕ ЭЛЕМЕНТЫ ПРИЛОЖЕНИЯ

GameteowebApi.Controllers.CurrencyRateController: REST контролер конечной точки /api/currencyrate/
GameteowebApi.Controllers.OutGetRatesByDayHandler: Обработчик метода getratesbyday конечной точки /api/currencyrate/

ItZnak.Infrastruction.Services.LogService: Сервис логирования   
ItZnak.Infrastruction.Services.ConfigService: Сервис конфигурации
ItZnak.Infrastruction.Web.Middleware.GlobalExceptionMdl: ASP.NET Core Middleware глобальный обработчик ошибок REST сервиса

GameteoDTO.Services.SqlLiteContextService: Сервис доступа к данным и DTO обьекты приложения
GameteoDTO.Services.CurrencyDictionaryService: Справочник наименований валют  



ОБЩИЙ СЦЕНАРИЙ ОБРАБОТКИ ЗАПРОСОВ  
1. IMWebApiController принимает HTTP запрос и  маршрутизирует его в соответсвующий обработчик операции (IWebApiControllerHandler)  
2. IMWebApiController задает обаботчик операции (IWebApiControllerHandler) и инициалиирует его набором сервисов необходимых для обработки запроса (ILogService,IConfigService,IDbcontext ...) 
    * IMWebApiController получает зависимости в конструкторе с использованием мехнизма Dependency Injection
    ** IMWebApiController сохраняет зависимости как свойства экземрляра объекта 
    *** IMWebApiController инициализирует обработчик IWebApiControllerHandler ссылкой на собственный экземпляр
3. IWebApiControllerHandler параметризирует процедуру HandleAsync или Handle обработчика  IWebApiControllerHandler параметрами полученными в рамках HTTP запроса и  запускает выполнение обработчика операции 
4. IWebApiControllerHandler на основании результатов  работы функции HandleAsync формирует HTTPResonse    


5. В случае появления Exception или BusinessException, глобальный обработчик исключений GlobalExceptionMdl 
формирует ответный JSON и выставлет Response.StatusCode=422 для BusinessException и 500 для критической ошибки

=======================================================================================================================================================*/

internal partial class Program
{
    const int HTTP_PORT = 8080;
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        /* INFRASTRACTION SERVICES*/
        builder.Services.AddLogService();
        builder.Services.AddConfigService();
        builder.Services.AddDbContextService();
        builder.Services.AddCurrencyDictionaryService();
        /* ConfigureKestrel*/
        builder.WebHost.ConfigureKestrel(options => options.Listen(IPAddress.Any, HTTP_PORT));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseDefaultFiles();

        //app.UseHttpsRedirection();
        app.UseStaticFiles(new StaticFileOptions()
        {
            OnPrepareResponse = (cntx) =>
            {
                string path = cntx.File.PhysicalPath;
                if (path.EndsWith(".gif") || path.EndsWith(".jpg") || path.EndsWith(".png") || path.EndsWith(".svg"))
                {
                    TimeSpan maxAge = new(7, 0, 0, 0);
                    cntx.Context.Response.Headers.Add("Cache-Control", "max-age=" + maxAge.TotalSeconds.ToString("0"));
                }
            },
            FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))
        });
        app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        app.UseAuthorization();
        app.UseGlobalExceptionMdl();
        app.MapControllers();
        app.Run();
    }
}