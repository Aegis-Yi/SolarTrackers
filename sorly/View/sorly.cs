using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Threading;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows.Forms.DataVisualization.Charting;

namespace Solar
{
    public partial class SolarTrackers : Form
    {
        private PortForm portform;
        static string checkTime = "";
        DataTable dt = null;
        bool flagAD = false;
        bool flagAU = false;

        public SolarTrackers()
        {
            InitializeComponent();
        }

        public void SolarTrackers_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 历史数据初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SolarTrackers_Focus(object sender, EventArgs e)
        {
            this.LabRename.Text = ConfigurationManager.AppSettings["MacName"];              //显示最后一次更新的仪器码
            this.setTimeNewdata.Text = ConfigurationManager.AppSettings["NewTime"];         //显示最后一次记录的更新时间设置
            this.setTimeUpdata.Text = ConfigurationManager.AppSettings["UpTime"];           //显示最后一次记录的上传时间设置
            this.toolStripStatusLabel3.Text = ConfigurationManager.AppSettings["Newdata"];  //显示最后一次更新的更新时间
            this.toolStripStatusLabel4.Text = ConfigurationManager.AppSettings["Updata"];   //显示最后一次更新的上传时间
        }

        /// <summary>
        /// 端口界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PortSet_Click(object sender, EventArgs e)
        {
            if (portform == null || portform.IsDisposed)
            {
                portform = new PortForm();
                portform.Show();
            }
            if (portform.WindowState == FormWindowState.Minimized)
                portform.WindowState = FormWindowState.Normal;
        }

        /// <summary>
        /// “重命名”按钮事件
        /// </summary>
        /// <param name = "sender" ></ param >
        /// < param name="e"></param>
        private void ButRename_Click(object sender, EventArgs e)
        {
            if (LabRename.Text.Trim().Equals("") || LabRename.Text.Length != 4)
            {
                MessageBox.Show("记录仪名称长度必须为4位字符", "重命名", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.LabRename.Focus();
                return;
            }
            if (PortForm.port != "Not Connected！")  //判断串口是否打开
            {
                try
                {
                    PortForm._serialPort.WriteLine("#130N" + LabRename.Text + "GG");//重命名协议:#130N+记录仪编号+GG
                    Thread.Sleep(1000);
                }
                catch
                {
                    MessageBox.Show("串口通讯发送失败", "重命名", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                string backstring = PortForm.buckstr;
                PortForm.buckstr = "";
                if (backstring == "#130NGG") //下位机返回:#130NGG
                {
                    MessageBox.Show("重命名成功", "重命名", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    cfa.AppSettings.Settings["MacName"].Value = LabRename.Text.Trim(); // 记录仪编号
                    cfa.Save();
                }
                else
                {
                    MessageBox.Show("重命名失败", "重命名", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
                MessageBox.Show("串口未打开", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        /// <summary>
        /// 时间端口状态显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick_1(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = "         Time: " + DateTime.Now.ToString();
            toolStripStatusLabel6.Text = "         Time: " + DateTime.Now.ToString();
        }

        /// <summary>
        /// 退出按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void quitClick_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        /// <summary>
        /// 关于界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm2 aboutform = new AboutForm2();
            aboutform.StartPosition = FormStartPosition.CenterScreen; //开始的窗体位置在屏幕中间
            aboutform.Show();
        }

        /// <summary>
        /// 界面状态更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            port.Text = PortForm.port;
            if (sqlCon.con.State == ConnectionState.Open)
                labSql.Text = "数据库已连接";
            else
                labSql.Text = "未连接数据库";
        }

        /// <summary>
        /// “实时更新”按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButNewdata_Click(object sender, EventArgs e)
        {
            if (LabRename.Text.Trim().Equals("") || LabRename.Text.Length != 4)
            {
                MessageBox.Show("记录仪名称长度必须为4位字符", "实时数据更新", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.LabRename.Focus();
                return;
            }
            if (PortForm.port != "Not Connected！")  //判断串口是否打开
            {
                try
                {
                    PortForm._serialPort.WriteLine("#" + LabRename.Text + "030CGG");//实时监控协议:#记录仪编号030CGG
                    Thread.Sleep(1000);
                }
                catch
                {
                    MessageBox.Show("串口通讯发送失败", "数据更新", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (PortForm.buckstr != "")
                {
                    int a = LabRename.Text.Length;
                    string backstring = PortForm.buckstr;
                    //MessageBox.Show(backstring);
                    PortForm.buckstr = "";
                    string checkstring = backstring.Substring(0, a + 5);
                    //MessageBox.Show(checkstring);
                    if (checkstring == "#" + LabRename.Text + "030C") //下位机返回:#记录仪编号030C + 数据 + GG
                    {
                        NewDataUp(LabRename.Text, backstring);

                        toolStripStatusLabel3.Text = "                           上次更新: " + DateTime.Now.ToString();
                        Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                        cfa.AppSettings.Settings["Newdata"].Value = toolStripStatusLabel3.Text; // 记录最后一次更新
                        cfa.Save();
                      
                        Thread.Sleep(500);
                        Draw();//画实时图
                    }
                    else
                    {
                        MessageBox.Show("数据通信失败", "数据更新", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                else
                {
                    MessageBox.Show("未接受到返回数据", "数据更新", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
                MessageBox.Show("串口未打开", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// “实时更新”数据更新方法
        /// </summary>
        /// <param name="LabRename"></param>
        /// <param name="backstring"></param>
        private void NewDataUp(string name, string str)
        {
            List<string> _list = Istring(name, str);
            List<double> _vs = Workdouble(_list);
            Workstring(_list, _vs);
            labName.Text = _list[0];
            labTime.Text = _list[1];
            labTem.Text = _list[2];
            lab1.Text = _list[3];
            lab2.Text = _list[4];
            lab3.Text = _list[5];
            lab4.Text = _list[6];
            lab5.Text = _list[7];
            labLu.Text = _list[8];
            labSD.Text = _list[9];
            lab6.Text = _list[13];
            lab7.Text = _list[14];
            lab8.Text = _list[15];
            lab9.Text = _list[16];
            lab10.Text = _list[17];
            lab11.Text = _list[18];
            lab12.Text = _list[19];
            lab13.Text = _list[20];
            lab14.Text = _list[21];
            lab15.Text = _list[22];
            lab16.Text = _list[23];
            lab17.Text = _list[24];
            lab18.Text = _list[26];
            lab19.Text = _list[37];
            lab20.Text = _list[38];
            lab21.Text = _list[39];
            lab22.Text = _list[40];
            lab23.Text = _list[41];
            lab24.Text = _list[42];
            lab25.Text = _list[43];
            lab26.Text = _list[44];
            lab27.Text = Convert.ToInt32(_list[45] + _list[46], 16).ToString();
            lab28.Text = Convert.ToInt32(_list[47].Substring(0, 2), 16).ToString();
        }

        /// <summary>
        /// “自动更新”复选框事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButAutoData_CheckedChanged(object sender, EventArgs e)
        {
            if (AutoData.Checked)  //判断自动发送是否被选择，如果选择了，则启用timer时钟
            {
                if (setTimeNewdata.Text.Trim().Equals(""))
                {
                    MessageBox.Show("请输入时间", "自动更新", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AutoData.Checked = false;
                    return;
                }
                else
                {
                    try
                    {
                        timeNewDate.Interval = int.Parse(setTimeNewdata.Text) * 60000; //自动更新时间单位 ms*倍率
                        Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                        cfa.AppSettings.Settings["NewTime"].Value = setTimeNewdata.Text.Trim(); // 记录更新时间间隔
                        cfa.Save();
                        setTimeNewdata.Enabled = false;  //设置的时间间距不能修改                        
                        timeNewDate.Enabled = true;
                        flagAD = true;
                    }
                    catch
                    {
                        MessageBox.Show("请输入纯数字", "自动更新", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        AutoData.Checked = false;
                        setTimeNewdata.Focus();
                    }
                }
            }
            else
            {
                timeNewDate.Enabled = false; //timer不能启用
                setTimeNewdata.Enabled = true;  // //设置的时间间距可以修改
                flagAD = false;
            }
        }

        /// <summary>
        /// “自动更新”触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timeNewDate_Tick(object sender, EventArgs e)
        {
            ButNewdata_Click(sender, e);//触发“自动更新”按钮事件
        }

        /// <summary>
        /// “数据上传”按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButUpdata_Click(object sender, EventArgs e)
        {
            if (labName.Text.Trim().Equals(""))
            {
                MessageBox.Show("未找到实时数据", "数据上传", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.ButNewdata.Focus();
                return;
            }

            if (checkTime.Equals(labTime.Text))
            {
                MessageBox.Show("上传时间间隔不足一分钟", "数据上传", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                new SqlCommand($"use Sorly INSERT INTO [Machine] ( [Name], [时间] ) VALUES ('{labName.Text}' , '{labTime.Text}')", sqlCon.con).ExecuteNonQuery();
                new SqlCommand($"use Sorly INSERT INTO [Tem] ( [时间], [环温], [环温1], [环温2], [环温3], [环温4], [环温5], [露点], [环湿] ) VALUES ('{labTime.Text}' , '{Convert.ToDouble(labTem.Text)}', '{Convert.ToDouble(lab1.Text)}', '{Convert.ToDouble(lab2.Text)}', '{Convert.ToDouble(lab3.Text)}','{Convert.ToDouble(lab4.Text)}','{Convert.ToDouble(lab5.Text)}','{Convert.ToDouble(labLu.Text)}','{Convert.ToDouble(labSD.Text)}')", sqlCon.con).ExecuteNonQuery();
                new SqlCommand($"use Sorly INSERT INTO [Air] ( [时间], [C02], [蒸发], [气压] ) VALUES ('{labTime.Text}' , '{Convert.ToDouble(lab6.Text)}', '{Convert.ToDouble(lab7.Text)}', '{Convert.ToDouble(lab8.Text)}')", sqlCon.con).ExecuteNonQuery();
                new SqlCommand($"use Sorly INSERT INTO [Wind] ( [时间], [风向], [风速], [10分钟风速] ) VALUES ('{labTime.Text}' , '{lab16.Text}', '{Convert.ToDouble(lab17.Text)}', '{Convert.ToDouble(lab18.Text)}')", sqlCon.con).ExecuteNonQuery();
                new SqlCommand($"use Sorly INSERT INTO [Raido] ( [时间], [总接辐射瞬时], [散辐射瞬时], [直接辐射瞬时], [反辐射瞬时], [净辐射瞬时], [光合瞬时], [紫外瞬时] ) VALUES ('{labTime.Text}' , '{Convert.ToDouble(lab9.Text)}', '{Convert.ToDouble(lab10.Text)}', '{Convert.ToDouble(lab11.Text)}', '{Convert.ToDouble(lab12.Text)}', '{Convert.ToDouble(lab13.Text)}', '{Convert.ToDouble(lab14.Text)}', '{Convert.ToDouble(lab15.Text)}')", sqlCon.con).ExecuteNonQuery();
                new SqlCommand($"use Sorly INSERT INTO [Total] ( [时间], [日照时日累计], [总接辐射日累计], [散辐射日累计], [直接辐射日累计], [反辐射日累计], [净辐射日累计], [光合日累计], [紫外日累计] )VALUES ('{labTime.Text}' , '{Convert.ToDouble(lab19.Text)}', '{Convert.ToDouble(lab20.Text)}', '{Convert.ToDouble(lab21.Text)}', '{Convert.ToDouble(lab22.Text)}', '{Convert.ToDouble(lab23.Text)}', '{Convert.ToDouble(lab24.Text)}', '{Convert.ToDouble(lab25.Text)}', '{Convert.ToDouble(lab26.Text)}')", sqlCon.con).ExecuteNonQuery();
                new SqlCommand($"use Sorly INSERT INTO [Power] ( [时间], [光照度], [电量] )VALUES ('{labTime.Text}' , '{Convert.ToDouble(lab27.Text)}', '{Convert.ToDouble(lab28.Text)}')", sqlCon.con).ExecuteNonQuery();
                checkTime = labTime.Text;
            }

            toolStripStatusLabel4.Text = "                           上次上传: " + DateTime.Now.ToString();
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            cfa.AppSettings.Settings["Updata"].Value = toolStripStatusLabel4.Text; // 记录最后一次上传时间
            cfa.Save();
        }

        /// <summary>
        /// “自动上传”复选框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutoUpdata_CheckedChanged(object sender, EventArgs e)
        {
            if (AutoUpdata.Checked)  //判断自动发送是否被选择，如果选择了，则启用timer时钟
            {
                if (setTimeNewdata.Text.Trim().Equals(""))
                {
                    MessageBox.Show("请输入自动更新时间间隔", "自动更新", MessageBoxButtons.OK, MessageBoxIcon.Error);                    
                    setTimeNewdata.Focus();
                    AutoUpdata.Checked = false;
                    return;
                }

                if (setTimeUpdata.Text.Trim().Equals(""))
                {
                    MessageBox.Show("请输入自动上传时间间隔", "自动上传", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    setTimeUpdata.Focus();
                    AutoUpdata.Checked = false;
                    return;
                }

                if (Convert.ToInt32(setTimeUpdata.Text) <= Convert.ToInt32(setTimeNewdata.Text))
                {
                    MessageBox.Show("上传时间间隔必须大于更新时间间隔", "自动更新", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    setTimeUpdata.Focus();
                    AutoUpdata.Checked = false;
                    return;
                }

                if (flagAD == false) 
                {
                    AutoData.Checked = !AutoData.Checked;
                    flagAU = true;
                }
                
                
                //else
                {
                    try
                    {
                        timeUpdata.Interval = int.Parse(setTimeUpdata.Text) * 60000;//自动上传时间单位 ms*倍率
                        Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                        cfa.AppSettings.Settings["UpTime"].Value = setTimeUpdata.Text.Trim(); // 记录上传时间间隔
                        cfa.Save();

                        AutoData.Enabled = false;
                        setTimeUpdata.Enabled = false;  //设置的时间间距不能修改                        
                        timeUpdata.Enabled = true;
                    }
                    catch
                    {
                        MessageBox.Show("请输入纯数字", "自动上传", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        AutoUpdata.Checked = false;
                        setTimeUpdata.Focus();
                    }
                }
            }
            else
            {
                AutoData.Enabled = true;
                if (flagAU == true)
                {
                    AutoData.Checked = !AutoData.Checked;
                    flagAU = false;
                }
                timeUpdata.Enabled = false; //timer不能启用
                setTimeUpdata.Enabled = true;  // //设置的时间间距可以修改
            }
        }

        /// <summary>
        /// “自动上传”触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timeUpdata_Tick(object sender, EventArgs e)
        {
            ButUpdata_Click(sender, e);//触发“数据上传”按钮事件
        }

        /// <summary>
        /// “时间同步”按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButTime_Click(object sender, EventArgs e)
        {
            if (LabRename.Text.Trim().Equals(""))
            {
                MessageBox.Show("记录仪名称不得为空", "时间同步", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.LabRename.Focus();
                return;
            }
            if (PortForm.port != "Not Connected！")  //判断串口是否打开
            {
                try
                {
                    PortForm._serialPort.WriteLine("#" + LabRename.Text + "010A" + DateTime.Now.Second.ToString("D2") + DateTime.Now.Minute.ToString("D2") + DateTime.Now.Hour.ToString("D2") + DateTime.Now.Day.ToString("D2") + DateTime.Now.Month.ToString("D2") + ((int)DateTime.Now.DayOfWeek).ToString("D2") + DateTime.Now.Year.ToString().Substring(2, 2) + "GG");//时间同步协议:#记录仪编号+010A + 数据 + GG 
                    Thread.Sleep(1000);
                }
                catch
                {
                    MessageBox.Show("串口通讯发送失败", "时间同步", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                string backstring = PortForm.buckstr;
                PortForm.buckstr = "";
                if (backstring == "#" + LabRename.Text + "010AGG") //下位机返回:#记录仪编号+010AGG
                {
                    MessageBox.Show("时间同步成功", "时间同步", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("时间同步失败", "时间同步", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
                MessageBox.Show("串口未打开", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// “更改密码”按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangePw_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// “历史界面”选择框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolBoxTable_TextChanged(object sender, EventArgs e)
        {
            toolBoxCol.Items.Clear();
            if (toolBoxTable.Text == "温湿度信息")
            {
                toolBoxCol.Items.Add("时间");
                toolBoxCol.Items.Add("环温");
                toolBoxCol.Items.Add("环温1");
                toolBoxCol.Items.Add("环温2");
                toolBoxCol.Items.Add("环温3");
                toolBoxCol.Items.Add("环温4");
                toolBoxCol.Items.Add("环温5");
                toolBoxCol.Items.Add("露点");
                toolBoxCol.Items.Add("环湿");
                toolBoxCol.Text = toolBoxCol.Items[0].ToString();
            }

            else if (toolBoxTable.Text == "气体信息")
            {
                toolBoxCol.Items.Add("时间");
                toolBoxCol.Items.Add("C02");
                toolBoxCol.Items.Add("蒸发");
                toolBoxCol.Items.Add("气压");
                toolBoxCol.Text = toolBoxCol.Items[0].ToString();
            }

            else if (toolBoxTable.Text == "风信息")
            {
                toolBoxCol.Items.Add("时间");
                toolBoxCol.Items.Add("风向");
                toolBoxCol.Items.Add("风速");
                toolBoxCol.Items.Add("10分钟风速");
                toolBoxCol.Text = toolBoxCol.Items[0].ToString();
            }

            else if (toolBoxTable.Text == "辐射瞬时信息")
            {
                toolBoxCol.Items.Add("时间");
                toolBoxCol.Items.Add("总接辐射瞬时");
                toolBoxCol.Items.Add("散辐射瞬时");
                toolBoxCol.Items.Add("直接辐射瞬时");
                toolBoxCol.Items.Add("反辐射瞬时");
                toolBoxCol.Items.Add("净辐射瞬时");
                toolBoxCol.Items.Add("光合瞬时");
                toolBoxCol.Items.Add("紫外瞬时");
                toolBoxCol.Text = toolBoxCol.Items[0].ToString();
            }

            else if (toolBoxTable.Text == "辐射日累计信息")
            {
                toolBoxCol.Items.Add("时间");
                toolBoxCol.Items.Add("日照时日累计");
                toolBoxCol.Items.Add("总接辐射日累计");
                toolBoxCol.Items.Add("散辐射日累计");
                toolBoxCol.Items.Add("直接辐射日累计");
                toolBoxCol.Items.Add("反辐射日累计");
                toolBoxCol.Items.Add("净辐射日累计");
                toolBoxCol.Items.Add("光合日累计");
                toolBoxCol.Items.Add("紫外日累计");
                toolBoxCol.Text = toolBoxCol.Items[0].ToString();
            }

            else if (toolBoxTable.Text == "电量信息")
            {
                toolBoxCol.Items.Add("时间");
                toolBoxCol.Items.Add("光照度");
                toolBoxCol.Items.Add("电量");
                toolBoxCol.Text = toolBoxCol.Items[0].ToString();
            }
        }

        /// <summary>
        /// “历史界面”查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SqlSearch_Click(object sender, EventArgs e)
        {
            if (toolBoxTable.Text.Equals("请选择"))
            {
                MessageBox.Show("请选择查询内容", "查询", MessageBoxButtons.OK, MessageBoxIcon.Error);
                toolBoxTable.Focus();
                return;
            }

            if (SqlSearchBox.Text.Trim().Equals(""))
            {
                SqlSearchBox.Text = "*";
            }

            DataSet _dataset = new DataSet();

            if (SqlSearchBox.Text.Equals("*"))
            {
                if (toolBoxTable.Text.Equals("温湿度信息"))
                {
                    new SqlDataAdapter(new SqlCommand($"use Sorly SELECT * FROM [Tem]", sqlCon.con)).Fill(_dataset, "Sorly");
                }
                else if (toolBoxTable.Text.Equals("气体信息"))
                {
                    new SqlDataAdapter(new SqlCommand($"use Sorly SELECT * FROM [Air]", sqlCon.con)).Fill(_dataset, "Sorly");
                }
                else if (toolBoxTable.Text.Equals("风信息"))
                {
                    new SqlDataAdapter(new SqlCommand($"use Sorly SELECT * FROM [Wind]", sqlCon.con)).Fill(_dataset, "Sorly");
                }
                else if (toolBoxTable.Text.Equals("辐射瞬时信息"))
                {
                    new SqlDataAdapter(new SqlCommand($"use Sorly SELECT * FROM [Raido]", sqlCon.con)).Fill(_dataset, "Sorly");
                }
                else if (toolBoxTable.Text.Equals("辐射日累计信息"))
                {
                    new SqlDataAdapter(new SqlCommand($"use Sorly SELECT * FROM [Total]", sqlCon.con)).Fill(_dataset, "Sorly");
                }
                else if (toolBoxTable.Text.Equals("电量信息"))
                {
                    new SqlDataAdapter(new SqlCommand($"use Sorly SELECT * FROM [Power]", sqlCon.con)).Fill(_dataset, "Sorly");
                }
            }
            else
            {
                if (toolBoxTable.Text.Equals("温湿度信息"))
                {
                    new SqlDataAdapter(new SqlCommand($"use Sorly SELECT * FROM [Tem] WHERE [{toolBoxCol.Text}]like'%{SqlSearchBox.Text}%'", sqlCon.con)).Fill(_dataset, "Sorly");
                }
                else if (toolBoxTable.Text.Equals("气体信息"))
                {
                    new SqlDataAdapter(new SqlCommand($"use Sorly SELECT * FROM [Air] WHERE [{toolBoxCol.Text}]like'%{SqlSearchBox.Text}%'", sqlCon.con)).Fill(_dataset, "Sorly");
                }
                else if (toolBoxTable.Text.Equals("风信息"))
                {
                    new SqlDataAdapter(new SqlCommand($"use Sorly SELECT * FROM [Wind] WHERE [{toolBoxCol.Text}]like'%{SqlSearchBox.Text}%'", sqlCon.con)).Fill(_dataset, "Sorly");
                }
                else if (toolBoxTable.Text.Equals("辐射瞬时信息"))
                {
                    new SqlDataAdapter(new SqlCommand($"use Sorly SELECT * FROM [Raido] WHERE [{toolBoxCol.Text}]like'%{SqlSearchBox.Text}%'", sqlCon.con)).Fill(_dataset, "Sorly");
                }
                else if (toolBoxTable.Text.Equals("辐射日累计信息"))
                {
                    new SqlDataAdapter(new SqlCommand($"use Sorly SELECT * FROM [Total] WHERE [{toolBoxCol.Text}]like'%{SqlSearchBox.Text}%'", sqlCon.con)).Fill(_dataset, "Sorly");
                }
                else if (toolBoxTable.Text.Equals("电量信息"))
                {
                    new SqlDataAdapter(new SqlCommand($"use Sorly SELECT * FROM [Power] WHERE [{toolBoxCol.Text}]like'%{SqlSearchBox.Text}%'", sqlCon.con)).Fill(_dataset, "Sorly");
                }
            }
            dt = new DataTable();
            dt = _dataset.Tables["Sorly"];
            dataGridView1.DataSource = dt;
        }

        /// <summary>
        /// “历史界面”绘图按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sqlDraw_Click(object sender, EventArgs e)
        {
            if (dt == null)
            {
                MessageBox.Show("无数据", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(toolBoxTable.Text == "温湿度信息")
            {
                DrawLine();
            }
            else
            {
                MessageBox.Show("此信息不支持绘图", "绘图", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// “历史界面”导出按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExcelOut_Click(object sender, EventArgs e)
        {
            string fileName = "";
            string saveFileName = "";
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.DefaultExt = "xlsx";
            saveDialog.Filter = "Excel文件|*.xlsx";            
            saveDialog.ShowDialog();
            saveFileName = saveDialog.FileName;
            fileName = saveDialog.FileName;
            if (saveFileName.IndexOf(":") < 0) return; //被点了取消
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            if (xlApp == null)
            {
                MessageBox.Show("无法创建Excel对象，你设备未安装Excel");
                return;
            }
            Microsoft.Office.Interop.Excel.Workbooks workbooks = xlApp.Workbooks;
            Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
            Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];//取得sheet1

            //写入标题
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                worksheet.Cells[1, i + 1] = dataGridView1.Columns[i].HeaderText;
            }
            //写入数值
            for (int r = 0; r < dataGridView1.Rows.Count; r++)
            {
                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    worksheet.Cells[r + 2, i + 1] = dataGridView1.Rows[r].Cells[i].Value;
                }
                System.Windows.Forms.Application.DoEvents();
            }
            worksheet.Columns.EntireColumn.AutoFit();//列宽自适应
            if (saveFileName != "")
            {
                try
                {
                    workbook.Saved = true;
                    workbook.SaveCopyAs(saveFileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("导出文件时出错,文件可能正被打开！\n" + ex.Message);
                }
            }
            MessageBox.Show("文件已导出到" + fileName, "导出成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            xlApp.Quit();
            GC.Collect();//强行销毁           
        }

    }
}

