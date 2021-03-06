using SQLite.Net;
using SQLite.Net.Interop;
using System;
using System.Linq;

namespace Stocks
{
    public class Database : SQLiteConnection
    {
        public Database(string path, ISQLitePlatform platform) : base(platform, path)
        {
            Execute(
            "CREATE TABLE VALUATION( " +
            "    ID STRING PRIMARY KEY     NOT NULL, " +
            "    STOCKID           INT NOT NULL, " +
            "    Price           TEXT NOT NULL " +
           ");");      
        }

        public SQLiteCommandResult GetData()
        {
            var command = CreateCommand("SELECT * FROM 'VALUATION'");
            var data = command.ExecuteDeferredQuery();

            return data;
        }

        public void UpdateStock(string stockSymbol)
        {
            var stock = new Stock { Symbol = stockSymbol };

            try
            {
                var valuations = new YahooScraper().GetValuations(stock, DateTime.Now.AddYears(1), DateTime.Now).ToList();

                BeginTransaction();

                foreach (var valuation in valuations)
                {
                    Execute("INSERT INTO VALUATION (ID, STOCKID, Price) VALUES(@Param1, @Param2, @Param3);", new object[] { Guid.NewGuid().ToString(), valuation.StockId, valuation.Price });
                }

                Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}