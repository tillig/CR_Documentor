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
using System.Collections;
using System.IO;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;

using CR_Documentor.Options;
using CR_Documentor.Reflector;
using CR_Documentor.Transformation;

using DevExpress.CodeRush.Diagnostics.ToolWindows;
using DevExpress.CodeRush.StructuralParser;

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

		#region DocumentationControl Variables

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
		protected static readonly System.Xml.XmlDocument DefaultDocument;

		#endregion



		#region DocumentationControl Properties

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
					this._browser.ResetToBaseDocument(_transformer.GetHtmlPage(Browser.DefaultBodyMessage));
				}
			}
		}

		/// <summary>
		/// Gets the currently displayed HTML document.
		/// </summary>
		/// <value>
		/// An <see cref="mshtml.IHTMLDocument2"/> that contains the HTML
		/// document currently being displayed.
		/// </value>
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

		#endregion



		#region DocumentationControl Implementation

		#region Constructors

		/// <summary>
		/// Performs <see langword="static" /> member initialization for the <see cref="DocumentationControl"/> class.
		/// </summary>
		static DocumentationControl()
		{
			DefaultDocument = new System.Xml.XmlDocument();
			DefaultDocument.LoadXml("<member><summary>This window will show a preview of XML documentation when an XML comment is entered.</summary></member>");
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CR_Documentor.Controls.DocumentationControl" /> class.
		/// </summary>
		public DocumentationControl()
		{

			// Set control properties
			this.TabStop = false;
			this.Dock = DockStyle.Fill;
			this._browser.TabStop = false;
			this._browser.Dock = DockStyle.Fill;
			this.Transformer = new CR_Documentor.Transformation.MSDN.Engine();
			this.Controls.Add(this._browser);
		}

		#endregion

		#region Methods

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
				this._browser.PreviewDocument = this._transformer.ToString();

				// For a while it seemed that when the browser refreshed, it was
				// stealing focus. This is noted in various forums like this
				// one:
				// http://www.tech-archive.net/Archive/InetSDK/microsoft.public.inetsdk.programming.webbrowser_ctl/2005-05/msg00040.html
				//
				// For a while it was being fixed by saving a reference to the active
				// document, like this:
				// DevExpress.CodeRush.Core.Document doc = DevExpress.CodeRush.Core.CodeRush.Documents.Active;
				// ...then doing the refresh, then restoring the active document:
				// DevExpress.CodeRush.Core.CodeRush.Editor.Activate(doc);
				//
				// However, that didn't really address the issue because if you
				// highlight very slowly over a long block of code, possibly crossing
				// XML doc comments, the browser refreshes and this save/restore
				// stops your consistent highlight.
				//
				// After recompiling in .NET 2.0 in VS 2008 and updating to the
				// new DXCore (3.0.8.0), this behavior can't be reproduced. If
				// it comes back, it will either need to be fixed here or set
				// on the AxWebBrowser control proper.

				this._browser.Refresh();
				System.Windows.Forms.Application.DoEvents();
				this.Invalidate(true);
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

		#endregion

		#endregion
	}
}
