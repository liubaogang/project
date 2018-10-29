using Macrosage.RabbitMQ.Server.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Macrosage.RabbitMQ.Server.Product
{
    public class MessageProduct : IMessageProduct
    {
        private string _queueName { get; set; }
        public MessageProduct(string queueName)
        {
            _queueName = queueName;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message">消息体</param>
        /// <param name="queueName">消息队列</param>
        public void Publish(string message, string queueName=null)
        {
            if (queueName == null) {
                queueName = _queueName;
            }

            var factory = RabbitMQFactory.Instance.CreateFactory();
            using (var connection = factory.CreateConnection())
            {
                using (var model = connection.CreateModel())
                {

                    try
                    {
                        //消息持久化，防止丢失
                        model.QueueDeclare(queueName, RabbitMQConfig.IsDurable, false, false, null);
                        var properties = model.CreateBasicProperties();
                        properties.Persistent = RabbitMQConfig.IsDurable;
                        properties.DeliveryMode = 2;
                        properties.MessageId = Guid.NewGuid().ToString();
                        //properties.Expiration = "6000";
                        //消息转换为二进制
                        var msgBody = Encoding.UTF8.GetBytes(message);
                        model.TxSelect();
                        
                        //消息发出到队列
                        model.BasicPublish("", queueName, properties, msgBody);
                        //if (message == "消息体7")
                        //{
                        //    Console.WriteLine("出错了");
                        //    throw new Exception("出错了");                            
                        //}
                        model.TxCommit();
                       
                    }
                    catch
                    {
                        model.TxRollback();
                    }
                }
            }
        }
        /// <summary>
        /// 异步多条发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="queueName"></param>
        public void Publish(List<string> message, string queueName)
        {
            if (queueName == null)
            {
                queueName = _queueName;
            }

            var factory = RabbitMQFactory.Instance.CreateFactory();
            using (var connection = factory.CreateConnection())
            {
                using (var model = connection.CreateModel())
                {
                    //消息持久化，防止丢失
                    model.QueueDeclare(queueName, RabbitMQConfig.IsDurable, false, false, null);
                    // model.ExchangeDeclare
                    //
                    var properties = model.CreateBasicProperties();
                    properties.Persistent = RabbitMQConfig.IsDurable;
                    properties.DeliveryMode = 2;

                    //异步多条发送消息
                    Parallel.For(0, message.Count, i => {
                        //消息转换为二进制
                        var msgBody = Encoding.UTF8.GetBytes(message[i]);
                        //消息发出到队列
                        model.BasicPublish("", queueName, properties, msgBody);
                    });
                   
                }
            }
        }
    }
}
