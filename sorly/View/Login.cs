using System;
using System.Configuration;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Solar
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(login_KeyDown);
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 登录界面聚焦
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Login_Focus(object sender, EventArgs e)
        {
            // 账号默认记住
            this.txtUserName.Text = ConfigurationManager.AppSettings["userName"];
            //如果记住密码为true 那么把值赋给文本框
            if (ConfigurationManager.AppSettings["rememberMe"].Equals("true"))
            {
                this.txtUserPw.Text = ConfigurationManager.AppSettings["passWord"];
                cheSavePw.Checked = true;
            }

            // 获取焦点

            string uneText = this.txtUserName.Text.Trim();
            string pwdText = this.txtUserPw.Text.Trim();
            if (uneText.Equals(""))
            {
                txtUserName.Focus(); // 获取输入账号焦点
            }
            else if (pwdText.Equals(""))
            {
                txtUserPw.Focus(); // 获取输入密码焦点
            }
            else
                butLogin.Focus();
        }

        /// <summary>
        /// 登录按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butLogin_Click(object sender, EventArgs e)
        {

            string uneText = this.txtUserName.Text.Trim();
            string pwdText = this.txtUserPw.Text.Trim();

            if (uneText.Equals(""))
            {
                MessageBox.Show("账号不能为空！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.txtUserName.Focus();
            }
            else if (pwdText.Equals(""))
            {
                MessageBox.Show("密码不能为空！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.txtUserPw.Focus();
            }
            else if (uneText != "" && pwdText != "")
            {
                SqlCommand cmd = new SqlCommand($"use LocalUser select count(*) from Login where Name='{uneText}' and PW='{pwdText}'", sqlCon.con);
                int i = Convert.ToInt32(cmd.ExecuteScalar());
                if (i == 1)
                {
                    MessageBox.Show("登陆成功", "登录", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // 记住账号密码
                    Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    cfa.AppSettings.Settings["userName"].Value = uneText; // 账号（默认记住）

                    if (this.cheSavePw.Checked)
                    {
                        cfa.AppSettings.Settings["rememberMe"].Value = "true"; // 自动赋值
                        cfa.AppSettings.Settings["passWord"].Value = pwdText; // 密码
                    }
                    else
                    {
                        cfa.AppSettings.Settings["rememberMe"].Value = "false"; // 自动赋值
                        cfa.AppSettings.Settings["passWord"].Value = ""; // 密码
                    }

                    cfa.Save(); // 保存数据
                    DialogResult = DialogResult.OK;// 记录完数据，提示登录成功
                    this.Close(); // 登录成功关闭当前页面，启动新页面
                }
                else
                    MessageBox.Show("登录失败，账号或密码错误！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 登录按钮回车监控
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true; // 将Handled设置为true，指示已经处理过KeyPress事件
                butLogin_Click(sender, e); // 登录界面如果监听到回车按钮，则触发单击事件进行登录校验
            }
        }

        private void cheSavePW_CheckedChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 退出按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butCancle_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();    
        }

    }
}
