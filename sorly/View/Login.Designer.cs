using System.Windows.Forms;

namespace Solar
{
    partial class Login
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.cheSavePw = new System.Windows.Forms.CheckBox();
            this.butLogin = new System.Windows.Forms.Button();
            this.labUserName = new System.Windows.Forms.Label();
            this.labUserPw = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.txtUserPw = new System.Windows.Forms.TextBox();
            this.butCancle = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cheSavePw
            // 
            this.cheSavePw.AutoSize = true;
            this.cheSavePw.Location = new System.Drawing.Point(91, 144);
            this.cheSavePw.Name = "cheSavePw";
            this.cheSavePw.Size = new System.Drawing.Size(72, 16);
            this.cheSavePw.TabIndex = 6;
            this.cheSavePw.Text = "记住密码";
            this.cheSavePw.UseVisualStyleBackColor = false;
            this.cheSavePw.CheckedChanged += new System.EventHandler(this.cheSavePW_CheckedChanged);
            // 
            // butLogin
            // 
            this.butLogin.Location = new System.Drawing.Point(57, 183);
            this.butLogin.Name = "butLogin";
            this.butLogin.Size = new System.Drawing.Size(80, 36);
            this.butLogin.TabIndex = 4;
            this.butLogin.Text = "登录";
            this.butLogin.UseVisualStyleBackColor = true;
            this.butLogin.Click += new System.EventHandler(this.butLogin_Click);
            // 
            // labUserName
            // 
            this.labUserName.AutoSize = true;
            this.labUserName.Location = new System.Drawing.Point(42, 81);
            this.labUserName.Name = "labUserName";
            this.labUserName.Size = new System.Drawing.Size(41, 12);
            this.labUserName.TabIndex = 2;
            this.labUserName.Text = "账号：";
            // 
            // labUserPw
            // 
            this.labUserPw.AutoSize = true;
            this.labUserPw.Location = new System.Drawing.Point(42, 120);
            this.labUserPw.Name = "labUserPw";
            this.labUserPw.Size = new System.Drawing.Size(41, 12);
            this.labUserPw.TabIndex = 3;
            this.labUserPw.Text = "密码：";
            // 
            // txtUserName
            // 
            this.txtUserName.AutoCompleteCustomSource.AddRange(new string[] {
            "sa"});
            this.txtUserName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.txtUserName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtUserName.Location = new System.Drawing.Point(91, 78);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(188, 21);
            this.txtUserName.TabIndex = 0;
            // 
            // txtUserPw
            // 
            this.txtUserPw.Location = new System.Drawing.Point(91, 117);
            this.txtUserPw.Name = "txtUserPw";
            this.txtUserPw.Size = new System.Drawing.Size(188, 21);
            this.txtUserPw.TabIndex = 1;
            this.txtUserPw.UseSystemPasswordChar = true;
            // 
            // butCancle
            // 
            this.butCancle.Location = new System.Drawing.Point(186, 183);
            this.butCancle.Name = "butCancle";
            this.butCancle.Size = new System.Drawing.Size(80, 36);
            this.butCancle.TabIndex = 7;
            this.butCancle.Text = "取消";
            this.butCancle.UseVisualStyleBackColor = true;
            this.butCancle.Click += new System.EventHandler(this.butCancle_Click);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(27, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(274, 26);
            this.label4.TabIndex = 10;
            this.label4.Text = "太阳能追踪器数据库登录认证";
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(320, 243);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.butCancle);
            this.Controls.Add(this.cheSavePw);
            this.Controls.Add(this.txtUserPw);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.butLogin);
            this.Controls.Add(this.labUserPw);
            this.Controls.Add(this.labUserName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "登录";
            this.Load += new System.EventHandler(this.Login_Load);
            this.Shown += new System.EventHandler(this.Login_Focus);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.TextBox txtUserPw;
        private System.Windows.Forms.Label labUserName;
        private System.Windows.Forms.Label labUserPw;
        private System.Windows.Forms.Button butLogin;
        private System.Windows.Forms.CheckBox cheSavePw;
        private Button butCancle;
        private Label label4;
    }
}

