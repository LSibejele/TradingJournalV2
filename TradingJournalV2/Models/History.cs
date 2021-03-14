using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingJournalV2.Models
{
    public class History
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public string Trade { get; set; }
        public string CurrencyPair { get; set; }
        public double Lots { get; set; }
        public double Price { get; set; }
        public double StopLoss { get; set; }
        public double TakeProfit { get; set; }
        public string ChartPattern { get; set; }
        public string Result { get; set; }
        public byte[] Screenshot { get; set; }
        public string Comments { get; set; }
    }

}
