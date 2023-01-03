
using System.Globalization;
using System.Net;
using ItZnak.DbCoNsoleService.Exceptions;
using ItZnak.Infrastruction.Extentions;
using ItZnak.Infrastruction.Services;
using GameteoDTO.DTO;

namespace  GameteoConsole.Handlers.PopulateYearHandler{
      public interface IPopulateYarHtmlDataRequest{
       Task<List<ICollection<CurrencyRate>>> ReadPagesAsync(int year);
    }
/* ==============================================================================================================================
        Class:PopulateYarHtmlDataRequest
        Назначение: Считать из источника https://www.cnb.cz данные , рапарсить и сохранить в виде набора 
        страниц данных List<ICollection<CurrencyRate>> 
        
================================================================================================================================= */

    public class PopulateYarHtmlDataRequest:IPopulateYarHtmlDataRequest {
        const string DATA_URL= "https://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/year.txt?year=";
        ILogService _log;
         IConfigService _cnf;
      
        public PopulateYarHtmlDataRequest(ILogService log, IConfigService cnf ){
            _log=log; _cnf=cnf;
        }

        public async Task<List<ICollection<CurrencyRate>>> ReadPagesAsync(int year)
        {   
            List<ICollection<CurrencyRate>> rslt= new List<ICollection<CurrencyRate>>();
            try{
                string url=DATA_URL+year.ToString();
                _log.Info("try get "+url);    
                /* получить HTML */         
                var body =await GetHtml(url);
                /* разбить полученную строку на блоки */     
                var pages= body.Bind(ReadPages);
                /* обработать каждый блок и обьединить результат в общую коллекцию*/
                pages.ForEach(page=>{
                     rslt.Add(page.Bind(ParseCurrency).Bind(ParseRates));
                });
                return rslt;
                
                }catch(Exception e){
                    _log.Exception(e);
                    throw new HtmlParseException();
                }
        }

        /* HTML страница может содержать несколько блоков с массивами тип валюты-значение
            задача метода разбить полученную строку на блоки и каждый из них поместить в отдельную строку
            Date|1 AUD|1 BGN|1 BRL|1 CAD|1 CHF|1 CNY|1 DKK|1 EUR|1 GBP|1 HKD|1 HRK|100 HUF|1000 IDR|1 ILS|100 INR|100 ISK|100 JPY|100 KRW|1 MXN|1 MYR|1 NOK|1 NZD|100 PHP|1 PLN|1 RON|100 RUB|1 SEK|1 SGD|100 THB|1 TRY|1 USD|1 XDR|1 ZAR
            03.01.2022|15.818|12.690|3.906|17.210|23.931|3.441|3.337|24.820|29.502|2.803|3.301|6.751|1.532|7.068|29.412|16.816|19.016|1.833|1.068|5.239|2.481|14.902|42.757|5.408|5.016|29.362|2.410|16.189|65.897|1.646|21.860|30.595|1.382
            Date|1 AUD|1 BGN|1 BRL|1 CAD|1 CHF|1 CNY|1 DKK|1 EUR|1 GBP|1 HKD|1 HRK|100 HUF|1000 IDR|1 ILS|100 INR|100 ISK|100 JPY|100 KRW|1 MXN|1 MYR|1 NOK|1 NZD|100 PHP|1 PLN|1 RON|100 RUB|1 SEK|1 SGD|100 THB|1 TRY|1 USD|1 XDR|1 ZAR
            03.04.2022|15.818|12.690|3.906|17.210|23.931|3.441|3.337|24.820|29.502|2.803|3.301|6.751|1.532|7.068|29.412|16.816|19.016|1.833|1.068|5.239|2.481|14.902|42.757|5.408|5.016|29.362|2.410|16.189|65.897|1.646|21.860|30.595|1.382
         */
        private List<string[]> ReadPages(string html){
             List<string[]> pages = new List<string[]>();
             var rs=html.Split(new string[] { Environment.NewLine },StringSplitOptions.None);      
             List<string> page= new List<string>();
             foreach(var itm in rs){
                string [] row=itm.Split(new string[] {"|"},StringSplitOptions.None);
                if(row[0]=="Date" && page.Count>0){
                        pages.Add(page.ToArray());
                        page = new List<string>();
                }
                page.Add(itm);
            }  
            pages.Add(page.ToArray());
            return pages;

        }

        /* считать первую строчку и сформировать DTO  List<Currency>*/
        private Tuple<string[],List<Currency>> ParseCurrency(string[] p){
             string[] h=p[0].Split(new string[] {"|"},StringSplitOptions.None).Skip(1).ToArray();
             var d=h.Select(x=>new Currency(){
                                                Id=x.Split(Char.Parse(" "))[1],
                                                LotSize=int.Parse(x.Split(Char.Parse(" "))[0])
                                             }).ToList();               
             
             return new Tuple<string[] , List<Currency>>(p.Skip(1).ToArray(), d);  
        }

        /* для каждой валюты указанной в первой строке блока считать курсы */
        private ICollection<CurrencyRate> ParseRates(Tuple<string[],List<Currency>> p){
            var rateRows=p.Item1;
            var currencyRows=p.Item2;

            List<CurrencyRate> rslt = new List<CurrencyRate>();  
            /* цикл по валютам */
            for(int rateRowIdx=0;rateRowIdx<rateRows.Length;rateRowIdx++){
                if(string.IsNullOrEmpty(p.Item1[rateRowIdx]))
                    continue;

                string [] rowData=p.Item1[rateRowIdx].Split(new string[] {"|"},StringSplitOptions.None);
                DateTime rowDate=DateTime.ParseExact(rowData[0],"dd.MM.yyyy",null);
                /* цикл по курсам валюты */
                for(int curIdx=1;curIdx<rowData.Length;curIdx++){
                            rslt.Add(new CurrencyRate(){
                                            Currency=currencyRows[curIdx-1],
                                            CurrencyId=currencyRows[curIdx-1].Id,
                                            Value=float.Parse(rowData[curIdx]),
                                            Day=rowDate.DayOfYear,
                                            Year=rowDate.Year
                                        });                   
                }
            } 
             return rslt;
        }
        
        /* получить данные HTML страницы в виде одной строки*/
        private async Task<string> GetHtml(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            using(HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
            using(Stream stream = response.GetResponseStream())
            using(StreamReader reader = new StreamReader(stream))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}