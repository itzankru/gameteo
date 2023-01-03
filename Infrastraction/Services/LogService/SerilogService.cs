using Serilog;
/* реализация сервиса логирования сообщений на основе Serilog */
namespace ItZnak.Infrastruction.Services{
 public class SerilogService : ILogService
    {
        readonly Serilog.ILogger _sl;
        public SerilogService()
        {
             _sl =  new LoggerConfiguration()
                    .WriteTo
                    .Console()
                    .CreateLogger();
        }

        public void Exception(Exception e)
        {
            _sl.Error(e.ToString());
        }

        public void Exception(string s)
        {
           _sl.Error(s);
        }

        public void Info(string s)
        {
            _sl.Information(s);
        }
    }
}