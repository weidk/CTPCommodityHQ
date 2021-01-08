using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPHQ
{
    public class FutureHQ
    {

        private string sCode;
        [SugarColumn(IsPrimaryKey = true)]
        public string SCode
        {
            get { return sCode; }
            set { sCode = value; }
        }

        private string sName;
        public string SName
        {
            get { return sName; }
            set { sName = value; }
        }

        private double openPrice;
        public double OpenPrice
        {
            get { return openPrice; }
            set { openPrice = value; }
        }

        private double prePrice;
        public double PrePrice
        {
            get { return prePrice; }
            set { prePrice = value; }
        }

        private double highPrice;
        public double HighPrice
        {
            get { return highPrice; }
            set { highPrice = value; }
        }

        private double lowPrice;
        public double LowPrice
        {
            get { return lowPrice; }
            set { lowPrice = value; }
        }

        private double tradeValue;
        public double TradeValue
        {
            get { return tradeValue; }
            set { tradeValue = value; }
        }

        private int tradeVolume;
        public int TradeVolume
        {
            get { return tradeVolume; }
            set { tradeVolume = value; }
        }

        private double newPrice;
        public double NewPrice
        {
            get { return newPrice; }
            set { newPrice = value; }
        }

        private double selPrice1;
        public double SelPrice1
        {
            get { return selPrice1; }
            set { selPrice1 = value; }
        }

        private double selPrice2;
        public double SelPrice2
        {
            get { return selPrice2; }
            set { selPrice2 = value; }
        }

        private double selPrice3;
        public double SelPrice3
        {
            get { return selPrice3; }
            set { selPrice3 = value; }
        }

        private double selPrice4;
        public double SelPrice4
        {
            get { return selPrice4; }
            set { selPrice4 = value; }
        }

        private double selPrice5;
        public double SelPrice5
        {
            get { return selPrice5; }
            set { selPrice5 = value; }
        }

        private double buyPrice1;
        public double BuyPrice1
        {
            get { return buyPrice1; }
            set { buyPrice1 = value; }
        }

        private double buyPrice2;
        public double BuyPrice2
        {
            get { return buyPrice2; }
            set { buyPrice2 = value; }
        }

        private double buyPrice3;
        public double BuyPrice3
        {
            get { return buyPrice3; }
            set { buyPrice3 = value; }
        }

        private double buyPrice4;
        public double BuyPrice4
        {
            get { return buyPrice4; }
            set { buyPrice4 = value; }
        }

        private double buyPrice5;
        public double BuyPrice5
        {
            get { return buyPrice5; }
            set { buyPrice5 = value; }
        }

        private int buyVol1;
        public int BuyVol1
        {
            get { return buyVol1; }
            set { buyVol1 = value; }
        }

        private int buyVol2;
        public int BuyVol2
        {
            get { return buyVol2; }
            set { buyVol2 = value; }
        }

        private int buyVol3;
        public int BuyVol3
        {
            get { return buyVol3; }
            set { buyVol3 = value; }
        }

        private int buyVol4;
        public int BuyVol4
        {
            get { return buyVol4; }
            set { buyVol4 = value; }
        }

        private int buyVol5;
        public int BuyVol5
        {
            get { return buyVol5; }
            set { buyVol5 = value; }
        }

        private int selVol1;
        public int SelVol1
        {
            get { return selVol1; }
            set { selVol1 = value; }
        }

        private int selVol2;
        public int SelVol2
        {
            get { return selVol2; }
            set { selVol2 = value; }
        }

        private int selVol3;
        public int SelVol3
        {
            get { return selVol3; }
            set { selVol3 = value; }
        }

        private int selVol4;
        public int SelVol4
        {
            get { return selVol4; }
            set { selVol4 = value; }
        }

        private int selVol5;
        public int SelVol5
        {
            get { return selVol5; }
            set { selVol5 = value; }
        }

        private DateTime updatetime;
        public DateTime Updatetime
        {
            get { return updatetime; }
            set { updatetime = value; }
        }

        private double upperLimitPrice;
        public double UpperLimitPrice
        {
            get { return upperLimitPrice; }
            set { upperLimitPrice = value; }
        }

        private double lowerLimitPrice;
        public double LowerLimitPrice
        {
            get { return lowerLimitPrice; }
            set { lowerLimitPrice = value; }
        }
    }
}
