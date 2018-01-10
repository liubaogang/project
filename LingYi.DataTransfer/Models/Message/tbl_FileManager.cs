using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LingYi.DataTransfer.Models.Message
{
    public class tbl_FileManager
    {
        public string FileID { get; set; }
        public int FileType { get; set; }
        public string FileDirectory { get; set; }
        public string FileDate { get; set; }
    }
}
