
using ItZnak.Infrastruction.Services;
using ItZnak.Infrastruction.Web.Exceptions;
using Microsoft.AspNetCore.Http;


namespace  ItZnak.Infrastruction.Web.Middleware{
    
/*======================================================================================================================================================= 
Класс:GlobalExceptionMdl
Цель: В конвейеере обработки запроса  перехватить не обработнное исключение и в зависимости от типа исключения сформировать HTTP Response
StatusCode =422 для BusinessException
StatusCode =500 для Exception
======================================================================================================================================================= */

    public class GlobalExceptionMdl
    {
        readonly RequestDelegate _next;
        readonly ILogService _log;

        public GlobalExceptionMdl(RequestDelegate next, ILogService log )
        {
            _log=log;
            _next=next;
        }
       public async Task InvokeAsync(HttpContext context)
       {
        try{
             await _next(context);
        } catch(BusinessException ex){
             context.Response.StatusCode=422;
             await context.Response.WriteAsync(ex.ToJson());
        }
         catch(Exception ex){
            _log.Exception(ex.ToString());
            context.Response.StatusCode=500;
            await context.Response.WriteAsync("{}");
        }
        }
    }
}