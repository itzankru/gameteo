using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Mvc;
using ItZnak.Infrastruction.Services;
using System.Diagnostics;

/*======================================================================================================================================================= 
Класс:WebApiController
Назначение:Базовая реализация IMWebApiController
======================================================================================================================================================= */

namespace ItZnak.Infrastruction.Web.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class WebApiController : ControllerBase,IMWebApiController
    {
         protected readonly IConfigService _conf;
        protected ILogService _logger ;

        public IConfigService Configuration {get{return _conf;}}

        public IPrincipal CurrentPrincipal {get{return this.User;}}

        public ILogService Log {get{return _logger;}}

        public WebApiController( IConfigService conf, ILogService logger)
        {
            _conf = conf;
            _logger= logger;
        }
    }
}
