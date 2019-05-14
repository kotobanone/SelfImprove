using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace ComEditTool
{
    public partial class COMEditTool : Form
    {
        IniManager im = new IniManager("C:/windows/dll/sys.ini");
        
        public COMEditTool()
        {
            // 得到Port列表
            string[] ports = System.IO.Ports.SerialPort.GetPortNames();
            System.IO.Directory.CreateDirectory("C:/windows/dll");
            InitializeComponent();
            this.comboBox2.DataSource = (object[])ports;
            this.comboBox3.DataSource = (object[])ports;
            this.comboBox1.SelectedIndex = 0;
            this.comboBox4.SelectedIndex = 3;
            groupBox3.Hide();
           
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked)
            {
                this.groupBox3.Show();
            }
            else
            {
                this.groupBox3.Hide();
            }
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            //先刪除整個section
            im.WriteIniFile("ecard_dll", null, null);
            //新增資料
            im.WriteIniFile("ecard_dll","Count",this.comboBox1.Text);
            im.WriteIniFile("ecard_dll", "ComNum", this.comboBox2.Text);
            if (this.checkBox1.Checked)
            {
                im.WriteIniFile("ecard_dll", "LCom", this.comboBox1.Text);
            }
            im.WriteIniFile("ecard_dll", "LBaudrate", this.comboBox4.Text);
            this.label6.Text = "OK";
            this.label6.ForeColor = Color.Green;
        }
    }
}
