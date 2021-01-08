using SqlSugar;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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
            var Codes = db.Ado.SqlQuery<String>("select * from openquery(WINDNEW,'select s_info_code FutureCode  from CFUTURESDESCRIPTION t where   to_date(S_INFO_DELISTDATE,''yyyymmdd'')>=sysdate  and s_info_name not like ''%仿真%'' and  t.fs_info_sccode in (''RB'',''I'',''J'',''JM'',''ZC'',''FG'',''PF'',''CF'',''JD'',''AP'')  order by s_info_code')");
            rClient.StringSet("CommodityCodesList", Newtonsoft.Json.JsonConvert.SerializeObject(Codes)); // 将商品代码列表保存到redis

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
}
