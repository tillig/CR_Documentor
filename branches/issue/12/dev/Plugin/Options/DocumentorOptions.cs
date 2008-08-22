using System;
using System.Diagnostics;
using DevExpress.CodeRush.Core;

namespace CR_Documentor.Options
{
	/// <summary>
	/// Options window for the CR_Documentor plugin.
	/// </summary>
	[UserLevel(UserLevel.NewUser)]
	public class DocumentorOptions : OptionsPage
	{

		#region DocumentorOptions Variables

		#region Instance

		/// <summary>
		/// Internal storage for the
		/// <see cref="CR_Documentor.Options.DocumentorOptions.Options" />
		/// property.
		/// </summary>
		/// <seealso cref="CR_Documentor.Options.DocumentorOptions" />
		private OptionSet _options;

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

		private System.ComponentModel.Container components = null;

		#endregion

		#endregion



		#region DocumentorOptions Properties

		/// <summary>
		/// Gets a <see cref="System.String"/> with the options window category.
		/// </summary>
		/// <value>The options window category.</value>
		public override string Category
		{
			get
			{
				return DocumentorOptions.GetCategory();
			}
		}

		/// <summary>
		/// Gets the full path to this options page.
		/// </summary>
		public static string FullPath
		{
			get
			{
				return GetCategory() + "\\" + GetPageName();
			}
		}

		/// <summary>
		/// Gets the options being worked with in this window.
		/// </summary>
		/// <value>
		/// A <see cref="CR_Documentor.Options.OptionSet"/> with the current options.
		/// </value>
		public OptionSet Options
		{
			get
			{
				return _options;
			}
		}

		/// <summary>
		/// Gets a <see cref="System.String"/> with the options window title.
		/// </summary>
		/// <value>The options window title.</value>
		public override string PageName
		{
			get
			{
				return DocumentorOptions.GetPageName();
			}
		}

		/// <summary>
		/// Gets a <see cref="DevExpress.CodeRush.Core.DecoupledStorage"/> object that
		/// pertains to this options window.
		/// </summary>
		/// <value>The storage object for the options window.</value>
		public static DecoupledStorage Storage
		{
			get
			{
				return DevExpress.CodeRush.Core.CodeRush.Options.GetStorage(GetCategory(), GetPageName());
			}
		}

		#endregion



		#region DocumentorOptions Implementation

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="CR_Documentor.Options.DocumentorOptions" /> class.
		/// </summary>
		public DocumentorOptions()
			: base()
		{
			// Required for Windows.Forms Class Composition Designer support
			InitializeComponent();
		}

		#endregion

		#region Overrides

		/// <summary>
		/// Connects event handlers and sets up form values.
		/// </summary>
		protected override void Initialize()
		{
			base.Initialize();

			this.CommitChanges += new CommitChangesEventHandler(DocumentorOptions_CommitChanges);
			this.RestoreDefaults += new RestoreDefaultsEventHandler(DocumentorOptions_RestoreDefaults);
			this._options = OptionSet.GetOptionSetFromStorage(DocumentorOptions.Storage);
			this.SyncOptionsToForm(false);
		}

		#endregion

		#region Event Handlers

		/// <summary>
		/// Saves the changes made in the options form when "OK" or "Apply" is clicked.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="ea">
		/// A <see cref="DevExpress.CodeRush.Core.OptionsPageStorageEventArgs"/> with event data.
		/// </param>
		private void DocumentorOptions_CommitChanges(object sender, OptionsPageStorageEventArgs ea)
		{
			this.SyncOptionsFromForm();
			this._options.Save(DocumentorOptions.Storage);
		}


		/// <summary>
		/// Restores the default settings for a page.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="ea">
		/// A <see cref="DevExpress.CodeRush.Core.OptionsPageEventArgs"/> with event data.
		/// </param>
		private void DocumentorOptions_RestoreDefaults(object sender, OptionsPageEventArgs ea)
		{
			this.SyncOptionsToForm(true);
		}

		#endregion

		#region Methods

		#region Static

		/// <summary>
		/// Gets a <see cref="System.String"/> with the options window category.
		/// </summary>
		/// <returns>A <see cref="System.String"/> with the options window category.</returns>
		public static string GetCategory()
		{
			return @"Tool Windows";
		}

		/// <summary>
		/// Gets a <see cref="System.String"/> with the options window title.
		/// </summary>
		/// <returns>A <see cref="System.String"/> with the options window title.</returns>
		public static string GetPageName()
		{
			return @"Documentor";
		}

		/// <summary>
		/// Gets a <see cref="DevExpress.CodeRush.Core.UserLevel"/> that corresponds to
		/// the level this options window becomes available at.
		/// </summary>
		/// <returns>
		/// A <see cref="DevExpress.CodeRush.Core.UserLevel"/> with the options
		/// window user level.
		/// </returns>
		public static UserLevel GetLevel()
		{
			return DevExpress.CodeRush.Core.CodeRush.Options.GetLevel(typeof(DocumentorOptions));
		}

		/// <summary>
		/// Shows the options window with this options page open.
		/// </summary>
		public new static void Show()
		{
			DevExpress.CodeRush.Core.CodeRush.Command.Execute("Options", FullPath);
		}

		#endregion

		#region Windows Forms Designer

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
			this.lblServerPort = new System.Windows.Forms.Label();
			this.serverPort = new System.Windows.Forms.NumericUpDown();
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
			// lblServerPort
			// 
			resources.ApplyResources(this.lblServerPort, "lblServerPort");
			this.lblServerPort.Name = "lblServerPort";
			// 
			// portNumber
			// 
			resources.ApplyResources(this.serverPort, "portNumber");
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
			this.serverPort.Name = "portNumber";
			this.serverPort.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// DocumentorOptions
			// 
			this.Controls.Add(this.grpServerOptions);
			this.Controls.Add(this.grpIncludes);
			this.Controls.Add(this.grpPreviewStyle);
			this.Controls.Add(this.grpDisplay);
			this.Controls.Add(this.grpFormatOptions);
			this.Controls.Add(this.grpUnrecognizedTags);
			this.Controls.Add(this.grpDocTagCompat);
			this.Name = "DocumentorOptions";
			this.grpDocTagCompat.ResumeLayout(false);
			this.grpUnrecognizedTags.ResumeLayout(false);
			this.grpFormatOptions.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numSpacesPerTab)).EndInit();
			this.grpDisplay.ResumeLayout(false);
			this.grpPreviewStyle.ResumeLayout(false);
			this.grpIncludes.ResumeLayout(false);
			this.grpIncludes.PerformLayout();
			this.grpServerOptions.ResumeLayout(false);
			this.grpServerOptions.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.serverPort)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();
			this.ResumeLayout(false);

		}


		#endregion

		#region Instance

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
					components.Dispose();
			}

			base.Dispose(disposing);
		}

		/// <summary>
		/// Reads the form values and updates the <see cref="CR_Documentor.Options.DocumentorOptions.Options"/>
		/// with those values.
		/// </summary>
		protected virtual void SyncOptionsFromForm()
		{
			this._options.ConvertCodeTabsToSpaces = this.chkTabsToSpaces.Checked;
			this._options.ConvertCodeTabsToSpacesNum = Convert.ToUInt16(this.numSpacesPerTab.Value);
			if (this.rbPreviewStyleNDoc13.Checked)
			{
				this._options.PreviewStyle = OptionSet.BuildStoredTypeName(typeof(CR_Documentor.Transformation.MSDN.Engine));
			}
			else if (this.rbPreviewStyleSandcastle.Checked)
			{
				this._options.PreviewStyle = OptionSet.BuildStoredTypeName(typeof(CR_Documentor.Transformation.SandcastlePrototype.Engine));
			}
			this._options.ProcessDuplicateSeeLinks = this.chkProcessDuplicateSeeLinks.Checked;
			if (this.rbIncludeAbsolute.Checked)
			{
				this._options.ProcessIncludes = IncludeProcessing.Absolute;
			}
			else if (this.rbIncludeRelative.Checked)
			{
				this._options.ProcessIncludes = IncludeProcessing.Relative;
			}
			else
			{
				this._options.ProcessIncludes = IncludeProcessing.None;
			}
			this._options.ShowToolbar = this.chkShowToolbar.Checked;
			if (this.rbDocTagMSStrict.Checked)
			{
				this._options.TagCompatibilityLevel = TagCompatibilityLevel.MicrosoftStrict;
			}
			else if (this.rbDocTagNDoc13.Checked)
			{
				this._options.TagCompatibilityLevel = TagCompatibilityLevel.NDoc1_3;
			}
			else if (this.rbDocTagSandcastle.Checked)
			{
				this._options.TagCompatibilityLevel = TagCompatibilityLevel.Sandcastle;
			}
			if (this.rbUTHideTagAndContents.Checked)
			{
				this._options.UnrecognizedTagHandlingMethod = UnrecognizedTagHandlingMethod.HideTagAndContents;
			}
			else if (this.rbUTStripTagShowContents.Checked)
			{
				this._options.UnrecognizedTagHandlingMethod = UnrecognizedTagHandlingMethod.StripTagShowContents;
			}
			else if (this.rbUTHighlightUnknown.Checked)
			{
				this._options.UnrecognizedTagHandlingMethod = UnrecognizedTagHandlingMethod.HighlightTagAndContents;
			}
			else if (this.rbUTRenderContents.Checked)
			{
				this._options.UnrecognizedTagHandlingMethod = UnrecognizedTagHandlingMethod.RenderContents;
			}
			this._options.ServerPort = Convert.ToUInt16(this.serverPort.Value);
		}

		/// <summary>
		/// Reads the <see cref="CR_Documentor.Options.DocumentorOptions.Options"/> and updates the form
		/// values from that object.
		/// </summary>
		/// <param name="restoreDefaults">
		/// <see langword="true" /> restore options to default; <see langword="false" />
		/// to sync directly from current option set.
		/// </param>
		protected virtual void SyncOptionsToForm(bool restoreDefaults)
		{
			if (restoreDefaults)
			{
				this._options = new OptionSet();
			}

			this.chkTabsToSpaces.Checked = this._options.ConvertCodeTabsToSpaces;
			this.numSpacesPerTab.Value = this._options.ConvertCodeTabsToSpacesNum;
			if (this._options.PreviewStyle == OptionSet.BuildStoredTypeName(typeof(CR_Documentor.Transformation.MSDN.Engine)))
			{
				this.rbPreviewStyleNDoc13.Checked = true;
			}
			else if (this._options.PreviewStyle == OptionSet.BuildStoredTypeName(typeof(CR_Documentor.Transformation.SandcastlePrototype.Engine)))
			{
				this.rbPreviewStyleSandcastle.Checked = true;
			}
			else
			{
				// Default to NDoc 1.3
				this.rbPreviewStyleNDoc13.Checked = true;
			}
			this.chkProcessDuplicateSeeLinks.Checked = this._options.ProcessDuplicateSeeLinks;
			switch (this._options.ProcessIncludes)
			{
				case IncludeProcessing.Absolute:
					this.rbIncludeAbsolute.Checked = true;
					break;
				case IncludeProcessing.None:
					this.rbIncludeNone.Checked = true;
					break;
				case IncludeProcessing.Relative:
					this.rbIncludeRelative.Checked = true;
					break;
				default:
					Debug.WriteLine("Unexpected value for include processing: " + this._options.ProcessIncludes.ToString());
					break;
			}
			this.chkShowToolbar.Checked = this._options.ShowToolbar;
			switch (this._options.TagCompatibilityLevel)
			{
				case TagCompatibilityLevel.MicrosoftStrict:
					this.rbDocTagMSStrict.Checked = true;
					break;
				case TagCompatibilityLevel.NDoc1_3:
					this.rbDocTagNDoc13.Checked = true;
					break;
				case TagCompatibilityLevel.Sandcastle:
					this.rbDocTagSandcastle.Checked = true;
					break;
				default:
					Debug.WriteLine("Unexpected value for doc tag compatibility level: " + this._options.TagCompatibilityLevel.ToString());
					break;
			}
			switch (this._options.UnrecognizedTagHandlingMethod)
			{
				case UnrecognizedTagHandlingMethod.HideTagAndContents:
					this.rbUTHideTagAndContents.Checked = true;
					break;
				case UnrecognizedTagHandlingMethod.StripTagShowContents:
					this.rbUTStripTagShowContents.Checked = true;
					break;
				case UnrecognizedTagHandlingMethod.HighlightTagAndContents:
					this.rbUTHighlightUnknown.Checked = true;
					break;
				case UnrecognizedTagHandlingMethod.RenderContents:
					this.rbUTRenderContents.Checked = true;
					break;
				default:
					Debug.WriteLine("Unexpected value for unrecognized tag handling method: " + this._options.UnrecognizedTagHandlingMethod.ToString());
					break;
			}

			this.serverPort.Value = this._options.ServerPort;

			this.Invalidate();
		}

		#endregion

		#endregion

		#endregion

	}
}