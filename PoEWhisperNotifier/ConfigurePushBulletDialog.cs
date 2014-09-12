using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PoEWhisperNotifier {
	/// <summary>
	/// Provides a dialog to allow the user to configure and save their PushBullet account settings.
	/// </summary>
	public partial class ConfigurePushBulletDialog : Form {
		/// <summary>
		/// Gets the details the user has entered.
		/// </summary>
		public PushBulletDetails Details { get; private set; }

		public ConfigurePushBulletDialog() {
			InitializeComponent();
		}

		private void ConfigurePushBulletDialog_Load(object sender, EventArgs e) {
			this.Details = PushBulletDetails.LoadFromSettings();
			propSettings.SelectedObject = Details;
			propSettings.PropertyValueChanged += propSettings_PropertyValueChanged;
		}

		void propSettings_PropertyValueChanged(object s, PropertyValueChangedEventArgs e) {
			Details.SaveToSettings();
		}
	}
}
