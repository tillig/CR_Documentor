using System;
using System.Collections.Specialized;
using System.Windows.Forms;

namespace CR_Documentor.Controls
{
	/// <summary>
	/// Hosted web browser for displaying HTML.
	/// </summary>
	public class Browser : AxSHDocVw.AxWebBrowser
	{
		/// <summary>
		/// The prefix for a URL that refers to member documentation.
		/// </summary>
		public const string MEMBER_URL_PREFIX = "urn:member:";

		/// <summary>
		/// Initializes a new instance of the <see cref="CR_Documentor.Controls.Browser" /> class.
		/// </summary>
		public Browser()
		{
			this.BeforeNavigate2 += new AxSHDocVw.DWebBrowserEvents2_BeforeNavigate2EventHandler(Browser_BeforeNavigate2);
			this.SafeUrls = new StringCollection();
		}

		/// <summary>
		/// Gets the list of URLs the browser is allowed to navigate to.
		/// </summary>
		/// <value>
		/// A <see cref="System.Collections.Specialized.StringCollection"/> with the list of
		/// URLs the browser is allowed to visit.
		/// </value>
		public StringCollection SafeUrls { get; private set; }

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
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.BeforeNavigate2 -= new AxSHDocVw.DWebBrowserEvents2_BeforeNavigate2EventHandler(Browser_BeforeNavigate2);
			}
			base.Dispose(disposing);
		}

		/// <summary>
		/// Stops the browser from attempting to navigate member-to-member or out onto
		/// the web.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An <see cref="System.EventArgs" /> that contains the event data.</param>
		private void Browser_BeforeNavigate2(object sender, AxSHDocVw.DWebBrowserEvents2_BeforeNavigate2Event e)
		{
			string url = e.uRL.ToString();
			if (url.StartsWith(MEMBER_URL_PREFIX) || !this.SafeUrls.Contains(url))
			{
				// Cancel non-local navigation
				e.cancel = true;
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
		}

		/// <summary>
		/// Reloads the currently loaded page.
		/// </summary>
		public virtual void Reload()
		{
			object dummy = new object();
			this.Refresh2(ref dummy);
			this.Refresh();
		}
	}
}
