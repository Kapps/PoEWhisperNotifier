using PoEWhisperNotifier.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PoEWhisperNotifier {
	public partial class Main : Form {

		[StructLayout(LayoutKind.Sequential)]
		struct LASTINPUTINFO {
			public static readonly int SizeOf = Marshal.SizeOf(typeof(LASTINPUTINFO));

			[MarshalAs(UnmanagedType.U4)]
			public UInt32 cbSize;
			[MarshalAs(UnmanagedType.U4)]
			public UInt32 dwTime;
		}

		[DllImport("user32.dll")]
		private static extern IntPtr GetForegroundWindow();
		[DllImport("user32.dll")]
		public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
		[DllImport("user32.dll")]
		static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

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
			tsmEnableSMTPNotifications.Checked = Settings.Default.EnableSmtpNotifications;
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
			if(Settings.Default.NotifyMinimizedOnly && IsPoeActive() && !IsUserIdle())
				return;
			string StampedMessage = "[" + obj.Date.ToShortTimeString() + "] " + obj.Sender + ": " + obj.Message + "\r\n";
			Invoke(new Action(() => rtbHistory.AppendText(StampedMessage)));
			if(Settings.Default.TrayNotifications) {
				Invoke(new Action(() => {
					NotificationIcon.Visible = true;
					NotificationIcon.ShowBalloonTip(5000, "Path of Exile Whisper", obj.Sender + ": " + obj.Message, ToolTipIcon.Info);
				}));
			}
			if(Settings.Default.EnableSmtpNotifications) {
				try {
					// Feels wasteful to always reload, but really it should only take a millisecond or less.
					var SmtpSettings = SmtpDetails.LoadFromSettings();
					if(!SmtpSettings.NotifyOnlyIfIdle || IsUserIdle())
						SendSmtpNotification(SmtpSettings, StampedMessage);
				} catch(Exception ex) {
					Invoke(new Action(() => rtbHistory.AppendText("<Failed to send SMTP: " + ex.Message + ">\r\n")));
				}
			}
		}

		private bool IsUserIdle() {
			var LastInput = new LASTINPUTINFO() {
				cbSize = (uint)Marshal.SizeOf(typeof(LASTINPUTINFO)),
				dwTime = 0
			};
			GetLastInputInfo(ref LastInput);
			var IdleTime = (Environment.TickCount - LastInput.dwTime);
			var MinIdleDelay = TimeSpan.FromMinutes(2).TotalMilliseconds;
			// Less than 0 to account for rollover.
			return IdleTime < 0 || IdleTime > MinIdleDelay;
		}

		private void SendSmtpNotification(SmtpDetails SmtpSettings, string StampedMessage) {
			using(var Client = new SmtpClient()) {
				Client.UseDefaultCredentials = false;
				Client.Credentials = new NetworkCredential(SmtpSettings.Username, SmtpSettings.Password);
				Client.Host = SmtpSettings.SmtpServer;
				Client.Port = SmtpSettings.SmtpPort;
				Client.EnableSsl = true;
				Client.DeliveryMethod = SmtpDeliveryMethod.Network;
				string FromAddress = SmtpSettings.Username + "@" + GuessHost(SmtpSettings.SmtpServer);
				using(var Message = new MailMessage(FromAddress, SmtpSettings.Target)) {
					Message.Subject = "Path of Exile Whisper Notification";
					// Limit messages to around 120 characters, some phone providers don't like more.
					Message.Body = StampedMessage;
					if(Message.Body.Length + Message.Subject.Length > 120)
						Message.Body = Message.Body.Substring(0, 120 - Message.Subject.Length) + "..";
					Client.Send(Message);
				}
			};
		}

		private string GuessHost(string SmtpHost) {
			if(!SmtpHost.Contains("."))
				throw new ArgumentException("Invalid SMTP server. Ex: smtp.gmail.com");
			int TldStart = SmtpHost.LastIndexOf('.');
			string TLD = SmtpHost.Substring(TldStart + 1);
			SmtpHost = SmtpHost.Substring(0, TldStart);
			return (SmtpHost.Contains(".") ? SmtpHost.Substring(SmtpHost.LastIndexOf('.') + 1) : SmtpHost) + "." + TLD;
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
			return FgProc.ProcessName.Contains("PathOfExile");
		}

		private void tsmEnableTrayNotifications_Click(object sender, EventArgs e) {
			tsmEnableTrayNotifications.Checked = !tsmEnableTrayNotifications.Checked;
			Settings.Default.TrayNotifications = tsmEnableTrayNotifications.Checked;
			Settings.Default.Save();
			NotificationIcon.Visible = Settings.Default.TrayNotifications;
		}

		private void tsmEnableSMTPNotifications_Click(object sender, EventArgs e) {
			tsmEnableSMTPNotifications.Checked = !tsmEnableSMTPNotifications.Checked;
			Settings.Default.EnableSmtpNotifications = tsmEnableSMTPNotifications.Checked;
			Settings.Default.Save();
			if(Settings.Default.EnableSmtpNotifications) {
				var CurrSettings = SmtpDetails.LoadFromSettings();
				if(!CurrSettings.IsValid)
					ShowSmtpDialog();
			}
		}

		private void configureSMTPToolStripMenuItem_Click(object sender, EventArgs e) {
			ShowSmtpDialog();
		}

		private void ShowSmtpDialog() {
			using(var SmtpDialog = new ConfigureSmtpDialog()) {
				SmtpDialog.ShowDialog();
				if(!SmtpDialog.Details.IsValid && Settings.Default.EnableSmtpNotifications) {
					tsmEnableSMTPNotifications.Checked = false;
					Settings.Default.EnableSmtpNotifications = false;
					Settings.Default.Save();
					MessageBox.Show("Disabled SMTP notifications as your settings are invalid.");
				}
			}
		}

		private DateTime LastSmtp = DateTime.MinValue;
		private LogMonitor Monitor;
	}
}
