using System;
using System.Diagnostics;
using DevExpress.CodeRush.Core;

namespace CR_Documentor.Options
{
	/// <summary>
	/// Options window for the CR_Documentor plugin.
	/// </summary>
	[UserLevel(UserLevel.NewUser)]
	public partial class DocumentorOptions : OptionsPage
	{
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
		public OptionSet Options { get; private set; }

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

		/// <summary>
		/// Initializes a new instance of the <see cref="CR_Documentor.Options.DocumentorOptions" /> class.
		/// </summary>
		public DocumentorOptions()
			: base()
		{
			// Required for Windows.Forms Class Composition Designer support
			InitializeComponent();
		}

		/// <summary>
		/// Connects event handlers and sets up form values.
		/// </summary>
		protected override void Initialize()
		{
			base.Initialize();

			this.CommitChanges += new CommitChangesEventHandler(DocumentorOptions_CommitChanges);
			this.RestoreDefaults += new RestoreDefaultsEventHandler(DocumentorOptions_RestoreDefaults);
			this.Options = OptionSet.GetOptionSetFromStorage(DocumentorOptions.Storage);
			this.SyncOptionsToForm(false);
		}

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
			this.Options.Save(DocumentorOptions.Storage);
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

		/// <summary>
		/// Reads the form values and updates the <see cref="CR_Documentor.Options.DocumentorOptions.Options"/>
		/// with those values.
		/// </summary>
		protected virtual void SyncOptionsFromForm()
		{
			this.Options.ConvertCodeTabsToSpaces = this.chkTabsToSpaces.Checked;
			this.Options.ConvertCodeTabsToSpacesNum = Convert.ToUInt16(this.numSpacesPerTab.Value);
			if (this.rbPreviewStyleNDoc13.Checked)
			{
				this.Options.PreviewStyle = OptionSet.BuildStoredTypeName(typeof(CR_Documentor.Transformation.MSDN.Engine));
			}
			else if (this.rbPreviewStyleSandcastle.Checked)
			{
				this.Options.PreviewStyle = OptionSet.BuildStoredTypeName(typeof(CR_Documentor.Transformation.SandcastlePrototype.Engine));
			}
			this.Options.ProcessDuplicateSeeLinks = this.chkProcessDuplicateSeeLinks.Checked;
			if (this.rbIncludeAbsolute.Checked)
			{
				this.Options.ProcessIncludes = IncludeProcessing.Absolute;
			}
			else if (this.rbIncludeRelative.Checked)
			{
				this.Options.ProcessIncludes = IncludeProcessing.Relative;
			}
			else
			{
				this.Options.ProcessIncludes = IncludeProcessing.None;
			}
			this.Options.ShowToolbar = this.chkShowToolbar.Checked;
			if (this.rbDocTagMSStrict.Checked)
			{
				this.Options.TagCompatibilityLevel = TagCompatibilityLevel.MicrosoftStrict;
			}
			else if (this.rbDocTagNDoc13.Checked)
			{
				this.Options.TagCompatibilityLevel = TagCompatibilityLevel.NDoc1_3;
			}
			else if (this.rbDocTagSandcastle.Checked)
			{
				this.Options.TagCompatibilityLevel = TagCompatibilityLevel.Sandcastle;
			}
			if (this.rbUTHideTagAndContents.Checked)
			{
				this.Options.UnrecognizedTagHandlingMethod = UnrecognizedTagHandlingMethod.HideTagAndContents;
			}
			else if (this.rbUTStripTagShowContents.Checked)
			{
				this.Options.UnrecognizedTagHandlingMethod = UnrecognizedTagHandlingMethod.StripTagShowContents;
			}
			else if (this.rbUTHighlightUnknown.Checked)
			{
				this.Options.UnrecognizedTagHandlingMethod = UnrecognizedTagHandlingMethod.HighlightTagAndContents;
			}
			else if (this.rbUTRenderContents.Checked)
			{
				this.Options.UnrecognizedTagHandlingMethod = UnrecognizedTagHandlingMethod.RenderContents;
			}
			this.Options.ServerPort = Convert.ToUInt16(this.serverPort.Value);
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
				this.Options = new OptionSet();
			}

			this.chkTabsToSpaces.Checked = this.Options.ConvertCodeTabsToSpaces;
			this.numSpacesPerTab.Value = this.Options.ConvertCodeTabsToSpacesNum;
			if (this.Options.PreviewStyle == OptionSet.BuildStoredTypeName(typeof(CR_Documentor.Transformation.MSDN.Engine)))
			{
				this.rbPreviewStyleNDoc13.Checked = true;
			}
			else if (this.Options.PreviewStyle == OptionSet.BuildStoredTypeName(typeof(CR_Documentor.Transformation.SandcastlePrototype.Engine)))
			{
				this.rbPreviewStyleSandcastle.Checked = true;
			}
			else
			{
				// Default to Sandcastle
				this.rbPreviewStyleSandcastle.Checked = true;
			}
			this.chkProcessDuplicateSeeLinks.Checked = this.Options.ProcessDuplicateSeeLinks;
			switch (this.Options.ProcessIncludes)
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
					Debug.WriteLine("Unexpected value for include processing: " + this.Options.ProcessIncludes.ToString());
					break;
			}
			this.chkShowToolbar.Checked = this.Options.ShowToolbar;
			switch (this.Options.TagCompatibilityLevel)
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
					Debug.WriteLine("Unexpected value for doc tag compatibility level: " + this.Options.TagCompatibilityLevel.ToString());
					break;
			}
			switch (this.Options.UnrecognizedTagHandlingMethod)
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
					Debug.WriteLine("Unexpected value for unrecognized tag handling method: " + this.Options.UnrecognizedTagHandlingMethod.ToString());
					break;
			}

			this.serverPort.Value = this.Options.ServerPort;

			this.Invalidate();
		}
	}
}