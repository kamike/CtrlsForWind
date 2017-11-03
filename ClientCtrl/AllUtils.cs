using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ClientCtrl
{
    class AllUtils
    {
        internal static Object jsonToObj(String jsonStr, Type type)
        {
            try
            {
                return JavaScriptConvert.DeserializeObject(jsonStr, type);

            }
            catch (Exception e) {
                Console.WriteLine("json数据异常："+e.Message);
                return null;
            }
        }

        internal static String getIpsCity(String ip)
        {
            String ipWeb = "http://ip.taobao.com/service/getIpInfo.php?ip=" + ip;
            try
            {
                //WebClient webClient = new WebClient();
                //webClient.Credentials = CredentialCache.DefaultCredentials;
                //byte[] data = webClient.DownloadData(ipWeb);
                //String pageHtml = Encoding.Default.GetString(data);
                //IpInfoBean info = jsonToObj(pageHtml, typeof(IpInfoBean)) as IpInfoBean;
                //if (info.code == 0)
                //{
                //    if (info.data != null)
                //    {
                //        return info.data.region + info.data.city;
                //    }
                //}
            }
            catch (Exception e)
            {

            }
            return "未知";

        }
        public static String getTimeFormat(long time)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
             TimeSpan toNow = new TimeSpan(time*10000);
            DateTime targetDt = dtStart.Add(toNow);
            return targetDt.ToString("F");

        }
    }


}
