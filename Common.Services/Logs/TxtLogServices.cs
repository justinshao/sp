using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web.Hosting;

namespace Common.Services
{
    public class TxtLogServices
    {
        static readonly object Lock = new object();
        /// <summary>
        /// 记录日志,文件名为yyyyMMdd.log
        /// </summary>
        /// <param name="format">日志内容</param>
        /// <param name="args">参数</param>
        public static void WriteTxtLog(string format, params object[] args)
        {
            WriteTxtLogEx(DateTime.Now.ToString("yyyyMMdd"), format, args);
        }

        /// <summary>
        /// 写日志,以天为单位建立文件夹,后跟文件名
        /// </summary>
        /// <param name="filename">日志文件名</param>
        /// <param name="format">日志内容</param>
        /// <param name="args">参数</param>
        public static void WriteTxtLogEx(string filename, string format, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                return;
            }
            lock (Lock)
            {
                try
                {
                    var body = args.Length > 0 ? string.Format(format, args) : format;
                    body = string.Format("{0:HH:mm:ss fff}\t{1}", DateTime.Now, body);
                    var logPath = GetLogPath();
                    if (string.IsNullOrEmpty(logPath))
                        return;
                    logPath = Path.Combine(logPath, DateTime.Now.ToString("yyyyMMdd"));
                    if (!Directory.Exists(logPath))
                    {
                        Directory.CreateDirectory(logPath);
                    }
                    logPath = Path.Combine(logPath, filename + ".log");
                    using (var sw = File.AppendText(logPath))
                    {
                        sw.WriteLine(body);
                    }
                }
                catch (Exception ex)
                {
                    //ExceptionsServices.(ex, "记录文本日志出错");
                }
            }
        }
        public static void WriteTxtLogEx(string filename,Exception ex)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                return;
            }
            WriteTxtLogEx(filename, string.Format("错误描述:{0},堆栈信息：{1}", ex.Message, ex.StackTrace));
        }
        public static void WriteTxtLogEx(string filename,string description, Exception ex)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                return;
            }
            WriteTxtLogEx(filename, string.Format("错误描述:{0}；{1},堆栈信息：{2}",description,ex.Message, ex.StackTrace));
        }
        private static string _logPath;
        private static string GetLogPath()
        {
            if (_logPath == null)
            {
                _logPath = HostingEnvironment.MapPath("~/Logs/");
                if (string.IsNullOrEmpty(_logPath))
                    _logPath = string.Empty;
            }
            return _logPath;
        }

        private static void SetLogPath(string logPath)
        {
            _logPath = logPath;
        }
    }
}
