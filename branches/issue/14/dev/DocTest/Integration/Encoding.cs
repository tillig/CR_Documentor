using System;

namespace DocTest.Integration
{
	/// <summary>
	/// Class used for testing encoding/display issues.
	/// </summary>
	public class Encoding
	{
		/// <summary>
		/// This method illustrates different HTML encoding issues.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Standard text tests:
		/// </para>
		/// <para>
		/// Encoded characters that should display as literals:
		/// &lt; &apos; &quot; &amp; &gt;
		/// </para>
		/// <para>
		/// Unencoded characters that are optionally encoded: " '
		/// </para>
		/// <para>
		/// Code block tests:
		/// </para>
		/// <code>
		/// Encoded characters that should display as literals:
		/// &lt; &apos; &quot; &amp; &gt;
		/// </code>
		/// <code>
		/// Unencoded characters that are optionally encoded: " '
		/// </code>
		/// </remarks>
		public void HtmlEncoding()
		{
		}
	}
}
