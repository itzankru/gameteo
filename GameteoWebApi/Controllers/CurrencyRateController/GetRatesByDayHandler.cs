
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using ItZnak.Infrastruction.Extentions;
using ItZnak.Infrastruction.Web.Controllers;
using GameteoDTO.Services;

namespace GameteowebApi.Controllers
{
 /*=======================================================================================================================================================
    Class: GetRatesByDayHandler 
    Интерфейсы: IWebApiControllerHandler

    Обязанности:Бизнес логика операции получения из базы данных списка котировок на заданный день. ([Route("getratesbyday")])
    Бизнес-логика: 1. Выбрать ближайший к запрашиваемому дню рабочий день исходя из следующих ограничений.
                        Если текущее время меньше заданного в настройке времени перехода - выбираем предидущий день
                        Если выбранный день суббота или воскресенье - выбираем последнюю пятницу
                        Если выбранный день один из государственных праздников выбираем последний рабочй день.
                   2. Запросить дкурсы валюты из БД
                   3. Обогатиь полученный набор данных наименованием валюты из справочника на зананном национальном языке  

============================================================================================*/

     /* структура данных представляющая результаты работы хэндлера */
     public class OutGetRatesByDayHandler{
        /* курсы валют */
        public List<Rate> rates {get; set;}= new List<Rate>();
        /* дата курса */
        public double ratedate {get;set;}
    }
     public class Rate{
        public string name {get;set;}
        public int lotsize {get;set;}
        public string code{get;set;} 
        public string rate {get;set;}
    };

     public class GetRatesByDayHandler:WebApiControllerHandler<DateTime,OutGetRatesByDayHandler>
    {
        IGameteoWebApiController Context => _context as IGameteoWebApiController;
        public GetRatesByDayHandler(IGameteoWebApiController context) : base(context)
        {}

        /* 
           GameteoWebApiController context - экземпляр контроллера содержащий согласно IGameteoWebApiController,IMWebApiController
           все необходимые зависимости для обеспечения работы процедуры.

           Процедура HandleAsync получает на вход дату, далее 
            chiftToWorkDay - выбрать ближайший рабочий день 
            populateRs выбрать данные из БД  

        */

        public override async Task<OutGetRatesByDayHandler> HandleAsync(DateTime p)
        {
            return await Task.Run(()=>{
                    return p.Bind(ChiftToWorkDay)
                            .Bind(day1 => PopulateRs(day1, GetCurrencyName));
                }
            );
        }

        private string GetCurrencyName(string key)
        {
            if (Context.CurrencyDictionary.ContainsKey(key))
                return Context.CurrencyDictionary[key];
            return "";
        }

        private DateTime ChiftToWorkDay(DateTime day)
        {
            float currentTime = (float)DateTime.Now.Hour + ((float)DateTime.Now.Minute / 100);
            float rateChangeTime = float.Parse(_context.Configuration.GetString("rateChangeTime"), CultureInfo.InvariantCulture);

            /* if it is current day before 14.30*/
            if (day.Date == DateTime.Now.Date && (currentTime < rateChangeTime))
            {
                day = day.AddDays(-1);
                _context.Log.Info("ShiftDay:" + day.ToString());
            }

            /* if da is goverment weekedn or weekend */
            DayOfWeek[] weekend = { DayOfWeek.Sunday, DayOfWeek.Saturday };
            List<int> govWeekend = GetGovermentDays(day.Year);
            while (true)
            {
                if (weekend.Contains(day.DayOfWeek) || govWeekend.Contains(day.DayOfYear))
                {
                    day = day.AddDays(-1);
                    _context.Log.Info("ShiftDay:" + day.ToString());
                }
                else
                {
                    return day;
                }
            }
        }

        private OutGetRatesByDayHandler PopulateRs(DateTime day, Func<string, string> getCurrencyName)
        {
            OutGetRatesByDayHandler rslt = new()
            {
                ratedate = day.ToUnixTime(),
                rates = Context.DbContext.CURRENCY_RATE
                        .Where(x => x.Year == day.Year && x.Day == day.DayOfYear)
                        .Include(rate => rate.Currency)
                        .Select(d => new Rate()
                        {
                            code = d.Currency.Id,
                            lotsize = d.Currency.LotSize,
                            name = getCurrencyName(d.Currency.Id),
                            rate = System.Math.Round(d.Value, 3).ToString()
                        }).ToList()
            };

            _context.Log.Info("GetRates:  Date=" + rslt.ratedate.FromUnixTime().ToString() + " Count: " + rslt.rates.Count);
            return rslt;
        }

        private List<int> GetGovermentDays(int year) => _context.Configuration.GetString("govermentWeekends")
                    .Split(",")
                    .Select(x => x.Split("_"))
                    .Select(a => new DateTime(year, int.Parse(a[0]), int.Parse(a[1])))
                    .Select(y => y.DayOfYear).ToList();
    }
}