using System;
using Cicada;
using Cicada.Mq;
using Cicada.DI;
using Newtonsoft.Json;
using LingYi.DataTransfer.Models.Message;
using System.Collections.Generic;
using System.Text;
using System.IO;
using LingYi.NetToolClass.ZIP;

namespace MQClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Cicada.Boot.CicadaApplication.Run();
            #region 文件压缩测试
            //List<tbl_FriendRelation> list = new List<tbl_FriendRelation>();
            //for (int i = 0; i < 11000; i++)
            //{
            //    list.Add(new tbl_FriendRelation
            //    {
            //        RelationID = Guid.NewGuid().ToPureString(),
            //        FriendGroupID = Guid.NewGuid().ToPureString(),
            //        FromUserID = Guid.NewGuid().ToPureString(),
            //        ToUserID = Guid.NewGuid().ToPureString(),
            //        FriendAuthority = new Dictionary<string, string>(),
            //        RelationRemark = "sdfsdfsdfsdfsd是的发送到发送到",
            //        SpaceGroupID = Guid.NewGuid().ToPureString(),
            //        RelationDate = DateTime.Now
            //    });
            //}
            //using (FileStream fs = new FileStream("E:\\Spark\\pppppppp.txt", FileMode.OpenOrCreate))
            //{
            //    //获得字节数组
            //    byte[] data = Encoding.Default.GetBytes(JsonConvert.SerializeObject(list));
            //    //开始写入
            //    fs.Write(data, 0, data.Length);
            //    //清空缓冲区、关闭流
            //    fs.Flush();
            //}
            //new ZipHelper().ZipManyFilesOrDictorys
            //(
            //    new List<string>() { "E:\\Spark\\pppppppp.txt" },
            //    "E:\\Spark\\pppppppp.zip",
            //    "123"
            //);

            #endregion
            var _rabbitDB = CicadaFacade.Container.Resolve<ISender>();
            Console.WriteLine("请输入任意字符 回车！");
            while (Console.ReadLine() != "exit")
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("filed1", "123");
                dic.Add("filed2", "456");
                dic.Add("filed3", "789");
                var Msg = new tbl_Message
                {
                    MsgID = Guid.NewGuid().ToPureString(),
                    MsgFrom = "57fefb8559242e1e8ca5b13f",
                    MsgTo = "57ff1ac4592435206863d855",
                    MsgType = "chat",
                    MsgStanza = "",
                    MsgDate = DateTime.Now
                };
                var MqTest = new
                {
                    // Type ：chat（默认） | error | headline | normal
                    Type = "chat",
                    // FromID：可赋值也可不赋值（不赋值 收到时为system）
                    FromID = "",
                    ToID = "57e36fd12368600324f95126",
                    Body = Msg,
                    BodyExtend = dic
                };
                 
                _rabbitDB.Send("lingyi.receivemessage",MqTest);


                var MqTest1 = new
                {
                    // Type ：chat（默认） | error | headline | normal
                    Type = "chat",
                    // FromID：可赋值也可不赋值（不赋值 收到时为system）
                    FromID = "",
                    ToID = "57ea36362368600fa8bffe58",
                    Body = ">",
                    BodyExtend = dic
                };

                _rabbitDB.Send("lingyi.receivemessage", MqTest1);

                Console.WriteLine("发送完成，如果需要请再次输入！");
                //_rabbitDB.Send("lingyi.setbussmessage", Msg);
                //_rabbitDB.Send("lingyi.getchatmessage", Msg);                 

            }
        }
    }
}
