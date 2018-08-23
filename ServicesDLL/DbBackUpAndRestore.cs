
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Collections;
using System.Windows.Forms;
using Common;
using Common.Utilities.Helpers;
using Common.Core.Helpers;
namespace ServicesDLL

{
    class DbBackUpAndRestore
    {

        SqlConnection conn = new SqlConnection(OptionHelper.ConnectionString);
        /// <summary>
        /// 备份指定的数据库文件
        /// </summary>
        /// <param name="databasename">要还原的数据库</param>
        /// <returns></returns>
        public bool BackUpDataBase( string databasefile)
        {
            if (!File.Exists(databasefile))
            {

            }
            //还原的数据库MyDataBase
            string sql = "BACKUP DATABASE " + "SmartSystem" + " TO DISK = '" + databasefile + ".bak' ";
            conn.Open();
            SqlCommand comm = new SqlCommand(sql, conn);
            comm.CommandType = CommandType.Text;
            try
            {
                comm.ExecuteNonQuery();
            }
            catch (Exception err)
            {
                string str = err.Message;
                conn.Close();

                return false;
            }

            conn.Close();//关闭数据库连接
            return true;
        }

//以下是还原数据库，稍微麻烦些，要关闭所有与当前数据库相连的连接------------------------------------

//--------------------------------------------------------------------------------------------------------------------------
        public string RestoreDatabase(string backfile, string MyDataBase)
        {
            ///杀死原来所有的数据库连接进程
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Data Source=.;Initial Catalog=master;User ID=sa;pwd =";
            conn.Open();
            string sql = "SELECT spid FROM sysprocesses ,sysdatabases WHERE sysprocesses.dbid=sysdatabases.dbid AND sysdatabases.Name='" + 
                          "MyDataBase"+ "'";
            SqlCommand cmd1 = new SqlCommand(sql, conn);
            SqlDataReader dr;
            ArrayList list = new ArrayList();
            try
            {
                dr = cmd1.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(dr.GetInt16(0));
                }
                dr.Close();
            }
            catch (SqlException eee)
            {
                MessageBox.Show(eee.ToString());
            }
            finally
            {
                conn.Close();
            }
            //MessageBox.Show(list.Count.ToString());
            for (int i = 0; i < list.Count; i++)
            {
                conn.Open();
                cmd1 = new SqlCommand(string.Format("KILL {0}", list[i].ToString()), conn);
                cmd1.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("系统已经清除的数据库线程： " + list[i].ToString() + "\r\n正在还原数据库！");
            }
            //这里一定要是master数据库，而不能是要还原的数据库，因为这样便变成了有其它进程
            //占用了数据库。
            string constr = OptionHelper.ConnectionString;
            string database = MyDataBase;
            string path = backfile;
            string BACKUP = String.Format("RESTORE DATABASE {0} FROM DISK = '{1}'", database, path);
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(BACKUP, con);
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("还原成功,点击退出系统！");
                Application.Exit();
            }
            catch (SqlException ee)
            {
                //throw(ee);

                //MessageBox.Show("还原失败");

                MessageBox.Show(ee.ToString());

            }
            finally
            {
                con.Close();
            }
            return "成功与否字符串";
        }
    }
}