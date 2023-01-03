using GameteoDTO.Services;
using ItZnak.Infrastruction.Web.Controllers;

/* Интерфейс контроллера конечной точки приложения  GameteoWebApi обогащающий стандартный контроллер сервисом доступа 
к БД с DTO обьектами приложения и справочнком наименований валют    */
namespace GameteowebApi.Controllers{
  public interface IGameteoWebApiController : IMWebApiController
    {
        /* доступ к БД */
        IDbContextService DbContext {get;}
        /* справочник валют */
        Dictionary <string,string>  CurrencyDictionary {get;}
    }
}