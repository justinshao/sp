using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PlatformWcfServers
{
    static class Util
    {
        public static void SendSms(string tpl_id, IDictionary<string, string> tpl_values, string mobile)
        {
            var query = new Dictionary<string, string>() {
                { "mobile", mobile },
                { "tpl_id", tpl_id },
                { "key", "71fe46e446d3f1cb46f32ac9644150a0" },
            }.Aggregate(string.Empty, (q, kv) => q += $"{kv.Key}={kv.Value}&");

            var tpl_value = tpl_values.Aggregate(string.Empty, (q, kv) => q += $"#{kv.Key}#={kv.Value}&");
            
            var url = "http://v.juhe.cn/sms/send?" + query + "tpl_value=" + HttpUtility.UrlEncode(tpl_value.TrimEnd('&'), Encoding.UTF8);
            
            using (var reader = new StreamReader(((HttpWebRequest)WebRequest.Create(url)).GetResponse().GetResponseStream()))
            {
                var json = reader.ReadToEnd();
                var ret = JsonConvert.DeserializeObject<SmsResult>(json);

                if(ret.Code != 0)
                {
                    throw new Exception($"短信发送失败:{ret.Reason} 详细信息:{json}");
                }
            }
        }

        public class SmsResult
        {
            public int Code { get; set; }
            public string Reason { get; set; }
        }
    }
}