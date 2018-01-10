using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LingYi.DataTransfer.Models.Message
{
    public class tbl_DataChange
    {
        public int Version { get; set; }
        public string DataType { get; set; }
        public object DataList { get; set; }
    }
}
