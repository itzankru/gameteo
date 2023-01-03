using Microsoft.AspNetCore.Mvc;
using GameteoDTO.Services;
using ItZnak.Infrastruction.Services;
using ItZnak.Infrastruction.Web.Controllers;

namespace GameteowebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameteoWebApiController : WebApiController,IGameteoWebApiController
    {
        protected INationalCurrencyDictionaryService _currencyDict;
        public GameteoWebApiController( IConfigService conf, ILogService logger,INationalCurrencyDictionaryService currencyDict, IDbContextService dbContext ) : base( conf, logger)
        {
             _currencyDict=currencyDict;
            DbContext = dbContext;
        }
        public IDbContextService DbContext { get; }
        public Dictionary<string, string> CurrencyDictionary => _currencyDict.CurrencyList;
    }
}