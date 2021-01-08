using CTPMarketApi;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPHQ
{
    public class MQClient
    {
        private IModel channel;
        private string ExchangeName;
        public MQClient()
        {
            var factory = new ConnectionFactory();
            factory.UserName = ConfigurationManager.AppSettings["UserName"];
            factory.Password = ConfigurationManager.AppSettings["Password"];
            factory.HostName = ConfigurationManager.AppSettings["HostName"];
            ExchangeName = ConfigurationManager.AppSettings["FutureHQExchangeName"];

            //factory.RequestedHeartbeat = new TimeSpan(0);

            var connection = factory.CreateConnection();

            channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange: ExchangeName, type: "fanout", durable: true);
        }

        public void PublisHQ(FutureHQ hq)
        {
            string message = Newtonsoft.Json.JsonConvert.SerializeObject(hq);
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: ExchangeName,
                             routingKey: "",
                             basicProperties: null,
                             body: body);
        }
    }
}
