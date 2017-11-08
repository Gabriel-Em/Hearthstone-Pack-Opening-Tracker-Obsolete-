using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace PackOpeningTracker.View
{
    public partial class HelpUI : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                         int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public HelpUI()
        {
            InitializeComponent();
        }

        private void pic_MouseEnter(object sender, EventArgs e)
        {
            PictureBox _sender = (PictureBox)sender;
            switch (_sender.Name)
            {
                case "picBtnPreparation": picBtnPreparation.BackgroundImage = PackOpeningTracker.Properties.Resources.PreparationHovered; break;
                case "picBtnHowToUse": picBtnHowToUse.BackgroundImage = PackOpeningTracker.Properties.Resources.HowToUseHovered; break;
                case "picBtnTroubleshooting": picBtnTroubleshooting.BackgroundImage = PackOpeningTracker.Properties.Resources.TroubleshootingHovered; break;
                case "picBtnExit": picBtnExit.BackgroundImage = PackOpeningTracker.Properties.Resources.ExitBtnHovered; break;
                case "picBtnMinimize": picBtnMinimize.BackgroundImage = PackOpeningTracker.Properties.Resources.MinimizeBtnHovered; break;
                case "picBtnHere": picBtnHere.BackgroundImage = PackOpeningTracker.Properties.Resources.HereHovered; break;
            }
        }

        private void pic_MouseDown(object sender, MouseEventArgs e)
        {
            PictureBox _sender = (PictureBox)sender;
            switch (_sender.Name)
            {
                case "picBtnPreparation": picBtnPreparation.BackgroundImage = PackOpeningTracker.Properties.Resources.PreparationPressed; break;
                case "picBtnHowToUse": picBtnHowToUse.BackgroundImage = PackOpeningTracker.Properties.Resources.HowToUsePressed; break;
                case "picBtnTroubleshooting": picBtnTroubleshooting.BackgroundImage = PackOpeningTracker.Properties.Resources.TroubleshootingPressed; break;
                case "picBtnExit": picBtnExit.BackgroundImage = PackOpeningTracker.Properties.Resources.ExitBtnPressed; break;
                case "picBtnMinimize": picBtnMinimize.BackgroundImage = PackOpeningTracker.Properties.Resources.MinimizeBtnPressed; break;
                case "picTitleBar":
                    if (e.Button == MouseButtons.Left)
                    {
                        ReleaseCapture();
                        SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                    }
                    break;
                case "picBtnHere": picBtnHere.BackgroundImage = PackOpeningTracker.Properties.Resources.HerePressed; break;
            }
        }

        private void pic_MouseLeave(object sender, EventArgs e)
        {
            PictureBox _sender = (PictureBox)sender;
            switch (_sender.Name)
            {
                case "picBtnPreparation": picBtnPreparation.BackgroundImage = PackOpeningTracker.Properties.Resources.PreparationNormal; break;
                case "picBtnHowToUse": picBtnHowToUse.BackgroundImage = PackOpeningTracker.Properties.Resources.HowToUseNormal; break;
                case "picBtnTroubleshooting": picBtnTroubleshooting.BackgroundImage = PackOpeningTracker.Properties.Resources.TroubleshootingNormal; break;
                case "picBtnExit": picBtnExit.BackgroundImage = PackOpeningTracker.Properties.Resources.ExitBtnNormal; break;
                case "picBtnMinimize": picBtnMinimize.BackgroundImage = PackOpeningTracker.Properties.Resources.MinimizeBtnNormal; break;
                case "picBtnHere": picBtnHere.BackgroundImage = PackOpeningTracker.Properties.Resources.HereNormal; break;
            }
        }

        private void picBtnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void picBtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void picBtnPreparation_Click(object sender, EventArgs e)
        {
            if (picBtnHere.Visible)
                picBtnHere.Visible = false;

            picHelpText.BackgroundImage = PackOpeningTracker.Properties.Resources.HelpPreparation;
            picBtnPreparation.BackgroundImage = PackOpeningTracker.Properties.Resources.PreparationHovered;
        }

        private void picBtnHowToUse_Click(object sender, EventArgs e)
        {
            if (picBtnHere.Visible)
                picBtnHere.Visible = false;

            picHelpText.BackgroundImage = PackOpeningTracker.Properties.Resources.HelpHowtouse;
            picBtnHowToUse.BackgroundImage = PackOpeningTracker.Properties.Resources.HowToUseHovered;
        }

        private void picBtnTroubleshooting_Click(object sender, EventArgs e)
        {
            picHelpText.BackgroundImage = PackOpeningTracker.Properties.Resources.HelpTroubleshooting;

            if (!picBtnHere.Visible)
                picBtnHere.Visible = true;

            picBtnTroubleshooting.BackgroundImage = PackOpeningTracker.Properties.Resources.TroubleshootingHovered;
        }

        private void picBtnHere_Click(object sender, EventArgs e)
        {
            Clipboard.SetText("[Achievements]\r\nLogLevel=1\r\nFilePrinting=true\r\nConsolePrinting=true\r\nScreenPrinting=false");
            MessageBox.Show("Successfully copied to clipboard.", "Success", MessageBoxButtons.OK, MessageBoxIcon.None);
        }
    }
}
