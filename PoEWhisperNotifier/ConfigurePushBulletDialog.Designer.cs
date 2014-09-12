namespace PoEWhisperNotifier {
	partial class ConfigurePushBulletDialog {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.propSettings = new System.Windows.Forms.PropertyGrid();
			this.SuspendLayout();
			// 
			// propSettings
			// 
			this.propSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.propSettings.Location = new System.Drawing.Point(13, 13);
			this.propSettings.Margin = new System.Windows.Forms.Padding(4);
			this.propSettings.Name = "propSettings";
			this.propSettings.PropertySort = System.Windows.Forms.PropertySort.NoSort;
			this.propSettings.Size = new System.Drawing.Size(1193, 529);
			this.propSettings.TabIndex = 1;
			// 
			// ConfigurePushBulletDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1219, 555);
			this.Controls.Add(this.propSettings);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Name = "ConfigurePushBulletDialog";
			this.Text = "Configure PushBullet";
			this.Load += new System.EventHandler(this.ConfigurePushBulletDialog_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PropertyGrid propSettings;
	}
}