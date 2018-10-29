using Macrosage.RabbitMQ.Server.Customer;
using Macrosage.RabbitMQ.Server.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RabbitMQ.NETClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var testQueueName = "test";
            IMessageProduct product = new MessageProduct(testQueueName);
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine("正在发送第" + i + "条消息...");
                System.Threading.Thread.Sleep(800);
                product.Publish("消息体" + i);
            }

            Console.Read();
        }
    }
}
