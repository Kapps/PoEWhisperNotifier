using PoEWhisperNotifier.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PoEWhisperNotifier {
	public partial class Main : Form {

		[DllImport("user32.dll")]
		private static extern IntPtr GetForegroundWindow();
		[DllImport("user32.dll")]
		public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);


		public Main() {
			InitializeComponent();
			NotificationIcon.Icon = SystemIcons.Information;
		}

		private void Form1_Load(object sender, EventArgs e) {
			NotificationIcon.Visible = Settings.Default.TrayNotifications;
			txtLogPath.TextChanged += txtLogPath_TextChanged;
			txtLogPath.Click += txtLogPath_Click;
			txtLogPath.Text = Settings.Default.LogPath;
			tsmNotifyMinimizedOnly.Checked = Settings.Default.NotifyMinimizedOnly;
			tsmEnableTrayNotifications.Checked = Settings.Default.TrayNotifications;
		}

		void txtLogPath_Click(object sender, EventArgs e) {
			using(var OFD = new OpenFileDialog()) {
				string CurrPath = txtLogPath.Text ?? Settings.Default.LogPath;
				OFD.Filter = "Log File|Client.txt|All Files (*.*)|*.*";
				if(LogMonitor.IsValidLogPath(CurrPath))
					OFD.InitialDirectory = CurrPath;
				if(OFD.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
					txtLogPath.Text = OFD.FileName;
				}
			}
		}

		void txtLogPath_TextChanged(object sender, EventArgs e) {
			if(LogMonitor.IsValidLogPath(txtLogPath.Text)) {
				txtLogPath.BackColor = Color.FromKnownColor(KnownColor.Window);
				Settings.Default.LogPath = txtLogPath.Text;
				Settings.Default.Save();
			} else {
				txtLogPath.BackColor = Color.DarkRed;
			}
		}

		private void cmdStart_Click(object sender, EventArgs e) {
			if(!LogMonitor.IsValidLogPath(txtLogPath.Text)) {
				MessageBox.Show("The log path you have entered is invalid. Please select the client.txt file located in the PoE folder.");
				return;
			}
			cmdStop.Enabled = true;
			cmdStart.Enabled = false;
			this.Monitor = new LogMonitor(txtLogPath.Text);
			Monitor.BeginMonitoring();
			Monitor.MessageReceived += Monitor_MessageReceived;
		}

		void Monitor_MessageReceived(MessageData obj) {
			if(Settings.Default.NotifyMinimizedOnly && IsPoeActive())
				return;
			Invoke(new Action(() => rtbHistory.AppendText("[" + obj.Date.ToShortTimeString() + "] " + obj.Sender + ": " + obj.Message + "\r\n")));
			if(Settings.Default.TrayNotifications) {
				Invoke(new Action(() => {
					NotificationIcon.Visible = true;
					NotificationIcon.ShowBalloonTip(5000, "Path of Exile Whisper", obj.Sender + ": " + obj.Message, ToolTipIcon.Info);
				}));
			}
		}

		private void cmdStop_Click(object sender, EventArgs e) {
			cmdStart.Enabled = true;
			cmdStop.Enabled = false;
			this.Monitor.StopMonitoring();
			this.Monitor.MessageReceived -= Monitor_MessageReceived;
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
			this.Close();
		}

		private void notifyOnlyWhenMinimizedToolStripMenuItem_Click(object sender, EventArgs e) {
			tsmNotifyMinimizedOnly.Checked = !tsmNotifyMinimizedOnly.Checked;
			Settings.Default.NotifyMinimizedOnly = tsmNotifyMinimizedOnly.Checked;
			Settings.Default.Save();
		}

		private bool IsPoeActive() {
			var hWndFg = GetForegroundWindow();
			uint pid;
			GetWindowThreadProcessId(hWndFg, out pid);
			var FgProc = Process.GetProcessById((int)pid);
			if(FgProc == null)
				return false;
			return FgProc.ProcessName == "PathOfExile";
		}

		private void tsmEnableTrayNotifications_Click(object sender, EventArgs e) {
			tsmEnableTrayNotifications.Checked = !tsmEnableTrayNotifications.Checked;
			Settings.Default.TrayNotifications = tsmEnableTrayNotifications.Checked;
			Settings.Default.Save();
			NotificationIcon.Visible = Settings.Default.TrayNotifications;
		}

		private LogMonitor Monitor;
	}
}
