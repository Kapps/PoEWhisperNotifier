namespace PoEWhisperNotifier
{
	partial class Main
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.txtLogPath = new System.Windows.Forms.TextBox();
			this.cmdStop = new System.Windows.Forms.Button();
			this.cmdStart = new System.Windows.Forms.Button();
			this.rtbHistory = new System.Windows.Forms.RichTextBox();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmNotifyMinimizedOnly = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmEnableTrayNotifications = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmEnableSound = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmEnableSMTPNotifications = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmEnablePushBullet = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmMinimizeToTray = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmStartMinimized = new System.Windows.Forms.ToolStripMenuItem();
			this.configureSMTPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.configurePushBulletToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.configureMessageTypesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmTrimLogFile = new System.Windows.Forms.ToolStripMenuItem();
			this.testNotificationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.NotificationIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.tableLayoutPanel1.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.txtLogPath, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.cmdStop, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.cmdStart, 2, 2);
			this.tableLayoutPanel1.Controls.Add(this.rtbHistory, 0, 1);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 32);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(901, 569);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(2, 5);
			this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(81, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "Log Path:";
			// 
			// txtLogPath
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.txtLogPath, 2);
			this.txtLogPath.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtLogPath.Location = new System.Drawing.Point(87, 2);
			this.txtLogPath.Margin = new System.Windows.Forms.Padding(2);
			this.txtLogPath.Name = "txtLogPath";
			this.txtLogPath.Size = new System.Drawing.Size(812, 26);
			this.txtLogPath.TabIndex = 1;
			// 
			// cmdStop
			// 
			this.cmdStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdStop.Enabled = false;
			this.cmdStop.Location = new System.Drawing.Point(695, 522);
			this.cmdStop.Margin = new System.Windows.Forms.Padding(2);
			this.cmdStop.Name = "cmdStop";
			this.cmdStop.Size = new System.Drawing.Size(100, 45);
			this.cmdStop.TabIndex = 2;
			this.cmdStop.Text = "Stop";
			this.cmdStop.UseVisualStyleBackColor = true;
			this.cmdStop.Click += new System.EventHandler(this.cmdStop_Click);
			// 
			// cmdStart
			// 
			this.cmdStart.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.cmdStart.Location = new System.Drawing.Point(799, 522);
			this.cmdStart.Margin = new System.Windows.Forms.Padding(2);
			this.cmdStart.Name = "cmdStart";
			this.cmdStart.Size = new System.Drawing.Size(100, 45);
			this.cmdStart.TabIndex = 3;
			this.cmdStart.Text = "Start";
			this.cmdStart.UseVisualStyleBackColor = true;
			this.cmdStart.Click += new System.EventHandler(this.cmdStart_Click);
			// 
			// rtbHistory
			// 
			this.rtbHistory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(40)))), ((int)(((byte)(34)))));
			this.tableLayoutPanel1.SetColumnSpan(this.rtbHistory, 3);
			this.rtbHistory.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rtbHistory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(239)))), ((int)(((byte)(239)))));
			this.rtbHistory.HideSelection = false;
			this.rtbHistory.Location = new System.Drawing.Point(2, 32);
			this.rtbHistory.Margin = new System.Windows.Forms.Padding(2);
			this.rtbHistory.Name = "rtbHistory";
			this.rtbHistory.ReadOnly = true;
			this.rtbHistory.Size = new System.Drawing.Size(897, 486);
			this.rtbHistory.TabIndex = 4;
			this.rtbHistory.Text = "";
			// 
			// menuStrip1
			// 
			this.menuStrip1.ImageScalingSize = new System.Drawing.Size(40, 40);
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.toolsToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 1, 0, 1);
			this.menuStrip1.Size = new System.Drawing.Size(902, 26);
			this.menuStrip1.TabIndex = 1;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
			this.exitToolStripMenuItem.Text = "E&xit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// editToolStripMenuItem
			// 
			this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmNotifyMinimizedOnly,
            this.tsmEnableTrayNotifications,
            this.tsmEnableSound,
            this.tsmEnableSMTPNotifications,
            this.tsmEnablePushBullet,
            this.tsmMinimizeToTray,
            this.tsmStartMinimized,
            this.configureSMTPToolStripMenuItem,
            this.configurePushBulletToolStripMenuItem,
            this.configureMessageTypesToolStripMenuItem});
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			this.editToolStripMenuItem.Size = new System.Drawing.Size(74, 24);
			this.editToolStripMenuItem.Text = "&Settings";
			// 
			// tsmNotifyMinimizedOnly
			// 
			this.tsmNotifyMinimizedOnly.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.tsmNotifyMinimizedOnly.Name = "tsmNotifyMinimizedOnly";
			this.tsmNotifyMinimizedOnly.Size = new System.Drawing.Size(290, 26);
			this.tsmNotifyMinimizedOnly.Text = "Notify Only When &Minimized";
			this.tsmNotifyMinimizedOnly.Click += new System.EventHandler(this.notifyOnlyWhenMinimizedToolStripMenuItem_Click);
			// 
			// tsmEnableTrayNotifications
			// 
			this.tsmEnableTrayNotifications.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.tsmEnableTrayNotifications.Name = "tsmEnableTrayNotifications";
			this.tsmEnableTrayNotifications.Size = new System.Drawing.Size(290, 26);
			this.tsmEnableTrayNotifications.Text = "Enable &Tray Notifications";
			this.tsmEnableTrayNotifications.Click += new System.EventHandler(this.tsmEnableTrayNotifications_Click);
			// 
			// tsmEnableSound
			// 
			this.tsmEnableSound.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.tsmEnableSound.Name = "tsmEnableSound";
			this.tsmEnableSound.Size = new System.Drawing.Size(290, 26);
			this.tsmEnableSound.Text = "Enable S&ound";
			this.tsmEnableSound.Click += new System.EventHandler(this.tsmEnableSound_Click);
			// 
			// tsmEnableSMTPNotifications
			// 
			this.tsmEnableSMTPNotifications.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.tsmEnableSMTPNotifications.Name = "tsmEnableSMTPNotifications";
			this.tsmEnableSMTPNotifications.Size = new System.Drawing.Size(290, 26);
			this.tsmEnableSMTPNotifications.Text = "Enable &SMTP Notifications";
			this.tsmEnableSMTPNotifications.Click += new System.EventHandler(this.tsmEnableSMTPNotifications_Click);
			// 
			// tsmEnablePushBullet
			// 
			this.tsmEnablePushBullet.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.tsmEnablePushBullet.Name = "tsmEnablePushBullet";
			this.tsmEnablePushBullet.Size = new System.Drawing.Size(290, 26);
			this.tsmEnablePushBullet.Text = "Enable &PushBullet Notifications";
			this.tsmEnablePushBullet.Click += new System.EventHandler(this.tsmEnablePushbullet_Click);
			// 
			// tsmMinimizeToTray
			// 
			this.tsmMinimizeToTray.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.tsmMinimizeToTray.Name = "tsmMinimizeToTray";
			this.tsmMinimizeToTray.Size = new System.Drawing.Size(290, 26);
			this.tsmMinimizeToTray.Text = "M&inimize to System Tray";
			this.tsmMinimizeToTray.Click += new System.EventHandler(this.tsmMinimizeToTray_Click);
			// 
			// tsmStartMinimized
			// 
			this.tsmStartMinimized.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.tsmStartMinimized.Name = "tsmStartMinimized";
			this.tsmStartMinimized.Size = new System.Drawing.Size(290, 26);
			this.tsmStartMinimized.Text = "Start Minimiz&ed";
			this.tsmStartMinimized.Click += new System.EventHandler(this.tsmStartMinimized_Click);
			// 
			// configureSMTPToolStripMenuItem
			// 
			this.configureSMTPToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.configureSMTPToolStripMenuItem.Name = "configureSMTPToolStripMenuItem";
			this.configureSMTPToolStripMenuItem.Size = new System.Drawing.Size(290, 26);
			this.configureSMTPToolStripMenuItem.Text = "Configure SMTP";
			this.configureSMTPToolStripMenuItem.Click += new System.EventHandler(this.configureSMTPToolStripMenuItem_Click);
			// 
			// configurePushBulletToolStripMenuItem
			// 
			this.configurePushBulletToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.configurePushBulletToolStripMenuItem.Name = "configurePushBulletToolStripMenuItem";
			this.configurePushBulletToolStripMenuItem.Size = new System.Drawing.Size(290, 26);
			this.configurePushBulletToolStripMenuItem.Text = "Configure PushBullet";
			this.configurePushBulletToolStripMenuItem.Click += new System.EventHandler(this.configurePushBulletToolStripMenuItem_Click);
			// 
			// configureMessageTypesToolStripMenuItem
			// 
			this.configureMessageTypesToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.configureMessageTypesToolStripMenuItem.Name = "configureMessageTypesToolStripMenuItem";
			this.configureMessageTypesToolStripMenuItem.Size = new System.Drawing.Size(290, 26);
			this.configureMessageTypesToolStripMenuItem.Text = "Configure M&essage Types";
			this.configureMessageTypesToolStripMenuItem.Click += new System.EventHandler(this.configureMessageTypesToolStripMenuItem_Click);
			// 
			// toolsToolStripMenuItem
			// 
			this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmTrimLogFile,
            this.testNotificationToolStripMenuItem});
			this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
			this.toolsToolStripMenuItem.Size = new System.Drawing.Size(56, 24);
			this.toolsToolStripMenuItem.Text = "&Tools";
			// 
			// tsmTrimLogFile
			// 
			this.tsmTrimLogFile.Name = "tsmTrimLogFile";
			this.tsmTrimLogFile.Size = new System.Drawing.Size(193, 26);
			this.tsmTrimLogFile.Text = "Trim Client.txt";
			this.tsmTrimLogFile.Click += new System.EventHandler(this.trimClienttxtToolStripMenuItem_Click);
			// 
			// testNotificationToolStripMenuItem
			// 
			this.testNotificationToolStripMenuItem.Name = "testNotificationToolStripMenuItem";
			this.testNotificationToolStripMenuItem.Size = new System.Drawing.Size(193, 26);
			this.testNotificationToolStripMenuItem.Text = "Test &Notification";
			this.testNotificationToolStripMenuItem.Click += new System.EventHandler(this.testNotificationToolStripMenuItem_Click);
			// 
			// NotificationIcon
			// 
			this.NotificationIcon.Text = "PoE Whisper Notifier";
			this.NotificationIcon.Visible = true;
			// 
			// Main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.ClientSize = new System.Drawing.Size(902, 601);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.menuStrip1);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.Margin = new System.Windows.Forms.Padding(2);
			this.Name = "Main";
			this.Text = "PoE Whisper Notifier";
			this.Load += new System.EventHandler(this.Main_Load);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtLogPath;
		private System.Windows.Forms.Button cmdStop;
		private System.Windows.Forms.Button cmdStart;
		private System.Windows.Forms.RichTextBox rtbHistory;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem tsmNotifyMinimizedOnly;
		private System.Windows.Forms.ToolStripMenuItem tsmEnableTrayNotifications;
		private System.Windows.Forms.NotifyIcon NotificationIcon;
		private System.Windows.Forms.ToolStripMenuItem tsmEnableSMTPNotifications;
		private System.Windows.Forms.ToolStripMenuItem configureSMTPToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem tsmEnablePushBullet;
		private System.Windows.Forms.ToolStripMenuItem configurePushBulletToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem tsmEnableSound;
		private System.Windows.Forms.ToolStripMenuItem tsmMinimizeToTray;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem tsmTrimLogFile;
		private System.Windows.Forms.ToolStripMenuItem testNotificationToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem tsmStartMinimized;
		private System.Windows.Forms.ToolStripMenuItem configureMessageTypesToolStripMenuItem;
	}
}

