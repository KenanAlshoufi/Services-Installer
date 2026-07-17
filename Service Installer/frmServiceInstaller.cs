using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Service_Installer
{
    public partial class frmServicesIntaller : Form
    {
        public frmServicesIntaller()
        {
            InitializeComponent();
        }

       static string NameService = "";
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Brows1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();

            folder.Description = "Select Release Folder";

            if (folder.ShowDialog() == DialogResult.OK)
            {
                txtDebugorRelease.Text = folder.SelectedPath;
                gbTheDebug.Enabled = true;
            }
        }

        private void brows2_Click(object sender, EventArgs e)
        {

            ofdSelectImage.Filter = "Executable Files (*.exe)|*.exe";
           
            ofdSelectImage.FilterIndex = 1;
            ofdSelectImage.InitialDirectory = "C:\\Windows\\Microsoft.NET\\";
            ofdSelectImage.RestoreDirectory = true;

            if (ofdSelectImage.ShowDialog() == DialogResult.OK)
            {
               
                string selectedFilePath = ofdSelectImage.FileName;
               
                txtInsalUtil.Text = selectedFilePath;

            }
        }

        private void Brows3_Click(object sender, EventArgs e)
        {

            FolderBrowserDialog folder = new FolderBrowserDialog();

            folder.Description = "Select Release Folder";

            if (folder.ShowDialog() == DialogResult.OK)
            {
                txtDebugFolderWillAcopied.Text = folder.SelectedPath;
            }
        }


        public static void DirectoryCopy(string sourceDir, string destDir)
        {
            // إنشاء المجلد الهدف إذا لم يكن موجوداً
            DirectoryInfo dir = new DirectoryInfo(sourceDir);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory not found: " + sourceDir);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            Directory.CreateDirectory(destDir);


            // نسخ الملفات
            FileInfo[] files = dir.GetFiles();

            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDir, file.Name);

                if (file.Name.EndsWith(".exe"))
                {
                    NameService=file.Name;
                }
               

                file.CopyTo(tempPath, true);
            }


            // نسخ المجلدات الفرعية
            foreach (DirectoryInfo subdir in dirs)
            {
                string tempPath = Path.Combine(destDir, subdir.Name);

                DirectoryCopy(
                    subdir.FullName,
                    tempPath);
            }
        }
       

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (!PanelInstalltion.Visible)
            {
                PanelInstalltion.Visible = true;
                gbTheDebug.Enabled = false;
                btnBack.Enabled = true;
                txtInsalUtil.Text = "C:\\Windows\\Microsoft.NET\\Framework64\\v4.0.30319\\InstallUtil.exe";
                return;
            }

            if (!this.ValidateChildren())
            {
                return;
            }

            if (PanelInstalltion.Visible && btnNext.Text == "Next")
            {
                try
                {
                DirectoryCopy(txtDebugorRelease.Text, txtDebugFolderWillAcopied.Text);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }


                Process process =new Process();
                process.StartInfo.FileName =txtInsalUtil.Text;
                process.StartInfo.Arguments ="\"" + txtDebugFolderWillAcopied.Text + "\\"+ NameService + "\"";
                process.StartInfo.Verb = "runas";
                process.Start();
                process.WaitForExit();

              
                PanalFinish.Visible=true;

                btnNext.Text = "Finish";
            }
            else
            {
                btnBack.Enabled = false;

                this.Close();
            }
          
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (PanelInstalltion.Visible)
            {
                PanelInstalltion.Visible=false;

            }
        }

        private void frmServicesIntaller_Load(object sender, EventArgs e)
        {
            PanelInstalltion.Visible = false;
            btnBack.Enabled=false;
        }

        private void txtDebugFolderWillAcopied_Validating(object sender, CancelEventArgs e)
        {
           
            if (string.IsNullOrEmpty(txtDebugFolderWillAcopied.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtDebugFolderWillAcopied, "This Filed Is requir !");
            }
            else
            {
                errorProvider1.SetError(txtDebugFolderWillAcopied, null);
            }
        }

        private void txtInsalUtil_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtInsalUtil.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtInsalUtil, "This Filed Is requir !");
            }
            else
            {
                errorProvider1.SetError(txtInsalUtil, null);
            }
        }

        private void txtDebugorRelease_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtDebugorRelease.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtDebugorRelease, "This Filed Is requir !");
            }
            else
            {
                errorProvider1.SetError(txtDebugorRelease, null);
            }
        }
    }
}
