// Original code based on Lutz Roeder's Documentor; this
// version is modified from the original.  Original copyright
// follows:
// ---------------------------------------------------------
// Lutz Roeder's .NET Reflector, October 2000.
// Copyright (C) 2000-2003 Lutz Roeder. All rights reserved.
// http://www.aisto.com/roeder/dotnet
// roeder@aisto.com
// ---------------------------------------------------------
using System;
using System.Windows.Forms;
using System.Xml;

using CR_Documentor.Transformation;

using DevExpress.CodeRush.Diagnostics.ToolWindows;
using DevExpress.CodeRush.StructuralParser;
using CR_Documentor.Server;

namespace CR_Documentor.Controls
{
	/// <summary>
	/// Marries the transformation engine with the browser to display documentation.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Uses the <see cref="CR_Documentor.Transformation.TransformEngine"/> class to transform the
	/// XML documentation into HTML and then renders it in a <see cref="CR_Documentor.Controls.Browser"/>
	/// control.
	/// </para>
	/// </remarks>
	/// <seealso cref="CR_Documentor.Transformation.TransformEngine" />
	/// <seealso cref="CR_Documentor.Controls.Browser" />
	public class DocumentationControl : Control
	{
		/// <summary>
		/// The browser that will display rendered documentation.
		/// </summary>
		private CR_Documentor.Controls.Browser _browser = new CR_Documentor.Controls.Browser();

		/// <summary>
		/// The transformation class that will convert the XML doc to HTML.
		/// </summary>
		private TransformEngine _transformer = null;

		/// <summary>
		/// The default message that gets put into the Documentor window.
		/// </summary>
		private static readonly System.Xml.XmlDocument DefaultDocument;

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
					this.WebServer.Content = _transformer.GetHtmlPage(Browser.DefaultBodyMessage);
					this._browser.Refresh();
				}
			}
		}

		/// <summary>
		/// Gets or sets the web server instance serving the documentation preview.
		/// </summary>
		/// <value>
		/// A <see cref="CR_Documentor.Server.WebServer"/> that will serve up
		/// preview content to the browser.
		/// </value>
		public virtual WebServer WebServer { get; set; }

		/// <summary>
		/// Gets the currently displayed HTML document.
		/// </summary>
		/// <value>
		/// An <see cref="mshtml.IHTMLDocument2"/> that contains the HTML
		/// document currently being displayed.
		/// </value>
		/// <remarks>
		/// <para>
		/// This is used by toolbar options in the preview window. Executing
		/// actions on the browser requires access to the document as HTML.
		/// </para>
		/// </remarks>
		private mshtml.IHTMLDocument2 HtmlDocument
		{
			get
			{
				mshtml.IHTMLDocument2 retVal = null;
				if (this._browser != null && this._browser.IsHandleCreated)
				{
					retVal = this._browser.Document as mshtml.IHTMLDocument2;
				}
				return retVal;
			}
		}

		/// <summary>
		/// Performs <see langword="static" /> member initialization for the <see cref="DocumentationControl"/> class.
		/// </summary>
		static DocumentationControl()
		{
			DefaultDocument = new System.Xml.XmlDocument();
			DefaultDocument.LoadXml("<member><summary>" + Browser.DefaultBodyMessage + "</summary></member>");
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
			this.TabStop = false;
			this.Dock = DockStyle.Fill;
			this._browser.TabStop = false;
			this._browser.Dock = DockStyle.Fill;
			this._browser.HandleCreated += new EventHandler(Browser_HandleCreated);
			this.Controls.Add(this._browser);
			this.Transformer = new CR_Documentor.Transformation.MSDN.Engine();
		}

		/// <summary>
		/// Refreshes the browser to the first page view.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An <see cref="System.EventArgs" /> that contains the event data.</param>
		private void Browser_HandleCreated(object sender, EventArgs e)
		{
			string[] prefixes = new string[this.WebServer.Prefixes.Count];
			this.WebServer.Prefixes.CopyTo(prefixes, 0);
			foreach (string prefix in prefixes)
			{
				// Prefixes are wildcards; we need a host name in there.
				string url = prefix.Replace("*", "localhost");
				this._browser.SafeUrls.Add(url);
			}
			this._browser.Navigate(this._browser.SafeUrls[0]);
		}

		/// <summary>
		/// Prints the currently displayed document.
		/// </summary>
		public virtual void Print()
		{
			mshtml.IHTMLDocument2 printdoc = this.HtmlDocument;
			if (printdoc != null)
			{
				Log.Send("Executing Print command on document.");
				if (printdoc.execCommand("Print", true, null))
				{
					Log.Send("Printing successful.");
				}
				else
				{
					Log.SendWarning("Printing unsuccessful.");
				}
			}
			else
			{
				Log.SendError("Print document is null. Unable to print.");
			}
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
				this.WebServer.Content = this._transformer.ToString();
				this._browser.Reload();
			}
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
