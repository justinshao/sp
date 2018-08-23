using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Common.DataAccess;
using Common;
using Common.Entities;
using System.Data.Common;
using Common.Utilities;
using Common.Entities.Parking;
using System.IO;
using Common.Core;

namespace ServicesDLL
{
    public class ClearData
    {
        /// <summary>
        /// 日志记录对象
        /// </summary>
        public static bool _IsDS = true;
        static Logger logger = LogManager.GetLogger("定期清理数据");
        static bool IsStartClear = true;
        public static string Path = "";
        static List<BaseParkinfo> listparkinfo = new List<BaseParkinfo>();
        /// <summary>
        /// 上传车场系统数据
        /// </summary>
        public static void StartClearClientData()
        {
            while (IsStartClear)
            {
                if (DateTime.Now.Hour == 3  || !_IsDS)
                {
                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                    {
                        string strsql = "select * from BaseParkinfo a left join BaseVillage b on a.VID=b.VID ";
                        strsql += "where a.DataStatus<2 and b.DataStatus<2";

                        if (_IsDS)
                        {
                            strsql += "and b.ProxyNo='" + SystemInfo.ProxyNo + "'";
                        }

                        dbOperator.ClearParameters();
                        using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                        {
                            while (reader.Read())
                            {
                                BaseParkinfo model = DataReaderToModel<BaseParkinfo>.ToModel(reader);
                                listparkinfo.Add(model);
                            }
                        }
                    }
                    ClearPic();
                    ClearParkEvent();
                    ClearParkIORecord();
                    ClearParkOrder();
                    ClearParkTimeseries();
                }
                Thread.Sleep(1000 * 60 * 30);
            }
        }

        /// <summary>
        /// 开始上传数据
        /// </summary>
        public static void StartClear()
        {
            IsStartClear = true;
            Thread thread = new Thread(new ThreadStart(StartClearClientData));
            thread.IsBackground = true;
            thread.Start();
        }


        #region 清理数据
        /// <summary>
        /// 清理通道事件纪录
        /// </summary>
        public static void ClearParkEvent()
        {
            foreach (var pkinfo in listparkinfo)
            {
                if (pkinfo.DataSaveDays != 0)
                {
                    try
                    {
                        using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                        {
                            int count = 1;
                            while (count > 0)
                            {
                                string strsql = "select count(RecordID) as countr from ParkEvent where datediff(d,RecTime,getdate())>'" + pkinfo.DataSaveDays + "' ";

                                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                                {
                                    if (reader.Read())
                                    {
                                        count = reader["countr"].ToInt();
                                    }
                                }
                                strsql = "delete ParkEvent where RecordID in( select top 1000 RecordID from ParkEvent where datediff(d,RecTime,getdate())>'" + pkinfo.DataSaveDays + "' )";
                                dbOperator.ClearParameters();
                                int res = dbOperator.ExecuteNonQuery(strsql);
                                Thread.Sleep(3000);
                            }
                        }
                        logger.Fatal("清理" + pkinfo.PKName + "通道信息ClearParkEvent()成功\r\n");
                    }
                    catch(Exception ex)
                    {
                        logger.Fatal("清理" + pkinfo.PKName + "通道信息异常！ClearParkEvent()" + ex.Message + "\r\n");
                    }
                }
            }
        }

        /// <summary>
        /// 清理进出信息
        /// </summary>
        public static void ClearParkIORecord()
        {
            foreach (var pkinfo in listparkinfo)
            {
                if (pkinfo.DataSaveDays != 0)
                {
                    try
                    {
                        using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                        {
                            int count = 1;
                            while (count > 0)
                            {
                                string strsql = "select count(RecordID) as countr from ParkIORecord where IsExit=1 and datediff(d,ExitTime,getdate())>'" + pkinfo.DataSaveDays + "' ";

                                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                                {
                                    if (reader.Read())
                                    {
                                        count = reader["countr"].ToInt();
                                    }
                                }
                                strsql = "delete ParkIORecord where RecordID in( select top 1000 RecordID from ParkIORecord where IsExit=1 and  datediff(d,ExitTime,getdate())>'" + pkinfo.DataSaveDays + "' )";
                                dbOperator.ClearParameters();
                                int res = dbOperator.ExecuteNonQuery(strsql);
                                Thread.Sleep(3000);
                            }

                        }
                        logger.Fatal("清理" + pkinfo.PKName + "进出信息ClearParkIORecord()成功\r\n");
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("清理" + pkinfo.PKName + "进出信息异常！ClearParkIORecord()" + ex.Message + "\r\n");
                    }
                }
            }
        }

        /// <summary>
        /// 清理订单信息
        /// </summary>
        public static void ClearParkOrder()
        {
            foreach (var pkinfo in listparkinfo)
            {
                if (pkinfo.DataSaveDays != 0)
                {
                    try
                    {
                        using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                        {
                            int count = 1;
                            while (count > 0)
                            {
                                string strsql = "select count(RecordID) as countr from ParkOrder where  datediff(d,OrderTime,getdate())>'" + pkinfo.DataSaveDays + "' ";

                                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                                {
                                    if (reader.Read())
                                    {
                                        count = reader["countr"].ToInt();
                                    }
                                }
                                strsql = "delete ParkOrder where RecordID in( select top 1000 RecordID from ParkOrder where  datediff(d,OrderTime,getdate())>'" + pkinfo.DataSaveDays + "' )";
                                dbOperator.ClearParameters();
                                int res = dbOperator.ExecuteNonQuery(strsql);
                                Thread.Sleep(3000);
                            }
                           
                        }
                        logger.Fatal("清理" + pkinfo.PKName + "订单信息ClearParkOrder()成功\r\n");
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("清理" + pkinfo.PKName + "订单信息异常！ClearParkOrder()" + ex.Message + "\r\n");
                    }
                }
            }
        }

        /// <summary>
        /// 清理时序表
        /// </summary>
        public static void ClearParkTimeseries()
        {
            foreach (var pkinfo in listparkinfo)
            {
                if (pkinfo.DataSaveDays != 0)
                {
                    try
                    {
                        using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                        {
                            int count = 1;
                            while (count > 0)
                            {
                                string strsql = "select count(TimeseriesID) as countr from ParkTimeseries where IsExit=1 and datediff(d,ExitTime,getdate())>'" + pkinfo.DataSaveDays + "' ";

                                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                                {
                                    if (reader.Read())
                                    {
                                        count = reader["countr"].ToInt();
                                    }
                                }
                                strsql = "delete ParkTimeseries where TimeseriesID in( select top 1000 TimeseriesID from ParkTimeseries where  IsExit=1 and  datediff(d,ExitTime,getdate())>'" + pkinfo.DataSaveDays + "' )";
                                dbOperator.ClearParameters();
                                int res = dbOperator.ExecuteNonQuery(strsql);
                                Thread.Sleep(3000);
                            }

                         
                        }
                        logger.Fatal("清理" + pkinfo.PKName + "时序信息ClearParkTimeseries()成功\r\n");
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("清理" + pkinfo.PKName + "时序信息异常！ClearParkTimeseries()" + ex.Message + "\r\n");
                    }
                }
            }
        }
        #endregion

        #region 清理图片

        public static void ClearPic()
        {
            foreach (var pkinfo in listparkinfo)
            {
             
                if (pkinfo.PictureSaveDays != 0)
                {
                    
                    try
                    {
                        using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                        {
                            string path = ClearData.Path + "\\" + pkinfo.PKName;
                            string[] strs= Directory.GetDirectories(path);
                            foreach (var rec in strs)
                            {
                                logger.Fatal("开始清理" + pkinfo.PKName + "图片信息:" + rec + "\r\n");
                                string[] aa = rec.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                                if (aa.Length > 0)
                                {
                                   DateTime dt = aa[aa.Length - 1].ToDateTime();
                                    if (dt!=DateTime.MinValue && (DateTime.Now - dt).Days > pkinfo.PictureSaveDays)
                                    {                                     
                                         Directory.Delete(rec, true);
                                         logger.Fatal("清理" + pkinfo.PKName + "图片信息成功" + rec + "\r\n");
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("清理" + pkinfo.PKName + "图片信息异常！ClearPic()" + ex.Message + "\r\n");
                    }
                }
            }
        }

        public static void DeletePic(string imgpath)
        {
            try
            {
                FileInfo file = new FileInfo(imgpath);
                if (!file.Exists)
                {
                    logger.Info("删除文件找不到文件:" + imgpath);
                    return;
                }
                file.Delete();
               
            }
            catch(Exception ex)
            {
                logger.Info("删除图片失败:" + ex.Message);
            }
        }
        #endregion

    }
}