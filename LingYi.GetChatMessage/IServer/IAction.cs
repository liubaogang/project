using LingYi.DataTransfer.Models.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LingYi.GetChatMessage.IServer
{
    public interface IAction
    {
        void Execute(tbl_Message Message);
    }
}
