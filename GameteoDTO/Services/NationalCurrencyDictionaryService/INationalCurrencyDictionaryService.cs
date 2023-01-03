namespace GameteoDTO.Services{
    /* 
        Interface: INationalCurrencyDictionaryService 
        Назначение:справочник наименований валют 
        Мотивация:Справочник позволяет выводить в интерфейс приложения наименования валюты на языке 
        содержащемся в справочнике (текущая версия английский)    
     */
    public interface INationalCurrencyDictionaryService
    {
         public Dictionary <string,string> CurrencyList{get;}
    }
}