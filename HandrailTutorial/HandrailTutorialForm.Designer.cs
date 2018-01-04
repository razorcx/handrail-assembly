namespace HandrailTutorial
{
	partial class HandrailTutorialForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HandrailTutorialForm));
			this.comboBoxHandrailSide = new System.Windows.Forms.ComboBox();
			this.buttonSelectBeam = new System.Windows.Forms.Button();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// comboBoxHandrailSide
			// 
			this.comboBoxHandrailSide.FormattingEnabled = true;
			this.comboBoxHandrailSide.Items.AddRange(new object[] {
            "LEFT",
            "RIGHT"});
			this.comboBoxHandrailSide.Location = new System.Drawing.Point(49, 87);
			this.comboBoxHandrailSide.Name = "comboBoxHandrailSide";
			this.comboBoxHandrailSide.Size = new System.Drawing.Size(160, 24);
			this.comboBoxHandrailSide.TabIndex = 16;
			this.comboBoxHandrailSide.Text = "LEFT";
			// 
			// buttonSelectBeam
			// 
			this.buttonSelectBeam.Location = new System.Drawing.Point(49, 117);
			this.buttonSelectBeam.Name = "buttonSelectBeam";
			this.buttonSelectBeam.Size = new System.Drawing.Size(160, 38);
			this.buttonSelectBeam.TabIndex = 15;
			this.buttonSelectBeam.Text = "Select Beam";
			this.buttonSelectBeam.UseVisualStyleBackColor = true;
			this.buttonSelectBeam.Click += new System.EventHandler(this.buttonSelectBeam_Click);
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::HandrailTutorial.Properties.Resources.Logo;
			this.pictureBox1.Location = new System.Drawing.Point(49, 12);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(160, 41);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox1.TabIndex = 17;
			this.pictureBox1.TabStop = false;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(49, 64);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(76, 17);
			this.label1.TabIndex = 18;
			this.label1.Text = "Beam Side";
			// 
			// HandrailTutorialForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(253, 182);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.comboBoxHandrailSide);
			this.Controls.Add(this.buttonSelectBeam);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "HandrailTutorialForm";
			this.Text = "Handrail Tutorial";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.HandrailTutorialForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox comboBoxHandrailSide;
		private System.Windows.Forms.Button buttonSelectBeam;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label label1;
	}
}

