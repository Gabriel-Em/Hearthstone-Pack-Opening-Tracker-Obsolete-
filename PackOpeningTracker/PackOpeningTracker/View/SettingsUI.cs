using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

namespace PackOpeningTracker
{
    public partial class SettingsUI : Form
    {
        private Controller ctrl;
        private const string logConfig = "[Achievements]\r\nLogLevel=1\r\nFilePrinting=true\r\nConsolePrinting=true\r\nScreenPrinting=false";
        private string configPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Blizzard\Hearthstone\log.config";

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                         int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public SettingsUI(Controller ctrl_)
        {
            InitializeComponent();

            ctrl = ctrl_;

            txtHSFolderPath.Text = ctrl.getHSPath();

            setStatus();
        }

        private void picBtnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Select Hearthstone's installation directory:";
            DialogResult result = fbd.ShowDialog(); // Show the dialog.

            if (result == DialogResult.OK) // Test result.
            {
                txtHSFolderPath.Text = fbd.SelectedPath;
                ctrl.setHSPath(fbd.SelectedPath);
                MessageBox.Show("Path selected successfully.", "Path selected", MessageBoxButtons.OK, MessageBoxIcon.None);
            }

            picBtnBrowse.BackgroundImage = PackOpeningTracker.Properties.Resources.BrowseNormal;
        }

        private void picExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void picBtnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void picBtnPatch_Click(object sender, EventArgs e)
        {
            int result = isPatched();
            try
            {
                if (result == 2)   // the file exists and it is not patched, therefore we apply the patch
                {
                    string fileContent = File.ReadAllText(configPath);
                    fileContent += "\r\n" + logConfig;
                    File.WriteAllText(configPath, fileContent);
                    setStatus();
                    MessageBox.Show("Patch applied succesfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.None);
                }
                else
                    if (result == 1) // the file is already patched
                        MessageBox.Show("Already patched!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else
                        if (result == 3) // the file does not exist, so we create it and patch it
                        {
                            File.WriteAllText(configPath, logConfig);
                            setStatus();
                            MessageBox.Show("Patch applied succesfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.None);
                        }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unhandled exception found. A log file containing the details of the exception has been saved to parent directory. If possible, send the log to the owner of this product using one of the means displayed in 'About'. Sorry for any inconveniences caused by this error.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StreamWriter file = new StreamWriter(DateTime.Now.ToString("yyyy-dd-MM--HH-mm-ss") + "_Crash_Log.txt");
                file.Write(ex.ToString());
                file.Close();
            }
        }

        private void setStatus()
        {
            if (isPatched() == 1)
                picStatus.BackgroundImage = PackOpeningTracker.Properties.Resources.Patched;
            else
                picStatus.BackgroundImage = PackOpeningTracker.Properties.Resources.NotPatched;
        }

        private int isPatched()
        {
            try
            {
                if (File.Exists(configPath))
                {
                    string file = File.ReadAllText(configPath);
                    string[] lines = file.Split('\n');
                    foreach (string line in lines)
                    {
                        if (line.Contains("[Achievements]"))
                            return 1;   // 1 = The file exists and it is patched
                    }
                    return 2;   // 2 = The file exists but it is not patched
                }
                else
                    return 3;   // 3 = The file does not exist
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unhandled exception found. A log file containing the details of the exception has been saved to parent directory. If possible, send the log to the owner of this product using one of the means displayed in 'About'. Sorry for any inconveniences caused by this error.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StreamWriter file = new StreamWriter(DateTime.Now.ToString("yyyy-dd-MM--HH-mm-ss") + "_Crash_Log.txt");
                file.Write(ex.ToString());
                file.Close();
                return -1;  // there was a problem trying to open the file
            }
        }

        // PICTUREBOX VISUAL EVENTS //

        private void pic_MouseEnter(object sender, EventArgs e)
        {
            PictureBox _sender = (PictureBox)sender;
            switch (_sender.Name)
            {
                case "picBtnBrowse": picBtnBrowse.BackgroundImage = PackOpeningTracker.Properties.Resources.BrowseHovered; break;
                case "picBtnPatch": picBtnPatch.BackgroundImage = PackOpeningTracker.Properties.Resources.PatchHovered; break;
                case "picBtnExit": picBtnExit.BackgroundImage = PackOpeningTracker.Properties.Resources.ExitBtnHovered; break;
                case "picBtnMinimize": picBtnMinimize.BackgroundImage = PackOpeningTracker.Properties.Resources.MinimizeBtnHovered; break;
            }
        }

        private void pic_MouseDown(object sender, MouseEventArgs e)
        {
            PictureBox _sender = (PictureBox)sender;
            switch (_sender.Name)
            {
                case "picBtnBrowse": picBtnBrowse.BackgroundImage = PackOpeningTracker.Properties.Resources.BrowsePressed; break;
                case "picBtnPatch": picBtnPatch.BackgroundImage = PackOpeningTracker.Properties.Resources.PatchPressed; break;
                case "picBtnExit": picBtnExit.BackgroundImage = PackOpeningTracker.Properties.Resources.ExitBtnPressed; break;
                case "picBtnMinimize": picBtnMinimize.BackgroundImage = PackOpeningTracker.Properties.Resources.MinimizeBtnPressed; break;
                case "picTitleBar":
                    if (e.Button == MouseButtons.Left)
                    {
                        ReleaseCapture();
                        SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                    }
                    break;
            }
        }

        private void pic_MouseLeave(object sender, EventArgs e)
        {
            PictureBox _sender = (PictureBox)sender;
            switch (_sender.Name)
            {
                case "picBtnBrowse": picBtnBrowse.BackgroundImage = PackOpeningTracker.Properties.Resources.BrowseNormal; break;
                case "picBtnPatch": picBtnPatch.BackgroundImage = PackOpeningTracker.Properties.Resources.PatchNormal; break;
                case "picBtnExit": picBtnExit.BackgroundImage = PackOpeningTracker.Properties.Resources.ExitBtnNormal; break;
                case "picBtnMinimize": picBtnMinimize.BackgroundImage = PackOpeningTracker.Properties.Resources.MinimizeBtnNormal; break;
            }
        }
    }
}
