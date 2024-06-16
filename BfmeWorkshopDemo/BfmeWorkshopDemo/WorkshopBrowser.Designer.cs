namespace WorkshopEditor
{
    partial class WorkshopBrowser
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
            txtKeyword = new TextBox();
            btnSearch = new Button();
            lbxResults = new ListBox();
            groupBox1 = new GroupBox();
            btnDownload = new Button();
            label9 = new Label();
            label8 = new Label();
            groupBox2 = new GroupBox();
            label1 = new Label();
            txtDownloads = new TextBox();
            label7 = new Label();
            label13 = new Label();
            label6 = new Label();
            label4 = new Label();
            txtFiles = new TextBox();
            label2 = new Label();
            txtType = new TextBox();
            txtGame = new TextBox();
            txtOwner = new TextBox();
            label5 = new Label();
            txtAuthor = new TextBox();
            label3 = new Label();
            txtDescription = new TextBox();
            txtArtworkUrl = new TextBox();
            txtVersion = new TextBox();
            txtName = new TextBox();
            cbbGame = new ComboBox();
            cbbSortMode = new ComboBox();
            nudPage = new NumericUpDown();
            txtOwnerUuid = new TextBox();
            btnSearchByOwner = new Button();
            label10 = new Label();
            label11 = new Label();
            label12 = new Label();
            label14 = new Label();
            label15 = new Label();
            menuStrip1 = new MenuStrip();
            toolsToolStripMenuItem = new ToolStripMenuItem();
            workshopEntryEditorToolStripMenuItem = new ToolStripMenuItem();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudPage).BeginInit();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // txtKeyword
            // 
            txtKeyword.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtKeyword.Location = new Point(12, 56);
            txtKeyword.Name = "txtKeyword";
            txtKeyword.Size = new Size(466, 23);
            txtKeyword.TabIndex = 0;
            // 
            // btnSearch
            // 
            btnSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSearch.Location = new Point(823, 56);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(75, 23);
            btnSearch.TabIndex = 1;
            btnSearch.Text = "Search";
            btnSearch.UseVisualStyleBackColor = true;
            btnSearch.Click += btnSearch_Click;
            // 
            // lbxResults
            // 
            lbxResults.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lbxResults.FormattingEnabled = true;
            lbxResults.ItemHeight = 15;
            lbxResults.Location = new Point(12, 131);
            lbxResults.Name = "lbxResults";
            lbxResults.Size = new Size(514, 724);
            lbxResults.TabIndex = 2;
            lbxResults.SelectedIndexChanged += lbxResults_SelectedIndexChanged;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnDownload);
            groupBox1.Controls.Add(label9);
            groupBox1.Controls.Add(label8);
            groupBox1.Controls.Add(groupBox2);
            groupBox1.Controls.Add(label7);
            groupBox1.Controls.Add(label13);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(txtFiles);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(txtType);
            groupBox1.Controls.Add(txtGame);
            groupBox1.Controls.Add(txtOwner);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(txtAuthor);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(txtDescription);
            groupBox1.Controls.Add(txtArtworkUrl);
            groupBox1.Controls.Add(txtVersion);
            groupBox1.Controls.Add(txtName);
            groupBox1.Location = new Point(532, 131);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(366, 724);
            groupBox1.TabIndex = 3;
            groupBox1.TabStop = false;
            groupBox1.Text = "Entry preview";
            // 
            // btnDownload
            // 
            btnDownload.Location = new Point(285, 695);
            btnDownload.Name = "btnDownload";
            btnDownload.Size = new Size(75, 23);
            btnDownload.TabIndex = 18;
            btnDownload.Text = "Download";
            btnDownload.UseVisualStyleBackColor = true;
            btnDownload.Click += btnDownload_Click;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(6, 371);
            label9.Name = "label9";
            label9.Size = new Size(30, 15);
            label9.TabIndex = 9;
            label9.Text = "Files";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(6, 327);
            label8.Name = "label8";
            label8.Size = new Size(31, 15);
            label8.TabIndex = 9;
            label8.Text = "Type";
            // 
            // groupBox2
            // 
            groupBox2.BackColor = SystemColors.Control;
            groupBox2.Controls.Add(label1);
            groupBox2.Controls.Add(txtDownloads);
            groupBox2.Location = new Point(6, 619);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(354, 70);
            groupBox2.TabIndex = 4;
            groupBox2.TabStop = false;
            groupBox2.Text = "Metadata";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 19);
            label1.Name = "label1";
            label1.Size = new Size(66, 15);
            label1.TabIndex = 14;
            label1.Text = "Downloads";
            // 
            // txtDownloads
            // 
            txtDownloads.Location = new Point(6, 37);
            txtDownloads.Name = "txtDownloads";
            txtDownloads.ReadOnly = true;
            txtDownloads.Size = new Size(342, 23);
            txtDownloads.TabIndex = 17;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(6, 283);
            label7.Name = "label7";
            label7.Size = new Size(38, 15);
            label7.TabIndex = 10;
            label7.Text = "Game";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(6, 239);
            label13.Name = "label13";
            label13.Size = new Size(42, 15);
            label13.TabIndex = 11;
            label13.Text = "Owner";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(6, 195);
            label6.Name = "label6";
            label6.Size = new Size(44, 15);
            label6.TabIndex = 12;
            label6.Text = "Author";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(6, 107);
            label4.Name = "label4";
            label4.Size = new Size(67, 15);
            label4.TabIndex = 13;
            label4.Text = "Description";
            // 
            // txtFiles
            // 
            txtFiles.Location = new Point(6, 389);
            txtFiles.Multiline = true;
            txtFiles.Name = "txtFiles";
            txtFiles.ReadOnly = true;
            txtFiles.ScrollBars = ScrollBars.Both;
            txtFiles.Size = new Size(354, 224);
            txtFiles.TabIndex = 22;
            txtFiles.WordWrap = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 19);
            label2.Name = "label2";
            label2.Size = new Size(39, 15);
            label2.TabIndex = 14;
            label2.Text = "Name";
            // 
            // txtType
            // 
            txtType.Location = new Point(6, 345);
            txtType.Name = "txtType";
            txtType.ReadOnly = true;
            txtType.Size = new Size(354, 23);
            txtType.TabIndex = 22;
            // 
            // txtGame
            // 
            txtGame.Location = new Point(6, 301);
            txtGame.Name = "txtGame";
            txtGame.ReadOnly = true;
            txtGame.Size = new Size(354, 23);
            txtGame.TabIndex = 22;
            // 
            // txtOwner
            // 
            txtOwner.Location = new Point(6, 257);
            txtOwner.Name = "txtOwner";
            txtOwner.ReadOnly = true;
            txtOwner.Size = new Size(354, 23);
            txtOwner.TabIndex = 22;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(6, 151);
            label5.Name = "label5";
            label5.Size = new Size(64, 15);
            label5.TabIndex = 15;
            label5.Text = "ArtworkUrl";
            // 
            // txtAuthor
            // 
            txtAuthor.Location = new Point(6, 213);
            txtAuthor.Name = "txtAuthor";
            txtAuthor.ReadOnly = true;
            txtAuthor.Size = new Size(354, 23);
            txtAuthor.TabIndex = 21;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(6, 63);
            label3.Name = "label3";
            label3.Size = new Size(45, 15);
            label3.TabIndex = 16;
            label3.Text = "Version";
            // 
            // txtDescription
            // 
            txtDescription.Location = new Point(6, 125);
            txtDescription.Name = "txtDescription";
            txtDescription.ReadOnly = true;
            txtDescription.Size = new Size(354, 23);
            txtDescription.TabIndex = 19;
            // 
            // txtArtworkUrl
            // 
            txtArtworkUrl.Location = new Point(6, 169);
            txtArtworkUrl.Name = "txtArtworkUrl";
            txtArtworkUrl.ReadOnly = true;
            txtArtworkUrl.Size = new Size(354, 23);
            txtArtworkUrl.TabIndex = 20;
            // 
            // txtVersion
            // 
            txtVersion.Location = new Point(6, 81);
            txtVersion.Name = "txtVersion";
            txtVersion.ReadOnly = true;
            txtVersion.Size = new Size(354, 23);
            txtVersion.TabIndex = 18;
            // 
            // txtName
            // 
            txtName.Location = new Point(6, 37);
            txtName.Name = "txtName";
            txtName.ReadOnly = true;
            txtName.Size = new Size(354, 23);
            txtName.TabIndex = 17;
            // 
            // cbbGame
            // 
            cbbGame.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbGame.FormattingEnabled = true;
            cbbGame.Items.AddRange(new object[] { "-1 = All games", "0 = BFME1", "1 = BFME2", "2 = RotWK" });
            cbbGame.Location = new Point(484, 56);
            cbbGame.Name = "cbbGame";
            cbbGame.Size = new Size(121, 23);
            cbbGame.TabIndex = 4;
            // 
            // cbbSortMode
            // 
            cbbSortMode.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbSortMode.FormattingEnabled = true;
            cbbSortMode.Items.AddRange(new object[] { "0 = Most downloads", "1 = Most recent" });
            cbbSortMode.Location = new Point(611, 56);
            cbbSortMode.Name = "cbbSortMode";
            cbbSortMode.Size = new Size(138, 23);
            cbbSortMode.TabIndex = 4;
            // 
            // nudPage
            // 
            nudPage.Location = new Point(755, 56);
            nudPage.Name = "nudPage";
            nudPage.Size = new Size(62, 23);
            nudPage.TabIndex = 5;
            // 
            // txtOwnerUuid
            // 
            txtOwnerUuid.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtOwnerUuid.Location = new Point(12, 102);
            txtOwnerUuid.Name = "txtOwnerUuid";
            txtOwnerUuid.Size = new Size(466, 23);
            txtOwnerUuid.TabIndex = 0;
            // 
            // btnSearchByOwner
            // 
            btnSearchByOwner.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSearchByOwner.Location = new Point(484, 101);
            btnSearchByOwner.Name = "btnSearchByOwner";
            btnSearchByOwner.Size = new Size(75, 23);
            btnSearchByOwner.TabIndex = 1;
            btnSearchByOwner.Text = "Search";
            btnSearchByOwner.UseVisualStyleBackColor = true;
            btnSearchByOwner.Click += btnSearchByOwner_Click;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(12, 38);
            label10.Name = "label10";
            label10.Size = new Size(53, 15);
            label10.TabIndex = 6;
            label10.Text = "Keyword";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(484, 38);
            label11.Name = "label11";
            label11.Size = new Size(38, 15);
            label11.TabIndex = 6;
            label11.Text = "Game";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(611, 38);
            label12.Name = "label12";
            label12.Size = new Size(37, 15);
            label12.TabIndex = 6;
            label12.Text = "Order";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(755, 38);
            label14.Name = "label14";
            label14.Size = new Size(33, 15);
            label14.TabIndex = 6;
            label14.Text = "Page";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(12, 84);
            label15.Name = "label15";
            label15.Size = new Size(121, 15);
            label15.TabIndex = 6;
            label15.Text = "Search by owner uuid";
            // 
            // menuStrip1
            // 
            menuStrip1.BackColor = Color.FromArgb(224, 224, 224);
            menuStrip1.Items.AddRange(new ToolStripItem[] { toolsToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(910, 24);
            menuStrip1.TabIndex = 7;
            menuStrip1.Text = "menuStrip1";
            // 
            // toolsToolStripMenuItem
            // 
            toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { workshopEntryEditorToolStripMenuItem });
            toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            toolsToolStripMenuItem.Size = new Size(46, 20);
            toolsToolStripMenuItem.Text = "Tools";
            // 
            // workshopEntryEditorToolStripMenuItem
            // 
            workshopEntryEditorToolStripMenuItem.Name = "workshopEntryEditorToolStripMenuItem";
            workshopEntryEditorToolStripMenuItem.Size = new Size(192, 22);
            workshopEntryEditorToolStripMenuItem.Text = "Workshop Entry Editor";
            workshopEntryEditorToolStripMenuItem.Click += workshopEntryEditorToolStripMenuItem_Click;
            // 
            // WorkshopBrowser
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(910, 878);
            Controls.Add(label14);
            Controls.Add(label12);
            Controls.Add(label15);
            Controls.Add(label11);
            Controls.Add(label10);
            Controls.Add(nudPage);
            Controls.Add(cbbSortMode);
            Controls.Add(cbbGame);
            Controls.Add(groupBox1);
            Controls.Add(lbxResults);
            Controls.Add(btnSearchByOwner);
            Controls.Add(btnSearch);
            Controls.Add(txtOwnerUuid);
            Controls.Add(txtKeyword);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "WorkshopBrowser";
            Text = "BfmeWorkshopKit Workshop Browser";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudPage).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtKeyword;
        private Button btnSearch;
        private ListBox lbxResults;
        private GroupBox groupBox1;
        private Label label8;
        private Label label7;
        private Label label13;
        private Label label6;
        private Label label4;
        private Label label2;
        private TextBox txtType;
        private TextBox txtGame;
        private TextBox txtOwner;
        private Label label5;
        private TextBox txtAuthor;
        private Label label3;
        private TextBox txtDescription;
        private TextBox txtArtworkUrl;
        private TextBox txtVersion;
        private TextBox txtName;
        private GroupBox groupBox2;
        private Label label1;
        private TextBox txtDownloads;
        private Button btnDownload;
        private ComboBox cbbGame;
        private ComboBox cbbSortMode;
        private NumericUpDown nudPage;
        private Label label9;
        private TextBox txtFiles;
        private TextBox txtOwnerUuid;
        private Button btnSearchByOwner;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label14;
        private Label label15;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem workshopEntryEditorToolStripMenuItem;
    }
}
