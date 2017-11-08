using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace PackOpeningTracker.View
{
    public partial class SaveMessageBox : Form
    {
        string saveFileName, textFileName;
        bool saved;
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                         int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public SaveMessageBox()
        {
            InitializeComponent();

            saveFileName = string.Empty;
            textFileName = string.Empty;

            txtSaveFileName.Text = DateTime.Now.ToString("yyyy-dd-MM--HH-mm-ss") + "_PackOpening";
            txtTextFileName.Text = DateTime.Now.ToString("yyyy-dd-MM--HH-mm-ss") + "_Statistics";

            rchInformation.AppendText("- A ");
            rchInformation.SelectionFont = new Font(rchInformation.Font, FontStyle.Bold);
            rchInformation.AppendText("'Save'");
            rchInformation.SelectionFont = new Font(rchInformation.Font, FontStyle.Regular);
            rchInformation.AppendText(" file is what Pack Tracker needs, to load old pack openings. It is recommended that you create such a file at the end of every pack opening if you want to keep an eye on your packs for the long term. You can find it in the 'Saves' directory.\r\n- A ");
            rchInformation.SelectionFont = new Font(rchInformation.Font, FontStyle.Bold);
            rchInformation.AppendText("'Text'");
            rchInformation.SelectionFont = new Font(rchInformation.Font, FontStyle.Regular);
            rchInformation.AppendText(" file is just a detailed text version of all the displayed statistics. Pack Tracker doesn't need this file and this should only be created if you want to dig in deeper regarding the statistics. It can be found in the 'Statistics' directory.");
            saved = false;
        }

        public string SaveFileName
        {
            get
            {
                return saveFileName;
            }
        }

        public string TextFileName
        {
            get
            {
                return textFileName;
            }
        }

        public bool CreateSave
        {
            get
            {
                if (checkSaveFile.Checked)
                    return true;
                else
                    return false;
            }
        }

        public bool CreateText
        {
            get
            {
                if (checkTextFile.Checked)
                    return true;
                else
                    return false;
            }
        }

        public bool Saved
        {
            get
            {
                return saved;
            }
        }
        private void check_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;

            switch (cb.Name)
            {
                case "checkSaveFile":
                    if (checkSaveFile.Checked)
                    {
                        lblSaveFileName.Enabled = true;
                        txtSaveFileName.Enabled = true;

                        if (!picBtnSave.Enabled)
                        {
                            picBtnSave.BackgroundImage = PackOpeningTracker.Properties.Resources.SaveNormal;
                            picBtnSave.Enabled = true;
                        }
                    }
                    else
                    {
                        lblSaveFileName.Enabled = false;
                        txtSaveFileName.Enabled = false;

                        if (!checkTextFile.Checked) 
                        {
                            picBtnSave.Enabled = false;
                            picBtnSave.BackgroundImage = PackOpeningTracker.Properties.Resources.SaveGray;
                        }
                    }
                    break;

                case "checkTextFile":

                    if (checkTextFile.Checked)
                    {
                        lblTextFileName.Enabled = true;
                        txtTextFileName.Enabled = true;

                        if (!picBtnSave.Enabled)
                        {
                            picBtnSave.BackgroundImage = PackOpeningTracker.Properties.Resources.SaveNormal;
                            picBtnSave.Enabled = true;
                        }
                    }
                    else
                    {
                        lblTextFileName.Enabled = false;
                        txtTextFileName.Enabled = false;

                        if (!checkSaveFile.Checked)
                        {
                            picBtnSave.Enabled = false;
                            picBtnSave.BackgroundImage = PackOpeningTracker.Properties.Resources.SaveGray;
                        }
                    }
                    break;
            }
        }

        private void txt_KeyPress(object sender, KeyPressEventArgs e)
        {
           if(System.IO.Path.GetInvalidFileNameChars().Contains(e.KeyChar) && e.KeyChar != '\b')
                e.Handled = true;
        }

        private void picBtnSave_Click(object sender, EventArgs e)
        {
            if (checkSaveFile.Checked && checkTextFile.Checked)
                if (validateFileName(txtSaveFileName.Text) && validateFileName(txtTextFileName.Text))
                {
                    saveFileName = txtSaveFileName.Text;
                    textFileName = txtTextFileName.Text;
                    saved = true;
                    this.Close();
                }
                else
                    MessageBox.Show("One or both filenames are invalid. Filenames can't be empty or contain any of the following characters:\r\n\\ / : * ? \" < > |", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                if (checkSaveFile.Checked && !checkTextFile.Checked)
                {
                    if (validateFileName(txtSaveFileName.Text))
                    {
                        saveFileName = txtSaveFileName.Text;
                        saved = true;
                        this.Close();
                    }
                    else
                        MessageBox.Show("Invalid filename. Filenames can't be empty or contain any of the following characters:\r\n\\ / : * ? \" < > |", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    if (!checkSaveFile.Checked && checkTextFile.Checked)
                    {
                        if (validateFileName(txtTextFileName.Text))
                        {
                            textFileName = txtTextFileName.Text;
                            saved = true;
                            this.Close();
                        }
                        else
                            MessageBox.Show("Invalid filename. Filenames can't be empty or contain any of the following characters:\r\n\\ / : * ? \" < > |", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
        }

        private void picBtnCancel_Click(object sender, EventArgs e)
        {
            saved = false;
            this.Close();
        }

        bool validateFileName(string fileName)
        {
            if (fileName == string.Empty)
                return false;

            bool valid = false;
            foreach (char c in fileName)
                if (c != ' ')
                {
                    valid = true;
                    break;
                }

            if (!valid)
                return false;
            else
                if (fileName.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) != -1)
                    return false;
                else
                    return true;
        }

        private void pic_MouseEnter(object sender, EventArgs e)
        {
            PictureBox _sender = (PictureBox)sender;
            switch (_sender.Name)
            {
                case "picBtnSave": picBtnSave.BackgroundImage = PackOpeningTracker.Properties.Resources.SaveHovered; break;
                case "picBtnCancel": picBtnCancel.BackgroundImage = PackOpeningTracker.Properties.Resources.CancelHovered; break;
            }
        }

        private void pic_MouseDown(object sender, MouseEventArgs e)
        {
            PictureBox _sender = (PictureBox)sender;
            switch (_sender.Name)
            {
                case "picBtnSave": picBtnSave.BackgroundImage = PackOpeningTracker.Properties.Resources.SavePressed; break;
                case "picBtnCancel": picBtnCancel.BackgroundImage = PackOpeningTracker.Properties.Resources.CancelPressed; break;
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
                case "picBtnSave": picBtnSave.BackgroundImage = PackOpeningTracker.Properties.Resources.SaveNormal; break;
                case "picBtnCancel": picBtnCancel.BackgroundImage = PackOpeningTracker.Properties.Resources.CancelNormal; break;
            }
        }
    }
}
