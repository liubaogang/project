using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LingYi.DataTransfer.Models.Message
{
    public class tbl_FriendGroups
    {
        public string GroupID { get; set; }
        public int GroupCategory { get; set; }
        public int GroupType { get; set; }
        public string GroupName { get; set; }
        public string GroupImage { get; set; }
        public string GroupRemark { get; set; }
        public string GroupUserID { get; set; }
        public DateTime? GroupDate { get; set; }
    }
}
