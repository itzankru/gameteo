using System.Threading.Tasks;

/*======================================================================================================================================================= 
Класс:WebApiControllerHandler
Абстрактный класс реализация IWebApiControllerHandler
======================================================================================================================================================= */

namespace ItZnak.Infrastruction.Web.Controllers
{
    public  abstract class WebApiControllerHandler<TIn,TOut>:IWebApiControllerHandler<TIn,TOut>{
        protected IMWebApiController _context;
        protected WebApiControllerHandler(IMWebApiController context) => _context = context;
        public virtual TOut Handle(TIn p)
        {
            throw new System.NotImplementedException();
        }
        public virtual Task<TOut> HandleAsync(TIn p)
        {
            throw new System.NotImplementedException();
        }
    }
}