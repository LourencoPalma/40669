using MonitrGw.Handlers;
using MonitrGw.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonitrGw
{
    public class MonitrEvent
    {
        private readonly DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private Dictionary<string, System.Delegate> dictionaryOfEvents;

        public MonitrEvent()
        {
            dictionaryOfEvents = new Dictionary<string, System.Delegate>();
            dictionaryOfEvents.Add("LastNewsEvent", null);
            dictionaryOfEvents.Add("StockCompetitorsEvent", null);
            dictionaryOfEvents.Add("StockAnalysisEvent", null);
        }

        public event LastNewsEventHandler LastNewsEvent
        {
            add
            {
                dictionaryOfEvents["LastNewsEvent"] = (LastNewsEventHandler)dictionaryOfEvents["LastNewsEvent"] + value;
            }
            remove
            {
                dictionaryOfEvents["LastNewsEvent"] = (LastNewsEventHandler)dictionaryOfEvents["LastNewsEvent"] - value;
            }
        }

        public event StockCompetitorsEventHandler StockCompetitorsEvent
        {
            add
            {
                dictionaryOfEvents["StockCompetitorsEvent"] = (StockCompetitorsEventHandler)dictionaryOfEvents["StockCompetitorsEvent"] + value;
            }
            remove
            {
                dictionaryOfEvents["StockCompetitorsEvent"] = (StockCompetitorsEventHandler)dictionaryOfEvents["StockCompetitorsEvent"] - value;
            }
        }

        public event StockAnalysisEventHandler StockAnalysisEvent
        {
            add
            {
                dictionaryOfEvents["StockAnalysisEvent"] = (StockAnalysisEventHandler)dictionaryOfEvents["StockAnalysisEvent"] + value;
            }
            remove
            {
                dictionaryOfEvents["StockAnalysisEvent"] = (StockAnalysisEventHandler)dictionaryOfEvents["StockAnalysisEvent"] - value;
            }
        }
        
        public void OnLastNewsEvent() {
            MonitrMarketData data = MonitrApi.GetLastNews();
            //var tmp = LastNewsEvent;
            var tmp = (LastNewsEventHandler)dictionaryOfEvents["LastNewsEvent"];
            if (tmp != null) 
            {
                DateTime date = start.AddMilliseconds(data.Time).ToLocalTime();
                tmp(data.Title, data.Link, date);
            }
        }

        public void OnStockCompetitorsEvent(string stockSymbol)
        {
            List<string> data = MonitrApi.GetStockCompetitors(stockSymbol);
            //var tmp = StockCompetitorsEvent;
            var tmp = (StockCompetitorsEventHandler)dictionaryOfEvents["StockCompetitorsEvent"];
            if (tmp != null) tmp(stockSymbol, data);
        }

        public void OnStockAnalysisEvent(string stockSymbol)
        {
            MonitrAnalysisData data = MonitrApi.GetStockAnalysis(stockSymbol);
            //var tmp = StockAnalysisEvent;
            var tmp = (StockAnalysisEventHandler)dictionaryOfEvents["StockAnalysisEvent"];
            if (tmp != null) tmp(data);
        }
    }
}
