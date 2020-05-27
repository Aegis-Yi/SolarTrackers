using System;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace Solar
{

    public partial class PortForm : Form
    {
        static public SerialPort _serialPort;
        static public string port = "Not Connected！";
        static public string buckstr = "";
        static public bool flag = false;
        public PortForm()
        {
            _serialPort = new SerialPort();     
            InitializeComponent();
        }

        /// <summary>
        /// 初始化串口设置
        /// </summary>
        /// 
        private void MainForm_Load(object sender, EventArgs e)
        {
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false; //禁用了所有的控件合法性检查
            baudRateComboBox.Items.Add(4800); //波特率参数设置
            baudRateComboBox.Items.Add(9600);
            baudRateComboBox.Items.Add(19200);
            baudRateComboBox.Items.Add(38400);
            baudRateComboBox.Text = baudRateComboBox.Items[2].ToString(); //波特率默认值

            dataBitComboBox.Items.Add(5);  //数据位参数设置
            dataBitComboBox.Items.Add(6);
            dataBitComboBox.Items.Add(7);
            dataBitComboBox.Items.Add(8);
            dataBitComboBox.Text = dataBitComboBox.Items[3].ToString();  //数据位默认值

            stopBitComboBox.Items.Add(1);  //停止位参数设置
            stopBitComboBox.Items.Add(1.5);
            stopBitComboBox.Items.Add(2);
            stopBitComboBox.Text = stopBitComboBox.Items[0].ToString(); //停止位设置

            string[] ArrayComPortsName = SerialPort.GetPortNames(); //获取当前串口个数名称
            if (ArrayComPortsName.Length != 0)
            {
                Array.Sort(ArrayComPortsName);

                for (int i = 0; i < ArrayComPortsName.Length; i++)
                {
                    portComboBox.Items.Add(ArrayComPortsName[i]);
                }
                portComboBox.Text = ArrayComPortsName[0];

                if (portComboBox.Items.Count == 1)
                    serialPortStated.Text = portComboBox.Items[0].ToString() + " is Connected !";
                else
                    serialPortStated.Text = portComboBox.Items[portComboBox.SelectedIndex].ToString() + " is Connected !";

            }

            openButton.Enabled = true;  //“打开按钮”可用
            closeButton.Enabled = false; //“关闭按钮”不可用，没有打开，不能关闭；没有关闭，不能打开       
        }

        /// <summary>
        /// 显示COM状态栏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void portComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (portComboBox.Items.Count == 0)
                serialPortStated.Text = "Not Connected！";
            else
                serialPortStated.Text = portComboBox.Items[portComboBox.SelectedIndex].ToString() + " is Connected !";


        }       

        /// <summary>
        /// “打开” 按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openButton_Click(object sender, EventArgs e)
        {
            try
            {
                _serialPort.PortName = portComboBox.Text; //端口设置
                _serialPort.BaudRate = Convert.ToInt32(baudRateComboBox.Text); //波特率设置
                _serialPort.DataBits = Convert.ToInt16(dataBitComboBox.Text);  //数据位设置
                _serialPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), stopBitComboBox.Text); //停止位设置
                _serialPort.Parity = 0; //奇偶校检位
                _serialPort.Open();

                _serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);

                openButton.Enabled = false;  //打开按钮不可用
                closeButton.Enabled = true;
                groupBox1.Enabled = false;

                serialPortStated.Text = portComboBox.Text + " is Ok !";
                port = serialPortStated.Text;
                flag = true;
            }
            catch
            {
                MessageBox.Show("串口设置错误");
            }
        }

        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                //* Byte数组的个数是不确定的，根据实时返回的指令个数为准,而且返回的都是10进制数
                Byte[] dataBuff = new Byte[_serialPort.BytesToRead];
                _serialPort.Read(dataBuff, 0, dataBuff.Length);  //串口读取接收缓存区的数据
                buckstr += Encoding.Default.GetString(dataBuff).Trim();
            }
            catch (Exception ex)
            {
                MessageBox.Show("error:接收返回信息异常：" + ex.Message);
            }
        }

        /// <summary>
        /// “关闭”按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeButton_Click(object sender, EventArgs e)
        {
            _serialPort.Close();
            port = "Not Connected！";
            openButton.Enabled = true;
            closeButton.Enabled = false; //关闭按钮不可用

            groupBox1.Enabled = true;

            if (portComboBox.Items.Count == 0)
            {
                serialPortStated.Text = "Not Connected！";
            }
            else
                serialPortStated.Text = portComboBox.Items[portComboBox.SelectedIndex].ToString() + " is Connected !";
        }

        /// <summary>
        /// 添加双击托盘图标事件（双击显示窗口）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                WindowState = FormWindowState.Normal;//还原窗体显示
                this.Visible = true;
                this.ShowInTaskbar = true; //任务栏显示图标       
            }

        }

        /// <summary>
        /// 判断是否最小化，然后显示托盘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                MessageBox.Show("程序以最小到托盘", "端口设置");
                this.ShowInTaskbar = false;
                notifyIcon1.Visible = true;
            }
        }

        /// <summary>
        /// 确认是否退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("是否确认退出程序？\n退出程序将关闭端口", "端口设置", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (flag == true)
                    _serialPort.Close();
                port = "Not Connected！";
                this.Dispose();
                this.Close();
            }
            else
                e.Cancel = true;
        }

        /// <summary>
        /// 显示窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
        }

        /// <summary>
        /// 托盘右键推出程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (flag == true)
                _serialPort.Close();
            port = "Not Connected！";
            this.Dispose();
            this.Close();
        }
    }
}
