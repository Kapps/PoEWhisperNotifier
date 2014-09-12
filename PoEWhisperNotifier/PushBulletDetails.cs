using PoEWhisperNotifier.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;

namespace PoEWhisperNotifier {
	/// <summary>
	/// Provides details on how to send a message through Pushbullet.
	/// </summary>
	[Serializable]
	public class PushBulletDetails {
		/// <summary>
		/// Gets or sets the API key to use for sending messages.
		/// </summary>
		[Description("The PushBullet API key to use, requires an account.")]
		public string ApiKey { get; set; }

		/// <summary>
		/// Indicates whether the user should only be notified if not at their computer.
		/// </summary>
		[DisplayName("Notify Only If Idle")]
		[Description("If true, sends a notification only if you have not done any input for 2 minutes.")]
		public bool NotifyOnlyIfIdle { get; set; }

		/// <summary>
		/// Gets whether all fields have potentially valid values.
		/// </summary>
		[Description("Indicates if all of the required fields contain potentially valid data.")]
		public bool IsValid {
			get { return !String.IsNullOrWhiteSpace(ApiKey); }
		}

		/// <summary>
		/// Creates a new instance of this class with NotifyOnlyIfIdle set to true.
		/// </summary>
		public PushBulletDetails() {
			this.NotifyOnlyIfIdle = true;
		}

		/// <summary>
		/// Loads the currently stored Pushbullet settings.
		/// </summary>
		public static PushBulletDetails LoadFromSettings() {
			try {
				var StoredDetailBase64 = Settings.Default.SerializedPushBulletData;
				if(String.IsNullOrWhiteSpace(StoredDetailBase64))
					return new PushBulletDetails();
				var StoredDetailBytes = Convert.FromBase64String(StoredDetailBase64);
				using(var MS = new MemoryStream(StoredDetailBytes)) {
					var Formatter = new BinaryFormatter();
					return (PushBulletDetails)Formatter.Deserialize(MS);
				}
			} catch {
				MessageBox.Show("Failed to deserialize PushBullet settings. Reverting to default settings.");
				return new PushBulletDetails();
			}
		}

		/// <summary>
		/// Saves the current settings to the setting store.
		/// </summary>
		public void SaveToSettings() {
			using(var MS = new MemoryStream()) {
				var Formatter = new BinaryFormatter();
				Formatter.Serialize(MS, this);
				var Bytes = MS.ToArray();
				var Base64 = Convert.ToBase64String(Bytes);
				Settings.Default.SerializedPushBulletData = Base64;
				Settings.Default.Save();
			}
		}
	}
}
