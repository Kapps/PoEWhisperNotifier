using PoEWhisperNotifier.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace PoEWhisperNotifier {
	/// <summary>
	/// A dialog to allow the user to enter their Smtp details, saved to the local settings file.
	/// </summary>
	public partial class ConfigureSmtpDialog : Form {
		/// <summary>
		/// Gets the resulting details entered by the user.
		/// </summary>
		public SmtpDetails Details { get; private set; }

		public ConfigureSmtpDialog() {
			InitializeComponent();
		}

		private void ConfigureSmtpDialog_Load(object sender, EventArgs e) {
			this.Details = SmtpDetails.LoadFromSettings();
			propSettings.SelectedObject = Details;
			propSettings.PropertyValueChanged += propSettings_PropertyValueChanged;
		}

		void propSettings_PropertyValueChanged(object s, PropertyValueChangedEventArgs e) {
			Details.SaveToSettings();
		}
	}
}
