using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LingYi.DataTransfer.Models.Message
{
    public class tbl_DeskTopIcon
    {
        public int IconID { get; set; }
        public string IconType { get; set; }
        public string IconName { get; set; }
        public string IconImage { get; set; }
        public string AppUrl { get; set; }
        public string Mark { get; set; }
        public int Sort { get; set; }
    }
}
