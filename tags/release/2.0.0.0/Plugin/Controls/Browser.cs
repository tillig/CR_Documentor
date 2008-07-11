using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using mshtml;

namespace CR_Documentor.Controls
{
	/// <summary>
	/// Hosted web browser for displaying HTML.
	/// </summary>
	public class Browser : AxSHDocVw.AxWebBrowser
	{

		#region Browser Variables

		#region Constants

		/// <summary>
		/// The prefix for a URL that refers to member documentation.
		/// </summary>
		public const string MEMBER_URL_PREFIX = "urn:member:";

		/// <summary>
		/// The default text that should appear in the window.
		/// </summary>
		public const string DefaultBodyMessage = "This window will show a preview of XML documentation when an XML comment is entered.";

		#endregion

		#region Instance

		/// <summary>
		/// String containing a temporary file location for the default HTML.
		/// </summary>
		private string _tempUrl = string.Empty;

		#endregion

		#endregion



		#region Browser Properties

		/// <summary>
		/// Sets the HTML content for the browser document.
		/// </summary>
		/// <value>
		/// A <see cref="System.String"/> with the complete document content.
		/// </value>
		/// <remarks>
		/// <para>
		/// If the handle to the browser has not been created or if there is no document,
		/// setting this property will have no effect.
		/// </para>
		/// </remarks>
		public virtual string PreviewDocument
		{
			set
			{
				// If there's no handle, we can't set the body.
				if (!this.IsHandleCreated)
				{
					return;
				}

				// Get the document and set the body content.
				mshtml.IHTMLDocument2 document = this.Document as mshtml.IHTMLDocument2;
				if (document == null)
				{
					return;
				}
				document.clear();
				document.write(value);
				document.close();
			}
		}

		#endregion



		#region Browser Implementation

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="CR_Documentor.Controls.Browser" /> class.
		/// </summary>
		public Browser()
		{
			this.EnlistEvents(true);
		}

		#endregion

		#region Overrides

		/// <summary>
		/// Navigates to the temporary HTML location when the handle is created.
		/// </summary>
		/// <param name="e">An <see cref="System.EventArgs" /> that contains the event data.</param>
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			this.ResetToBaseDocument(
@"<!-- saved from url=(0014)about:internet -->
<html><head>
</head><body>" + DefaultBodyMessage + "&nbsp;</body></html>");
		}

		/// <summary>
		/// Removes ESC from the list of input keys.
		/// </summary>
		/// <param name="keyData">One of the <see cref="System.Windows.Forms.Keys"/> values.</param>
		/// <returns><see langword="true" /> if the specified key is a regular input key; otherwise, <see langword="false" />.</returns>
		protected override bool IsInputKey(Keys keyData)
		{
			return (keyData == Keys.Escape) ? false : base.IsInputKey(keyData);
		}

		/// <summary>
		/// Removes self-handling event handlers.
		/// </summary>
		/// <param name="disposing">
		/// <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.
		/// </param>
		/// <seealso cref="CR_Documentor.Controls.Browser.EnlistEvents" />
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.EnlistEvents(false);
			}
			base.Dispose(disposing);
		}

		#endregion

		#region Event Handlers

		/// <summary>
		/// Stops the browser from attempting to navigate member-to-member or out onto
		/// the web.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An <see cref="System.EventArgs" /> that contains the event data.</param>
		protected virtual void Browser_BeforeNavigate2(object sender, AxSHDocVw.DWebBrowserEvents2_BeforeNavigate2Event e)
		{
			string url = e.uRL.ToString();
			if (url.StartsWith(MEMBER_URL_PREFIX) ||
				url.StartsWith("http"))
			{
				// Cancel non-local navigation
				e.cancel = true;
			}

		}

		/// <summary>
		/// Removes temporary HTML files as needed.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An <see cref="System.EventArgs" /> that contains the event data.</param>
		protected virtual void Browser_NavigateComplete2(object sender, AxSHDocVw.DWebBrowserEvents2_NavigateComplete2Event e)
		{
			if (this._tempUrl.Length != 0)
			{
				if (File.Exists(this._tempUrl))
				{
					File.Delete(this._tempUrl);
				}

				this._tempUrl = string.Empty;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Adds or removes the event handlers that the browser handles itself.
		/// </summary>
		/// <param name="add">
		/// <see langword="true" /> to add the event handlers; <see langword="false" /> to remove them.
		/// </param>
		/// <remarks>
		/// <para>
		/// This method is helpful in centralizing the addition and removal of event handlers
		/// on the browser instance.  In creation, events should be enlisted; in disposal,
		/// events should be removed.
		/// </para>
		/// </remarks>
		/// <seealso cref="CR_Documentor.Controls.Browser" />
		/// <seealso cref="CR_Documentor.Controls.Browser.Dispose" />
		protected virtual void EnlistEvents(bool add)
		{
			if (add)
			{
				this.NavigateComplete2 += new AxSHDocVw.DWebBrowserEvents2_NavigateComplete2EventHandler(Browser_NavigateComplete2);
				this.BeforeNavigate2 += new AxSHDocVw.DWebBrowserEvents2_BeforeNavigate2EventHandler(Browser_BeforeNavigate2);
			}
			else
			{
				this.NavigateComplete2 -= new AxSHDocVw.DWebBrowserEvents2_NavigateComplete2EventHandler(Browser_NavigateComplete2);
				this.BeforeNavigate2 -= new AxSHDocVw.DWebBrowserEvents2_BeforeNavigate2EventHandler(Browser_BeforeNavigate2);
			}
		}

		/// <summary>
		/// Navigates the browser to a given URL.
		/// </summary>
		/// <param name="url">The URL to navigate to.</param>
		/// <remarks>
		/// <para>
		/// Generally this method should only be used to navigate to local filesystem
		/// pages and only once the handle to the control has been created.
		/// </para>
		/// </remarks>
		public virtual void Navigate(string url)
		{
			if (this.IsHandleCreated)
			{
				object flags = 0;
				object targetFrame = string.Empty;
				object postData = string.Empty;
				object headers = string.Empty;
				this.Navigate(url, ref flags, ref targetFrame, ref postData, ref headers);
			}
			else
			{
				this._tempUrl = url;
			}
		}

		/// <summary>
		/// Replaces the complete HTML document in the preview window.
		/// </summary>
		/// <param name="html">
		/// The complete HTML contents of the document to display.
		/// </param>
		/// <remarks>
		/// <para>
		/// Generally this only occurs on initialization or when options change
		/// to a different transformation engine. You have to navigate the browser
		/// to somewhere other than about:blank or there won't be a document
		/// available to modify later.
		/// </para>
		/// </remarks>
		public virtual void ResetToBaseDocument(string html)
		{
			if (this._tempUrl.Length == 0)
			{
				// Get a temp file name
				this._tempUrl = System.IO.Path.GetTempFileName();
				if (File.Exists(this._tempUrl))
				{
					File.Delete(this._tempUrl);
				}

				// Make it an HTML file
				this._tempUrl += ".htm";
			}

			using (StreamWriter writer = File.CreateText(this._tempUrl))
			{
				writer.WriteLine(html);
			}

			this.Navigate(this._tempUrl);
		}

		#endregion

		#endregion

	}
}
