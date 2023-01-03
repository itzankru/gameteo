namespace ItZnak.Infrastruction.Services{
    /* сервис конфигурации приложения */
    public interface IConfigService
    {
        int GetInt (string field);
        string GetString(string field);
    }
}
