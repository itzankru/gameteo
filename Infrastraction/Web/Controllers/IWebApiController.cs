using ItZnak.Infrastruction.Services;
using System.Security.Principal;
namespace ItZnak.Infrastruction.Web.Controllers
{
/*======================================================================================================================================================= 
Интерфейс:IMWebApiController
Назначение: 
    Описывает обязнности класса контроллера системы. 
    В базе контроллер содержит следующие вспомогательные сервисы
     Configuration - доступ к appsettings
     CurrentPrincipal - текущий пользователь и роль (для аутентифисированных запросов)
     Log - логер
======================================================================================================================================================= */
    public interface IMWebApiController
    {
        IConfigService Configuration {get;}
        IPrincipal CurrentPrincipal {get;}
        ILogService Log {get;}
    }
}