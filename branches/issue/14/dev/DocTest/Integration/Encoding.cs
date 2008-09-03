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
		/// HTML tag inserted in doc: &lt;faketag /&gt;.
		/// </para>
		/// <para>
		/// Encoded characters that should display as literals:
		/// &lt; &#xe1; &gt; &apos; &quot; &amp;
		/// </para>
		/// <para>
		/// Unencoded characters that are optionally encoded: " ' á
		/// </para>
		/// <para>
		/// CDATA block: <![CDATA[< á > ' " &]]>
		/// </para>
		/// <para>
		/// Code block tests:
		/// </para>
		/// <code>
		/// Encoded characters that should display as literals:
		/// &lt; &#xe1; &apos; &quot; &amp; &gt;
		/// </code>
		/// <code>
		/// Unencoded characters that are optionally encoded: " ' á
		/// </code>
		/// <code>
		/// CDATA block: <![CDATA[< á > ' " &]]>
		/// </code>
		/// </remarks>
		public void HtmlEncoding()
		{
		}
	}
}
