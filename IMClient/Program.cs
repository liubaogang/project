using LingYi.DataTransfer.Models.Message;
using Newtonsoft.Json;
using Sharp.Xmpp;
using Sharp.Xmpp.Client;
using Sharp.Xmpp.Im;
using Sharp.Xmpp.Extensions;
 
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace IMClient
{

    class test
    {
        public string ID { get; set; }
        public string FromID { get; set; }
        public string ToID { get; set; }
        public string Body { get; set; }
        public string Date { get; set; }

    }

    class test1
    {
        public string filed1 { get; set; }
        public string filed2 { get; set; }
        public string filed3 { get; set; }
    }

    class Program
    {
        #region  多任务案例
        //static List<string> list = new List<string>();
        //static readonly object _locker = new object();

        //static string getlistitem()
        //{
        //    if (list.Count > 0)
        //    {
        //        lock (_locker)
        //        {
        //            if (list.Count > 0)
        //            {
        //                var a = list[0]+ DateTime.Now;
        //                list.RemoveAt(0);
        //                return a;
        //            }
        //        }
        //    }
        //    return string.Empty;
        //}
        //    Console.WriteLine(DateTime.Now);
        //    var parent = Task.Factory.StartNew(() =>
        //    {
        //        var task1 = Task.Factory.StartNew(() =>
        //        {
        //            for (int u = 0; u < 500; u++)
        //            {
        //                //System.Threading.Thread.Sleep(500);
        //                Program.list.Add("task1===" + u);                        
        //            }
        //            Console.WriteLine("task1完成");
        //        });
        //        var task2 = Task.Factory.StartNew(() =>
        //        {
        //            task1.Wait();
        //            if (task1.IsCompleted)
        //            {
        //                while(list.Count>0)
        //                {
        //                    Console.WriteLine("task2:******" + Program.getlistitem());
        //                }
        //                Console.WriteLine("task2 完成");
        //            }
        //        });
        //        var task3 = Task.Factory.StartNew(() =>
        //        {
        //            task1.Wait();
        //            if (task1.IsCompleted)
        //            {
        //                while (list.Count > 0)
        //                {
        //                    Console.WriteLine("task3:******" + Program.getlistitem());
        //                }
        //                Console.WriteLine("task3 完成");
        //            }
        //        });
        //        var task4 = Task.Factory.StartNew(() =>
        //        {
        //            task1.Wait();
        //            if (task1.IsCompleted)
        //            {
        //                while (list.Count > 0)
        //                {
        //                    Console.WriteLine("task4:******" + Program.getlistitem());
        //                }
        //                Console.WriteLine("task4 完成");
        //            }

        //        });
        //    });
        //    parent.Wait();
        //    Console.ReadKey();

        #endregion

        static XmppClient cl;
        static void Main(string[] args)
        {
            string hostname = "csxmpp.lingyi365.com",
            username = "57fefb8559242e1e8ca5b13f",
            password = "9Nuj4x2qExPgV3T7GTH2JnKg-zzjjghh6l1i9PGpLLflhvp6YIMBkw..";
            using (cl = new XmppClient(hostname, username, password))
            {
                cl.Port = 5333;
                cl.Im.CustomIqDelegate += (jid, str) =>
                {
                    Console.WriteLine("**********:" + str);
                    return "urn:sharp.xmpp:customiq";
                };
                cl.Connect("cs");
                cl.Error += Cl_Error;
                cl.Message += Cl_Message;
                var ConnInfo = new { Version = "1.0.0", Description = "I am online" };
                cl.SetStatus(new Status(Availability.Online, JsonConvert.SerializeObject(ConnInfo), 1));


                //Sharp.Xmpp.Core.XmppCore im1 = new Sharp.Xmpp.Core.XmppCore(hostname);
                //im1.Username = username;
                //im1.Password = password;
                //im1.Port = 5333;
                //im1.Connect("cs1");
                //Sharp.Xmpp.Core.Iq a = new Sharp.Xmpp.Core.Iq(Sharp.Xmpp.Core.IqType.Get, "5566");
                //im1.IqRequest(a);

                cl.RequestCustomIq(cl.Jid, "<iq type=\"get\"  to=\"111222@csxmpp.lingyi365.com\"><query xmlns=\"customiq\"/></iq>");

                var task1 = Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        try
                        {
                            TimeSpan time = cl.Ping(cl.Jid);
                            Console.WriteLine(time);                            
                        }
                        catch
                        {
                            try
                            {
                                cl = new XmppClient(hostname, username, password);
                                cl.Port = 5333;
                                cl.Im.CustomIqDelegate += (jid, str) => { return "44444444"; };
                                cl.Connect("cs");
                                cl.Error += Cl_Error;
                                cl.Message += Cl_Message;                              
                            }
                            catch(XmppDisconnectionException)
                            {
                                Console.WriteLine("XmppDisconnectionException:连接断开异常.....！");
                            }
                            catch(IOException)
                            {
                                Console.WriteLine("IOException:连接失败！");
                            }
                        }
                        finally
                        {
                            System.Threading.Thread.Sleep(5000);
                        }
                    }
                });

                while (true)
                {
                    //System.Threading.Thread.Sleep(200);
                    Console.WriteLine("Type a message or type 'quit' to exit: ");
                    string s = Console.ReadLine();
                    if (s == "quit")
                        return;
                    //1:group-chat

                    //Jid juliet = "822c75ec-9d9f-4ff8-8287-c82987c949b7@" + hostname;
                    //Message JabberMsg = new Message(juliet, s, null, null, Sharp.Xmpp.Im.MessageType.Chat);
                    //JabberMsg.Data.SetAttribute("id", Guid.NewGuid().ToString());
                    //var jabberXml = JabberMsg.Data.OwnerDocument;
                    //var xmlNode = jabberXml.CreateElement("bodyExtend");

                    //var Node = jabberXml.CreateElement("command");
                    //Node.InnerText = "cmd:groupchat";
                    //xmlNode.AppendChild(Node);

                    //Node = jabberXml.CreateElement("version");
                    //Node.InnerText = "1.0.0";
                    //xmlNode.AppendChild(Node);

                    //JabberMsg.Data.AppendChild(xmlNode);

                    //2: chat  
                    //   578c709f59242e19b8573b77   57ff1ac4592435206863d855   5785d39059242e0d04a1be3f

                    Jid juliet = "57ff1ac4592435206863d855@" + hostname;
                    Message JabberMsg = new Message(juliet, s, null, null, Sharp.Xmpp.Im.MessageType.Chat);
                    JabberMsg.Data.SetAttribute("id", Guid.NewGuid().ToString());

                    var jabberXml = JabberMsg.Data.OwnerDocument;
                    var xmlNode = jabberXml.CreateElement("bodyExtend");

                    var Node = jabberXml.CreateElement("command");
                    Node.InnerText = "cmd:chat";
                    xmlNode.AppendChild(Node);

                    Node = jabberXml.CreateElement("version");
                    Node.InnerText = "1.0.0";
                    xmlNode.AppendChild(Node);

                    JabberMsg.Data.AppendChild(xmlNode);

                    cl.SendMessage(JabberMsg);
                }
            }
            #region 
            //string hostname = "csxmpp.lingyi365.com",
            //username = "57fefb8559242e1e8ca5b13f",
            //password = "9Nuj4x2qExPgV3T7GTH2JnKg-zzjjghhBGDtqHJz0AjqAV1w3ERgPg..";

            //int i = 1;
            //using (var cl = new XmppClient(hostname, username, password))
            //{
            //    cl.Port = 5333;
            //    cl.Connect();
            //    cl.Error += Cl_Error;
            //    while (true)
            //    {
            //        cl.RequestCustomIq("57fefb8559242e1e8ca5b13f@csxmpp.lingyi365.com",@"<iq from='57fefb8559242e1e8ca5b13f@longbourn.lit/garden' type='get'  id='roster1'> <query xmlns='jabber:iq:roster'/> </iq>");

            //        Console.Write("Type a message or type 'quit' to exit: ");
            //        string s = Console.ReadLine();
            //        if (s == "quit")
            //            return;
            //        Jid juliet = "57ff1ac4592435206863d855@csxmpp.lingyi365.com/Spark0";
            //        var Msg = new tbl_Message
            //        {
            //            MsgID = DateTime.Now.ToString("yyyyMMddhhmmss"),
            //            MsgFrom = Guid.NewGuid().ToString().Replace("-", ""),
            //            MsgTo = Guid.NewGuid().ToString().Replace("-", ""),
            //            MsgType = "chat",
            //            MsgDate = DateTime.Now
            //        };
            //        Msg.MsgBody.Add("CHAT-TXT", "谁谁被谁提出了群聊"); 
            //        s = JsonConvert.SerializeObject(Msg);



            //        cl.SendMessage(juliet, s,null,null , Sharp.Xmpp.Im.MessageType.Chat);
            //        i++;
            //    }
            //}
            #endregion
        }

        private static void Cl_Message(object sender, MessageEventArgs e)
        {
            Console.WriteLine("==========我收到消息了 begin=========");
            Regex reg = new Regex("<command>(.*?)</command>");
            Match match = reg.Match(e.Message.Data.OuterXml);
            if (match.Success)
            {
                if (match.Groups[1].Value == "cmd:chat" || match.Groups[1].Value == "cmd:groupchat")
                    Console.ForegroundColor = ConsoleColor.Yellow;
                if (match.Groups[1].Value == "cmd:chat-arrive-receipt")
                    Console.ForegroundColor = ConsoleColor.Blue;
                if (match.Groups[1].Value == "cmd:chat-viewed-receipt")
                    Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("消息内容:" + e.Message.Body);
                Console.WriteLine("消息类型:" + match.Groups[1].Value);
                Console.WriteLine("消息XML:" + e.Message.Data.OuterXml);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("未知类型:.......");
                Console.WriteLine("消息 XML:" + e.Message.Data.OuterXml);
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("==========我收到消息了  end=========");
            Console.WriteLine();
            Console.WriteLine();
        }
        private static void Cl_Error(object sender, Sharp.Xmpp.Im.ErrorEventArgs e)
        {
            Console.WriteLine("***************错误了:" + e.Reason);
        }
    }
}
