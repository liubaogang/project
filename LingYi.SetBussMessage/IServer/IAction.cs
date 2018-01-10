using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LingYi.DataTransfer.Models.Message;

namespace LingYi.SetBussMessage.IServer
{
    public interface IAction
    {
        void Execute(tbl_Message Message, string Key);
    }
}
