using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Solar
{
    static class sqlCon
    {
        static public SqlConnection con = new SqlConnection
        {
            ConnectionString = "Data Source = SURFACE-LEMMON\\SQLEXPRESS ; User ID = sa ; Password = zym1006x ; Connect Timeout =2"
        };
    }
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                sqlCon.con.Open();
                if (sqlCon.con.State == ConnectionState.Open)
                    MessageBox.Show("数据库初始化成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Login login = new Login();
                if (login.ShowDialog() == DialogResult.OK)
                {
                    login.Dispose();
                    Application.Run(new SolarTrackers());
                }
            }
            catch
            {
                MessageBox.Show("数据库初始化失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
