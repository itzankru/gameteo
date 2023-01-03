using Microsoft.AspNetCore.Mvc;
using GameteoDTO.Services;
using ItZnak.Infrastruction.Services;

/*=======================================================================================================================================================
    Class: CurrencyRateController 
    Интерфейсы: IGameteoWebApiController,IMWebApiController
    Обязанности:Входная точка запроса. 
        1.Маршрутизировать запрос согласно ROUTE. 
        2.Создать соответсвующий обработчик.
        3.Параметризовать обработчик ссылкой на себя. (таким образом через IGameteoWebApiController,IMWebApiController передаем весь необходимый набор зависимостей)
        4.Запустить обработчик IWebApiControllerHandler
        5.Результаты работы записать в OkObjectResult  

===================================================================================================================================================================*/
namespace GameteowebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyRateController : GameteoWebApiController
    {
        public CurrencyRateController(IConfigService conf, ILogService logger, INationalCurrencyDictionaryService currencyDict, IDbContextService dbContext) : base(conf, logger, currencyDict, dbContext)
        {
        }

        [HttpGet]
        [Route("getratesbyday")]
        public async Task<IActionResult> GetRatesByDayAsync(DateTime day)
        {
            var rslt=await new GetRatesByDayHandler(this).HandleAsync(day);
            return this.Ok(rslt);
        }
    }
}