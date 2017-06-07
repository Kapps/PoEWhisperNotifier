using Newtonsoft.Json;
using PoEWhisperNotifier.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PoEWhisperNotifier {
	/// <summary>
	/// A dialog that allows the user to configure their settings for individual message types.
	/// </summary>
	public partial class ConfigureMessageSettingsDialog : Form {
		/// <summary>
		/// Creates a new instance of this dialog.
		/// </summary>
		public ConfigureMessageSettingsDialog() {
			InitializeComponent();
		}

		private void lstMessageTypes_SelectedIndexChanged(object sender, EventArgs e) {
			int Index = lstMessageTypes.SelectedIndex;
			if (Index < 0 || Index >= lstMessageTypes.Items.Count)
				propSettings.SelectedObject = null;
			else
				propSettings.SelectedObject = lstMessageTypes.Items[lstMessageTypes.SelectedIndex];
		}

		private void ConfigureMessageSettingsDialog_Load(object sender, EventArgs e) {
			this.Config = MessageSettings.LoadFromConfig();
			foreach (var Setting in Config.Where(c=>c.MessageType != LogMessageType.Unknown && c.MessageType != LogMessageType.Status && c.MessageType != LogMessageType.Disconnect))
				lstMessageTypes.Items.Add(Setting);
		}

		private void cmdCancel_Click(object sender, EventArgs e) {
			this.DialogResult = DialogResult.Cancel;
		}

		private void cmdSave_Click(object sender, EventArgs e) {
			MessageSettings.SaveToConfig(this.Config);
			this.DialogResult = DialogResult.OK;
		}

		private MessageSettings[] Config;
	}
}
