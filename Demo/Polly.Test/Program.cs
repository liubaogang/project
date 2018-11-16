using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polly.Test
{
    class Program
    {
        

        static void Main(string[] args)
        {


            #region PS：这个示例使用了MemoryCache
            //            PS：这个示例使用了MemoryCache，需要使用Nuget安装Polly.Caching.MemoryCache程序包，以及添加System.Runtime.Caching的引用。

            //从运行结果可以看到，虽然三次执行都有结果，但系统只有第一次才需要执行函数，剩下两次都是直接从缓存中获取的结果。

            //系统也提供了多种不同的过期策略：

            //Policy.Cache(memoryCacheProvider, new AbsoluteTtl(DateTimeOffset.Now.Date.AddDays(1)));
            //            Policy.Cache(memoryCacheProvider, new SlidingTtl(TimeSpan.FromMinutes(5)));

            //            对于布式缓存，Polly也有默认的实现，只需要安装Polly.Caching.IdistributedCache程序包即可，它提供了SqlServer和Redis的支持 
            #endregion
            var memoryCache = new System.Runtime.Caching.MemoryCache("cache5566");
            var memoryCacheProvider = new Polly.Caching.MemoryCache.MemoryCacheProvider(memoryCache);

            var cachePolicy = Policy.Cache(memoryCacheProvider, TimeSpan.FromMilliseconds(1000*5));

            //Context.ExecutionKey就是cache的key
            var context11 = new Context("cache5566");
            for (int i = 0; i < 3000; i++)
            {
                var cache = cachePolicy.Execute(_ =>
                {
                    Console.WriteLine("===重新获取的值===");
                    return Guid.NewGuid().ToString();
                }, context11);
                Console.WriteLine(cache);

                System.Threading.Thread.Sleep(500);
            }

            Console.WriteLine("======================");
            Console.ReadKey();


            Policy policy = Policy
            .Handle<Exception>()
            .CircuitBreaker(6, TimeSpan.FromSeconds(15));//连续出错6次之后熔断15秒(不会再去尝试执行业务代码）。
            while (true)
            {
                Console.WriteLine("开始Execute");
                try
                {
                    policy.Execute(() => {
                        Console.WriteLine("开始任务");
                        throw new Exception("出错");
                        Console.WriteLine("完成任务");
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine("execute出错******" );
                }
                System.Threading.Thread.Sleep(500);
            }


            Policy
              .Timeout(3, onTimeout: (context, timespan, task) =>
              {
                  Console.WriteLine("超时了，奶奶的！");
              }).Execute(()=> 
              {
                  System.Threading.Thread.Sleep(5000);
              });

            

            Policy
              .Handle<Exception>()
              .WaitAndRetry(new[]
              {
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(2),
                TimeSpan.FromSeconds(3)
              }, (exception, timeSpan, context) =>
              {
                  Console.WriteLine("dsfdsfds9999999999");
              }).Execute(()=> {
                  Compute();
                  //Console.WriteLine("8888888888888");
              });

            

            var fallBackPolicy =
                Policy<List<string>>
                    .Handle<Exception>()
                    .Fallback<List<string>>(new List<string>() { "@@@@@","#####" });

            var fallBack = fallBackPolicy.Execute(() =>
            {
                return ThrowException();
            });

            fallBack.ForEach(Item =>
            {
                Console.WriteLine(Item);
            });

            



            #region MyRegion
            //try
            //{
            //    var politicaWaitAndRetry = Policy
            //        .Handle<DivideByZeroException>()
            //        .WaitAndRetry(new[]
            //        {
            //            TimeSpan.FromSeconds(1),
            //            TimeSpan.FromMinutes(1),
            //            TimeSpan.FromSeconds(5),
            //            TimeSpan.FromSeconds(7)
            //        }, ReportaError);
            //    politicaWaitAndRetry.Execute(() =>
            //    {
            //        ZeroExcepcion();
            //    });
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine($"Executed Failed,Message:({e.Message})");
            //} 
            #endregion


            #region 重试次数
            //try
            //{
            //    var retryTwoTimesPolicy =
            //         Policy
            //             .Handle<DivideByZeroException>()
            //             .Or<ArgumentException>()
            //             .Retry(3, (ex, count) =>
            //             {
            //                 Console.WriteLine("执行失败! 重试次数 {0}", count);
            //                 Console.WriteLine("异常来自 {0}", ex.GetType().Name);
            //             });
            //    retryTwoTimesPolicy.Execute(() =>
            //    {
            //        Compute();
            //    });
            //}
            //catch (DivideByZeroException e)
            //{
            //    Console.WriteLine($"0-Excuted Failed,Message: ({e.Message})");
            //}
            //catch(ArgumentException e)
            //{
            //    Console.WriteLine($"1-Excuted Failed,Message: ({e.Message})");
            //} 
            #endregion

            Console.ReadKey();
        }



        static List<string> ThrowException()
        {
            throw new Exception();
        }

        static int Compute()
        {
            //var a = 0;
            //return 1 / a;
            int.Parse("sdfds");
            return 1;
        }
        /// <summary>
        /// 抛出异常
        /// </summary>
        static void ZeroExcepcion()
        {
            throw new DivideByZeroException();
        }
        /// <summary>
        /// 异常信息
        /// </summary>
        /// <param name="e"></param>
        /// <param name="tiempo"></param>
        /// <param name="intento"></param>
        /// <param name="contexto"></param>
        static void ReportaError(Exception e, TimeSpan tiempo, int intento, Context contexto)
        {
            Console.WriteLine($"异常: {intento:00} (调用秒数: {tiempo.Seconds} 秒)\t执行时间: {DateTime.Now}");
        }
    }
}
