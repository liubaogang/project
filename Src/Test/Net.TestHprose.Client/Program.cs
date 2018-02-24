﻿using Hprose.Client;
using Hprose.Common;
using Hprose.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Net.TestHprose.Client
{
    public class User
    {
        public string name;
        public int age;
        public bool male;
        public List<User> friends;
    }

    public interface IStub
    {
        [SimpleMode(true)]
        Task<string> Hello(string name);
    }
    class Program
    {
        static HproseHttpClient client;
        static async void Hello()
        {
            var stub = client.UseService<IStub>();
            Console.WriteLine(await stub.Hello("World"));
        }

        static void Main(string[] args)
        {
            HproseClassManager.Register(typeof(User), "User");
            client = new HproseHttpClient(" http://localhost:2018/");
            client.KeepAlive = true;
            var Result= client.Invoke<String>("hello", new Object[] { "abc123" });
            Console.WriteLine(Result);
            /*
                        List<User> users = new List<User>();
                        User user1 = new User();
                        user1.name = "李雷";
                        user1.age = 32;
                        user1.male = true;
                        user1.friends = new List<User>();
                        User user2 = new User();
                        user2.name = "韩梅梅";
                        user2.age = 31;
                        user2.male = false;
                        user2.friends = new List<User>();
                        user1.friends.Add(user2);
                        user2.friends.Add(user1);
                        users.Add(user1);
                        users.Add(user2);
                        Func<List<User>, List<User>> SendUsers = userList => client.Invoke<List<User>>("sendUsers", new object[] { userList });
                        Hello();
                        MemoryStream stream = (MemoryStream)HproseFormatter.Serialize(SendUsers(users));
                        Console.WriteLine(Encoding.UTF8.GetString(stream.ToArray()));
            */
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var ts = new List<Thread>();
            for (int i = 0; i < 3000; i++)
            {
                ts.Add(new Thread(() =>
                {
                    for (int j = 1; j < 2; j++)
                    {
                        try
                        {
                            Result = client.Invoke<String>("hello", new Object[] { "abc123" + j });
                            Console.WriteLine("***====" + Result);
                        }
                        catch(Exception ex)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(ex.Message);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }
                }));
            }
            ts.ForEach(a => a.Start());
            ts.ForEach(a => a.Join());
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
            Console.WriteLine(stopwatch.ElapsedMilliseconds / 2000.0);
            Console.ReadLine();
        }
    }
}
