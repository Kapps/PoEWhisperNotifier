using PoEWhisperNotifier.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace PoEWhisperNotifier {
	/// <summary>
	/// Provides details for how messages should be sent for Smtp notifications.
	/// </summary>
	[Serializable]
	public class SmtpDetails {
		/// <summary>
		/// The server, such as "smtp.gmail.com".
		/// </summary>
		[Description("The address of the SMTP server, such as smtp.gmail.com.")]
		public string SmtpServer { get; set; }
		/// <summary>
		/// The port for the server, defaults to 587.
		/// </summary>
		[Description("The port to use for the SMTP server. This is usually 587.")]
		public ushort SmtpPort { get; set; }
		/// <summary>
		/// The target address to send the email to.
		/// </summary>
		[Description("The email to send notifications to. If you wish to receive text notifications, an example would be: 5551234567@sms.rogers.com (depending on your provider).")]
		public string Target { get; set; }
		/// <summary>
		/// The username to authenticate as for sending email.
		/// </summary>
		[Description("Your username with the mail service (such as Gmail account name)")]
		public string Username { get; set; }
		/// <summary>
		/// Indicates whether the user should only be notified if not at their computer.
		/// </summary>
		[DisplayName("Notify Only If Idle")]
		[Description("If true, sends a notification only if you have not done any input for 2 minutes.")]
		public bool NotifyOnlyIfIdle { get; set; }
		/// <summary>
		/// The password to authenticate with for sending email.
		/// This property encrypts and decrypts a backing field, but since it is stored as a string is vulnerable to being sniffed from memory.
		/// </summary>
		[PasswordPropertyText(true)]
		[XmlIgnore]
		[Description("The password to go with your username for the mail service.")]
		public string Password {
			get {
				if(_EncryptedPassword == null)
					return null;
				var Decrypted = ProtectedData.Unprotect(_EncryptedPassword, PasswordEntropy, DataProtectionScope.CurrentUser);
				return Encoding.UTF8.GetString(Decrypted);
			}
			set {
				if(String.IsNullOrWhiteSpace(value)) {
					this._EncryptedPassword = null;
				} else {
					var Bytes = Encoding.UTF8.GetBytes(value);
					this._EncryptedPassword = ProtectedData.Protect(Bytes, PasswordEntropy, DataProtectionScope.CurrentUser);
				}
			}
		}
		/// <summary>
		/// Indicates whether the current settings may be valid (no missing fields).
		/// </summary>
		[Description("Indicates whether the current settings may be valid (no missing fields). This does not mean the settings are necessarily correct however.")]
		public bool IsValid {
			get {
				return !String.IsNullOrWhiteSpace(Username) && !String.IsNullOrWhiteSpace(Password) && !String.IsNullOrWhiteSpace(Target)
					&& !String.IsNullOrWhiteSpace(SmtpServer) && SmtpPort > 0;
			}
		}
		/// <summary>
		/// Gets the entropy data for storing the encrypted password.
		/// </summary>
		[Browsable(false)]
		public byte[] PasswordEntropy {
			get { return _PasswordEntropy; }
		}

		/// <summary>
		/// Creates a new instance of SmtpDetails with a default generated PasswordEntropy.
		/// </summary>
		public SmtpDetails() {
			this._PasswordEntropy = new byte[32];
			this.SmtpServer = "smtp.gmail.com";
			this.SmtpPort = 587;
			this.NotifyOnlyIfIdle = true;
			using(var RNG = RNGCryptoServiceProvider.Create()) {
				RNG.GetBytes(PasswordEntropy);
			}
		}

		/// <summary>
		/// Saves the current SmtpDetails to the settings file.
		/// </summary>
		public void SaveToSettings() {
			using(var MS = new MemoryStream()) {
				var Formatter = new BinaryFormatter();
				Formatter.Serialize(MS, this);
				var Bytes = MS.ToArray();
				var Base64 = Convert.ToBase64String(Bytes);
				Settings.Default.SerializedSmtpData = Base64;
				Settings.Default.Save();
			}
		}

		/// <summary>
		/// Returns the SmtpDetails currently saved in the settings file.
		/// </summary>
		public static SmtpDetails LoadFromSettings() {
			try {
				var StoredDetailBase64 = Settings.Default.SerializedSmtpData;
				if(String.IsNullOrWhiteSpace(StoredDetailBase64))
					return new SmtpDetails();
				var StoredDetailBytes = Convert.FromBase64String(StoredDetailBase64);
				using(var MS = new MemoryStream(StoredDetailBytes)) {
					var Formatter = new BinaryFormatter();
					return (SmtpDetails)Formatter.Deserialize(MS);
				}
			} catch {
				MessageBox.Show("Failed to deserialize SMTP settings. Reverting to default settings.");
				return new SmtpDetails();
			}
		}

		private byte[] _PasswordEntropy;
		private byte[] _EncryptedPassword;
	}
}
