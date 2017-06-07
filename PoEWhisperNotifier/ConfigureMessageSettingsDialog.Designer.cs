namespace PoEWhisperNotifier {
	partial class ConfigureMessageSettingsDialog {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.cmdSave = new System.Windows.Forms.Button();
			this.lstMessageTypes = new System.Windows.Forms.ListBox();
			this.propSettings = new System.Windows.Forms.PropertyGrid();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
			this.tableLayoutPanel1.Controls.Add(this.cmdCancel, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.cmdSave, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.lstMessageTypes, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.propSettings, 1, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(741, 495);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// cmdCancel
			// 
			this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.cmdCancel.Location = new System.Drawing.Point(4, 437);
			this.cmdCancel.Margin = new System.Windows.Forms.Padding(4);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size(108, 54);
			this.cmdCancel.TabIndex = 0;
			this.cmdCancel.Text = "Cancel";
			this.cmdCancel.UseVisualStyleBackColor = true;
			this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
			// 
			// cmdSave
			// 
			this.cmdSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.cmdSave.Location = new System.Drawing.Point(629, 437);
			this.cmdSave.Margin = new System.Windows.Forms.Padding(4);
			this.cmdSave.Name = "cmdSave";
			this.cmdSave.Size = new System.Drawing.Size(108, 54);
			this.cmdSave.TabIndex = 1;
			this.cmdSave.Text = "Save";
			this.cmdSave.UseVisualStyleBackColor = true;
			this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
			// 
			// lstMessageTypes
			// 
			this.lstMessageTypes.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lstMessageTypes.FormattingEnabled = true;
			this.lstMessageTypes.IntegralHeight = false;
			this.lstMessageTypes.ItemHeight = 20;
			this.lstMessageTypes.Location = new System.Drawing.Point(3, 3);
			this.lstMessageTypes.Name = "lstMessageTypes";
			this.lstMessageTypes.Size = new System.Drawing.Size(142, 427);
			this.lstMessageTypes.TabIndex = 2;
			this.lstMessageTypes.SelectedIndexChanged += new System.EventHandler(this.lstMessageTypes_SelectedIndexChanged);
			// 
			// propSettings
			// 
			this.propSettings.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propSettings.LineColor = System.Drawing.SystemColors.ControlDark;
			this.propSettings.Location = new System.Drawing.Point(151, 3);
			this.propSettings.Name = "propSettings";
			this.propSettings.Size = new System.Drawing.Size(587, 427);
			this.propSettings.TabIndex = 3;
			this.propSettings.ToolbarVisible = false;
			// 
			// ConfigureMessageSettingsDialog
			// 
			this.AcceptButton = this.cmdSave;
			this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.CancelButton = this.cmdCancel;
			this.ClientSize = new System.Drawing.Size(741, 495);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "ConfigureMessageSettingsDialog";
			this.Text = "Configure Message Settings";
			this.Load += new System.EventHandler(this.ConfigureMessageSettingsDialog_Load);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.Button cmdSave;
		private System.Windows.Forms.ListBox lstMessageTypes;
		private System.Windows.Forms.PropertyGrid propSettings;
	}
}