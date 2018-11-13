﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Rpc.Client
{
    class Program
    {
        static int i = 1;
        static void Main(string[] args)
        {
            var rpcClient = new RpcClient();

            while (true)
            {
                string str = Console.ReadLine();
                if (str == "esc")
                    break;
                Console.WriteLine(" [x] Requesting fib(" + i + ")");
                var response = rpcClient.Call(i.ToString());

                Console.WriteLine(" [.] Got '{0}'", response);
                i++;
            }
            rpcClient.Close();
            
        }
    }

    public class RpcClient
    {
        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly string replyQueueName;
        private readonly EventingBasicConsumer consumer;
        private readonly BlockingCollection<string> respQueue = new BlockingCollection<string>();
        private readonly IBasicProperties props;

        public RpcClient()
        {
            var factory = new ConnectionFactory() { HostName = "192.168.1.167" };

            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            replyQueueName = channel.QueueDeclare().QueueName;
            consumer = new EventingBasicConsumer(channel);

            props = channel.CreateBasicProperties();
            var correlationId = Guid.NewGuid().ToString();
            props.CorrelationId = correlationId;
            props.ReplyTo = replyQueueName;

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var response = Encoding.UTF8.GetString(body);
                if (ea.BasicProperties.CorrelationId == correlationId)
                {
                    respQueue.Add(response);
                }
            };
        }

        public string Call(string message)
        {
            var messageBytes = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(
                exchange: "",
                routingKey: "rpc_queue",
                basicProperties: props,
                body: messageBytes);

            channel.BasicConsume(
                consumer: consumer,
                queue: replyQueueName,
                autoAck: true);
            //return respQueue.Take();

            string a = string.Empty;
            respQueue.TryTake(out a, 1000 * 10);
            return a;
        }

        public void Close()
        {
            connection.Close();
        }
    }

}
