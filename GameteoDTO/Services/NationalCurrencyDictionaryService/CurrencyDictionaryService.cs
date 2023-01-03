
using ItZnak.Infrastruction.Services;
namespace GameteoDTO.Services{
    /* 
            Реализация INationalCurrencyDictionaryService
            Общий алгоритм:
                1. Считать JSON файл указанный в nationalCurrencyDictionaryPath
                2. Серилизвать файл во внутренню переменную в виде Dictionary<string, string>
     */
    public class CurrencyDictionaryService : INationalCurrencyDictionaryService
    {
        const string PATH_KEY="nationalCurrencyDictionaryPath";
        readonly IConfigService _cnf;
        readonly ILogService _log;
        static readonly Dictionary<string, string> s_currencyList = new();

        public CurrencyDictionaryService(
            IConfigService cnf,
            ILogService log)
        {
            _cnf=cnf;
            _log=log;
            ReadDictionary();
        }
        private  void ReadDictionary(){
            string jsonString = File.ReadAllText(_cnf.GetString(PATH_KEY));
            System.Text.Json.JsonSerializer
                            .Deserialize<Dictionary<string, string>>(jsonString)
                            .ToList()
                            .ForEach(itm =>s_currencyList.Add(itm.Key,itm.Value));
            _log.Info("currency dictionary loaded");
        }
        public Dictionary<string, string> CurrencyList { get => s_currencyList; }
    }
}