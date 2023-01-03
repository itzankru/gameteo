using ItZnak.Infrastruction.Services;
using Microsoft.EntityFrameworkCore;
using GameteoDTO.DTO;

namespace GameteoDTO.Services{

    /* 
        Class: SqlLiteContextService
        Назначение: Реализация IDbContextService 
        Реализация: ADO.NET Entity Framework, исочник данных DB Sqlite.
   */
    public class SqlLiteContextService :DbContext, IDbContextService
    {
        private readonly ILogService _log;
        private readonly IConfigService _conf;
        private readonly string _dbPatch;
        public SqlLiteContextService(
            ILogService log,
            IConfigService conf)
        {
            _log=log;
            _conf=conf;
            _dbPatch=conf.GetString("dbPath");
        }

        public DbSet<CurrencyRate> CURRENCY_RATE { get;set;}
        public DbSet<Currency> CURRENCY { get;set; }

        public DbContext CTX {get{return this;}}

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Data Source={_dbPatch}");
        }
    }
}