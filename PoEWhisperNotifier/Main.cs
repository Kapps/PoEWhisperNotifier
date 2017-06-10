using PoEWhisperNotifier.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PoEWhisperNotifier {
	public partial class Main : Form {
		[DllImport("user32.dll")]
		private static extern IntPtr GetForegroundWindow();
		[DllImport("user32.dll")]
		public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);


		private bool IsMonitoring {
			get { return this.Monitor != null && this.Monitor.IsMonitoring; }
		}

		public Main() {
			InitializeComponent();
			NotificationIcon.Icon = this.Icon;

			// Have to initialize in the ctor in order to handle Load with Minimize to Tray.
			this.MessageConfig = MessageSettings.LoadFromConfig();
			NotificationIcon.Visible = Settings.Default.TrayNotifications || Settings.Default.MinimizeToTray;
			NotificationIcon.BalloonTipClicked += NotificationIconClick;
			NotificationIcon.DoubleClick += NotificationIconClick;
			txtLogPath.TextChanged += txtLogPath_TextChanged;
			txtLogPath.Click += txtLogPath_Click;
			txtLogPath.Text = Settings.Default.LogPath;
			// TODO: Most of these could be easily replaced with a method to map the toolstrip to the setting.
			tsmNotifyMinimizedOnly.Checked = Settings.Default.NotifyMinimizedOnly;
			tsmEnableTrayNotifications.Checked = Settings.Default.TrayNotifications;
			tsmEnableSMTPNotifications.Checked = Settings.Default.EnableSmtpNotifications;
			tsmEnablePushBullet.Checked = Settings.Default.EnablePushbullet;
			tsmEnableSound.Checked = Settings.Default.EnableSound;
			tsmMinimizeToTray.Checked = Settings.Default.MinimizeToTray;
			tsmStartMinimized.Checked = Settings.Default.StartMinimized;
			RestoreSize();
			this.Resize += Main_Resize;
			if (!LogMonitor.IsValidLogPath(txtLogPath.Text)) {
				if (LogMonitor.TryGetDefaultLogPath(out string DefaultLogPath))
					txtLogPath.Text = DefaultLogPath;
				else
					LogMessage("Unable to figure out client.txt location. You will have to manually set the path.", null, LogMessageType.Status);
			}
			this.ResizeEnd += OnResizeEnd;
		}

		private void RestoreSize() {
			if (Settings.Default.PreviousSize.Width > 50 && Settings.Default.PreviousSize.Height > 50) {
				this.StartPosition = FormStartPosition.Manual;
				this.Location = Settings.Default.PreviousLocation;
				this.Size = Settings.Default.PreviousSize;
			}
		}

		private void OnResizeEnd(object sender, EventArgs e) {
			// This event is called even when the user is only moving the window, not just resizing.
			Settings.Default.PreviousSize = this.Size;
			Settings.Default.PreviousLocation = this.Location;
			Settings.Default.Save();
		}

		private void NotificationIconClick(object sender, EventArgs e) {
			this.Visible = true;
			this.WindowState = FormWindowState.Normal;
			this.Show();
		}

		void Main_Resize(object sender, EventArgs e) {
			if (Settings.Default.MinimizeToTray) {
				if (this.WindowState == FormWindowState.Minimized) {
					this.Visible = false;
					NotificationIcon.Visible = true;
				} else {
					this.Visible = true;
				}
			}
		}

		void txtLogPath_Click(object sender, EventArgs e) {
			using (var OFD = new OpenFileDialog()) {
				string CurrPath = txtLogPath.Text ?? Settings.Default.LogPath;
				OFD.Filter = "Log File|Client.txt|All Files (*.*)|*.*";
				if (LogMonitor.IsValidLogPath(CurrPath))
					OFD.InitialDirectory = CurrPath;
				if (OFD.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
					txtLogPath.Text = OFD.FileName;
				}
			}
		}

		void txtLogPath_TextChanged(object sender, EventArgs e) {
			if (LogMonitor.IsValidLogPath(txtLogPath.Text)) {
				txtLogPath.BackColor = Color.FromKnownColor(KnownColor.Window);
				Settings.Default.LogPath = txtLogPath.Text;
				Settings.Default.Save();
			} else {
				txtLogPath.BackColor = Color.DarkRed;
			}
		}

		private void cmdStart_Click(object sender, EventArgs e) {
			StartMonitoring(false);
		}

		private void StartMonitoring(bool AutoStarted) {
			if (!LogMonitor.IsValidLogPath(txtLogPath.Text)) {
				string ErrMsg = "Failed to start " + (AutoStarted ? "automatically " : "") + "as the log path is invalid.";
				if (AutoStarted)
					LogMessage(ErrMsg, null, LogMessageType.Status);
				else
					MessageBox.Show(ErrMsg);
				return;
			}
			if (new FileInfo(txtLogPath.Text).IsReadOnly)
				LogMessage("Warning: Your client.txt file appears to be readonly. This will likely prevent the program from working.", null, LogMessageType.Status);
			cmdStop.Enabled = true;
			cmdStart.Enabled = false;
			this.Monitor = new LogMonitor(txtLogPath.Text);
			Monitor.BeginMonitoring();
			Monitor.MessageReceived += ProcessMessage;
			IdleManager.BeginMonitoring();
			LogMessage("Program started at " + DateTime.Now.ToShortTimeString() + ".", null, LogMessageType.Status);
		}

		private void StopMonitoring() {
			cmdStart.Enabled = true;
			cmdStop.Enabled = false;
			this.Monitor.StopMonitoring();
			IdleManager.StopMonitoring();
			this.Monitor.MessageReceived -= ProcessMessage;
			LogMessage("Program stopped at " + DateTime.Now.ToShortTimeString() + ".", null, LogMessageType.Status);
		}

		private void testNotificationToolStripMenuItem_Click(object sender, EventArgs e) {
			var Message = new MessageData(DateTime.Now, "Tester", "This is a fake whisper for testing notifications.", LogMessageType.Whisper);
			HandleMessage(Message, true);
		}

		void ProcessMessage(MessageData obj) {
			Invoke(new Action(() => HandleMessage(obj, false)));
		}

		private void PlayNotificationsForMessage(MessageData Message, bool AssumeInactive) {
			if (Settings.Default.NotifyMinimizedOnly && IsPoeActive()) {
				if (!IdleManager.IsUserIdle) {
					// If the user isn't yet idle, replay the message if they do go idle.
					IdleManager.AddIdleAction(() => PlayNotificationsForMessage(Message, AssumeInactive));
					return;
				}
				// Otherwise, they are idle, so process the message anyways.
			}
			if (Settings.Default.TrayNotifications) {
				NotificationIcon.Visible = true;
				NotificationIcon.ShowBalloonTip(5000, Message.Title, (Message.Sender == null ? "" : (Message.Sender + ": ")) + Message.Message, ToolTipIcon.Info);
			}
			if (Settings.Default.EnableSound) {
				try {
					this.SoundPlayer.Play();
				} catch (Exception ex) {
					LogMessage("<Error playing sound. This usually occurs due to the Content folder being missing.\r\n  Additional Info: " + ex.Message + ">", null, LogMessageType.Status);
				}
			}
			if (Settings.Default.EnableSmtpNotifications) {
				var SmtpSettings = SmtpDetails.LoadFromSettings();
				var SmtpAct = CheckedAction("SMTP", () => SendSmtpNotification(SmtpSettings, Message.DisplayMessage));
				if (!SmtpSettings.NotifyOnlyIfIdle || AssumeInactive)
					SmtpAct();
				else
					IdleManager.AddIdleAction(SmtpAct);
			}
			if (Settings.Default.EnablePushbullet) {
				var PbSettings = PushBulletDetails.LoadFromSettings();
				Regex Pattern = null;
				Match Matches = null;
				if (!String.IsNullOrWhiteSpace(PbSettings.NotifyOnlyIfMatches)) {
					Pattern = new Regex(PbSettings.NotifyOnlyIfMatches);
					Matches = Pattern.Match(Message.Message);
				}
				if (Pattern == null || ((Pattern != null) && Matches.Success)) {
					var PbAct = CheckedAction("PushBullet", () => {
						var Client = new PushBulletClient(PbSettings);
						Client.SendPush(Message.Title, Message.DisplayMessage);
					});
					if (AssumeInactive || !PbSettings.NotifyOnlyIfIdle)
						PbAct();
					else
						IdleManager.AddIdleAction(PbAct);
				}
			}
			if (Settings.Default.FlashTaskbar && (!IsPoeActive() || AssumeInactive)) {
				var PoeProcess = GetPoeProcess();
				if (PoeProcess != null) {
					var FlashAct = CheckedAction("Taskbar Flash", () => WindowFlasher.FlashWindow(PoeProcess.MainWindowHandle, FlashStyle.All));
					FlashAct();
				} else {
					LogMessage("<Could not find the PoE process to flash the taskbar>", null, LogMessageType.Status);
				}
			}
		}

		private void HandleMessage(MessageData Message, bool AssumeInactive) {
			var Config = MessageConfig.Single(c => c.MessageType == Message.MessageType);
			if (Config.ExcludeFilter != null && Config.ExcludeFilter.Any(c => Message.Message.Contains(c)))
				return;
			if (Config.IncludeFilter != null && Config.IncludeFilter.Any() && !Config.IncludeFilter.Any(c => Message.Message.Contains(c)))
				return;
			if(Config.LogMessage)
				LogMessage(Message.Prefix, Message.Message, Message.MessageType);
			if (!Config.PlayNotification)
				return;
			PlayNotificationsForMessage(Message, AssumeInactive);
		}

		// Wraps an Action in a try-catch and appends to the history any errors with it.
		private Action CheckedAction(string Task, Action Act) {
			return () => {
				try {
					Act();
				} catch (Exception ex) {
					Invoke(new Action(() => LogMessage("<Failed to send " + Task + " notification: " + ex.Message + ">", null, LogMessageType.Status)));
				}
			};
		}

		private void LogMessage(string Prefix, string Message, LogMessageType MessageType) {
			var Config = MessageConfig.Single(c => c.MessageType == MessageType);
			Color StandardColor = Color.FromArgb(239, 239, 239);
			Font MessageFont = null;
			if ((IdleManager.IsUserIdle || !IsPoeActive()) && new[] { LogMessageType.Whisper, LogMessageType.Party, LogMessageType.Disconnect }.Contains(MessageType))
				MessageFont = new Font(rtbHistory.Font.FontFamily, rtbHistory.Font.Size + 2, FontStyle.Bold);

			if (!String.IsNullOrWhiteSpace(Prefix))
				LogMessagePart(Prefix, Config.ForegroundColor, MessageFont);
			if (!String.IsNullOrWhiteSpace(Message))
				LogMessagePart(": " + Message, StandardColor, MessageFont);
			LogMessagePart("\r\n", StandardColor, null);
		}

		private void LogMessagePart(string Part, Color ForegroundColor, Font Font) {
			int Start = rtbHistory.TextLength;
			rtbHistory.AppendText(Part);
			int End = rtbHistory.TextLength;
			rtbHistory.Select(Start, End - Start);
			rtbHistory.SelectionColor = ForegroundColor;
			if (Font != null)
				rtbHistory.SelectionFont = Font;
			else
				rtbHistory.SelectionFont = rtbHistory.Font;
			rtbHistory.SelectionLength = 0;
		}

		private void SendSmtpNotification(SmtpDetails SmtpSettings, string StampedMessage) {
			using (var Client = new SmtpClient()) {
				Client.UseDefaultCredentials = false;
				Client.Credentials = new NetworkCredential(SmtpSettings.Username, SmtpSettings.Password);
				Client.Host = SmtpSettings.SmtpServer;
				Client.Port = SmtpSettings.SmtpPort;
				Client.EnableSsl = true;
				Client.DeliveryMethod = SmtpDeliveryMethod.Network;
				using (var Message = new MailMessage(SmtpSettings.FromEmail, SmtpSettings.Target)) {
					Message.Subject = "Path of Exile Whisper Notification";
					// Limit messages to around 120 characters, some phone providers don't like more.
					Message.Body = StampedMessage;
					if (Message.Body.Length + Message.Subject.Length > 120)
						Message.Body = Message.Body.Substring(0, 120 - Message.Subject.Length) + "..";
					Client.Send(Message);
				}
			};
		}

		private void cmdStop_Click(object sender, EventArgs e) {
			StopMonitoring();
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
			this.Close();
		}

		private void Main_Load(object sender, EventArgs e) {
			if (Settings.Default.MinimizeToTray) {
				ThreadPool.QueueUserWorkItem((_) => {
					Thread.Sleep(100); // HACK: Work around issue where minimizing to tray keeps icon in taskbar.
					Invoke(new Action(() => {
						this.WindowState = FormWindowState.Minimized;
						this.Visible = false;
						NotificationIcon.Visible = true;
						NotificationIcon.ShowBalloonTip(2000, "Whisper Notifier Started Minimized", "PoEWhisperNotifier started minimized. Double click the icon to show the window.", ToolTipIcon.Info);
					}));
				});
			} else if(Settings.Default.StartMinimized) {
				this.WindowState = FormWindowState.Minimized;
			} else {
				this.WindowState = FormWindowState.Normal;
			}

			StartMonitoring(true);
		}

		private void notifyOnlyWhenMinimizedToolStripMenuItem_Click(object sender, EventArgs e) {
			tsmNotifyMinimizedOnly.Checked = !tsmNotifyMinimizedOnly.Checked;
			Settings.Default.NotifyMinimizedOnly = tsmNotifyMinimizedOnly.Checked;
			Settings.Default.Save();
		}

		private bool IsPoeActive() {
			var hWndFg = GetForegroundWindow();
			GetWindowThreadProcessId(hWndFg, out uint pid);
			var FgProc = Process.GetProcessById((int)pid);
			if (FgProc == null)
				return false;
			return FgProc.ProcessName.Contains("PathOfExile");
		}

		/// <summary>
		/// Helper function to try and get the PoE process, if it's currently running.
		/// </summary>
		internal static Process GetPoeProcess() {
			// This doesn't belong in Main, but oh well.
			Func<Process, bool> IsPoE = (c => c.MainWindowTitle.Equals("Path of Exile", StringComparison.InvariantCultureIgnoreCase) && c.ProcessName.Contains("PathOfExile"));
			return Process.GetProcesses().FirstOrDefault(IsPoE);
		}

		private void tsmEnableTrayNotifications_Click(object sender, EventArgs e) {
			tsmEnableTrayNotifications.Checked = !tsmEnableTrayNotifications.Checked;
			Settings.Default.TrayNotifications = tsmEnableTrayNotifications.Checked;
			Settings.Default.Save();
			NotificationIcon.Visible = Settings.Default.TrayNotifications || Settings.Default.MinimizeToTray;
		}

		private void tsmEnableSound_Click(object sender, EventArgs e) {
			tsmEnableSound.Checked = !tsmEnableSound.Checked;
			Settings.Default.EnableSound = tsmEnableSound.Checked;
			Settings.Default.Save();
		}

		private void tsmStartMinimized_Click(object sender, EventArgs e) {
			tsmStartMinimized.Checked = !tsmStartMinimized.Checked;
			Settings.Default.StartMinimized = tsmStartMinimized.Checked;
			Settings.Default.Save();
		}

		private void tsmMinimizeToTray_Click(object sender, EventArgs e) {
			tsmMinimizeToTray.Checked = !tsmMinimizeToTray.Checked;
			Settings.Default.MinimizeToTray = tsmMinimizeToTray.Checked;
			Settings.Default.Save();
			NotificationIcon.Visible = Settings.Default.TrayNotifications || Settings.Default.MinimizeToTray;
		}

		// TODO: Merge common functionality below, and ideally of most of these enable buttons too.

		private void tsmEnablePushbullet_Click(object sender, EventArgs e) {
			tsmEnablePushBullet.Checked = !tsmEnablePushBullet.Checked;
			Settings.Default.EnablePushbullet = tsmEnablePushBullet.Checked;
			Settings.Default.Save();
			if (Settings.Default.EnablePushbullet) {
				var CurrSettings = PushBulletDetails.LoadFromSettings();
				if (!CurrSettings.IsValid)
					ShowPushBulletDialog();
			}
		}

		private void configurePushBulletToolStripMenuItem_Click(object sender, EventArgs e) {
			ShowPushBulletDialog();
		}

		private void tsmEnableSMTPNotifications_Click(object sender, EventArgs e) {
			tsmEnableSMTPNotifications.Checked = !tsmEnableSMTPNotifications.Checked;
			Settings.Default.EnableSmtpNotifications = tsmEnableSMTPNotifications.Checked;
			Settings.Default.Save();
			if (Settings.Default.EnableSmtpNotifications) {
				var CurrSettings = SmtpDetails.LoadFromSettings();
				if (!CurrSettings.IsValid)
					ShowSmtpDialog();
			}
		}

		private void configureSMTPToolStripMenuItem_Click(object sender, EventArgs e) {
			ShowSmtpDialog();
		}

		private void trimClienttxtToolStripMenuItem_Click(object sender, EventArgs e) {
			string LogPath = txtLogPath.Text;
			if (!LogMonitor.IsValidLogPath(LogPath)) {
				MessageBox.Show("You must select a valid client.txt first.");
				return;
			}
			var LogLength = new FileInfo(LogPath).Length;
			long DesiredLength = 10 * 1024 * 1024;
			if (LogLength <= DesiredLength) {
				MessageBox.Show("Your client.txt is already below 10MB. No action has been taken.");
				return;
			}
			if (MessageBox.Show("This will remove old data from your client.txt (currently " + LogLength / (1024 * 1024) + "MB) to reduce it to 10MB. Are you sure you wish to do this? This process is NOT reversible.", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) {
				return;
			}
			if (GetPoeProcess() != null) {
				MessageBox.Show("You must close Path of Exile for this operation to work.", "Failed to Trim Log", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			bool RestartLog = Monitor.IsMonitoring;
			if (Monitor.IsMonitoring)
				StopMonitoring();
			try {
				string CopyLocation = LogPath + ".new";
				string BackupLocation = LogPath + ".old";
				if (File.Exists(BackupLocation)) {
					MessageBox.Show("A log file already appears to exist from before. Please delete it if it is not valid.");
					Process.Start(new ProcessStartInfo() {
						UseShellExecute = true,
						Verb = "open",
						FileName = Path.GetDirectoryName(BackupLocation)
					});
					return;
				}
				using (var LogFile = File.Open(LogPath, FileMode.Open, FileAccess.ReadWrite, FileShare.Delete)) {
					LogFile.Seek(-DesiredLength, SeekOrigin.End);
					if (File.Exists(CopyLocation))
						File.Delete(CopyLocation);
					using (var OutFile = File.CreateText(CopyLocation))
						LogFile.CopyTo(OutFile.BaseStream);
					File.Move(LogPath, BackupLocation);
					File.Move(CopyLocation, LogPath);
					File.Delete(BackupLocation);
				}
				LogMessage("Trimmed log file from " + (LogLength / (1024 * 1024)) + "MB to 10MB.", null, LogMessageType.Status);
				MessageBox.Show("Done. Your log file has been trimmed.");
			} catch (Exception ex) {
				MessageBox.Show("Failed to trim log file:\r\n\t" + ex.Message.Replace("\n", "\n\t") + "\r\nIf your client.txt was modified, you may find a backup at client.txt.old.");
			}
			if (RestartLog)
				StartMonitoring(false);
		}

		private void ShowPushBulletDialog() {
			using (var PbDialog = new ConfigurePushBulletDialog()) {
				PbDialog.ShowDialog();
				if (!PbDialog.Details.IsValid) {
					tsmEnablePushBullet.Checked = false;
					Settings.Default.EnablePushbullet = false;
					Settings.Default.Save();
					MessageBox.Show("Disabled PushBullet notifications as your settings are invalid.");
				}
			}
		}

		private void ShowSmtpDialog() {
			using (var SmtpDialog = new ConfigureSmtpDialog()) {
				SmtpDialog.ShowDialog();
				if (!SmtpDialog.Details.IsValid && Settings.Default.EnableSmtpNotifications) {
					tsmEnableSMTPNotifications.Checked = false;
					Settings.Default.EnableSmtpNotifications = false;
					Settings.Default.Save();
					MessageBox.Show("Disabled SMTP notifications as your settings are invalid.");
				}
			}
		}

		private void configureMessageTypesToolStripMenuItem_Click(object sender, EventArgs e) {
			using (var Dialog = new ConfigureMessageSettingsDialog())
				if (Dialog.ShowDialog() == DialogResult.OK)
					this.MessageConfig = MessageSettings.LoadFromConfig();
		}

		private SoundPlayer SoundPlayer = new SoundPlayer("Content\\notify.wav");
		private LogMonitor Monitor;
		private MessageSettings[] MessageConfig;
	}
}