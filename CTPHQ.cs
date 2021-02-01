using CTPMarketApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CTPHQ
{
    public class CTPHQ
    {
        public delegate void OnReceiveHQEventHandler(CThostFtdcDepthMarketDataField hq);
        public event OnReceiveHQEventHandler OnReceiveHQ;
        /// <summary>
        /// 行情接口实例
        /// </summary>
        private MarketApi _api;

        /// <summary>
        /// 连接地址
        /// </summary>
        private string _frontAddr;

        /// <summary>
        /// 经纪商代码
        /// </summary>
        private string _brokerID;

        /// <summary>
        /// 投资者账号
        /// </summary>
        private string _investor;

        /// <summary>
        /// 密码
        /// </summary>
        private string _password;

        /// <summary>
        /// 是否登录
        /// </summary>
        private bool _isLogin;

        private string[] _codeList;


        public CTPHQ(string frontAddr, string brokerID, string user, string pwd,string[] codes)
        {
            this._frontAddr = frontAddr;
            this._brokerID = brokerID;
            this._investor = user;
            this._password = pwd;
            this._codeList = codes;
        }

        public void Initialize()
        {
            Console.WriteLine("CTP行情订阅初始化");
            _api = new MarketApi(_brokerID, _frontAddr);
            _api.OnRspError += new MarketApi.RspError((ref CThostFtdcRspInfoField pRspInfo, int nRequestID, byte bIsLast) =>
            {
                Console.WriteLine("ErrorID: {0}, ErrorMsg: {1}", pRspInfo.ErrorID, pRspInfo.ErrorMsg);
            });

            _api.OnFrontConnected += new MarketApi.FrontConnected(() =>
            {
                Console.WriteLine("连接成功");
                _api.UserLogin(-3, _investor, _password);
            });

            _api.OnRspUserLogin += new MarketApi.RspUserLogin((ref CThostFtdcRspUserLoginField pRspUserLogin,
                ref CThostFtdcRspInfoField pRspInfo, int nRequestID, byte bIsLast) =>
            {
                if (_isLogin == false)
                {
                    _isLogin = true;
                    _api.SubscribeMarketData(this._codeList);
                    Console.WriteLine("   ");
                    Console.WriteLine("***********************************************");
                    Console.WriteLine("*****  CTP行情接收中，请勿关闭窗口！！！  *****");
                    Console.WriteLine("*****  CTP行情接收中，请勿关闭窗口！！！  *****");
                    Console.WriteLine("*****  CTP行情接收中，请勿关闭窗口！！！  *****");
                    Console.WriteLine("***********************************************");
                    Console.WriteLine("   ");
                }
            });

            _api.OnRspUserLogout += new MarketApi.RspUserLogout((ref CThostFtdcUserLogoutField pRspUserLogout,
                ref CThostFtdcRspInfoField pRspInfo, int nRequestID, byte bIsLast) =>
            {
                _isLogin = false;
                _api.Disconnect();
            });

            _api.OnFrontDisconnected += new MarketApi.FrontDisconnected((int nReasion) =>
            {
                _isLogin = false;
            });

            _api.OnHeartBeatWarning += new MarketApi.HeartBeatWarning((int nTimeLapse) =>
            {
                Console.WriteLine(DateTime.Now.ToString());
                _api.Connect();
            });

            _api.OnRspSubMarketData += new MarketApi.RspSubMarketData((ref CThostFtdcSpecificInstrumentField pSpecificInstrument,
                ref CThostFtdcRspInfoField pRspInfo, int nRequestID, byte bIsLast) =>
            {
                Console.WriteLine("订阅{0}成功", pSpecificInstrument.InstrumentID);
                Thread.Sleep(10);
            });

            _api.OnRtnDepthMarketData += new MarketApi.RtnDepthMarketData((ref CThostFtdcDepthMarketDataField pDepthMarketData) =>
            {
                try
                {
                    OnReceiveHQ(pDepthMarketData);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                

            });

            _api.Connect();
            Console.WriteLine("初始化完成");

        }


        //获取交易日
        public void GetTradingDay()
        {
            string result = _api.GetTradingDay();
            Console.WriteLine(result);
        }

        //订阅行情
        public void SubscribeFutureMarketData(string[] CodeList)
        {

            _api.SubscribeMarketData(CodeList);
        }
    }
}
