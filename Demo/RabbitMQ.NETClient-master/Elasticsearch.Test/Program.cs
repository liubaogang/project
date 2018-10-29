using PlainElastic.Net;
using PlainElastic.Net.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Elasticsearch.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = new ElasticConnection("192.168.1.167", 9200);


            var tweet = new Tweet { User = "刘宝刚" , Address="黑龙江省大庆市", Age="33" };

            string tweetJson = JsonConvert.SerializeObject(tweet);

            string result = connection.Put(new IndexCommand("twitter", "tweet", id: Guid.NewGuid().ToString()), tweetJson);
            
        }
    }

    internal class Tweet
    {
        public string User { get; set; }

        public string Address { get; set; }

        public string Age { get; set; }


        public int big { get; set; }
    }
}
