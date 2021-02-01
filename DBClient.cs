using Newtonsoft.Json;
using SqlSugar;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CTPHQ
{
    public class DBClient
    {
        SqlSugarClient db;
        public IDatabase rClient;
        public DBClient()
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(ConfigurationManager.AppSettings["REDIS"]);
            rClient = redis.GetDatabase();
            db = GetInstance("SqlConnString");
        }

        public void UpdateDBHQ(FutureHQ hq)
        {
            db.Updateable(hq).ExecuteCommand();
        }


        public string[] GetCodeList()
        {
            string comdityKey = rClient.StringGet("commodityKey");
            List<FutureInfo> CodeInfo = db.Ado.SqlQuery<FutureInfo>("select * from openquery(WINDNEW,'select  *  from CFUTURESDESCRIPTION t where   to_date(S_INFO_DELISTDATE,''yyyymmdd'')>=sysdate  and s_info_name not like ''%仿真%'' and  t.fs_info_sccode in " + comdityKey + "  order by s_info_code')");
            //List<FutureInfo> CodeInfo = db.Ado.SqlQuery<FutureInfo>("select * from openquery(WINDNEW,'select  *  from CFUTURESDESCRIPTION t where   to_date(S_INFO_DELISTDATE,''yyyymmdd'')>=sysdate  and s_info_name not like ''%仿真%'' order by s_info_code')");
            foreach (FutureInfo future in CodeInfo)
            {
                if (future.S_INFO_EXCHMARKET == "CFFEX")
                {
                    future.HS_MARKET_CODE = "7";
                }
                else if (future.S_INFO_EXCHMARKET == "CZCE")
                {
                    future.HS_MARKET_CODE = "4";
                }
                else if (future.S_INFO_EXCHMARKET == "DCE")
                {
                    future.HS_MARKET_CODE = "9";
                    future.S_INFO_CODE = future.S_INFO_CODE.ToLower();
                }
                else if (future.S_INFO_EXCHMARKET == "INE")
                {
                    future.HS_MARKET_CODE = "k";
                }
                else if (future.S_INFO_EXCHMARKET == "SHFE")
                {
                    future.HS_MARKET_CODE = "3";
                    future.S_INFO_CODE = future.S_INFO_CODE.ToLower();
                }


                switch (future.FS_INFO_SCCODE)
                {
                    case "RB":
                        future.CHANGE_TICK = 1;
                        break;
                    case "I":
                        future.CHANGE_TICK = 0.5;
                        break;
                    case "J":
                        future.CHANGE_TICK = 0.5;
                        break;
                    case "JM":
                        future.CHANGE_TICK = 0.5;
                        break;
                    case "ZC":
                        future.CHANGE_TICK = 0.2;
                        break;
                    case "FG":
                        future.CHANGE_TICK = 1;
                        break;
                    case "PF":
                        future.CHANGE_TICK = 2;
                        break;
                    case "CF":
                        future.CHANGE_TICK = 5;
                        break;
                    case "JD":
                        future.CHANGE_TICK = 1;
                        break;
                    case "T":
                        future.CHANGE_TICK = 0.005;
                        break;
                    case "AP":
                        future.CHANGE_TICK = 1;
                        break;
                    default:
                        future.CHANGE_TICK = 1;
                        break;
                }
                rClient.HashSet("FutureBasicInfo", future.S_INFO_CODE,JsonConvert.SerializeObject(future));
            }

            var Codes = CodeInfo.Select(o => o.S_INFO_CODE).ToList();
            rClient.StringSet("CommodityCodesList", JsonConvert.SerializeObject(Codes)); // 将商品代码列表保存到redis

            return Codes.ToArray();
        }
        public static SqlSugarClient GetInstance(string connstr)
        {
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = ConfigurationManager.AppSettings[connstr],
                DbType = DbType.SqlServer,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            });
            return db;
        }

    }

    public class FutureInfo
    {
        public string S_INFO_NAME { get; set; }
        public string S_INFO_CODE { get; set; }
        public string S_INFO_EXCHMARKET { get; set; }
        public string FS_INFO_SCCODE { get; set; }
        public string S_INFO_LISTDATE { get; set; }
        public string S_INFO_DELISTDATE { get; set; }
        public string FS_INFO_DLMONTH { get; set; }
        public string FS_INFO_LTDLDATE { get; set; }
        public string S_INFO_FULLNAME { get; set; }
        public string HS_MARKET_CODE { get; set; }
        public double CHANGE_TICK { get; set; }
    }


}
