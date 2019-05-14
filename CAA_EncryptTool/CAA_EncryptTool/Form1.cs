using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CAA_EncryptTool
{
    public partial class Form1 : Form
    {
        //Key
        byte[] b_Key = { 70, 49, 58, 50, 88, 51, 67, 52, 48, 53, 78, 54, 78, 55, 107, 56 };
        
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder EncryptStr= new StringBuilder();
            Encrypt_CAA.encrypt(textBox1.Text.ToCharArray(), EncryptStr);
            textBox2.Text = EncryptStr.ToString();
            //textBox2.Text = cshop_Aes.AESEncrypt(textBox1.Text, b_Key);//
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StringBuilder DecryptStr = new StringBuilder();
            string InputTextError = "Input Text Error!";
            try
            {
                Encrypt_CAA.decrypt(textBox1.Text.ToCharArray(), DecryptStr);
                textBox2.Text = DecryptStr.ToString();
            }
            catch (Exception ex)
            {
                textBox2.Text = InputTextError;
            }            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string DeStr = cshop_Aes.AESDecrypt(textBox1.Text, b_Key);
            textBox2.Text = DeStr;
        }
    }
}
