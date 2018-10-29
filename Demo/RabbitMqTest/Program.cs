using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMqTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "192.168.1.166" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                #region Direct类型的exchange
                /*
                // Direct类型的exchange, 名称 pdf_events
                channel.ExchangeDeclare(exchange: "pdf_events",
                                        type: ExchangeType.Direct,
                                        durable: true,
                                        autoDelete: false,
                                        arguments: null);

                // 创建create_pdf_queue队列
                channel.QueueDeclare(queue: "create_pdf_queue",
                                        durable: true,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);

                //创建 pdf_log_queue队列
                channel.QueueDeclare(queue: "pdf_log_queue",
                                        durable: true,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);

                //绑定 pdf_events --> create_pdf_queue 使用routingkey:pdf_create
                channel.QueueBind(queue: "create_pdf_queue",
                                    exchange: "pdf_events",
                                    routingKey: "pdf_create",
                                    arguments: null);

                //绑定 pdf_events --> pdf_log_queue 使用routingkey:pdf_log
                channel.QueueBind(queue: "pdf_log_queue",
                                    exchange: "pdf_events",
                                    routingKey: "pdf_log",
                                    arguments: null);


                var message = "Demo some pdf creating...";
                var body = Encoding.UTF8.GetBytes(message);
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                //发送消息到exchange :pdf_events ,使用routingkey: pdf_create
                //通过binding routinekey的比较，次消息会路由到队列 create_pdf_queue
                channel.BasicPublish(exchange: "pdf_events",
                            routingKey: "pdf_create",
                            basicProperties: properties,
                            body: body);

                message = "pdf loging ...";
                body = Encoding.UTF8.GetBytes(message);
                properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                //发送消息到exchange :pdf_events ,使用routingkey: pdf_log
                //通过binding routinekey的比较，次消息会路由到队列 pdf_log_queue
                channel.BasicPublish(exchange: "pdf_events",
                        routingKey: "pdf_log",
                        basicProperties: properties,
                        body: body); 
                */
                #endregion

                #region Topic类型的exchange
                /*
                // Topic类型的exchange, 名称 agreements
                channel.ExchangeDeclare(exchange: "agreements",
                                        type: ExchangeType.Topic,
                                        durable: true,
                                        autoDelete: false,
                                        arguments: null);

                // 创建berlin_agreements队列
                channel.QueueDeclare(queue: "berlin_agreements",
                                        durable: true,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);

                //创建 all_agreements 队列
                channel.QueueDeclare(queue: "all_agreements",
                                        durable: true,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);

                //创建 headstore_agreements 队列
                channel.QueueDeclare(queue: "headstore_agreements",
                                        durable: true,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);

                //绑定 agreements --> berlin_agreements 使用routingkey:agreements.eu.berlin.#
                channel.QueueBind(queue: "berlin_agreements",
                                    exchange: "agreements",
                                    routingKey: "agreements.eu.berlin.#",
                                    arguments: null);

                //绑定 agreements --> all_agreements 使用routingkey:agreements.#
                channel.QueueBind(queue: "all_agreements",
                                    exchange: "agreements",
                                    routingKey: "agreements.#",
                                    arguments: null);

                //绑定 agreements --> headstore_agreements 使用routingkey:agreements.eu.*.headstore
                channel.QueueBind(queue: "headstore_agreements",
                                    exchange: "agreements",
                                    routingKey: "agreements.eu.*.headstore",
                                    arguments: null);


                var message = "hello world TTTTT";
                var body = Encoding.UTF8.GetBytes(message);
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                //发送消息到exchange :agreements ,使用routingkey: agreements.eu.berlin
                //agreements.eu.berlin 匹配  agreements.eu.berlin.# 和agreements.#
                //agreements.eu.berlin 不匹配  agreements.eu.*.headstore
                //最终次消息会路由到队里：berlin_agreements（agreements.eu.berlin.#） 和 all_agreements（agreements.#）
                channel.BasicPublish(exchange: "agreements",
                                        routingKey: "agreements.eu.ttttt.headstore",
                                        basicProperties: properties,
                                        body: body); 
                */
                #endregion

                // Headers类型的exchange, 名称 agreements

                Dictionary<string, object> aHeader = new Dictionary<string, object>();
                aHeader.Add("x-max-length", 5);

                channel.ExchangeDeclare(exchange: "liubaogang",
                                        type: ExchangeType.Headers,
                                        durable: true,
                                        autoDelete: false,
                                        arguments: null);

                // 创建queue.A队列
                channel.QueueDeclare(queue: "queue.liubaogang",
                                    durable: true,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: aHeader);


                //绑定 agreements --> queue.A 使用arguments (format=pdf, type=report, x-match=all)

                channel.QueueBind(queue: "queue.liubaogang",
                                    exchange: "liubaogang",
                                    routingKey: string.Empty,
                                    arguments: null);



                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                for (int i = 1; i <= 7; i++)
                {
                    string message1 = "hello world-" + i;
                    var body = Encoding.UTF8.GetBytes(message1);
                    channel.BasicPublish(exchange: "liubaogang",
                                        routingKey: string.Empty,
                                        basicProperties: properties,
                                        body: body);
                }
            }
        }
    }
}
