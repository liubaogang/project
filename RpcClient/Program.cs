using Cicada;
using Cicada.DI;
using Im.Rpc.Mysql;
using Im.Rpc.Redis;
using Im.Rpc.Public;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using LingYi.DataTransfer.Models.Message;


namespace RpcClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Cicada.Boot.CicadaApplication.Run();

            #region 多线程RPC测试
            var mysql = CicadaFacade.Container.Resolve<MysqlService.Iface>();
            var redis = CicadaFacade.Container.Resolve<RedisService.Iface>();
            var _public = CicadaFacade.Container.Resolve<PublicService.Iface>();

            var t= _public.GetGroupsInfoByOA("00115eee-667d-4e15-b424-4e1269cc66f4");

            for (int i = 0; i < 10; i++)
            {
                // 并发启动多线程
                Thread thread = new Thread(new ThreadStart(() =>
                {
                    for (int j = 0; j < 1; j++)
                    {
                        
                        try
                        {
                            mysql.FileInsert(JsonConvert.SerializeObject(new tbl_FileManager
                            {
                                FileID = Guid.NewGuid().ToString(),
                                FileDirectory = "group1/M00/05/1D/rBJxwFgJ1MKAJrCKAAC3oA4foGI919.jpg",
                                FileDate = "201710311556"
                            }));
                            var FileManager = mysql.GetFileByID("07bed506-8f57-4b48-8495-342eaf9df393");
                            //redis.FriendGroupIns(new tbl_FriendGroup
                            //{
                            //    GroupID = Guid.NewGuid().ToString(),
                            //    GroupName = "liubaogang",
                            //    GroupImage = "group1/M00/05/1D/rBJxwFgJ1MKAJrCKAAC3oA4foGI919.jpg",
                            //    GroupRemark = "水电费水电费sdfsdfsd地方是的发生的",
                            //    GroupDate = "201710311831",
                            //    GroupUserID = Guid.NewGuid().ToString()
                            //});
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        Thread.Sleep(0);
                    }
                }));
                thread.Start();
            }
            #endregion


            //Stopwatch timer = new Stopwatch();
            //timer.Start();
            //Random r = new Random();

            //var redis2 = CicadaFacade.Container.Resolve<MysqlService.Iface>();
            //for (int i = 0; i < 500; i++)
            //{
            //    if (i % 20 == 0)
            //        Console.ReadLine();
            //    redis2.FileInsert(new tbl_FileManager
            //    {
            //        FileID = Guid.NewGuid().ToString(),
            //        FileDirectory = "http://www.cnblogs.com/Contoso/p/5117383.html",
            //        FileDate = "201710311556"
            //    });
            //}

            //timer.Stop();
            //Console.WriteLine("总用时：" + timer.ElapsedMilliseconds);
            Console.WriteLine("===完成===");
            Console.ReadKey();
        }
    }
}
