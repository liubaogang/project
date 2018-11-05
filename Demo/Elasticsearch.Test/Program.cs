using Elasticsearch.Net;
using Nest;
using System;
using System.Collections.Generic;

namespace Elasticsearch.Test
{
    class Tweet
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public DateTime PostDate { get; set; }
        public string Message { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            //http://192.168.1.167:9200/_search
            //http://192.168.1.167:9200/twitter/tweet/10

            var node = new Uri("http://192.168.1.167:9200");
            var settings = new ConnectionSettings(node);
            settings.DefaultIndex("tweets");
            var client = new ElasticClient(settings);

            var list = new List<Tweet>();
            for (int i = 1; i < 20; i++)
            {
                list.Add(new Tweet
                {
                    Id = i,
                    UserName = "liubaogang-" + i,
                    PostDate = DateTime.Now,
                    Message = "Trying out NEST, so far so good?"
                });
            }
            //var response = client.IndexMany<Tweet>(list);

             
            client.DeleteMany(list, "tweets", "tweet");

            //或
            //IDeleteRequest request = new DeleteRequest("tweets", "tweet", 16);
            //client.Delete(request);
            //request = new DeleteRequest("tweets", "tweet", 17);
            //client.Delete(request);
            //request = new DeleteRequest("tweets", "tweet", 18);
            //client.Delete(request);
            //request = new DeleteRequest("tweets", "tweet", 15);
            //client.Delete(request);
            //request = new DeleteRequest("tweets", "tweet", 2);
            //client.Delete(request);
            //request = new DeleteRequest("tweets", "tweet", 13);
            //client.Delete(request);




            //var result = client.Get<Tweet>(1, idx => idx.Index("tweets")); 
            //var tweet_result = result.Source;

            //var result1 = client.Search<Tweet>(s => s
            //                .From(0)
            //                .Size(10)
            //                .Query(q => q
            //                    .Term(t => t.UserName, "刘") || q
            //                    .Match(mq => mq.Field(f => f.UserName).Query("nest"))
            //                )
            //            );

            var result1 = client.Search<Tweet>(s => s
                            //.AllTypes()
                            //.AllIndices()
                            //.Scroll( new Time(15))
                            .From(0)
                            .Size(10)
                            .Query(q =>
                                q.Match(mq => mq.Field(f => f.UserName).Query("刚"))
                                 
                            )
                        );
            //client.IndexMany(new List<Tweet>());

           
        }
    }
 
}
