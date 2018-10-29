using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooKeeperNet;

namespace Zookepeer
{
    class Program
    {
        static void Main(string[] args)
        {
            //创建一个Zookeeper实例，第一个参数为目标服务器地址和端口，第二个参数为Session超时时间，第三个为节点变化时的回调方法 
            ZooKeeper zk = new ZooKeeper("192.168.1.167:2181", new TimeSpan(1, 0, 0, 50000), new Watcher());
            //zk.Create("/locks", new byte[0], Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
            zk.Create("/locks/kucun_lock_", "liubaogang1".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.EphemeralSequential);
            zk.Create("/locks/kucun_lock_", "liubaogang2".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.EphemeralSequential);

            var stat = zk.Exists("/root", true);
            ////创建一个节点root，数据是mydata,不进行ACL权限控制，节点为永久性的(即客户端shutdown了也不会消失) 
            if (zk.Exists("/root", true) == null)
            {
                string a = zk.Create("/root", "mydata".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
            }                

            //在root下面创建一个childone znode,数据为childone,不进行ACL权限控制，节点为永久性的 
            zk.Create("/root/childone", "childone".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
            //取得/root节点下的子节点名称,返回List<String> 
            zk.GetChildren("/root", true);
            //取得/root/childone节点下的数据,返回byte[] 
            //zk.GetData("/root/childone", true, null);

            ////修改节点/root/childone下的数据，第三个参数为版本，如果是-1，那会无视被修改的数据版本，直接改掉
            //zk.SetData("/root/childone", "childonemodify".GetBytes(), -1);
            ////删除/root/childone这个节点，第二个参数为版本，－1的话直接删除，无视版本 
            //zk.Delete("/root/childone", -1);


            
            Console.ReadKey();
        }
    }

    class Watcher : IWatcher
    {
        public void Process(WatchedEvent @event)
        {
            switch (@event.Type)
            {
                case EventType.NodeChildrenChanged:
                    {
                        Console.WriteLine(@event.Path+":NodeChildrenChanged");
                    };break;
                case EventType.NodeCreated:
                    {
                        Console.WriteLine(@event.Path + ":NodeCreated");
                    }; break;
                case EventType.NodeDataChanged:
                    {
                        Console.WriteLine(@event.Path + ":NodeDataChanged");
                    }; break;
                case EventType.NodeDeleted:
                    {
                        Console.WriteLine(@event.Path + ":NodeDeleted");
                    }; break;
                case EventType.None:
                    {
                        Console.WriteLine(@event.Path + ":None......");
                    }; break;
            }
        }
    }
}
