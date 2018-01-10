using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LingYi.GetPushMessage.IServer
{
    public interface IMessagePush
    {
        /// <summary>
        /// 开始
        /// </summary>
        void Begin();
        /// <summary>
        /// 推送
        /// </summary>
        void Push(string deviceToken, string pushBody);
        /// <summary>
        /// 结束
        /// </summary>
        void End();
    }
}
