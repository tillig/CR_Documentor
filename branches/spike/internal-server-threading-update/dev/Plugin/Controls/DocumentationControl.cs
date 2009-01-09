using System;
using System.Windows.Forms;
using System.Xml;

using CR_Documentor.Server;
using CR_Documentor.Transformation;

using DevExpress.CodeRush.Diagnostics.ToolWindows;
using DevExpress.CodeRush.StructuralParser;

namespace CR_Documentor.Controls
{
	/// <summary>
	/// Marries the transformation engine with the browser to display documentation.
	/// </summary>
	/// <remarks>
	/// Uses the <see cref="CR_Documentor.Transformation.TransformEngine"/> class to transform the
	/// XML documentation into HTML and then renders it in a <see cref="CR_Documentor.Controls.Browser"/>
	/// control.
	/// </remarks>
	/// <seealso cref="CR_Documentor.Transformation.TransformEngine"/>
	/// <seealso cref="CR_Documentor.Controls.Browser"/>
	public class DocumentationControl : Control
	{
		/// <summary>
		/// The browser that will display rendered documentation.
		/// </summary>
		private CR_Documentor.Controls.Browser _browser = new CR_Documentor.Controls.Browser();

		/// <summary>
		/// The default text that should appear in the window.
		/// </summary>
		public const string DefaultBodyMessage = "This window will show a preview of XML documentation when an XML comment is entered.";

		/// <summary>
		/// The transformation class that will convert the XML doc to HTML.
		/// </summary>
		private TransformEngine _transformer = null;

		/// <summary>
		/// Indicates whether the browser is currently refreshing the preview.
		/// </summary>
		private bool _isRefreshing = false;

		/// <summary>
		/// Indicates if the browser has been initialized and has navigated to the preview page.
		/// </summary>
		private bool _isBrowserInitialized = false;

		/// <summary>
		/// The default message that gets put into the Documentor window.
		/// </summary>
		private static readonly System.Xml.XmlDocument DefaultDocument;

		/// <summary>
		/// Gets or sets the preview content.
		/// </summary>
		/// <value>
		/// A <see cref="System.String"/> that should be used as the content
		/// for preview requests. Generally this is HTML that will be displayed
		/// in the browser.
		/// </value>
		private string PreviewContent { get; set; }

		/// <summary>
		/// Gets or sets the transformation engine.
		/// </summary>
		/// <value>
		/// A <see cref="CR_Documentor.Transformation.TransformEngine"/> that will
		/// be used to render the XML into an HTML documentation preview.
		/// </value>
		public virtual TransformEngine Transformer
		{
			get
			{
				return _transformer;
			}
			set
			{
				_transformer = value;
				if (_transformer != null)
				{
					Log.Enter(ImageType.Method, "Transformation engine updated. Refreshing browser.");
					try
					{
						this.PreviewContent = _transformer.GetHtmlPage(DefaultBodyMessage);
						this.RefreshBrowser();
					}
					finally
					{
						Log.Exit();
					}
				}
			}
		}

		/// <summary>
		/// Gets the web server instance serving the documentation preview.
		/// </summary>
		/// <value>
		/// A <see cref="CR_Documentor.Server.WebServer"/> that will serve up
		/// preview content to the browser.
		/// </value>
		public virtual WebServer WebServer { get; private set; }

		/// <summary>
		/// Performs <see langword="static" /> member initialization for the <see cref="DocumentationControl"/> class.
		/// </summary>
		static DocumentationControl()
		{
			DefaultDocument = new System.Xml.XmlDocument();
			DefaultDocument.LoadXml("<member><summary>" + DefaultBodyMessage + "</summary></member>");
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CR_Documentor.Controls.DocumentationControl" /> class.
		/// </summary>
		/// <param name="previewServer">The web server that will serve up preview content to the browser.</param>
		public DocumentationControl(WebServer previewServer)
		{
			if (previewServer == null)
			{
				throw new ArgumentNullException("previewServer");
			}
			this.WebServer = previewServer;
			this.WebServer.OwnerControl = this;
			this.WebServer.IncomingRequest += new EventHandler<HttpRequestEventArgs>(WebServer_IncomingRequest);
			this.TabStop = false;
			this.Dock = DockStyle.Fill;
			this._browser.TabStop = false;
			this._browser.Dock = DockStyle.Fill;
			this._browser.ProgressChanged += new WebBrowserProgressChangedEventHandler(Browser_ProgressChanged);
			this.Controls.Add(this._browser);
			// Setting the transformation engine will refresh/initialize the browser.
			this.Transformer = new CR_Documentor.Transformation.MSDN.Engine();
		}

		/// <summary>
		/// Re-enables the control after the browser is done refreshing.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An <see cref="System.Windows.Forms.WebBrowserProgressChangedEventArgs" /> that contains the event data.</param>
		/// <remarks>
		/// <para>
		/// This handler works in conjunction with <see cref="CR_Documentor.Controls.DocumentationControl.RefreshBrowser()"/>
		/// to safely refresh the browser without stealing focus.
		/// </para>
		/// </remarks>
		private void Browser_ProgressChanged(object sender, WebBrowserProgressChangedEventArgs e)
		{
			Log.Send(String.Format("CR_Documentor browser updating: {0} of {1}.", e.CurrentProgress, e.MaximumProgress));
			if (this._isRefreshing && e.CurrentProgress == e.MaximumProgress)
			{
				this._isRefreshing = false;
				this.Enabled = true;
			}
		}

		/// <summary>
		/// Refreshes the browser to the first page view.
		/// </summary>
		private void NavigateToInitialPage()
		{
			Log.Enter(ImageType.Method, "Initializing browser and navigating to initial page.");
			try
			{
				string url = this.WebServer.Url.ToString();
				this._browser.SafeUrls.Add(url);
				this._browser.Navigate(url);
			}
			finally
			{
				Log.Exit();
			}
		}

		/// <summary>
		/// Prints the currently displayed document.
		/// </summary>
		public virtual void Print()
		{
			Log.Send("Printing documentation preview.");
			this._browser.ShowPrintDialog();
		}

		/// <summary>
		/// Refreshes the browser contents with new documentation.
		/// </summary>
		/// <param name="documentation">
		/// The <see cref="System.Xml.XmlDocument"/> containing the XML comments to render.
		/// <see langword="null" /> sets the document back to default.
		/// </param>
		/// <param name="code">
		/// The code element that the documentation refers to.
		/// </param>
		public virtual void RefreshBrowser(XmlDocument documentation, LanguageElement code)
		{
			bool refresh = false;

			// Set documentation
			if (documentation == null)
			{
				// Set the document back to default if needed
				if (this.Transformer.Document != DefaultDocument)
				{
					this.Transformer.Document = DefaultDocument;
					refresh = true;
				}
			}
			else
			{
				// Set the transformation document
				this.Transformer.Document = documentation;
				refresh = true;
			}

			// Set code element
			if (this.Transformer.CodeTarget != code)
			{
				this.Transformer.CodeTarget = code;
				refresh = true;
			}

			// Refresh if there was a change
			if (refresh)
			{
				Log.Enter(ImageType.Method, "Refreshing browser with new content.");
				try
				{
					this.PreviewContent = this._transformer.ToString();
					this.RefreshBrowser();
				}
				finally
				{
					Log.Exit();
				}
			}
		}

		/// <summary>
		/// Safely does a browser refresh without stealing focus.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This is actually a multi-part solution. The standard
		/// <see cref="System.Windows.Forms.WebBrowser.Refresh()"/> method
		/// will steal focus when you execute it. To avoid that, you need to
		/// disable the control that hosts the browser (this control) during the
		/// browser refresh and re-enable it when it's done refreshing.
		/// </para>
		/// <para>
		/// This method disables this control, sets a flag to indicate the browser
		/// is refreshing, and hits the refresh. In a handler for the browser's
		/// <see cref="System.Windows.Forms.WebBrowser.ProgressChanged"/>
		/// event, we check to see if the document is done loading and if we're
		/// refreshing and, if so, we re-enable the control.
		/// </para>
		/// <para>
		/// Note that if the browser has not been initialized (i.e., it hasn't
		/// navigated to the intial page yet), it does so via
		/// <see cref="CR_Documentor.Controls.DocumentationControl.NavigateToInitialPage"/>.
		/// </para>
		/// </remarks>
		/// <seealso cref="CR_Documentor.Controls.DocumentationControl.Browser_ProgressChanged"/>
		private void RefreshBrowser()
		{
			if (!this._isBrowserInitialized)
			{
				this._isBrowserInitialized = true;
				this.NavigateToInitialPage();
			}
			else
			{
				this.Enabled = false;
				this._isRefreshing = true;
				this._browser.Refresh();
			}
		}

		/// <summary>
		/// Handles the IncomingRequest event of the current web server.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="CR_Documentor.Server.HttpRequestEventArgs"/> instance containing the event data.</param>
		private void WebServer_IncomingRequest(object sender, HttpRequestEventArgs e)
		{
			ResponseWriter.WriteHtml(e.RequestContext, this.PreviewContent);
		}

		/// <summary>
		/// Cleans up internal resources.
		/// </summary>
		/// <param name="disposing"><see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
		/// <remarks>
		/// <para>
		/// Removes events from the internal browser object and disposes of it correctly.
		/// This is needed to avoid exceptions when shutting down.
		/// </para>
		/// </remarks>
		protected override void Dispose(bool disposing)
		{
			if (disposing && this._browser != null)
			{
				this._browser.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
