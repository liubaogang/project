using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using LingYi.NetToolClass.Base;

namespace LingYi.DataTransfer.Models.Message
{
    public class tbl_Message
    {
        public tbl_Message()
        {

        }

        public tbl_Message(string XmlMessage)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(XmlMessage);
            XmlNode msgXml = xmlDoc.SelectSingleNode("message");
            Regex reg = new Regex("<command>(.*?)</command>");
            Match match = reg.Match(XmlMessage);
            if (match.Success)
            {
                MsgID = msgXml.Attributes["id"].Value;
                MsgFrom = msgXml.Attributes["from"].Value;
                MsgFrom = MsgFrom.Split('@')[0];
                MsgTo = msgXml.Attributes["to"].Value;
                MsgTo = MsgTo.Split('@')[0];
                MsgType = match.Groups[1].Value.Trim();
                MsgType = MsgType.Remove(0, 4);
                MsgStanza = xmlDoc.OuterXml;
                MsgDate = DateTime.Now;
                reg = new Regex("<datetime>(.*?)</datetime>");
                match = reg.Match(XmlMessage);
                if (match.Success)
                {
                    string TimeStamp = match.Groups[1].Value;
                    MsgDate = Base.ConvertToDateTime(TimeStamp);
                }
            }
        }
        public string MsgID { get; set; }
        public string MsgFrom { get; set; }
        public string MsgTo { get; set; }
        /// <summary>
        /// CHAT|GROUPCHAT
        /// </summary>
        public string MsgType { get; set; }
        public string MsgStanza { get; set; }
        public DateTime? MsgDate { get; set; }
    }
}
