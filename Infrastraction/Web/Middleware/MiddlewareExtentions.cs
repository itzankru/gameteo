using Microsoft.AspNetCore.Builder;
/* расширение для регистрации мидлваре в конвейере */
namespace ItZnak.Infrastruction.Web.Middleware{
    public static class MiddlewareExtentions
    {
        public static IApplicationBuilder UseGlobalExceptionMdl(this IApplicationBuilder builder)
            {
                return builder.UseMiddleware<GlobalExceptionMdl>();
            }
    }
}