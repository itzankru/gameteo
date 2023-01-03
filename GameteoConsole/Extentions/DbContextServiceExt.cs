
using GameteoDTO.DTO;
using GameteoDTO.Services;

namespace  ItZnak.DbCoNsoleService.Extentions{
    /* ===============================================================
        Class:DbContextServiceExt
        Назначение: Предоставить метод расширение позволяющее сохранять в БД коллекцию  List<CurrencyRate>,
        Мотивация: Сохранение коллекции имеет свою логику и используется на данный момент только  DbCoNsoleService
        переносить метод сохранения на базовый уровень IDbContextService не представляется целесообразным. 
        Там могут быть лишь операции гарантированно имеющий множество потребителей и использующие атомарную неизменную логику.  
        
    ================================================================== */
    public static class DbContextServiceExt{
          
            public static async Task SaveCurrencyRates (this IDbContextService dbService, List<CurrencyRate> rates) {
            foreach(var rate in rates){                    
                if(dbService.CURRENCY.Count(x=>x.Id==rate.CurrencyId)==0)
                                        dbService.CURRENCY.Add(rate.Currency);

                rate.Currency=null;
                if(dbService.CURRENCY_RATE.Count(x=>(x.Day==rate.Day && x.Year==rate.Year && x.CurrencyId==rate.CurrencyId ))==0)
                                dbService.CURRENCY_RATE.Add(rate);
            }

            await dbService.CTX.SaveChangesAsync();
                    
            }
    }

}