// This plugin is based on original code from Lutz Roeder's
// Documentor and contains some code from .NET Reflector.
// Original copyright follows:
// ---------------------------------------------------------
// Lutz Roeder's .NET Reflector, October 2000.
// Copyright (C) 2000-2003 Lutz Roeder. All rights reserved.
// http://www.aisto.com/roeder/dotnet
// roeder@aisto.com
// ---------------------------------------------------------
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using CR_Documentor.Controls;
using CR_Documentor.Options;
using CR_Documentor.Server;
using DevExpress.CodeRush.Core;
using DevExpress.CodeRush.Diagnostics.ToolWindows;
using DevExpress.CodeRush.PlugInCore;
using DevExpress.CodeRush.StructuralParser;
using XML = System.Xml;

namespace CR_Documentor
{
	/// <summary>
	/// The DocumentorWindow is a tool window that displays a preview of XML documentation.
	/// </summary>
	[Guid("a41747d2-692b-411e-ad57-a0cd4dd7c03c")]
	[Title("Documentor")]
	public class DocumentorWindow : ToolWindowPlugIn
	{
		#region DocumentorWindow Variables

		/// <summary>
		/// Event provider.
		/// </summary>
		private DevExpress.DXCore.PlugInCore.DXCoreEvents _events;

		/// <summary>
		/// Document rendering control.
		/// </summary>
		private DocumentationControl _previewer;

		/// <summary>
		/// The main menu control.
		/// </summary>
		private ToolBar _toolBar;

		/// <summary>
		/// Form components.
		/// </summary>
		private System.ComponentModel.IContainer components;

		/// <summary>
		/// A resource manager allowing us to internationalize strings.
		/// </summary>
		private System.Resources.ResourceManager _resourceManager = null;

		/// <summary>
		/// Indicator of whether the tool window is currently visible.
		/// </summary>
		private static bool _currentlyVisible = false;

		/// <summary>
		/// Prefix for log messages generated in this module.
		/// </summary>
		private const string LogPrefix = "CR_Documentor: ";

		/// <summary>
		/// Internal web server used to serve up the preview content.
		/// </summary>
		private WebServer _webServer = null;

		/// <summary>
		/// The default port the web server will listen on for preview requests.
		/// </summary>
		private const UInt16 WebServerPort = 11235;

		/// <summary>
		/// URL to the wiki page explaining the reasons the web server might fail to start.
		/// </summary>
		private const string ServerStartupErrorUrl = "http://code.google.com/p/cr-documentor/wiki/ServerStartupErrors";

		/// <summary>
		/// The format of the message to display when the server fails to start.
		/// Parameter {0} is the error message/code.
		/// </summary>
		private static readonly string ServerStartupErrorMessageFormat =
			String.Format(
				CultureInfo.CurrentCulture,
				"An exception has occurred. Error: {{0}}{0}Please check {1} for help with startup errors.{0}Would you like CR_Documentor to launch this url for you?",
				Environment.NewLine,
				ServerStartupErrorUrl
			);

		#endregion


		#region DocumentorWindow Properties

		/// <summary>
		/// Gets the control that contains the browser/preview.
		/// </summary>
		/// <value>A <see cref="CR_Documentor.Controls.DocumentationControl"/> that is the document previewer.</value>
		public DocumentationControl Previewer
		{
			get
			{
				return this._previewer;
			}
		}

		/// <summary>
		/// Gets a <see cref="System.Boolean"/> indicating if the tool window is currently visible.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if the tool window is visible; <see langword="false" /> otherwise.
		/// </value>
		public static bool CurrentlyVisible
		{
			get
			{
				return _currentlyVisible;
			}
		}

		#endregion


		#region DocumentorWindow Implementation

		#region Constructors

		/// <summary>
		/// Initializes a new <see cref="DocumentorWindow"/> object.
		/// </summary>
		public DocumentorWindow()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			this.SuspendLayout();

			Log.Enter(ImageType.Window, "{0}Constructing plugin.", LogPrefix);
			try
			{
				InitializeWebServer();
				// Create the controls for the form
				Log.Send("Building controls.");
				this._toolBar = new ToolBar();
				this._previewer = new DocumentationControl(this._webServer);

				// Refresh the options
				// Right now we do some manipulation of the controls during the
				// refresh of settings. This will make it difficult to change the
				// web server information based on config later. Maybe we need to
				// separate the load of settings from the update of control behavior.
				RefreshSettings();

				// Set doc control view info
				Log.Send("Setting browser properties.");
				this._previewer.Dock = DockStyle.Fill;
				this._previewer.Location = new System.Drawing.Point(0, 0);
				this._previewer.Name = "documentor";
				this._previewer.Size = new System.Drawing.Size(400, 150);
				this._previewer.TabIndex = 0;
				this._previewer.Text = "documentor";
				this.Controls.Add(this._previewer);

				// Create the toolbar ImageList
				Log.Send("Building toolbar image list.");
				ImageList imgList = new ImageList();
				bool showIcons = LoadIcons(imgList);
				CreateToolbar(imgList, showIcons);


				// Add the toolbar
				this.Controls.Add(this._toolBar);

				Log.Send("Construction complete.");
			}
			catch (Exception ex)
			{
				Log.SendException("Error initializing CR_Documentor window.", ex);
				throw;
				// This last 'Throw' should cause DXCore to abandon Toolbox window creation.
				// It would be better if we could test the facility to listen prior to entering this class constructor.
				// However I cannot find an earlier entry point at the moment in which to test this.
			}
			finally
			{
				Log.Exit();
			}

			this.ResumeLayout(false);
		}


		#endregion


		#region Overrides

		/// <summary>
		/// Initializes the plugin by setting event handlers, etc.
		/// </summary>
		public override void InitializePlugIn()
		{
			base.InitializePlugIn();

			this.WindowHide += new EventHandler(DocumentorWindow_WindowHide);
			this.WindowShow += new EventHandler(DocumentorWindow_WindowShow);

			this._events.LanguageElementActivated += new LanguageElementActivatedEventHandler(events_LanguageElementActivated);
			this._events.AfterParse += new AfterParseEventHandler(events_AfterParse);
			this._events.AfterClosingSolution += new DefaultHandler(events_AfterClosingSolution);
			this._events.SolutionOpened += new DefaultHandler(events_SolutionOpened);
			this._events.DocumentClosing += new DocumentEventHandler(events_DocumentClosing);
			this._events.OptionsChanged += new OptionsChangedEventHandler(events_OptionsChanged);
			this._events.BeginShutdown += new DefaultHandler(events_BeginShutdown);

			// Create resource manager for string localization.
			_resourceManager = new System.Resources.ResourceManager("CR_Documentor.Resources.Strings", typeof(DocumentorWindow).Assembly);
		}

		/// <summary>
		/// Cleans up and releases resources.
		/// </summary>
		public override void FinalizePlugIn()
		{
			Log.Send("Stopping web server.");
			this._webServer.Stop();
			this._webServer.Dispose();
			Log.Send("Web server stopped.");
			base.FinalizePlugIn();
		}

		#endregion

		#region Event Handlers

		/// <summary>
		/// Handles the click event for toolbar buttons.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An <see cref="System.Windows.Forms.ToolBarButtonClickEventArgs" /> that contains the event data.</param>
		private void ToolBar_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
		{
			string tag = e.Button.Tag.ToString();
			Log.Enter(ImageType.Info, "{0}Handling toolbar button click for tag [{1}].", LogPrefix, tag);
			try
			{
				switch (tag)
				{
					case "Print":
						this._previewer.Print();
						break;
					case "Settings":
						Log.Send("Showing CR_Documentor options.");
						DocumentorOptions.Show();
						break;
					default:
						Log.SendWarning("Unhandled button tag: " + tag);
						break;
				}
			}
			finally
			{
				Log.Exit();
			}
		}

		/// <summary>
		/// Handles the <c>WindowHide</c> event by toggling the "currently visible" flag.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An <see cref="System.EventArgs"/> that contains the event data.</param>
		private void DocumentorWindow_WindowHide(object sender, EventArgs e)
		{
			// Set the browser to defaults
			this._previewer.RefreshBrowser(null, null);

			// We use this.Window.Visible instead of false because the WindowHide
			// event gets raised when you dock/undock, but the corresponding
			// WindowShow event never gets raised.
			DocumentorWindow._currentlyVisible = this.Window.Visible;
		}

		/// <summary>
		/// Handles the <c>WindowShow</c> event by toggling the "currently visible" flag.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An <see cref="System.EventArgs"/> that contains the event data.</param>
		private void DocumentorWindow_WindowShow(object sender, EventArgs e)
		{
			// We use this.Window.Visible instead of false because the WindowHide
			// event gets raised when you dock/undock, but the corresponding
			// WindowShow event never gets raised.
			DocumentorWindow._currentlyVisible = this.Window.Visible;
		}

		/// <summary>
		/// Handles the <c>LanguageElementActivated</c> event by refreshing the preview window.
		/// </summary>
		/// <param name="ea">
		/// An <see cref="DevExpress.CodeRush.Core.LanguageElementActivatedEventArgs"/>
		/// that contains the event data.
		/// </param>
		private void events_LanguageElementActivated(LanguageElementActivatedEventArgs ea)
		{
			RefreshPreview();
		}

		/// <summary>
		/// Handles the <c>AfterParse</c> event by refreshing the preview window.
		/// </summary>
		/// <param name="ea">
		/// An <see cref="DevExpress.CodeRush.Core.AfterParseEventArgs"/>
		/// that contains the event data.
		/// </param>
		private void events_AfterParse(AfterParseEventArgs ea)
		{
			RefreshPreview();
		}

		/// <summary>
		/// Handles the <c>AfterClosingSolution</c> event by refreshing the preview window.
		/// </summary>
		private void events_AfterClosingSolution()
		{
			RefreshPreview();
		}

		/// <summary>
		/// Handles the <c>SolutionOpened</c> event by refreshing the preview window.
		/// </summary>
		private void events_SolutionOpened()
		{
			RefreshPreview();
		}

		/// <summary>
		/// Handles the <c>DocumentClosing</c> event by refreshing the preview window.
		/// </summary>
		/// <param name="ea">
		/// An <see cref="DevExpress.CodeRush.Core.DocumentEventArgs"/>
		/// that contains the event data.
		/// </param>
		private void events_DocumentClosing(DocumentEventArgs ea)
		{
			RefreshPreview();
		}

		/// <summary>
		/// Handles the <c>OptionsChanged</c> event by refreshing the settings on the
		/// window and refreshing the preview window.
		/// </summary>
		/// <param name="ea">
		/// An <see cref="DevExpress.CodeRush.Core.OptionsChangedEventArgs"/>
		/// that contains the event data.
		/// </param>
		private void events_OptionsChanged(OptionsChangedEventArgs ea)
		{
			Log.Enter(ImageType.Options, "{0}Options changed.", LogPrefix);
			try
			{
				RefreshSettings();
				RefreshPreview();
			}
			finally
			{
				Log.Exit();
			}
		}

		/// <summary>
		/// Handles the <c>BeginShutdown</c> event by cleaning up resources.
		/// </summary>
		private void events_BeginShutdown()
		{
			if (this._previewer != null)
			{
				this._previewer.Dispose();
			}
		}

		#endregion

		#region Methods

		#region Instance

		/// <summary>
		/// Sets up the toolbar with the appropriate icons during window initialization.
		/// </summary>
		/// <param name="imgList">The list of images containing the icons for the tool buttons.</param>
		/// <param name="showIcons"><see langword="true" /> to show icons on the buttons, <see langword="false" /> to show text.</param>
		private void CreateToolbar(ImageList imgList, bool showIcons)
		{
			// Create the toolbar
			Log.Send("Setting toolbar properties.");
			this._toolBar.ButtonClick += new ToolBarButtonClickEventHandler(ToolBar_ButtonClick);
			this._toolBar.ImageList = imgList;
			this._toolBar.Appearance = ToolBarAppearance.Flat;
			this._toolBar.TextAlign = ToolBarTextAlign.Right;

			// Add the toolbar buttons
			ToolBarButton tbb = null;

			// Print button
			tbb = new ToolBarButton();
			tbb.Tag = "Print";
			if (showIcons)
			{
				tbb.ImageIndex = 0;
				tbb.ToolTipText = this._resourceManager.GetString("CR_Documentor.DocumentorWindow.ToolBar.Print");
			}
			else
			{
				tbb.Text = this._resourceManager.GetString("CR_Documentor.DocumentorWindow.ToolBar.Print");
			}
			this._toolBar.Buttons.Add(tbb);

			// Settings button
			tbb = new ToolBarButton();
			tbb.Tag = "Settings";
			if (showIcons)
			{
				tbb.ImageIndex = 1;
				tbb.ToolTipText = this._resourceManager.GetString("CR_Documentor.DocumentorWindow.ToolBar.Settings");
			}
			else
			{
				tbb.Text = this._resourceManager.GetString("CR_Documentor.DocumentorWindow.ToolBar.Settings");
			}
			this._toolBar.Buttons.Add(tbb);
		}

		/// <summary>
		/// Starts up the internal web server.
		/// </summary>
		private void InitializeWebServer()
		{
			try
			{
				// Get the web server ready
				Log.Send("Starting web server.");
				this._webServer = new WebServer(WebServerPort);
				this._webServer.Start();
			}
			catch (Exception ex)
			{
				Log.SendException("Error starting CR_Documentor web server.", ex);
				String innerMessage = ex.Message;
				if (ex is HttpListenerException)
				{
					innerMessage = String.Format("(Code {0}) {1}", ((HttpListenerException)ex).ErrorCode, ex.Message);
				}
				if (MessageBox.Show(String.Format(ServerStartupErrorMessageFormat, innerMessage), "Error during startup", MessageBoxButtons.YesNo) == DialogResult.Yes)
				{
					// This version to launch in default browser (preferred)
					System.Diagnostics.Process.Start(ServerStartupErrorUrl);
					// This version to launch in VS internal broswer
					//CodeRush.ShowURL(ServerStartupErrorUrl);
				}
				throw;
			}
		}

		/// <summary>
		/// Refreshes the settings from the options window.
		/// </summary>
		public void RefreshSettings()
		{
			Log.Send("Updating transform options from storage.");
			OptionSet options = OptionSet.GetOptionSetFromStorage(DocumentorOptions.Storage);
			this._toolBar.Visible = options.ShowToolbar;

			// Load the appropriate transformation engine based on new settings
			Type transformType = typeof(CR_Documentor.Transformation.MSDN.Engine);
			try
			{
				transformType = Type.GetType(options.PreviewStyle, true);
			}
			catch (Exception err)
			{
				Log.SendException(String.Format("Unable to load specified preview style [{0}].  Defaulting to [CR_Documentor.Transformation.MSDN.Engine, CR_Documentor].", options.PreviewStyle), err);
			}

			Transformation.TransformEngine transformer = null;
			try
			{
				transformer = Activator.CreateInstance(transformType) as Transformation.TransformEngine;
			}
			catch (Exception err)
			{
				Log.SendException(String.Format("Unable to instantiate preview style [{0}].", transformType.AssemblyQualifiedName), err);
			}
			if (transformer == null)
			{
				transformer = new Transformation.MSDN.Engine();
			}
			transformer.Options = options;
			this.Previewer.Transformer = transformer;
		}


		/// <summary>
		/// Refreshes the content of the documentor window
		/// </summary>
		protected void RefreshPreview()
		{
			if (!DocumentorWindow.CurrentlyVisible)
			{
				return;
			}
			if (CodeRush.Source.InsideXMLDocComment)
			{
				XmlDocComment currentComment = CommentParser.GetXmlDocComment(CodeRush.Source.Active);
				if (currentComment != null)
				{
					// Get an XML document from the comment
					XML.XmlDocument document = CommentParser.ParseXmlCommentToXmlDocument(currentComment, _resourceManager.GetString("CR_Documentor.DocumentorWindow.ParseError"), _resourceManager.GetString("CR_Documentor.DocumentorWindow.GeneralError"));

					// Get the language element associated with the doc
					LanguageElement code = currentComment.TargetNode;

					// Refresh the preview with the new information
					this._previewer.RefreshBrowser(document, code);
					return;
				}
			}
			else
			{
				// Set the browser to defaults
				this._previewer.RefreshBrowser(null, null);
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
				if (this._previewer != null)
				{
					this._previewer.Dispose();
				}
			}

			base.Dispose(disposing);
		}

		#endregion

		#region Design-time Support

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DocumentorWindow));
			this._events = new DevExpress.DXCore.PlugInCore.DXCoreEvents(this.components);
			((System.ComponentModel.ISupportInitialize)(this._events)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			//
			// DocumentorWindow
			//
			this.Image = ((System.Drawing.Bitmap)(resources.GetObject("$this.Image")));
			this.ImageBackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(254)), ((System.Byte)(0)));
			this.Name = "DocumentorWindow";
			this.Size = new System.Drawing.Size(400, 150);
			((System.ComponentModel.ISupportInitialize)(this._events)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();

		}

		#endregion

		#region Static

		/// <summary>
		/// Shows the Documentor window.
		/// </summary>
		/// <returns>An instance of the <see cref="DocumentorWindow"/></returns>
		public static EnvDTE.Window ShowWindow()
		{
			return DevExpress.CodeRush.Core.CodeRush.ToolWindows.Show(typeof(DocumentorWindow).GUID);
		}

		/// <summary>
		/// Hides the Documentor window.
		/// </summary>
		/// <returns>An instance of the <see cref="DocumentorWindow"/></returns>
		public static EnvDTE.Window HideWindow()
		{
			return DevExpress.CodeRush.Core.CodeRush.ToolWindows.Hide(typeof(DocumentorWindow).GUID);
		}

		/// <summary>
		/// Extracts icons from a file.
		/// </summary>
		/// <param name="lpszFile">The file to extract icons from.</param>
		/// <param name="nIconIndex">The index of the icon to start extraction at.</param>
		/// <param name="phIconLarge">The large version of the icon (out)</param>
		/// <param name="phIconSmall">The small version of the icon (out)</param>
		/// <param name="nIcons">The number of icons to retrieve.</param>
		/// <returns>0 for success; HRESULT code for failure.</returns>
		[DllImport("Shell32", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
		internal extern static int ExtractIconEx(
			[MarshalAs(UnmanagedType.LPTStr)]
			string lpszFile,       //size of the icon
			int nIconIndex,        //index of the icon
			//(in case we have more
			//then 1 icon in the file
			IntPtr[] phIconLarge,  //32x32 icon
			IntPtr[] phIconSmall,  //16x16 icon
			int nIcons);           //how many to get

		/// <summary>
		/// Loads the set of toolbar icons into an image list.
		/// </summary>
		/// <param name="imgList">The list to populate with icons.</param>
		/// <returns><see langword="true" /> on successful load, <see langword="false" /> if the load fails.</returns>
		private static bool LoadIcons(ImageList imgList)
		{
			try
			{
				Icon icon = null;
				System.IO.Stream iconStream = null;
				System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();

				// Get the printer icon
				try
				{
					iconStream = asm.GetManifestResourceStream("CR_Documentor.Resources.Printer.ico");
					if (iconStream == null)
					{
						throw new IOException("Unable to load printer icon from embedded resources.");
					}
					icon = new Icon(iconStream);
					imgList.Images.Add(icon);
				}
				finally
				{
					if (iconStream != null)
					{
						iconStream.Close();
						iconStream = null;
					}
				}

				// Get the settings icon
				try
				{
					iconStream = asm.GetManifestResourceStream("CR_Documentor.Resources.Settings.ico");
					if (iconStream == null)
					{
						throw new IOException("Unable to load settings icon from embedded resources.");
					}
					icon = new Icon(iconStream);
					imgList.Images.Add(icon);
				}
				finally
				{
					if (iconStream != null)
					{
						iconStream.Close();
						iconStream = null;
					}
				}
				return true;
			}
			catch (Exception err)
			{
				Log.SendException(String.Format("{0}Error loading icons for toolbar. Not showing icons.", LogPrefix), err);
				return false;
			}
		}

		#endregion

		#endregion

		#endregion
	}
}