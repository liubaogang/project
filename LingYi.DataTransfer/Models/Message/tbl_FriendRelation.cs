using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LingYi.DataTransfer.Models.Message
{
    public class tbl_FriendRelation
    {
        public string RelationID { get; set; }
        public string FromUserID { get; set; }
        public string ToUserID { get; set; }
        public string FriendGroupID { get; set; }
        public string SpaceGroupID { get; set; }
        public string RelationRemark { get; set; }
        public int RelationStatus { get; set; }
        public Dictionary<string, string> FriendAuthority { get; set; }
        public string Background { get; set; }
        public DateTime RelationDate { get; set; }
    }
}
