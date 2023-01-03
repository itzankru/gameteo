
namespace ItZnak.Infrastruction.Extentions{
    public static class ObjectExtentions
    {
        public static TResult Bind<T,TResult>(this T source,  Func<T,TResult> next)
        {
            return next(source);
        }
    }
}