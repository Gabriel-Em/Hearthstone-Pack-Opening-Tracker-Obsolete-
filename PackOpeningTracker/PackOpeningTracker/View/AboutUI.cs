using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace PackOpeningTracker.View
{
    public partial class AboutUI : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                         int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        ToolTip tt;

        public AboutUI()
        {
            InitializeComponent();
            tt = new ToolTip();
        }

        private void pic_MouseEnter(object sender, EventArgs e)
        {
            PictureBox _sender = (PictureBox)sender;
            switch (_sender.Name)
            {
                case "picBtnExit": picBtnExit.BackgroundImage = PackOpeningTracker.Properties.Resources.ExitBtnHovered; break;
                case "picBtnMinimize": picBtnMinimize.BackgroundImage = PackOpeningTracker.Properties.Resources.MinimizeBtnHovered; break;
            }
        }

        private void pic_MouseDown(object sender, MouseEventArgs e)
        {
            PictureBox _sender = (PictureBox)sender;
            switch (_sender.Name)
            {
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
                case "picBtnExit": picBtnExit.BackgroundImage = PackOpeningTracker.Properties.Resources.ExitBtnNormal; break;
                case "picBtnMinimize": picBtnMinimize.BackgroundImage = PackOpeningTracker.Properties.Resources.MinimizeBtnNormal; break;
            }
        }

        private void picBtnDonate_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=LTX4XLKRBM6VC&lc=RO&item_name=Pack%20Tracker%20for%20Hearthstone&currency_code=EUR&bn=PP%2dDonationsBF%3abtn_donateCC_LG%2egif%3aNonHostedGuest");
        }

        private void picBtnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void picBtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pic_MouseHover(object sender, EventArgs e)
        {
            PictureBox _sender = (PictureBox)sender;
            switch (_sender.Name)
            {
                case "picBtnClipboardSkype": tt.SetToolTip(this.picBtnClipboardSkype, "Click to copy to clipboard"); break;
                case "picBtnClipboardEmail": tt.SetToolTip(this.picBtnClipboardEmail, "Click to copy to clipboard"); break;
            }
        }

        private void picBtnClipboardSkype_Click(object sender, EventArgs e)
        {
            Clipboard.SetText("gaby_em23");
        }

        private void picBtnClipboardEmail_Click(object sender, EventArgs e)
        {
            Clipboard.SetText("gaby_em23@yahoo.com");
        }
    }
}
