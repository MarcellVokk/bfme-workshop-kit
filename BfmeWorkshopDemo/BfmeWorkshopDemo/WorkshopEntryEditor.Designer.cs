namespace WorkshopEditor
{
    partial class WorkshopEntryEditor
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            groupBox1 = new GroupBox();
            gbxFileEdit = new GroupBox();
            label14 = new Label();
            label11 = new Label();
            label9 = new Label();
            label10 = new Label();
            cbbFileLanguage = new ComboBox();
            txtFileName = new TextBox();
            txtFileMd5 = new TextBox();
            txtFileUrl = new TextBox();
            listBox1 = new ListBox();
            button3 = new Button();
            button2 = new Button();
            cbbType = new ComboBox();
            cbbGame = new ComboBox();
            label1 = new Label();
            label8 = new Label();
            label7 = new Label();
            label13 = new Label();
            label6 = new Label();
            label4 = new Label();
            label12 = new Label();
            label2 = new Label();
            txtOwner = new TextBox();
            label5 = new Label();
            txtAuthor = new TextBox();
            label3 = new Label();
            txtDescription = new TextBox();
            txtArtworkUrl = new TextBox();
            txtGuid = new TextBox();
            txtVersion = new TextBox();
            txtName = new TextBox();
            groupBox2 = new GroupBox();
            txtJson = new TextBox();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            loadToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            workshopToolStripMenuItem = new ToolStripMenuItem();
            publishToWorkshopToolStripMenuItem = new ToolStripMenuItem();
            removeFromWorkshopToolStripMenuItem = new ToolStripMenuItem();
            groupBox1.SuspendLayout();
            gbxFileEdit.SuspendLayout();
            groupBox2.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            groupBox1.Controls.Add(gbxFileEdit);
            groupBox1.Controls.Add(listBox1);
            groupBox1.Controls.Add(button3);
            groupBox1.Controls.Add(button2);
            groupBox1.Controls.Add(cbbType);
            groupBox1.Controls.Add(cbbGame);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(label8);
            groupBox1.Controls.Add(label7);
            groupBox1.Controls.Add(label13);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label12);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(txtOwner);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(txtAuthor);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(txtDescription);
            groupBox1.Controls.Add(txtArtworkUrl);
            groupBox1.Controls.Add(txtGuid);
            groupBox1.Controls.Add(txtVersion);
            groupBox1.Controls.Add(txtName);
            groupBox1.Location = new Point(12, 27);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(488, 808);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Edit workshop entry";
            // 
            // gbxFileEdit
            // 
            gbxFileEdit.Controls.Add(label14);
            gbxFileEdit.Controls.Add(label11);
            gbxFileEdit.Controls.Add(label9);
            gbxFileEdit.Controls.Add(label10);
            gbxFileEdit.Controls.Add(cbbFileLanguage);
            gbxFileEdit.Controls.Add(txtFileName);
            gbxFileEdit.Controls.Add(txtFileMd5);
            gbxFileEdit.Controls.Add(txtFileUrl);
            gbxFileEdit.Location = new Point(250, 433);
            gbxFileEdit.Name = "gbxFileEdit";
            gbxFileEdit.Size = new Size(232, 334);
            gbxFileEdit.TabIndex = 10;
            gbxFileEdit.TabStop = false;
            gbxFileEdit.Text = "Edit file";
            gbxFileEdit.Visible = false;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(6, 152);
            label14.Name = "label14";
            label14.Size = new Size(59, 15);
            label14.TabIndex = 1;
            label14.Text = "Language";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(6, 108);
            label11.Name = "label11";
            label11.Size = new Size(31, 15);
            label11.TabIndex = 1;
            label11.Text = "Md5";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(6, 20);
            label9.Name = "label9";
            label9.Size = new Size(39, 15);
            label9.TabIndex = 1;
            label9.Text = "Name";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(6, 64);
            label10.Name = "label10";
            label10.Size = new Size(22, 15);
            label10.TabIndex = 1;
            label10.Text = "Url";
            // 
            // cbbFileLanguage
            // 
            cbbFileLanguage.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbFileLanguage.FormattingEnabled = true;
            cbbFileLanguage.Items.AddRange(new object[] { "ALL", "EN", "FR", "DE", "IT", "ES", "SV", "NL", "PL", "NO", "RU" });
            cbbFileLanguage.Location = new Point(6, 170);
            cbbFileLanguage.Name = "cbbFileLanguage";
            cbbFileLanguage.Size = new Size(220, 23);
            cbbFileLanguage.TabIndex = 14;
            cbbFileLanguage.SelectedIndexChanged += cbbFileLanguage_SelectedIndexChanged;
            // 
            // txtFileName
            // 
            txtFileName.Location = new Point(6, 38);
            txtFileName.Name = "txtFileName";
            txtFileName.Size = new Size(220, 23);
            txtFileName.TabIndex = 11;
            txtFileName.TextChanged += txtFileName_TextChanged;
            // 
            // txtFileMd5
            // 
            txtFileMd5.Location = new Point(6, 126);
            txtFileMd5.Name = "txtFileMd5";
            txtFileMd5.Size = new Size(220, 23);
            txtFileMd5.TabIndex = 13;
            txtFileMd5.TextChanged += txtFileMd5_TextChanged;
            // 
            // txtFileUrl
            // 
            txtFileUrl.Location = new Point(6, 82);
            txtFileUrl.Name = "txtFileUrl";
            txtFileUrl.Size = new Size(220, 23);
            txtFileUrl.TabIndex = 12;
            txtFileUrl.TextChanged += txtFileUrl_TextChanged;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(6, 433);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(238, 334);
            listBox1.TabIndex = 9;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // button3
            // 
            button3.Location = new Point(188, 773);
            button3.Name = "button3";
            button3.Size = new Size(25, 23);
            button3.TabIndex = 15;
            button3.Text = "-";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button2
            // 
            button2.Location = new Point(219, 773);
            button2.Name = "button2";
            button2.Size = new Size(25, 23);
            button2.TabIndex = 16;
            button2.Text = "+";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // cbbType
            // 
            cbbType.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbType.FormattingEnabled = true;
            cbbType.Items.AddRange(new object[] { "0 = Patch", "1 = Mod" });
            cbbType.Location = new Point(6, 389);
            cbbType.Name = "cbbType";
            cbbType.Size = new Size(476, 23);
            cbbType.TabIndex = 8;
            cbbType.SelectedIndexChanged += cbbType_SelectedIndexChanged;
            // 
            // cbbGame
            // 
            cbbGame.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbGame.FormattingEnabled = true;
            cbbGame.Items.AddRange(new object[] { "0 = BFME1", "1 = BFME2", "2 = RotWK" });
            cbbGame.Location = new Point(6, 345);
            cbbGame.Name = "cbbGame";
            cbbGame.Size = new Size(476, 23);
            cbbGame.TabIndex = 7;
            cbbGame.SelectedIndexChanged += cbbGame_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 416);
            label1.Name = "label1";
            label1.Size = new Size(30, 15);
            label1.TabIndex = 1;
            label1.Text = "Files";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(6, 371);
            label8.Name = "label8";
            label8.Size = new Size(31, 15);
            label8.TabIndex = 1;
            label8.Text = "Type";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(6, 327);
            label7.Name = "label7";
            label7.Size = new Size(38, 15);
            label7.TabIndex = 1;
            label7.Text = "Game";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(6, 283);
            label13.Name = "label13";
            label13.Size = new Size(42, 15);
            label13.TabIndex = 1;
            label13.Text = "Owner";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(6, 239);
            label6.Name = "label6";
            label6.Size = new Size(44, 15);
            label6.TabIndex = 1;
            label6.Text = "Author";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(6, 151);
            label4.Name = "label4";
            label4.Size = new Size(67, 15);
            label4.TabIndex = 1;
            label4.Text = "Description";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(6, 19);
            label12.Name = "label12";
            label12.Size = new Size(32, 15);
            label12.TabIndex = 1;
            label12.Text = "Guid";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 63);
            label2.Name = "label2";
            label2.Size = new Size(39, 15);
            label2.TabIndex = 1;
            label2.Text = "Name";
            // 
            // txtOwner
            // 
            txtOwner.Location = new Point(6, 301);
            txtOwner.Name = "txtOwner";
            txtOwner.ReadOnly = true;
            txtOwner.Size = new Size(476, 23);
            txtOwner.TabIndex = 6;
            txtOwner.TextChanged += txtAuthor_TextChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(6, 195);
            label5.Name = "label5";
            label5.Size = new Size(64, 15);
            label5.TabIndex = 1;
            label5.Text = "ArtworkUrl";
            // 
            // txtAuthor
            // 
            txtAuthor.Location = new Point(6, 257);
            txtAuthor.Name = "txtAuthor";
            txtAuthor.Size = new Size(476, 23);
            txtAuthor.TabIndex = 5;
            txtAuthor.TextChanged += txtAuthor_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(6, 107);
            label3.Name = "label3";
            label3.Size = new Size(45, 15);
            label3.TabIndex = 1;
            label3.Text = "Version";
            // 
            // txtDescription
            // 
            txtDescription.Location = new Point(6, 169);
            txtDescription.Name = "txtDescription";
            txtDescription.Size = new Size(476, 23);
            txtDescription.TabIndex = 3;
            txtDescription.TextChanged += txtDescription_TextChanged;
            // 
            // txtArtworkUrl
            // 
            txtArtworkUrl.Location = new Point(6, 213);
            txtArtworkUrl.Name = "txtArtworkUrl";
            txtArtworkUrl.Size = new Size(476, 23);
            txtArtworkUrl.TabIndex = 4;
            txtArtworkUrl.TextChanged += txtArtworkUrl_TextChanged;
            // 
            // txtGuid
            // 
            txtGuid.Location = new Point(6, 37);
            txtGuid.Name = "txtGuid";
            txtGuid.ReadOnly = true;
            txtGuid.Size = new Size(476, 23);
            txtGuid.TabIndex = 0;
            // 
            // txtVersion
            // 
            txtVersion.Location = new Point(6, 125);
            txtVersion.Name = "txtVersion";
            txtVersion.Size = new Size(476, 23);
            txtVersion.TabIndex = 2;
            txtVersion.Text = "1.0.0";
            txtVersion.TextChanged += txtVersion_TextChanged;
            // 
            // txtName
            // 
            txtName.Location = new Point(6, 81);
            txtName.Name = "txtName";
            txtName.Size = new Size(476, 23);
            txtName.TabIndex = 1;
            txtName.Text = "New workshop entry";
            txtName.TextChanged += txtName_TextChanged;
            // 
            // groupBox2
            // 
            groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBox2.Controls.Add(txtJson);
            groupBox2.Location = new Point(506, 27);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(534, 808);
            groupBox2.TabIndex = 0;
            groupBox2.TabStop = false;
            groupBox2.Text = "Preview";
            // 
            // txtJson
            // 
            txtJson.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtJson.BorderStyle = BorderStyle.None;
            txtJson.Location = new Point(6, 22);
            txtJson.Multiline = true;
            txtJson.Name = "txtJson";
            txtJson.ReadOnly = true;
            txtJson.Size = new Size(522, 780);
            txtJson.TabIndex = 17;
            txtJson.TabStop = false;
            // 
            // menuStrip1
            // 
            menuStrip1.BackColor = Color.FromArgb(224, 224, 224);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, workshopToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1052, 24);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { loadToolStripMenuItem, saveToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // loadToolStripMenuItem
            // 
            loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            loadToolStripMenuItem.Size = new Size(100, 22);
            loadToolStripMenuItem.Text = "Load";
            loadToolStripMenuItem.Click += loadToolStripMenuItem_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(100, 22);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // workshopToolStripMenuItem
            // 
            workshopToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { publishToWorkshopToolStripMenuItem, removeFromWorkshopToolStripMenuItem });
            workshopToolStripMenuItem.Name = "workshopToolStripMenuItem";
            workshopToolStripMenuItem.Size = new Size(73, 20);
            workshopToolStripMenuItem.Text = "Workshop";
            // 
            // publishToWorkshopToolStripMenuItem
            // 
            publishToWorkshopToolStripMenuItem.Name = "publishToWorkshopToolStripMenuItem";
            publishToWorkshopToolStripMenuItem.Size = new Size(203, 22);
            publishToWorkshopToolStripMenuItem.Text = "Publish to Workshop";
            publishToWorkshopToolStripMenuItem.Click += publishToWorkshopToolStripMenuItem_Click;
            // 
            // removeFromWorkshopToolStripMenuItem
            // 
            removeFromWorkshopToolStripMenuItem.Name = "removeFromWorkshopToolStripMenuItem";
            removeFromWorkshopToolStripMenuItem.Size = new Size(203, 22);
            removeFromWorkshopToolStripMenuItem.Text = "Remove from Workshop";
            removeFromWorkshopToolStripMenuItem.Click += removeFromWorkshopToolStripMenuItem_Click;
            // 
            // WorkshopEntryEditor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1052, 847);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "WorkshopEntryEditor";
            Text = "BfmeWorkshopKit Workshop Entry Editor";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            gbxFileEdit.ResumeLayout(false);
            gbxFileEdit.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox groupBox1;
        private Label label6;
        private Label label4;
        private Label label2;
        private Label label5;
        private TextBox txtAuthor;
        private Label label3;
        private TextBox txtDescription;
        private TextBox txtArtworkUrl;
        private TextBox txtVersion;
        private TextBox txtName;
        private GroupBox groupBox2;
        private TextBox txtJson;
        private Label label7;
        private Label label8;
        private ComboBox cbbGame;
        private ComboBox cbbType;
        private Label label1;
        private Button button2;
        private ListBox listBox1;
        private GroupBox gbxFileEdit;
        private Label label11;
        private Label label9;
        private Label label10;
        private TextBox txtFileName;
        private TextBox txtFileMd5;
        private TextBox txtFileUrl;
        private Button button3;
        private Label label12;
        private TextBox txtGuid;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem loadToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem workshopToolStripMenuItem;
        private ToolStripMenuItem publishToWorkshopToolStripMenuItem;
        private Label label13;
        private TextBox txtOwner;
        private Label label14;
        private ComboBox cbbFileLanguage;
        private ToolStripMenuItem removeFromWorkshopToolStripMenuItem;
    }
}
