using Macrosage.RabbitMQ.Server.Config;
using Macrosage.RabbitMQ.Server.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMQ.NETClient.Customer
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                 
                IMessageCustomer customer = new MessageCustomer("test");
                customer.StartListening();
                customer.ReceiveMessageCallback = message =>
                {
                    Console.WriteLine("接收到消息：" + message);
                    return false;
                };
                 
                Thread.Sleep(2000);
            }
            Console.Read();
        }
    }
}
