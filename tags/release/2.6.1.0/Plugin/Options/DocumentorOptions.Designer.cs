namespace CR_Documentor.Options
{
	partial class DocumentorOptions
	{
		// Form controls
		// Doc Tag Compatibility controls
		private System.Windows.Forms.GroupBox grpDocTagCompat;
		private System.Windows.Forms.RadioButton rbDocTagMSStrict;
		private System.Windows.Forms.RadioButton rbDocTagNDoc13;

		// Unrecognized Tag Handling controls
		private System.Windows.Forms.GroupBox grpUnrecognizedTags;
		private System.Windows.Forms.RadioButton rbUTHideTagAndContents;
		private System.Windows.Forms.RadioButton rbUTStripTagShowContents;
		private System.Windows.Forms.RadioButton rbUTHighlightUnknown;
		private System.Windows.Forms.RadioButton rbUTRenderContents;

		// Formatting Options
		private System.Windows.Forms.GroupBox grpFormatOptions;
		private System.Windows.Forms.CheckBox chkTabsToSpaces;
		private System.Windows.Forms.Label lblTxt2Space1;
		private System.Windows.Forms.NumericUpDown numSpacesPerTab;
		private System.Windows.Forms.Label lblTxt2Space2;
		private System.Windows.Forms.CheckBox chkProcessDuplicateSeeLinks;
		private System.Windows.Forms.GroupBox grpDisplay;
		private System.Windows.Forms.CheckBox chkShowToolbar;
		private System.Windows.Forms.GroupBox grpPreviewStyle;
		private System.Windows.Forms.RadioButton rbPreviewStyleNDoc13;
		private System.Windows.Forms.RadioButton rbPreviewStyleSandcastle;
		private System.Windows.Forms.RadioButton rbDocTagSandcastle;
		private System.Windows.Forms.GroupBox grpIncludes;
		private System.Windows.Forms.RadioButton rbIncludeRelative;
		private System.Windows.Forms.RadioButton rbIncludeAbsolute;
		private System.Windows.Forms.RadioButton rbIncludeNone;
		private System.Windows.Forms.GroupBox grpServerOptions;
		private System.Windows.Forms.Label lblServerPort;
		private System.Windows.Forms.NumericUpDown serverPort;

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DocumentorOptions));
			this.rbDocTagMSStrict = new System.Windows.Forms.RadioButton();
			this.grpDocTagCompat = new System.Windows.Forms.GroupBox();
			this.rbDocTagSandcastle = new System.Windows.Forms.RadioButton();
			this.rbDocTagNDoc13 = new System.Windows.Forms.RadioButton();
			this.grpUnrecognizedTags = new System.Windows.Forms.GroupBox();
			this.rbUTRenderContents = new System.Windows.Forms.RadioButton();
			this.rbUTHighlightUnknown = new System.Windows.Forms.RadioButton();
			this.rbUTStripTagShowContents = new System.Windows.Forms.RadioButton();
			this.rbUTHideTagAndContents = new System.Windows.Forms.RadioButton();
			this.grpFormatOptions = new System.Windows.Forms.GroupBox();
			this.chkProcessDuplicateSeeLinks = new System.Windows.Forms.CheckBox();
			this.lblTxt2Space2 = new System.Windows.Forms.Label();
			this.numSpacesPerTab = new System.Windows.Forms.NumericUpDown();
			this.lblTxt2Space1 = new System.Windows.Forms.Label();
			this.chkTabsToSpaces = new System.Windows.Forms.CheckBox();
			this.grpDisplay = new System.Windows.Forms.GroupBox();
			this.chkShowToolbar = new System.Windows.Forms.CheckBox();
			this.grpPreviewStyle = new System.Windows.Forms.GroupBox();
			this.rbPreviewStyleSandcastle = new System.Windows.Forms.RadioButton();
			this.rbPreviewStyleNDoc13 = new System.Windows.Forms.RadioButton();
			this.grpIncludes = new System.Windows.Forms.GroupBox();
			this.rbIncludeRelative = new System.Windows.Forms.RadioButton();
			this.rbIncludeAbsolute = new System.Windows.Forms.RadioButton();
			this.rbIncludeNone = new System.Windows.Forms.RadioButton();
			this.grpServerOptions = new System.Windows.Forms.GroupBox();
			this.serverPort = new System.Windows.Forms.NumericUpDown();
			this.lblServerPort = new System.Windows.Forms.Label();
			this.grpDocTagCompat.SuspendLayout();
			this.grpUnrecognizedTags.SuspendLayout();
			this.grpFormatOptions.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numSpacesPerTab)).BeginInit();
			this.grpDisplay.SuspendLayout();
			this.grpPreviewStyle.SuspendLayout();
			this.grpIncludes.SuspendLayout();
			this.grpServerOptions.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.serverPort)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			this.SuspendLayout();
			// 
			// rbDocTagMSStrict
			// 
			resources.ApplyResources(this.rbDocTagMSStrict, "rbDocTagMSStrict");
			this.rbDocTagMSStrict.Name = "rbDocTagMSStrict";
			// 
			// grpDocTagCompat
			// 
			this.grpDocTagCompat.Controls.Add(this.rbDocTagSandcastle);
			this.grpDocTagCompat.Controls.Add(this.rbDocTagNDoc13);
			this.grpDocTagCompat.Controls.Add(this.rbDocTagMSStrict);
			resources.ApplyResources(this.grpDocTagCompat, "grpDocTagCompat");
			this.grpDocTagCompat.Name = "grpDocTagCompat";
			this.grpDocTagCompat.TabStop = false;
			// 
			// rbDocTagSandcastle
			// 
			resources.ApplyResources(this.rbDocTagSandcastle, "rbDocTagSandcastle");
			this.rbDocTagSandcastle.Name = "rbDocTagSandcastle";
			// 
			// rbDocTagNDoc13
			// 
			resources.ApplyResources(this.rbDocTagNDoc13, "rbDocTagNDoc13");
			this.rbDocTagNDoc13.Name = "rbDocTagNDoc13";
			// 
			// grpUnrecognizedTags
			// 
			this.grpUnrecognizedTags.Controls.Add(this.rbUTRenderContents);
			this.grpUnrecognizedTags.Controls.Add(this.rbUTHighlightUnknown);
			this.grpUnrecognizedTags.Controls.Add(this.rbUTStripTagShowContents);
			this.grpUnrecognizedTags.Controls.Add(this.rbUTHideTagAndContents);
			resources.ApplyResources(this.grpUnrecognizedTags, "grpUnrecognizedTags");
			this.grpUnrecognizedTags.Name = "grpUnrecognizedTags";
			this.grpUnrecognizedTags.TabStop = false;
			// 
			// rbUTRenderContents
			// 
			resources.ApplyResources(this.rbUTRenderContents, "rbUTRenderContents");
			this.rbUTRenderContents.Name = "rbUTRenderContents";
			// 
			// rbUTHighlightUnknown
			// 
			resources.ApplyResources(this.rbUTHighlightUnknown, "rbUTHighlightUnknown");
			this.rbUTHighlightUnknown.Name = "rbUTHighlightUnknown";
			// 
			// rbUTStripTagShowContents
			// 
			resources.ApplyResources(this.rbUTStripTagShowContents, "rbUTStripTagShowContents");
			this.rbUTStripTagShowContents.Name = "rbUTStripTagShowContents";
			// 
			// rbUTHideTagAndContents
			// 
			resources.ApplyResources(this.rbUTHideTagAndContents, "rbUTHideTagAndContents");
			this.rbUTHideTagAndContents.Name = "rbUTHideTagAndContents";
			// 
			// grpFormatOptions
			// 
			this.grpFormatOptions.Controls.Add(this.chkProcessDuplicateSeeLinks);
			this.grpFormatOptions.Controls.Add(this.lblTxt2Space2);
			this.grpFormatOptions.Controls.Add(this.numSpacesPerTab);
			this.grpFormatOptions.Controls.Add(this.lblTxt2Space1);
			this.grpFormatOptions.Controls.Add(this.chkTabsToSpaces);
			resources.ApplyResources(this.grpFormatOptions, "grpFormatOptions");
			this.grpFormatOptions.Name = "grpFormatOptions";
			this.grpFormatOptions.TabStop = false;
			// 
			// chkProcessDuplicateSeeLinks
			// 
			this.chkProcessDuplicateSeeLinks.Checked = true;
			this.chkProcessDuplicateSeeLinks.CheckState = System.Windows.Forms.CheckState.Checked;
			resources.ApplyResources(this.chkProcessDuplicateSeeLinks, "chkProcessDuplicateSeeLinks");
			this.chkProcessDuplicateSeeLinks.Name = "chkProcessDuplicateSeeLinks";
			// 
			// lblTxt2Space2
			// 
			resources.ApplyResources(this.lblTxt2Space2, "lblTxt2Space2");
			this.lblTxt2Space2.Name = "lblTxt2Space2";
			// 
			// numSpacesPerTab
			// 
			resources.ApplyResources(this.numSpacesPerTab, "numSpacesPerTab");
			this.numSpacesPerTab.Name = "numSpacesPerTab";
			this.numSpacesPerTab.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
			// 
			// lblTxt2Space1
			// 
			resources.ApplyResources(this.lblTxt2Space1, "lblTxt2Space1");
			this.lblTxt2Space1.Name = "lblTxt2Space1";
			// 
			// chkTabsToSpaces
			// 
			this.chkTabsToSpaces.Checked = true;
			this.chkTabsToSpaces.CheckState = System.Windows.Forms.CheckState.Checked;
			resources.ApplyResources(this.chkTabsToSpaces, "chkTabsToSpaces");
			this.chkTabsToSpaces.Name = "chkTabsToSpaces";
			// 
			// grpDisplay
			// 
			this.grpDisplay.Controls.Add(this.chkShowToolbar);
			resources.ApplyResources(this.grpDisplay, "grpDisplay");
			this.grpDisplay.Name = "grpDisplay";
			this.grpDisplay.TabStop = false;
			// 
			// chkShowToolbar
			// 
			this.chkShowToolbar.Checked = true;
			this.chkShowToolbar.CheckState = System.Windows.Forms.CheckState.Checked;
			resources.ApplyResources(this.chkShowToolbar, "chkShowToolbar");
			this.chkShowToolbar.Name = "chkShowToolbar";
			// 
			// grpPreviewStyle
			// 
			this.grpPreviewStyle.Controls.Add(this.rbPreviewStyleSandcastle);
			this.grpPreviewStyle.Controls.Add(this.rbPreviewStyleNDoc13);
			resources.ApplyResources(this.grpPreviewStyle, "grpPreviewStyle");
			this.grpPreviewStyle.Name = "grpPreviewStyle";
			this.grpPreviewStyle.TabStop = false;
			// 
			// rbPreviewStyleSandcastle
			// 
			resources.ApplyResources(this.rbPreviewStyleSandcastle, "rbPreviewStyleSandcastle");
			this.rbPreviewStyleSandcastle.Name = "rbPreviewStyleSandcastle";
			// 
			// rbPreviewStyleNDoc13
			// 
			resources.ApplyResources(this.rbPreviewStyleNDoc13, "rbPreviewStyleNDoc13");
			this.rbPreviewStyleNDoc13.Name = "rbPreviewStyleNDoc13";
			// 
			// grpIncludes
			// 
			this.grpIncludes.Controls.Add(this.rbIncludeRelative);
			this.grpIncludes.Controls.Add(this.rbIncludeAbsolute);
			this.grpIncludes.Controls.Add(this.rbIncludeNone);
			resources.ApplyResources(this.grpIncludes, "grpIncludes");
			this.grpIncludes.Name = "grpIncludes";
			this.grpIncludes.TabStop = false;
			// 
			// rbIncludeRelative
			// 
			resources.ApplyResources(this.rbIncludeRelative, "rbIncludeRelative");
			this.rbIncludeRelative.Name = "rbIncludeRelative";
			this.rbIncludeRelative.TabStop = true;
			this.rbIncludeRelative.UseVisualStyleBackColor = true;
			// 
			// rbIncludeAbsolute
			// 
			resources.ApplyResources(this.rbIncludeAbsolute, "rbIncludeAbsolute");
			this.rbIncludeAbsolute.Name = "rbIncludeAbsolute";
			this.rbIncludeAbsolute.TabStop = true;
			this.rbIncludeAbsolute.UseVisualStyleBackColor = true;
			// 
			// rbIncludeNone
			// 
			resources.ApplyResources(this.rbIncludeNone, "rbIncludeNone");
			this.rbIncludeNone.Name = "rbIncludeNone";
			this.rbIncludeNone.TabStop = true;
			this.rbIncludeNone.UseVisualStyleBackColor = true;
			// 
			// grpServerOptions
			// 
			this.grpServerOptions.Controls.Add(this.serverPort);
			this.grpServerOptions.Controls.Add(this.lblServerPort);
			resources.ApplyResources(this.grpServerOptions, "grpServerOptions");
			this.grpServerOptions.Name = "grpServerOptions";
			this.grpServerOptions.TabStop = false;
			// 
			// serverPort
			// 
			resources.ApplyResources(this.serverPort, "serverPort");
			this.serverPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
			this.serverPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.serverPort.Name = "serverPort";
			this.serverPort.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// lblServerPort
			// 
			resources.ApplyResources(this.lblServerPort, "lblServerPort");
			this.lblServerPort.Name = "lblServerPort";
			// 
			// DocumentorOptions
			// 
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this.grpServerOptions);
			this.Controls.Add(this.grpIncludes);
			this.Controls.Add(this.grpPreviewStyle);
			this.Controls.Add(this.grpDisplay);
			this.Controls.Add(this.grpFormatOptions);
			this.Controls.Add(this.grpUnrecognizedTags);
			this.Controls.Add(this.grpDocTagCompat);
			this.Name = "DocumentorOptions";
			this.grpDocTagCompat.ResumeLayout(false);
			this.grpDocTagCompat.PerformLayout();
			this.grpUnrecognizedTags.ResumeLayout(false);
			this.grpUnrecognizedTags.PerformLayout();
			this.grpFormatOptions.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numSpacesPerTab)).EndInit();
			this.grpDisplay.ResumeLayout(false);
			this.grpPreviewStyle.ResumeLayout(false);
			this.grpPreviewStyle.PerformLayout();
			this.grpIncludes.ResumeLayout(false);
			this.grpIncludes.PerformLayout();
			this.grpServerOptions.ResumeLayout(false);
			this.grpServerOptions.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.serverPort)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion
	}
}