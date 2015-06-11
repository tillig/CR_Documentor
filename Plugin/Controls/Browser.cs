using System;
using System.Collections.Specialized;
using System.Windows.Forms;

namespace CR_Documentor.Controls
{
	/// <summary>
	/// Hosted web browser for displaying HTML.
	/// </summary>
	public class Browser : WebBrowser
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CR_Documentor.Controls.Browser" /> class.
		/// </summary>
		public Browser()
		{
			this.Navigating += new WebBrowserNavigatingEventHandler(Browser_Navigating);
			this.ScriptErrorsSuppressed = true;
			this.SafeUrls = new StringCollection();
			this.AllowNavigation = false;
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
		/// Stops the browser from attempting to navigate member-to-member or out onto
		/// the web.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An <see cref="System.Windows.Forms.WebBrowserNavigatingEventArgs" /> that contains the event data.</param>
		private void Browser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
		{
			string url = e.Url.ToString();
			if (url.StartsWith("urn:member:") || !this.SafeUrls.Contains(url))
			{
				// Cancel non-local navigation
				e.Cancel = true;
			}
		}

		/// <summary>
		/// Removes ESC from the list of input keys.
		/// </summary>
		/// <param name="charCode">The character code coming in.</param>
		/// <returns><see langword="true" /> if the specified key is a regular input key; otherwise, <see langword="false" />.</returns>
		protected override bool IsInputChar(char charCode)
		{
			return (charCode == (char)27) ? false : base.IsInputChar(charCode);
		}
	}
}
