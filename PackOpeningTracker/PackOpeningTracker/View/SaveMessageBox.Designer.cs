namespace PackOpeningTracker.View
{
    partial class SaveMessageBox
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblQuestion = new System.Windows.Forms.Label();
            this.txtSaveFileName = new System.Windows.Forms.TextBox();
            this.txtTextFileName = new System.Windows.Forms.TextBox();
            this.checkSaveFile = new System.Windows.Forms.CheckBox();
            this.checkTextFile = new System.Windows.Forms.CheckBox();
            this.rchInformation = new System.Windows.Forms.RichTextBox();
            this.lblSaveFileName = new System.Windows.Forms.Label();
            this.lblTextFileName = new System.Windows.Forms.Label();
            this.picTitleBar = new System.Windows.Forms.PictureBox();
            this.picBtnSave = new System.Windows.Forms.PictureBox();
            this.picBtnCancel = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picTitleBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBtnSave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBtnCancel)).BeginInit();
            this.SuspendLayout();
            // 
            // lblQuestion
            // 
            this.lblQuestion.AutoSize = true;
            this.lblQuestion.BackColor = System.Drawing.Color.Transparent;
            this.lblQuestion.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQuestion.ForeColor = System.Drawing.Color.Black;
            this.lblQuestion.Location = new System.Drawing.Point(19, 34);
            this.lblQuestion.Name = "lblQuestion";
            this.lblQuestion.Size = new System.Drawing.Size(295, 16);
            this.lblQuestion.TabIndex = 3;
            this.lblQuestion.Text = "What kind of file do you want to create?";
            // 
            // txtSaveFileName
            // 
            this.txtSaveFileName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSaveFileName.Location = new System.Drawing.Point(279, 56);
            this.txtSaveFileName.MaxLength = 50;
            this.txtSaveFileName.Name = "txtSaveFileName";
            this.txtSaveFileName.Size = new System.Drawing.Size(201, 20);
            this.txtSaveFileName.TabIndex = 1;
            this.txtSaveFileName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_KeyPress);
            // 
            // txtTextFileName
            // 
            this.txtTextFileName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTextFileName.Enabled = false;
            this.txtTextFileName.Location = new System.Drawing.Point(279, 78);
            this.txtTextFileName.MaxLength = 50;
            this.txtTextFileName.Name = "txtTextFileName";
            this.txtTextFileName.Size = new System.Drawing.Size(201, 20);
            this.txtTextFileName.TabIndex = 2;
            this.txtTextFileName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_KeyPress);
            // 
            // checkSaveFile
            // 
            this.checkSaveFile.AutoSize = true;
            this.checkSaveFile.BackColor = System.Drawing.Color.Transparent;
            this.checkSaveFile.Checked = true;
            this.checkSaveFile.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkSaveFile.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkSaveFile.ForeColor = System.Drawing.Color.Black;
            this.checkSaveFile.Location = new System.Drawing.Point(27, 58);
            this.checkSaveFile.Name = "checkSaveFile";
            this.checkSaveFile.Size = new System.Drawing.Size(89, 20);
            this.checkSaveFile.TabIndex = 6;
            this.checkSaveFile.Text = "Save file";
            this.checkSaveFile.UseVisualStyleBackColor = false;
            this.checkSaveFile.CheckedChanged += new System.EventHandler(this.check_CheckedChanged);
            // 
            // checkTextFile
            // 
            this.checkTextFile.AutoSize = true;
            this.checkTextFile.BackColor = System.Drawing.Color.Transparent;
            this.checkTextFile.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkTextFile.ForeColor = System.Drawing.Color.Black;
            this.checkTextFile.Location = new System.Drawing.Point(27, 78);
            this.checkTextFile.Name = "checkTextFile";
            this.checkTextFile.Size = new System.Drawing.Size(85, 20);
            this.checkTextFile.TabIndex = 7;
            this.checkTextFile.Text = "Text file";
            this.checkTextFile.UseVisualStyleBackColor = false;
            this.checkTextFile.CheckedChanged += new System.EventHandler(this.check_CheckedChanged);
            // 
            // rchInformation
            // 
            this.rchInformation.BackColor = System.Drawing.Color.LightBlue;
            this.rchInformation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rchInformation.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rchInformation.Location = new System.Drawing.Point(23, 104);
            this.rchInformation.Name = "rchInformation";
            this.rchInformation.ReadOnly = true;
            this.rchInformation.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.rchInformation.Size = new System.Drawing.Size(457, 128);
            this.rchInformation.TabIndex = 8;
            this.rchInformation.Text = "";
            // 
            // lblSaveFileName
            // 
            this.lblSaveFileName.AutoSize = true;
            this.lblSaveFileName.BackColor = System.Drawing.Color.Transparent;
            this.lblSaveFileName.Font = new System.Drawing.Font("Verdana", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSaveFileName.ForeColor = System.Drawing.Color.Black;
            this.lblSaveFileName.Location = new System.Drawing.Point(196, 57);
            this.lblSaveFileName.Name = "lblSaveFileName";
            this.lblSaveFileName.Size = new System.Drawing.Size(85, 16);
            this.lblSaveFileName.TabIndex = 9;
            this.lblSaveFileName.Text = "File Name:";
            // 
            // lblTextFileName
            // 
            this.lblTextFileName.AutoSize = true;
            this.lblTextFileName.BackColor = System.Drawing.Color.Transparent;
            this.lblTextFileName.Enabled = false;
            this.lblTextFileName.Font = new System.Drawing.Font("Verdana", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTextFileName.ForeColor = System.Drawing.Color.Black;
            this.lblTextFileName.Location = new System.Drawing.Point(196, 79);
            this.lblTextFileName.Name = "lblTextFileName";
            this.lblTextFileName.Size = new System.Drawing.Size(85, 16);
            this.lblTextFileName.TabIndex = 10;
            this.lblTextFileName.Text = "File Name:";
            // 
            // picTitleBar
            // 
            this.picTitleBar.BackColor = System.Drawing.Color.Transparent;
            this.picTitleBar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.picTitleBar.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.picTitleBar.Location = new System.Drawing.Point(0, 0);
            this.picTitleBar.Name = "picTitleBar";
            this.picTitleBar.Size = new System.Drawing.Size(500, 25);
            this.picTitleBar.TabIndex = 84;
            this.picTitleBar.TabStop = false;
            this.picTitleBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pic_MouseDown);
            // 
            // picBtnSave
            // 
            this.picBtnSave.BackColor = System.Drawing.Color.Transparent;
            this.picBtnSave.BackgroundImage = global::PackOpeningTracker.Properties.Resources.SaveNormal;
            this.picBtnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.picBtnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picBtnSave.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.picBtnSave.Location = new System.Drawing.Point(96, 231);
            this.picBtnSave.Name = "picBtnSave";
            this.picBtnSave.Size = new System.Drawing.Size(91, 49);
            this.picBtnSave.TabIndex = 85;
            this.picBtnSave.TabStop = false;
            this.picBtnSave.Click += new System.EventHandler(this.picBtnSave_Click);
            this.picBtnSave.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pic_MouseDown);
            this.picBtnSave.MouseEnter += new System.EventHandler(this.pic_MouseEnter);
            this.picBtnSave.MouseLeave += new System.EventHandler(this.pic_MouseLeave);
            // 
            // picBtnCancel
            // 
            this.picBtnCancel.BackColor = System.Drawing.Color.Transparent;
            this.picBtnCancel.BackgroundImage = global::PackOpeningTracker.Properties.Resources.CancelNormal;
            this.picBtnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.picBtnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picBtnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.picBtnCancel.Location = new System.Drawing.Point(321, 231);
            this.picBtnCancel.Name = "picBtnCancel";
            this.picBtnCancel.Size = new System.Drawing.Size(91, 49);
            this.picBtnCancel.TabIndex = 86;
            this.picBtnCancel.TabStop = false;
            this.picBtnCancel.Click += new System.EventHandler(this.picBtnCancel_Click);
            this.picBtnCancel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pic_MouseDown);
            this.picBtnCancel.MouseEnter += new System.EventHandler(this.pic_MouseEnter);
            this.picBtnCancel.MouseLeave += new System.EventHandler(this.pic_MouseLeave);
            // 
            // SaveMessageBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = global::PackOpeningTracker.Properties.Resources.SaveMessageBoxBackground;
            this.ClientSize = new System.Drawing.Size(500, 280);
            this.ControlBox = false;
            this.Controls.Add(this.picTitleBar);
            this.Controls.Add(this.txtTextFileName);
            this.Controls.Add(this.txtSaveFileName);
            this.Controls.Add(this.lblTextFileName);
            this.Controls.Add(this.lblSaveFileName);
            this.Controls.Add(this.rchInformation);
            this.Controls.Add(this.checkTextFile);
            this.Controls.Add(this.checkSaveFile);
            this.Controls.Add(this.lblQuestion);
            this.Controls.Add(this.picBtnSave);
            this.Controls.Add(this.picBtnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.Name = "SaveMessageBox";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Save to file";
            ((System.ComponentModel.ISupportInitialize)(this.picTitleBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBtnSave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBtnCancel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblQuestion;
        private System.Windows.Forms.TextBox txtSaveFileName;
        private System.Windows.Forms.TextBox txtTextFileName;
        private System.Windows.Forms.CheckBox checkSaveFile;
        private System.Windows.Forms.CheckBox checkTextFile;
        private System.Windows.Forms.RichTextBox rchInformation;
        private System.Windows.Forms.Label lblSaveFileName;
        private System.Windows.Forms.Label lblTextFileName;
        private System.Windows.Forms.PictureBox picTitleBar;
        private System.Windows.Forms.PictureBox picBtnSave;
        private System.Windows.Forms.PictureBox picBtnCancel;
    }
}