using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LingYi.SetBussMessage.Server
{
    public class BussBase
    {
        public string GetDatePrefix
        {
            get
            {
                return DateTime.Now.ToString("yyyyMM");
            }
        }
    }
}
