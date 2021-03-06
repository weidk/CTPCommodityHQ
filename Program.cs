﻿using System;
using System.Configuration;
using System.Threading;

namespace CTPHQ
{
    class Program
    {
        static void Main(string[] args)
        {
            MQClient mq = new MQClient();

            DBClient DB = new DBClient();
            var codes = DB.GetCodeList();

            CTPHQ CTP = new CTPHQ(ConfigurationManager.AppSettings["server"], ConfigurationManager.AppSettings["brokerid"], ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["pwd"], codes);
            
            CTP.OnReceiveHQ += (pDepthMarketData) => {
                Console.WriteLine($"{pDepthMarketData.InstrumentID}    最新价:{pDepthMarketData.LastPrice}");

                FutureHQ objFuture = new FutureHQ();
                objFuture.SCode = pDepthMarketData.InstrumentID;
                objFuture.SName = pDepthMarketData.ExchangeInstID;
                objFuture.OpenPrice = pDepthMarketData.OpenPrice > 1000000000 ? pDepthMarketData.PreClosePrice : pDepthMarketData.OpenPrice;
                objFuture.PrePrice = Math.Round(pDepthMarketData.PreClosePrice, 3);
                objFuture.HighPrice = pDepthMarketData.HighestPrice > 1000000000 ? 0 : pDepthMarketData.HighestPrice;
                objFuture.LowPrice = pDepthMarketData.LowestPrice > 1000000000 ? 0 : pDepthMarketData.LowestPrice;
                objFuture.NewPrice = pDepthMarketData.LastPrice > 1000000000 ? 0 : pDepthMarketData.LastPrice;
                objFuture.TradeValue = pDepthMarketData.OpenInterest;
                objFuture.TradeVolume = pDepthMarketData.Volume;
                objFuture.UpperLimitPrice = pDepthMarketData.UpperLimitPrice; 
                objFuture.LowerLimitPrice = pDepthMarketData.LowerLimitPrice;
                objFuture.BuyPrice1 = pDepthMarketData.BidPrice1 > 1000000000 ? 0 : pDepthMarketData.BidPrice1;
                objFuture.BuyPrice2 = pDepthMarketData.BidPrice2 > 1000000000 ? 0 : pDepthMarketData.BidPrice2;
                objFuture.BuyPrice3 = pDepthMarketData.BidPrice3 > 1000000000 ? 0 : pDepthMarketData.BidPrice3;
                objFuture.BuyPrice4 = pDepthMarketData.BidPrice4 > 1000000000 ? 0 : pDepthMarketData.BidPrice4;
                objFuture.BuyPrice5 = pDepthMarketData.BidPrice5 > 1000000000 ? 0 : pDepthMarketData.BidPrice5;
                objFuture.BuyVol1 = pDepthMarketData.BidVolume1;
                objFuture.BuyVol2 = pDepthMarketData.BidVolume2;
                objFuture.BuyVol3 = pDepthMarketData.BidVolume3;
                objFuture.BuyVol4 = pDepthMarketData.BidVolume4;
                objFuture.BuyVol5 = pDepthMarketData.BidVolume5;
                objFuture.SelPrice1 = pDepthMarketData.AskPrice1 > 1000000000 ? 0 : pDepthMarketData.AskPrice1;
                objFuture.SelPrice2 = pDepthMarketData.AskPrice2 > 1000000000 ? 0 : pDepthMarketData.AskPrice2;
                objFuture.SelPrice3 = pDepthMarketData.AskPrice3 > 1000000000 ? 0 : pDepthMarketData.AskPrice3;
                objFuture.SelPrice4 = pDepthMarketData.AskPrice4 > 1000000000 ? 0 : pDepthMarketData.AskPrice4;
                objFuture.SelPrice5 = pDepthMarketData.AskPrice5 > 1000000000 ? 0 : pDepthMarketData.AskPrice5;
                objFuture.SelVol1 = pDepthMarketData.AskVolume1;
                objFuture.SelVol2 = pDepthMarketData.AskVolume2;
                objFuture.SelVol3 = pDepthMarketData.AskVolume3;
                objFuture.SelVol4 = pDepthMarketData.AskVolume4;
                objFuture.SelVol5 = pDepthMarketData.AskVolume5;
                objFuture.Updatetime = Convert.ToDateTime(pDepthMarketData.TradingDay.Substring(0, 4) + "-" + pDepthMarketData.TradingDay.Substring(4, 2) + "-" + pDepthMarketData.TradingDay.Substring(6, 2) + " " + pDepthMarketData.UpdateTime);

                mq.PublisHQ(objFuture);  // 将报价发布到广播
                DB.rClient.StringSet(objFuture.SCode, Newtonsoft.Json.JsonConvert.SerializeObject(objFuture)); // 将最新的报价保存到数据库
                //DB.UpdateDBHQ(objFuture);

            };

            CTP.Initialize();

            

            

            //把主程序挂起
            TimeSpan closetime = new TimeSpan(23, 55, 0);
            while (DateTime.Now.TimeOfDay < closetime)
            {
                Thread.Sleep(1000 * 60 * 60);
            };

            Environment.Exit(0);
        }
    }
}
