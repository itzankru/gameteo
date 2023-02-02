
using Microsoft.EntityFrameworkCore;
using GameteoDTO.DTO;
/* 
        Interface: IDbContextService 
        Назначение:Точка доступа к БД
 */
namespace  GameteoDTO.Services
{
    public interface IDbContextService{
        /* курс валюты на дату */
         DbSet<CurrencyRate> CURRENCY_RATE { get; set; }
         /* справочник кодов валют с указанием размера лота по отношению к базовой валюте  */
         DbSet<Currency> CURRENCY { get; set; }

         DbContext CTX {get;}
    }
}