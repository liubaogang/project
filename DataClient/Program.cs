using System;
using System.Collections.Generic;
using Cicada;
using Cicada.DI;
using System.Data;
using Cicada.Configuration;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using LingYi.DataTransfer.Models.Message;
using Newtonsoft.Json;
using Wen.Helpers.Redis;
using System.Threading;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;

namespace DataClient
{
    class Program
    {
        static RedisHelper RedisDB1 = new RedisHelper(2);
        static readonly object _locker = new object();
        public static IDbConnection GetOpenConnection()
        {
            var config = "Server=10.100.105.142;Port=3306;Database=Im; User=Dev3;Password=Dev3;Protocol=TCP;Compress=false;Pooling=true;Min Pool Size=0;Max Pool Size=100;Connection Lifetime=";
            var Connection = new MySqlConnection(config);
            Connection.Open();
            return Connection;
        }
        public static string GetNewID(Guid id)
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssfff") + id.ToString("N").Substring(12);
        }
        static void GetData()
        {
            string userid = RedisDB1.ListLeftPop("aaaaa");
            var FriendGroups = new List<tbl_FriendGroups>();
            using (var conn = GetOpenConnection())
            {
                FriendGroups = conn.Query<tbl_FriendGroups>(@"
                        SELECT  GroupID as 'GroupID',
                                GroupCategory as 'GroupCategory',
                                GroupType as 'GroupType',
                                GroupName as 'GroupName',
                                GroupImage as 'GroupImage',
                                GroupMark as 'GroupRemark',
                                GroupUserID as 'GroupUserID',
                                GroupDate as 'GroupDate'
                        FROM  tbl_CustomGroup 
                        WHERE GroupUserID=@UserID;",
                    new { UserID = userid }).ToList();
            }
            var FriendRelation = new List<tbl_FriendRelation>();
            using (var conn = GetOpenConnection())
            {
                FriendRelation = conn.Query<tbl_FriendRelation>(@"
                        SELECT  RelationId,
                        RelationFromUserId as 'FromUserID',
                        RelationToUserId as 'ToUserID',
                        RelationGroupId as 'FriendGroupID',
                        RelationSpaceGroupId as 'SpaceGroupID',
                        RelationMark  as 'RelationRemark',
                        RelationBGFileID as 'Background',
                        RelationCreateTime as 'RelationDate'
                        from tbl_FriendRelationShip 
                    where RelationFromUserId=@UserID;",
                    new { UserID = userid }).ToList();
            }
            foreach (var fg in FriendGroups)
            {
                var oldGroupID = fg.GroupID;
                fg.GroupID = GetNewID(Guid.Parse(fg.GroupID));
                foreach (var fr in FriendRelation)
                {

                }
            }
        }

        public static string GenerateOrderNumber()
        {
            string strDateTimeNumber = DateTime.Now.ToString("yyyyMMddHHmmssms");
            string strRandomResult = NextRandom(1000, 1).ToString();
            return strDateTimeNumber + strRandomResult;
        }
        /// <summary>
        /// 参考：msdn上的RNGCryptoServiceProvider例子
        /// </summary>
        /// <param name="numSeeds"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private static int NextRandom(int numSeeds, int length)
        {
            // Create a byte array to hold the random value.  
            byte[] randomNumber = new byte[length];
            // Create a new instance of the RNGCryptoServiceProvider.  
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            // Fill the array with a random value.  
            rng.GetBytes(randomNumber);
            // Convert the byte to an uint value to make the modulus operation easier.  
            uint randomResult = 0x0;
            for (int i = 0; i < length; i++)
            {
                randomResult |= ((uint)randomNumber[i] << ((length - 1 - i) * 8));
            }
            return (int)(randomResult % numSeeds) + 1;
        }

        class test
        {
            public object id { get; set; }
            public string name { get; set; }

            //public static explicit operator test(object v)
            //{
            //    throw new NotImplementedException();
            //}
        }


        class CustomException : Exception
        {
            private string _Message;
            public CustomException(string message)
                : base(message)
            {
                _Message = message;
            }
            public override string ToString()
            {
                return _Message;
            }
        }

        static void Main(string[] args)
        {



            try
            {
                throw new CustomException("自定义异常！");
            }
            catch (CustomException ce)
            {
                Console.WriteLine(ce.Message);
            }


            



            RedisDB.ListLeftPush<double>("ggg", 52.653);

            var abc1 = RedisDB.ListRange<double>("ggg", 0, -1);



            RedisDB.ListLeftPush<int>("hhh", 56);

            var abc2 = RedisDB.ListRange<int>("hhh", 0, -1);


            RedisDB.ListLeftPush<string>("jjj", JsonConvert.SerializeObject("123456"));

            var abc3 = RedisDB.ListRange<string>("jjj", 0, -1);


            RedisDB.HashGet<int>("jjj", "");
            //var gggg= JsonConvert.DeserializeObject<string>("sdfsdfsdfsd");

            //RedisDB.ListLeftPush<string>("rrrr", JsonConvert.SerializeObject("sdfsdfsdfsd"));

            //var abc = RedisDB.ListRange<string>("rrrr", 0, -1);


            Console.ReadKey();
            var parent = Task.Factory.StartNew(() =>
            {
                var task1 = Task.Factory.StartNew(() =>
                {
                    if (!RedisDB1.KeyExists("aaaaa"))
                    {
                        using (var conn = GetOpenConnection())
                        {
                            var Model = conn.Query<string>(@"
                                    SELECT RelationFromUserId
                                    FROM tbl_FriendRelationShip 
                                    GROUP BY RelationFromUserId
                                    HAVING count(1)>1;");
                            foreach (var item in Model.ToList())
                            {
                                RedisDB1.ListLeftPush("aaaaa", item);
                            }
                        }
                    }
                });
                var task2 = Task.Factory.StartNew(() =>
                {
                    task1.Wait();
                    if (task1.IsCompleted)
                    {
                        if (RedisDB1.ListLength("aaaaa") > 0)
                        {
                            GetData();
                        }
                    }
                });
                var task3 = Task.Factory.StartNew(() =>
                {
                    task1.Wait();
                    if (task1.IsCompleted)
                    {
                        if (RedisDB1.ListLength("aaaaa") > 0)
                        {
                            GetData();
                        }
                    }
                });
                var task4 = Task.Factory.StartNew(() =>
                {
                    task1.Wait();
                    if (task1.IsCompleted)
                    {
                        if (RedisDB1.ListLength("aaaaa") > 0)
                        {
                            GetData();
                        }
                    }
                });
            });
        }
    }
}
