using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Windows.Media;
using System.Threading;
using System.Runtime.InteropServices;

namespace PackOpeningTracker
{
    public partial class UI : Form
    {
        private Controller ctrl;
        private int tracking;
        private int packType;

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                         int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public UI()
        {
            InitializeComponent();
            CenterToScreen();

            ctrl = new Controller(this);
            tracking = 2;
            this.DoubleBuffered = true;                     // less flickering
            this.SetStyle(ControlStyles.UserPaint |
              ControlStyles.AllPaintingInWmPaint |
              ControlStyles.ResizeRedraw |
              ControlStyles.ContainerControl |
              ControlStyles.OptimizedDoubleBuffer |
              ControlStyles.SupportsTransparentBackColor
              , true);

            packType = -1;
        }

        // METHODS //

        private void displayStats()
        {
            List<Card> cardsFromPacks = ctrl.getCardsFromPacks();
            List<int> stats = ctrl.getStats();

            lblSCommon.Text = "Commons: " + stats[3].ToString();
            lblSRare.Text = "Rares: " + stats[2].ToString();
            lblSEpic.Text = "Epics: " + stats[1].ToString();
            lblSLegendaries.Text = "Legendaries: " + stats[0].ToString();
            lblSTotal.Text = "Total: " + stats[4].ToString();

            lblGCommon.Text = "Commons: " + stats[8].ToString();
            lblGRare.Text = "Rares: " + stats[7].ToString();
            lblGEpic.Text = "Epics: " + stats[6].ToString();
            lblGLegendaries.Text = "Legendaries: " + stats[5].ToString();
            lblGTotal.Text = "Total: " + stats[9].ToString();

            lblECommons.Text = "Commons: " + stats[13].ToString(); ;
            lblERares.Text = "Rares: " + stats[12].ToString();
            lblEEpics.Text = "Epics: " + stats[11].ToString();
            lblELegendaries.Text = "Legendaries: " + stats[10].ToString();
            lblETotal.Text = "Total: " + stats[14].ToString();

            lblACommons.Text = "Commons: " + stats[18].ToString();
            lblARares.Text = "Rares: " + stats[17].ToString();
            lblAEpics.Text = "Epics: " + stats[16].ToString();
            lblALegendaries.Text = "Legendaries: " + stats[15].ToString();
            lblATotal.Text = "Total: " + stats[19].ToString();

            if (stats[19] != 0)
            {
                lblPCommons.Text = "Commons: " + ((float)((stats[18] * 100) / stats[19])).ToString("0.00") + "%";
                lblPRares.Text = "Rares: " + ((float)(stats[17] * 100) / stats[19]).ToString("0.00") + "%";
                lblPEpics.Text = "Epics: " + ((float)(stats[16] * 100) / stats[19]).ToString("0.00") + "%";
                lblPLegendaries.Text = "Legendaries: " + ((float)(stats[15] * 100) / stats[19]).ToString("0.00") + "%";
            }
            else
            {
                lblPCommons.Text = "Commons: 0%";
                lblPRares.Text = "Rares: 0%";
                lblPEpics.Text = "Epics: 0%";
                lblPLegendaries.Text = "Legendaries: 0%";
            }

            lblAvgDustPerPack.Text = "Average dust per pack: " + ctrl.getAvgDustPerPack().ToString("0.00");

            lblTotalDustW.Text = "Total dust worth: " + stats[20].ToString();
            lblHighestDustInPack.Text = "Highest dust value in a pack: " + stats[21].ToString();
            lblNoOf40DustPacks.Text = "Number of 40 dust packs: " + stats[22].ToString();
            lblTotalPacks.Text = "Total packs: " + stats[23];
            lblExtraDustCW.Text = "Extra cards dust worth: " + stats[24].ToString();

            List<Card> bestPackm = ctrl.getBestPack();
            if (bestPackm.Count() > 0)
                lblBestPack.Text = bestPackm[0].cardName + " [" + bestPackm[0].cardRarity + ", " + bestPackm[0].cardPremium + "]\n"
                    + bestPackm[1].cardName + " [" + bestPackm[1].cardRarity + ", " + bestPackm[1].cardPremium + "]\n"
                    + bestPackm[2].cardName + " [" + bestPackm[2].cardRarity + ", " + bestPackm[2].cardPremium + "]\n"
                    + bestPackm[3].cardName + " [" + bestPackm[3].cardRarity + ", " + bestPackm[3].cardPremium + "]\n"
                    + bestPackm[4].cardName + " [" + bestPackm[4].cardRarity + ", " + bestPackm[4].cardPremium + "]\n";
            else
                lblBestPack.Text = "-\n-\n-\n-\n-";
        }

        private void fillStatDataGrid()
        {
            int FDSRI = StatsDataGrindView.FirstDisplayedScrollingRowIndex;
            StatsDataGrindView.Rows.Clear();
            int no = 0;

            if (radAllCards.Checked)
            {
                foreach (Card card in ctrl.getCardsFromPacks())
                {
                    if (isClassValid(card.cardClass) && isTypeValid(card.cardPremium) && isRarityValid(card.cardRarity) && isSetValid(card.cardSet) && isCountValid(card.cardCount, card.cardRarity))
                    {
                        StatsDataGrindView.Rows.Add();
                        StatsDataGrindView.Rows[no].Cells[0].Value = no + 1; ;
                        StatsDataGrindView.Rows[no].Cells[1].Value = card.cardName;
                        StatsDataGrindView.Rows[no].Cells[2].Value = card.cardClass;
                        StatsDataGrindView.Rows[no].Cells[3].Value = card.cardRarity;
                        StatsDataGrindView.Rows[no].Cells[4].Value = card.cardSet;
                        StatsDataGrindView.Rows[no].Cells[5].Value = card.cardPremium;
                        StatsDataGrindView.Rows[no].Cells[6].Value = card.cardCount;

                        if (radExtraCardsAny.Checked)
                            StatsDataGrindView.Rows[no].Cells[6].Value = card.cardCount;
                        else
                        {
                            if (card.cardRarity == "Legendary")
                                StatsDataGrindView.Rows[no].Cells[6].Value = card.cardCount - 1;
                            else
                                StatsDataGrindView.Rows[no].Cells[6].Value = card.cardCount - 2;
                        }
                        no++;
                    }
                }
            }
            else
            {
                int initialPos = -23;

                switch (packType)
                {
                    case 1: if (cmbClassicPacks.SelectedIndex >= 0) initialPos = ctrl.getPacksPosition("Classic")[cmbClassicPacks.SelectedIndex]; break;
                    case 2: if (cmbGVGPacks.SelectedIndex >= 0) initialPos = ctrl.getPacksPosition("GVG")[cmbGVGPacks.SelectedIndex]; break;
                    case 3: if (cmbTGTPacks.SelectedIndex >= 0) initialPos = ctrl.getPacksPosition("TGT")[cmbTGTPacks.SelectedIndex]; break;
                }

                if (initialPos != -23)
                {
                    List<Card> cards = ctrl.getCardsFromPacksInOrder();
                    for (int i = initialPos; i < initialPos + 5; i++)
                    {
                        if (i < cards.Count())
                        {
                            StatsDataGrindView.Rows.Add();
                            StatsDataGrindView.Rows[no].Cells[0].Value = no + 1;
                            StatsDataGrindView.Rows[no].Cells[1].Value = cards[i].cardName;
                            StatsDataGrindView.Rows[no].Cells[2].Value = cards[i].cardClass;
                            StatsDataGrindView.Rows[no].Cells[3].Value = cards[i].cardRarity;
                            StatsDataGrindView.Rows[no].Cells[4].Value = cards[i].cardSet;
                            StatsDataGrindView.Rows[no].Cells[5].Value = cards[i].cardPremium;
                            StatsDataGrindView.Rows[no].Cells[6].Value = cards[i].cardCount;
                            no++;
                        }
                        else
                            break;
                    }
                }
            }

            if (FDSRI != -1 && FDSRI < StatsDataGrindView.Rows.Count)
                StatsDataGrindView.FirstDisplayedScrollingRowIndex = FDSRI;
        }

        private void fillComboBoxes()
        {
            int index = -1;

            switch (packType)
            {
                case 1: index = cmbClassicPacks.SelectedIndex; break;
                case 2: index = cmbGVGPacks.SelectedIndex; break;
                case 3: index = cmbTGTPacks.SelectedIndex; break;
            }

            cmbClassicPacks.Items.Clear();
            cmbGVGPacks.Items.Clear();
            cmbTGTPacks.Items.Clear();

            for (int i = 1; i <= ctrl.getPacksPosition("Classic").Count(); i++)
                cmbClassicPacks.Items.Add("Pack " + i);
            for (int i = 1; i <= ctrl.getPacksPosition("GVG").Count(); i++)
                cmbGVGPacks.Items.Add("Pack " + i);
            for (int i = 1; i <= ctrl.getPacksPosition("TGT").Count(); i++)
                cmbTGTPacks.Items.Add("Pack " + i);

            switch (packType)
            {
                case 1: if(index<cmbClassicPacks.Items.Count) cmbClassicPacks.SelectedIndex = index; break;
                case 2: if (index < cmbGVGPacks.Items.Count) cmbGVGPacks.SelectedIndex = index; break;
                case 3: if (index < cmbTGTPacks.Items.Count) cmbTGTPacks.SelectedIndex = index; break;
            }
        }

        // VALIDATIONS for GridView //

        private bool isCountValid(int cardCount, string cardRarity)
        {
            if (radExtraCardsAny.Checked)
                return true;
            else
            {
                if (cardRarity == "Legendary")
                    if (cardCount > 1)
                        return true;
                    else
                        return false;
                else
                    if (cardCount > 2)
                        return true;
                    else
                        return false;
            }
        }
        private bool isClassValid(string cardClass)
        {
            if (radClassAny.Checked)
                return true;
            else
                switch (cardClass)
                {
                    case "Druid":
                        if (radClassDruid.Checked)
                            return true;
                        else
                            return false;
                    case "Hunter":
                        if (radClassHunter.Checked)
                            return true;
                        else
                            return false;
                    case "Mage":
                        if (radClassMage.Checked)
                            return true;
                        else
                            return false;
                    case "Paladin":
                        if (radClassPaladin.Checked)
                            return true;
                        else
                            return false;
                    case "Priest":
                        if (radClassPriest.Checked)
                            return true;
                        else
                            return false;
                    case "Rogue":
                        if (radClassRogue.Checked)
                            return true;
                        else
                            return false;
                    case "Shaman":
                        if (radClassShaman.Checked)
                            return true;
                        else
                            return false;
                    case "Warlock":
                        if (radClassWarlock.Checked)
                            return true;
                        else
                            return false;
                    case "Warrior":
                        if (radClassWarrior.Checked)
                            return true;
                        else
                            return false;
                    case "Neutral":
                        if (radClassNeutral.Checked)
                            return true;
                        else
                            return false;
                }
            return true;
        }

        private bool isTypeValid(string cardType)
        {
            if (radTypeAny.Checked)
                return true;
            else
                switch (cardType)
                {
                    case "STANDARD":
                        if (radTypeStandard.Checked)
                            return true;
                        else
                            return false;
                    case "GOLDEN":
                        if (radTypeGolden.Checked)
                            return true;
                        else
                            return false;
                }
            return true;
        }

        private bool isRarityValid(string cardRarity)
        {
            if (radRarityAny.Checked)
                return true;
            else
                switch (cardRarity)
                {
                    case "Common":
                        if (radRarityCommon.Checked)
                            return true;
                        else
                            return false;
                    case "Rare":
                        if (radRarityRare.Checked)
                            return true;
                        else
                            return false;
                    case "Epic":
                        if (radRarityEpic.Checked)
                            return true;
                        else
                            return false;
                    case "Legendary":
                        if (radRarityLegendary.Checked)
                            return true;
                        else
                            return false;
                }
            return true;
        }

        private bool isSetValid(string cardSet)
        {
            if (radSetAny.Checked)
                return true;
            else
                switch (cardSet)
                {
                    case "Classic":
                        if (radSetClassic.Checked)
                            return true;
                        else
                            return false;
                    case "GVG":
                        if (radSetGVG.Checked)
                            return true;
                        else
                            return false;
                    case "TGT":
                        if (radSetTGT.Checked)
                            return true;
                        else
                            return false;
                }
            return true;
        }

        private bool isAchievementsActive(string path)
        {
            path = path + @"\Logs\Achievements.log";
            if (File.Exists(path))
                return true;
            else
                return false;
        }

        // MENU PICTUREBOX (AS BUTTONS) EVENTS //

        private void picBtnLoadFromFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Filter = "Save Files (*.sav)|*.sav";
            OFD.InitialDirectory = Directory.GetCurrentDirectory() + @"\Saves\";
            OFD.Multiselect = true;
            OFD.Title = "Load from file";
            DialogResult result = OFD.ShowDialog();

            while (result == DialogResult.OK)
            {
                bool Valid = true;
                foreach (string file in OFD.FileNames)
                {
                    if (Path.GetExtension(file) != ".sav")
                    {
                        Valid = false;
                        break;
                    }
                }
                if (!Valid)
                {
                    MessageBox.Show("One or more of the selected files is invalid!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    result = OFD.ShowDialog();
                }
                else
                    break;
            }

            if (result == DialogResult.OK)
            {
                ctrl.collectData(false, OFD.FileNames.ToList<string>());
                displayStats();
                fillComboBoxes();
                fillStatDataGrid();
                MessageBox.Show("Statistics loaded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
        }

        private void picBtnStart_Click(object sender, EventArgs e)
        {
            if (ctrl.getPatchState())
            {
                if (isAchievementsActive(ctrl.getHSPath()))
                {
                    picStatus.BackgroundImage = PackOpeningTracker.Properties.Resources.Tracking1;
                    timerTrack.Enabled = true;
                    picBtnStop.BackgroundImage = PackOpeningTracker.Properties.Resources.StopHovered;
                    picBtnStart.Visible = false;
                    picBtnStop.Visible = true;
                    picBtnSettings.Enabled = false;
                    picBtnSaveToFile.Enabled = false;
                    picBtnLoadFromFile.Enabled = false;
                    picBtnSettings.BackgroundImage = PackOpeningTracker.Properties.Resources.SettingsGray;
                    picBtnSaveToFile.BackgroundImage = PackOpeningTracker.Properties.Resources.SaveToFileGray;
                    picBtnLoadFromFile.BackgroundImage = PackOpeningTracker.Properties.Resources.LoadFromFileGray;
                    picDataGridViewMessage.Visible = true;
                }
                else
                    MessageBox.Show("'Achievements.log' was not found! Make sure you've set the right Hearthstone directory path [in Settings], and that Hearthstone is running.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                MessageBox.Show("'Config.log' is not patched! Go to settings and make sure 'Config.log status' is 'Patched'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void picBtnSettings_Click(object sender, EventArgs e)
        {
            SettingsUI SUI = new SettingsUI(ctrl);
            SUI.ShowDialog();
        }

        private void picBtnSaveToFile_Click(object sender, EventArgs e)
        {
            if (ctrl.getStats()[19] != 0)
            {
                View.SaveMessageBox SMB = new View.SaveMessageBox();
                SMB.ShowDialog();

                bool saved = SMB.Saved;

                if (saved)
                {
                    if (SMB.CreateSave)
                    {
                        ctrl.saveToFile(SMB.SaveFileName);
                    }

                    if (SMB.CreateText)
                    {
                        ctrl.writeToFile(SMB.TextFileName);
                    }

                    MessageBox.Show("Save successfull!", "Success", MessageBoxButtons.OK, MessageBoxIcon.None);
                }
            }
            else
                MessageBox.Show("There are no statistics to be saved!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void picBtnNext_Click(object sender, EventArgs e)
        {
            panelDStats.Visible = true;
            panelSStats.Visible = false;
            picBtnPrev.BackgroundImage = PackOpeningTracker.Properties.Resources.LeftHovered;
            picBtnNext.Visible = false;
            picBtnPrev.Visible = true;
        }

        private void picBtnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void picBtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void picBtnStop_Click(object sender, EventArgs e)
        {
            timerTrack.Enabled = false;
            picStatus.BackgroundImage = PackOpeningTracker.Properties.Resources.Stopped;
            picBtnStart.BackgroundImage = PackOpeningTracker.Properties.Resources.StartHovered;
            picBtnStop.Visible = false;
            picBtnStart.Visible = true;
            tracking = 2;
            picBtnSettings.Enabled = true;
            picBtnSaveToFile.Enabled = true;
            picBtnLoadFromFile.Enabled = true;
            picBtnSettings.BackgroundImage = PackOpeningTracker.Properties.Resources.SettingsNormal;
            picBtnSaveToFile.BackgroundImage = PackOpeningTracker.Properties.Resources.SaveToFileNormal;
            picBtnLoadFromFile.BackgroundImage = PackOpeningTracker.Properties.Resources.LoadFromFileNormal;
            picDataGridViewMessage.Visible = false;
            fillComboBoxes();
            fillStatDataGrid();
        }

        private void picBtnPrev_Click(object sender, EventArgs e)
        {
            panelSStats.Visible = true;
            panelDStats.Visible = false;
            picBtnNext.BackgroundImage = PackOpeningTracker.Properties.Resources.RightHovered;
            picBtnPrev.Visible = false;
            picBtnNext.Visible = true;
        }

        private void picBtnHelp_Click(object sender, EventArgs e)
        {
            if ((Application.OpenForms["HelpUI"] as View.HelpUI) == null)
            {
                View.HelpUI helpUI = new View.HelpUI();
                helpUI.Show();
            }
            else
            {
                if (Application.OpenForms["HelpUI"].WindowState != FormWindowState.Normal)
                    Application.OpenForms["HelpUi"].WindowState = FormWindowState.Normal;

                Application.OpenForms["HelpUI"].Focus();
            }
            picBtnHelp.BackgroundImage = PackOpeningTracker.Properties.Resources.HelpHovered;
        }

        private void picBtnAbout_Click(object sender, EventArgs e)
        {
            if ((Application.OpenForms["AboutUI"] as View.AboutUI) == null)
            {
                View.AboutUI aboutUI = new View.AboutUI();
                aboutUI.Show();
            }
            else
            {
                if (Application.OpenForms["AboutUI"].WindowState != FormWindowState.Normal)
                    Application.OpenForms["AboutUI"].WindowState = FormWindowState.Normal;

                Application.OpenForms["AboutUI"].Focus();
            }
            picBtnAbout.BackgroundImage = PackOpeningTracker.Properties.Resources.AboutHovered;
        }

        // RADIO BUTTON AND COMBOBOX EVENTS //

        private void radViewType_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;

            if (rb.Checked)
                if (radAllCards.Checked)
                {
                    fillStatDataGrid();
                    panelIndividualPacks.Enabled = false;
                    grpFilterBy.Enabled = true;
                }
                else
                {
                    fillStatDataGrid();
                    grpFilterBy.Enabled = false;
                    panelIndividualPacks.Enabled = true;
                }
        }
    
        private void radFilter_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;

            if (rb.Checked)
                fillStatDataGrid();
        }

        private void cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;

            if (cb.Focused)
            {
                switch (cb.Name)
                {
                    case "cmbClassicPacks": packType = 1; cmbGVGPacks.SelectedIndex = -1; cmbTGTPacks.SelectedIndex = -1; break;
                    case "cmbGVGPacks": packType = 2; cmbClassicPacks.SelectedIndex = -1; cmbTGTPacks.SelectedIndex = -1; break;
                    case "cmbTGTPacks": packType = 3; cmbClassicPacks.SelectedIndex = -1; cmbGVGPacks.SelectedIndex = -1; break;
                }
                fillStatDataGrid();
            }
        }

        // PICTUREBOX VISUAL EFFECTS - EVENTS //

        private void pic_MouseEnter(object sender, EventArgs e)
        {
            PictureBox _sender = (PictureBox)sender;
            switch (_sender.Name)
            {
                case "picBtnBrowse": picBtnStart.BackgroundImage = PackOpeningTracker.Properties.Resources.BrowseHovered; break;
                case "picBtnExit": picBtnExit.BackgroundImage = PackOpeningTracker.Properties.Resources.ExitBtnHovered; break;
                case "picBtnStart": picBtnStart.BackgroundImage = PackOpeningTracker.Properties.Resources.StartHovered; break;
                case "picBtnStop": picBtnStop.BackgroundImage = PackOpeningTracker.Properties.Resources.StopHovered; break;
                case "picBtnSettings": picBtnSettings.BackgroundImage = PackOpeningTracker.Properties.Resources.SettingsHovered; break;
                case "picBtnSaveToFile": picBtnSaveToFile.BackgroundImage = PackOpeningTracker.Properties.Resources.SaveToFileHovered; break;
                case "picBtnLoadFromFile": picBtnLoadFromFile.BackgroundImage = PackOpeningTracker.Properties.Resources.LoadFromFileHovered; break;
                case "picBtnNext": picBtnNext.BackgroundImage = PackOpeningTracker.Properties.Resources.RightHovered; break;
                case "picBtnPrev": picBtnPrev.BackgroundImage = PackOpeningTracker.Properties.Resources.LeftHovered; break;
                case "picBtnMinimize": picBtnMinimize.BackgroundImage = PackOpeningTracker.Properties.Resources.MinimizeBtnHovered; break;
                case "picBtnAbout": picBtnAbout.BackgroundImage = PackOpeningTracker.Properties.Resources.AboutHovered; break;
                case "picBtnHelp": picBtnHelp.BackgroundImage = PackOpeningTracker.Properties.Resources.HelpHovered; break;
            }
        }

        private void pic_MouseDown(object sender, MouseEventArgs e)
        {
            PictureBox _sender = (PictureBox)sender;
            switch (_sender.Name)
            {
                case "picBtnBrowse": picBtnStart.BackgroundImage = PackOpeningTracker.Properties.Resources.BrowsePressed; break;
                case "picBtnExit": picBtnExit.BackgroundImage = PackOpeningTracker.Properties.Resources.ExitBtnPressed; break;
                case "picBtnStart": picBtnStart.BackgroundImage = PackOpeningTracker.Properties.Resources.StartPressed; break;
                case "picBtnStop": picBtnStop.BackgroundImage = PackOpeningTracker.Properties.Resources.StopPressed; break;
                case "picBtnSettings": picBtnSettings.BackgroundImage = PackOpeningTracker.Properties.Resources.SettingsPressed; break;
                case "picBtnSaveToFile": picBtnSaveToFile.BackgroundImage = PackOpeningTracker.Properties.Resources.SaveToFilePressed; break;
                case "picBtnLoadFromFile": picBtnLoadFromFile.BackgroundImage = PackOpeningTracker.Properties.Resources.LoadFromFilePressed; break;
                case "picBtnNext": picBtnNext.BackgroundImage = PackOpeningTracker.Properties.Resources.RightPressed; break;
                case "picBtnPrev": picBtnPrev.BackgroundImage = PackOpeningTracker.Properties.Resources.LeftPressed; break;
                case "picBtnMinimize": picBtnMinimize.BackgroundImage = PackOpeningTracker.Properties.Resources.MinimizeBtnPressed; break;
                case "picTitleBar":
                    if (e.Button == MouseButtons.Left)
                    {
                        ReleaseCapture();
                        SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                    }
                    break;
                case "picBtnAbout": picBtnAbout.BackgroundImage = PackOpeningTracker.Properties.Resources.AboutPressed; break;
                case "picBtnHelp": picBtnHelp.BackgroundImage = PackOpeningTracker.Properties.Resources.HelpPressed; break;
            }
        }

        private void pic_MouseLeave(object sender, EventArgs e)
        {
            PictureBox _sender = (PictureBox)sender;
            switch (_sender.Name)
            {
                case "picBtnBrowse": picBtnStart.BackgroundImage = PackOpeningTracker.Properties.Resources.BrowseNormal; break;
                case "picBtnExit": picBtnExit.BackgroundImage = PackOpeningTracker.Properties.Resources.ExitBtnNormal; break;
                case "picBtnStart": picBtnStart.BackgroundImage = PackOpeningTracker.Properties.Resources.StartNormal; break;
                case "picBtnStop": picBtnStop.BackgroundImage = PackOpeningTracker.Properties.Resources.StopNormal; break;
                case "picBtnSettings": picBtnSettings.BackgroundImage = PackOpeningTracker.Properties.Resources.SettingsNormal; break;
                case "picBtnSaveToFile": picBtnSaveToFile.BackgroundImage = PackOpeningTracker.Properties.Resources.SaveToFileNormal; break;
                case "picBtnLoadFromFile": picBtnLoadFromFile.BackgroundImage = PackOpeningTracker.Properties.Resources.LoadFromFileNormal; break;
                case "picBtnNext": picBtnNext.BackgroundImage = PackOpeningTracker.Properties.Resources.RightNormal; break;
                case "picBtnPrev": picBtnPrev.BackgroundImage = PackOpeningTracker.Properties.Resources.LeftNormal; break;
                case "picBtnMinimize": picBtnMinimize.BackgroundImage = PackOpeningTracker.Properties.Resources.MinimizeBtnNormal; break;
                case "picBtnAbout": picBtnAbout.BackgroundImage = PackOpeningTracker.Properties.Resources.AboutNormal; break;
                case "picBtnHelp": picBtnHelp.BackgroundImage = PackOpeningTracker.Properties.Resources.HelpNormal; break;
            }
        }

        // TIMERS //

        private void timerDig_Tick(object sender, EventArgs e)
        {
            timerTrack.Enabled = false;
            ctrl.collectData(true,null);

            switch (tracking)
            {
                case 1: picStatus.BackgroundImage = PackOpeningTracker.Properties.Resources.Tracking1; tracking++; break;
                case 2: picStatus.BackgroundImage = PackOpeningTracker.Properties.Resources.Tracking2; tracking++; break;
                case 3: picStatus.BackgroundImage = PackOpeningTracker.Properties.Resources.Tracking3; tracking = 1; break;
            }
            displayStats();
            timerTrack.Enabled = true;
        }
    }
}
