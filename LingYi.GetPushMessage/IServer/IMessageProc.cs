using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LingYi.GetPushMessage.IServer
{
    public interface IMessageProc
    {
        void Begin();
        void PushMessage(string toUserID,string bodyContent);
        void End();
    }
}
