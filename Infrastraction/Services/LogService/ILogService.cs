/* сервис логирования сообщений */
namespace ItZnak.Infrastruction.Services{
    public interface ILogService
    {
        void Info(string s);
        void Exception(Exception e);
        void Exception(string s);
 
    }
}