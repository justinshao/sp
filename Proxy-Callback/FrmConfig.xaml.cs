using Common.Core.Helpers;
using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Forms;

namespace PlatformServers
{
    /// <summary>
    /// FrmConfig.xaml 的交互逻辑
    /// </summary>
    public partial class FrmConfig : Window
    {
        public FrmConfig()
        {
            InitializeComponent();
            txtDataSource.Text = OptionHelper.DataSource;
            txtDatabase.Text = OptionHelper.Database;
            txtLoginName.Text = OptionHelper.LoginName;
            pbPassword.Password = OptionHelper.Password;
            try
            {
                txtBasicHttpPort.Text = OptionHelper.ReadString("CenterServer", "BasicHttpPort", "80");
                txtNetTcpPort.Text = OptionHelper.ReadString("CenterServer", "NetTcpPort", "4508");
                txtIP.Text = OptionHelper.ReadString("CenterServer", "ServerIP", "proxy-callback.spsing.cn");
                txtOnlineIP.Text = OptionHelper.ReadString("CenterServer", "OnlineIP", "");
                txtImgPath.Text = OptionHelper.ReadString("CenterServer", "ImgPath", "");
                txtzx.Text = OptionHelper.ReadString("CenterServer", "ZXAddr", "");
            }
            catch (Exception)
            { }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string str = OptionHelper.ConnectionString;
            if (!string.IsNullOrEmpty(str))
            {
                string[] strs = str.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string stritem in strs)
                {
                    string[] strsitems = stritem.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                    if (strsitems[0] == "Data Source")
                    {
                        txtDataSource.Text = strsitems[1].Trim();
                    }
                    else if (strsitems[0] == "Initial Catalog")
                    {
                        txtDatabase.Text = strsitems[1].Trim();

                    }
                    else if (strsitems[0] == "User Id")
                    {
                        txtLoginName.Text = strsitems[1].Trim();

                    }
                    else if (strsitems[0] == "Password")
                    {
                        pbPassword.Password = strsitems[1].Trim();
                    }

                }
            }
        }

        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Test(object sender, RoutedEventArgs e)
        {
            if (txtDataSource.Text.Contains("local"))
            {
                System.Windows.MessageBox.Show("如需连接本地数据库，请使用\".\"或\"127.0.0.1\",不要使用local之类单词.");
                return;
            }

            string connectionstr = "Data Source =" + txtDataSource.Text + ";Initial Catalog =" + txtDatabase.Text + ";User Id = " + txtLoginName.Text + ";Password = " + pbPassword.Password + ";";
            SqlConnection conn = new SqlConnection(connectionstr);
            try
            {
                conn.Open();
                System.Windows.MessageBox.Show("连接成功!");
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("测试连接失败!");
            }
            finally
            {
                conn.Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtImgPath.Text = fbd.SelectedPath;
                }
            }
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if (txtDataSource.Text.Contains("local"))
            {
                System.Windows.MessageBox.Show("如需连接本地数据库，请使用\".\"或\"127.0.0.1\",不要使用local之类单词.");
                return;
            }

            UInt16 httpPort = 0;
            UInt16 tcpPort = 0;
            UInt16 oldPort = 0;
            if (!UInt16.TryParse(txtBasicHttpPort.Text, out httpPort) || !UInt16.TryParse(txtNetTcpPort.Text, out tcpPort))
            {
                System.Windows.MessageBox.Show("请输入有效的端口号!", "提示");
                return;
            }

            if (tcpPort > 4534 || tcpPort < 4502)
            {
                System.Windows.MessageBox.Show("NetTcp端口需要配置在4502~4534范围内!", "提示");
                return;
            }

            OptionHelper.ConnectionString = "Data Source=" + txtDataSource.Text + ";Initial Catalog=" + txtDatabase.Text + ";User ID= " + txtLoginName.Text + ";Password= " + pbPassword.Password + ";";


            try
            {
                bool r = true;
                r = r && OptionHelper.WriteString("CenterServer", "BasicHttpPort", txtBasicHttpPort.Text);
                r = r && OptionHelper.WriteString("CenterServer", "NetTcpPort", txtNetTcpPort.Text);
                r = r && OptionHelper.WriteString("CenterServer", "ServerIP", txtIP.Text);
                r = r && OptionHelper.WriteString("CenterServer", "OnlineIP", txtOnlineIP.Text);
                r = r && OptionHelper.WriteString("CenterServer", "ImgPath", txtImgPath.Text);
                r = r && OptionHelper.WriteString("CenterServer", "ZXAddr", txtzx.Text);

                if (!r)
                {
                    System.Windows.MessageBox.Show("保存失败!", "提示");
                    return;
                }

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "异常");
                return;
            }

            System.Windows.MessageBox.Show("保存配置后需要重新运行服务程序");
            this.DialogResult = true;

        }
    }
}
