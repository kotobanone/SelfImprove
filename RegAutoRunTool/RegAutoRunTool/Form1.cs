using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;

namespace RegAutoRunTool
{
    public partial class SetAutoRunTool : Form
    {
        public SetAutoRunTool()
        {
            InitializeComponent();
        }
        string g_filePath = "";  //檔案完整名稱
        private void btn_Open_Click(object sender, EventArgs e)
        {
            l_Result.Text = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "exe files (*.exe)|*.exe|All files(*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (openFileDialog.OpenFile() != null)
                    {
                        tB_FilePath.Text = openFileDialog.FileName;
                        g_filePath = openFileDialog.FileName;
                    }
                }
                catch (Exception ex)
                {
                    l_Result.Text = ex.Message;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string runName = Path.GetFileNameWithoutExtension(g_filePath);
                RegistryKey hkml = Registry.LocalMachine;
                RegistryKey runKey = hkml.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                runKey.SetValue(runName, g_filePath);
                runKey.Close();
                l_Result.Text = "註冊開機自動啟動完成！";
            }
            catch (Exception ex)
            {
                l_Result.Text = ex.Message;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string runName = Path.GetFileNameWithoutExtension(g_filePath);
                RegistryKey hkml = Registry.LocalMachine;
                RegistryKey runKey = hkml.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                runKey.DeleteValue(runName);
                runKey.Close();
                l_Result.Text = "註銷開機自動啟動完成！";
            }
            catch (Exception ex)
            {
                l_Result.Text = ex.Message;
            }
        }
    }
}
