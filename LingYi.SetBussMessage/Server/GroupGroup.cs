using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cicada.DI;
using Newtonsoft.Json;
using LingYi.SetBussMessage.IServer;
using LingYi.DataTransfer.Models.Message;

namespace LingYi.SetBussMessage.Server
{
    [ComponentAttribute("GROUPGROUP")]
    public class GroupGroup : BussBase, IAction
    {
        public void Execute(tbl_Message Message, string Key)
        {
            //var Relation = JsonConvert.DeserializeObject<>(data);
            switch (Key.Split('-')[2])
            {
                case "INSERT":
                    {

                    }; break;
                case "UPDATE":
                    {

                    }; break;
                case "DELETE":
                    {

                    }; break;
            }
        }
    }
}
