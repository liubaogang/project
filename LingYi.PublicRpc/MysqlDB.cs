using Cicada;
using Cicada.DI;
using System.Data;
using Cicada.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LingYi.PublicRpc
{
    public class MysqlDB
    {
        public static IDbConnection GetOpenConnection()
        {
            var config = CicadaFacade.Container.Resolve<IConfigurationDataRespository>();
            var Connection = new MySqlConnection(config.Get("Cicada.Data.ConnectionString"));
            Connection.Open();
            return Connection;
        }
        //http://www.cnblogs.com/Contoso/p/5117383.html
    }
}
