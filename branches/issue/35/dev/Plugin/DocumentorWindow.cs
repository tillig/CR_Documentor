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
using CR_Documentor.Diagnostics;
using CR_Documentor.Options;
using CR_Documentor.Server;
using DevExpress.CodeRush.Core;
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
		/// <summary>
		/// Log entry handler.
		/// </summary>
		private static readonly ILog Log = LogManager.GetLogger(typeof(DocumentorWindow));

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
		/// Internal web server used to serve up the preview content.
		/// </summary>
		private WebServer _webServer = null;

		/// <summary>
		/// Indicates whether the rendering engine should be paused (and not refresh automatically).
		/// </summary>
		private bool _pauseRendering = false;

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

		/// <summary>
		/// Initializes a new <see cref="DocumentorWindow"/> object.
		/// </summary>
		public DocumentorWindow()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// InitializePlugIn happens right after InitializeComponent,
			// and then we resume construction here.
			this.SuspendLayout();

			Log.Write(LogLevel.Info, "Constructing plugin.");
			try
			{
				StartWebServer();
				RebuildPreviewControl();

				// Create the toolbar. The toolbar needs to be added after the
				// preview window or it ends up covering the top bit of the browser.
				Log.Write(LogLevel.Info, "Building toolbar.");
				this._toolBar = new ToolBar();
				this.UpdateToolbarFromOptions();
				Log.Write(LogLevel.Info, "Building toolbar image list.");
				ImageList imgList = new ImageList();

				System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
				imgList.Images.Add(asm.ReadEmbeddedResourceIcon("CR_Documentor.Resources.Printer.ico"));
				imgList.Images.Add(asm.ReadEmbeddedResourceIcon("CR_Documentor.Resources.Settings.ico"));
				imgList.Images.Add(asm.ReadEmbeddedResourceIcon("CR_Documentor.Resources.Pause.ico"));
				SetupToolbar(imgList);
				this.Controls.Add(this._toolBar);

				Log.Write(LogLevel.Info, "Construction complete.");
			}
			catch (Exception ex)
			{
				Log.Write(LogLevel.Error, "Error initializing CR_Documentor window.", ex);
				throw;
				// This last 'Throw' should cause DXCore to abandon Toolbox window creation.
				// It would be better if we could test the facility to listen prior to entering this class constructor.
				// However I cannot find an earlier entry point at the moment in which to test this.
			}

			this.ResumeLayout(false);
		}

		/// <summary>
		/// Initializes the plugin by setting event handlers, etc.
		/// </summary>
		public override void InitializePlugIn()
		{
			base.InitializePlugIn();

			this.WindowHide += new EventHandler(DocumentorWindow_WindowHide);
			this.WindowShow += new EventHandler(DocumentorWindow_WindowShow);

			this._events.LanguageElementActivated += new LanguageElementActivatedEventHandler(RefreshPreviewDefaultEventHandler);
			this._events.AfterParse += new AfterParseEventHandler(RefreshPreviewDefaultEventHandler);
			this._events.AfterClosingSolution += new DefaultHandler(RefreshPreviewDefaultEventHandler);
			this._events.SolutionOpened += new DefaultHandler(RefreshPreviewDefaultEventHandler);
			this._events.DocumentClosing += new DocumentEventHandler(RefreshPreviewDefaultEventHandler);
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
			Log.Write(LogLevel.Info, "Stopping web server.");
			this._webServer.Stop();
			this._webServer.Dispose();
			Log.Write(LogLevel.Info, "Web server stopped.");
			base.FinalizePlugIn();
		}

		/// <summary>
		/// Handles the click event for toolbar buttons.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An <see cref="System.Windows.Forms.ToolBarButtonClickEventArgs" /> that contains the event data.</param>
		private void ToolBar_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
		{
			string tag = e.Button.Tag.ToString();
			Log.Write(LogLevel.Info, String.Format("Handling toolbar button click for tag [{0}].", tag));
			switch (tag)
			{
				case "Print":
					Log.Write(LogLevel.Info, "Printing CR_Documentor page.");
					this._previewer.Print();
					break;
				case "Settings":
					Log.Write(LogLevel.Info, "Showing CR_Documentor options.");
					DocumentorOptions.Show();
					break;
				case "Pause":
					this._pauseRendering = e.Button.Pushed;
					Log.Write(LogLevel.Info, "CR_Documentor rendering is " + (this._pauseRendering ? "" : "un") + "paused.");
					this.RefreshPreview(); // No-op if we've paused, but will automatically update if we've unpaused.
					break;
				default:
					Log.Write(LogLevel.Warn, "Unhandled button tag: " + tag);
					break;
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
		/// Handles any event by refreshing the preview window.
		/// </summary>
		private void RefreshPreviewDefaultEventHandler()
		{
			RefreshPreview();
		}

		/// <summary>
		/// Handles any event by refreshing the preview window.
		/// </summary>
		/// <param name="e">An <see cref="System.EventArgs" /> that contains the event data.</param>
		private void RefreshPreviewDefaultEventHandler(EventArgs e)
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
			Log.Write(LogLevel.Info, "Options changed.");
			// Things have to happen in exactly this order or the preview control
			// won't get updated with the proper URL when the web server port
			// gets updated.

			StartWebServer();
			RebuildPreviewControl();
			UpdatePreviewFromOptions();
			UpdateToolbarFromOptions();
			RefreshPreview();
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

		/// <summary>
		/// Sets up the toolbar with the appropriate icons during window initialization.
		/// </summary>
		/// <param name="imgList">The list of images containing the icons for the tool buttons.</param>
		private void SetupToolbar(ImageList imgList)
		{
			// Create the toolbar
			Log.Write(LogLevel.Info, "Setting toolbar properties.");
			this._toolBar.ButtonClick += new ToolBarButtonClickEventHandler(ToolBar_ButtonClick);
			this._toolBar.ImageList = imgList;
			this._toolBar.Appearance = ToolBarAppearance.Flat;
			this._toolBar.TextAlign = ToolBarTextAlign.Right;

			this._toolBar.Buttons.Add(this.BuildToolbarButton(0, "Print", "CR_Documentor.DocumentorWindow.ToolBar.Print"));
			this._toolBar.Buttons.Add(this.BuildToolbarButton(1, "Settings", "CR_Documentor.DocumentorWindow.ToolBar.Settings"));
			ToolBarButton pause = this.BuildToolbarButton(2, "Pause", "CR_Documentor.DocumentorWindow.ToolBar.Pause");
			pause.Style = ToolBarButtonStyle.ToggleButton;
			this._toolBar.Buttons.Add(pause);
		}

		/// <summary>
		/// Builds a single button for the main toolbar.
		/// </summary>
		/// <param name="imageIndex">The index in the image list for the icon, or -1 for text-only.</param>
		/// <param name="tag">The tag for the button so we know what command to execute when it's pressed.</param>
		/// <param name="resourcePath">The path to the embedded resource text to display on the button.</param>
		/// <returns>A button that can be added to the main toolbar.</returns>
		private ToolBarButton BuildToolbarButton(int imageIndex, string tag, string resourcePath)
		{
			ToolBarButton tbb = new ToolBarButton();
			string text = this._resourceManager.GetString(resourcePath);
			tbb.ImageIndex = imageIndex;
			tbb.Tag = tag;
			if (imageIndex >= 0)
			{
				tbb.ToolTipText = text;
			}
			else
			{
				tbb.Text = text;
			}
			return tbb;
		}

		/// <summary>
		/// Creates (or re-creates) the preview control and adds it to the control hierarchy.
		/// </summary>
		/// <remarks>
		/// <para>
		/// If the preview control does not already exist, it is created and added
		/// to the control hierarchy. If it does exist, and it is not listening
		/// to the current web server instance, it will be removed and re-created,
		/// then inserted back into the control hierarcy. If it exists and is
		/// already listening to the proper web server instance, there will be
		/// no change and no rebuild.
		/// </para>
		/// </remarks>
		private void RebuildPreviewControl()
		{
			int controlIndex = -1;
			if (this._previewer != null && this.Controls.Contains(this._previewer))
			{
				if (this._previewer.WebServer == this._webServer)
				{
					Log.Write(LogLevel.Info, "Preview control already pointing at the correct web server instance. Skipping rebuild.");
					return;
				}
				Log.Write(LogLevel.Info, "Removing existing preview control.");
				controlIndex = this.Controls.IndexOf(this._previewer);
				this.Controls.Remove(this._previewer);
				this._previewer.Dispose();
				this._previewer = null;
			}
			Log.Write(LogLevel.Info, "Building preview control.");
			this._previewer = new DocumentationControl(this._webServer);
			this.UpdatePreviewFromOptions();
			Log.Write(LogLevel.Info, "Setting browser properties.");
			this._previewer.Dock = DockStyle.Fill;
			this._previewer.Location = new System.Drawing.Point(0, 0);
			this._previewer.Name = "documentor";
			this._previewer.Size = new System.Drawing.Size(400, 150);
			this._previewer.TabIndex = 0;
			this._previewer.Text = "documentor";
			this.Controls.Add(this._previewer);
			if (controlIndex >= 0)
			{
				// Put the rebuilt previewer back exactly where it was before.
				// This is necessary because otherwise the top gets hidden by
				// the toolbar.
				this.Controls.SetChildIndex(this._previewer, controlIndex);
			}
		}

		/// <summary>
		/// Starts up the internal web server based on the user's options.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The web server will start if it is not already started, and will restart
		/// if the options specified by the user for the server (e.g., port to
		/// listen on) are different than what the web server is currently using.
		/// </para>
		/// </remarks>
		private void StartWebServer()
		{
			try
			{
				// Get the web server ready
				OptionSet options = OptionSet.GetOptionSetFromStorage(DocumentorOptions.Storage);
				if (this._webServer != null)
				{
					if (this._webServer.Port == options.ServerPort)
					{
						// Already on the specified port - don't restart.
						return;
					}
					// Not on the right port; shut the existing server down.
					this._webServer.Stop();
					this._webServer.Dispose();
					this._webServer = null;
				}
				Log.Write(LogLevel.Info, "Starting web server.");
				this._webServer = new WebServer(options.ServerPort);
				this._webServer.Start();
			}
			catch (Exception ex)
			{
				Log.Write(LogLevel.Error, "Error starting CR_Documentor web server.", ex);
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
		/// Updates the toolbar options based on settings from the options window.
		/// </summary>
		public void UpdateToolbarFromOptions()
		{
			Log.Write(LogLevel.Info, "Updating control options from storage.");
			OptionSet options = OptionSet.GetOptionSetFromStorage(DocumentorOptions.Storage);
			this._toolBar.Visible = options.ShowToolbar;
		}

		/// <summary>
		/// Updates the current preview based on settings from the options window.
		/// </summary>
		public void UpdatePreviewFromOptions()
		{
			Log.Write(LogLevel.Info, "Updating transform options from storage.");
			OptionSet options = OptionSet.GetOptionSetFromStorage(DocumentorOptions.Storage);

			// Load the appropriate transformation engine based on new settings
			Type transformType = typeof(CR_Documentor.Transformation.SandcastlePrototype.Engine);
			try
			{
				transformType = Type.GetType(options.PreviewStyle, true);
			}
			catch (Exception err)
			{
				Log.Write(LogLevel.Error, String.Format("Unable to load specified preview style [{0}].  Defaulting to [CR_Documentor.Transformation.SandcastlePrototype.Engine, CR_Documentor].", options.PreviewStyle), err);
			}

			Transformation.TransformEngine transformer = null;
			try
			{
				transformer = Activator.CreateInstance(transformType) as Transformation.TransformEngine;
			}
			catch (Exception err)
			{
				Log.Write(LogLevel.Error, String.Format("Unable to instantiate preview style [{0}].", transformType.AssemblyQualifiedName), err);
			}
			if (transformer == null)
			{
				transformer = new Transformation.SandcastlePrototype.Engine();
			}
			transformer.Options = options;
			this._previewer.Transformer = transformer;
		}

		/// <summary>
		/// Refreshes the content of the documentor window
		/// </summary>
		protected void RefreshPreview()
		{
			if (!DocumentorWindow.CurrentlyVisible || this._pauseRendering)
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
		/// Loads the set of toolbar icons into an image list.
		/// </summary>
		/// <param name="imgList">The list to populate with icons.</param>
		/// <returns><see langword="true" /> on successful load, <see langword="false" /> if the load fails.</returns>
		private static bool LoadIcons(ImageList imgList)
		{
			try
			{
				System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
				imgList.Images.Add(asm.ReadEmbeddedResourceIcon("CR_Documentor.Resources.Printer.ico"));
				imgList.Images.Add(asm.ReadEmbeddedResourceIcon("CR_Documentor.Resources.Settings.ico"));
				return true;
			}
			catch (Exception err)
			{
				Log.Write(LogLevel.Error, "Error loading icons for toolbar. Not showing icons.", err);
				return false;
			}
		}
	}
}