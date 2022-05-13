
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class SMSBL
    {
        public static void SendSMS(string mobile, string smsText)
        {
            string userName = "ASNTECHNOSOFT";
            string password = "Password@2017";
            string senderId = "ASNTEC";
            string msgType = "TXT";
            string uri = "http://www.smsjust.com/blank/sms/user/urlsms.php?username={0}&pass={1}&senderid={2}&dest_mobileno={3}&message={4}&msgtype={5}&response=Y";
            string url = string.Format(uri, userName, password, senderId, mobile, smsText, msgType);

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            StreamReader sr = new StreamReader(resp.GetResponseStream());
            string results = sr.ReadToEnd();
            sr.Close();
        }

        public static void SendSMSEnquiryConfirm(string mobile, string smsText)
        {
            string userName = "ASNTECHNOSOFT";
            string password = "Password@2017";
            string senderId = "ASNTEC";
            string msgType = "TXT";
            string uri = "http://www.smsjust.com/blank/sms/user/urlsms.php?username={0}&pass={1}&senderid={2}&dest_mobileno={3}&message={4}&msgtype={5}&response=Y";
            string url = string.Format(uri, userName, password, senderId, mobile, smsText, msgType);

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            StreamReader sr = new StreamReader(resp.GetResponseStream());
            string results = sr.ReadToEnd();
            sr.Close();
        }

        public static void SendSMSFeesFollowup(string mobile, string smsText)
        {
            string userName = "ASNTECHNOSOFT";
            string password = "Password@2017";
            string senderId = "ASNTEC";
            string msgType = "TXT";
            string uri = "http://www.smsjust.com/blank/sms/user/urlsms.php?username={0}&pass={1}&senderid={2}&dest_mobileno={3}&message={4}&msgtype={5}&response=Y";
            string url = string.Format(uri, userName, password, senderId, mobile, smsText, msgType);

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            StreamReader sr = new StreamReader(resp.GetResponseStream());
            string results = sr.ReadToEnd();
            sr.Close();
        }
    }
}
