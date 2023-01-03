using GameteoConsole.Handlers.PopulateYearHandler;
using GameteoDTO.Services;
using  ItZnak.DbCoNsoleService.Extentions;
using ItZnak.Infrastruction.Services;

namespace  GameteoConsole.Handlers.PopulateYearHandler{
    /* ===============================================================
        Class:PopulateYearHandler
        Назначение: Считать из источника https://www.cnb.cz данные , 
        рапарсить и сохранить в БД 

    ================================================================== */
   
    public class PopulateYearHandler:IPopulateYearHandler{
        ILogService _log;
        IPopulateYarHtmlDataRequest _request;
        IDbContextService _ctxDb;
        public PopulateYearHandler(ILogService log, IPopulateYarHtmlDataRequest request,IDbContextService ctx ){
            _log=log; _request=request;
            _ctxDb=ctx;
        }
        
        /*  */
        public  async Task PopulateYearDataAsync(int year){
            if(year<2000 || year>DateTime.Now.Year)
                throw new ArgumentException("Incorrect year");
            
            _log.Info("GET:https://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/year.txt?year="+year);
            var dataPages=await _request.ReadPagesAsync(year);
            foreach(var page in dataPages){ 
                await _ctxDb.SaveCurrencyRates(page.ToList());
            }
            _log.Info("saved");
        }
    }
}   