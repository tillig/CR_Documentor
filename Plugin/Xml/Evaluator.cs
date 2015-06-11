using System;
using System.Xml;

namespace CR_Documentor.Xml
{
	/// <summary>
	/// Provides helpful evaluation functions for working with XML.
	/// </summary>
	public static class Evaluator
	{

		/// <summary>
		/// Tests a selected node to see if it contains nodes matching the selected XPath.
		/// </summary>
		/// <param name="parent">
		/// The root <see cref="System.Xml.XmlNode"/> to select children of via XPath.
		/// </param>
		/// <param name="xpath">
		/// The XPath query to execute on the parent node.
		/// </param>
		/// <exception cref="System.ArgumentNullException">
		/// Thrown if <paramref name="parent" /> is <see langword="null" />.
		/// </exception>
		/// <exception cref="System.ArgumentException">
		/// Thrown if <paramref name="xpath" /> is <see langword="null" /> or <see cref="System.String.Empty" />.
		/// </exception>
		/// <returns>
		/// <see langword="true" /> if the parent contains nodes matching the
		/// XPath query; <see langword="false" /> otherwise.
		/// </returns>
		public static bool Test(XmlNode parent, string xpath)
		{
			if (parent == null)
			{
				throw new ArgumentNullException("parent");
			}
			if (String.IsNullOrEmpty(xpath))
			{
				throw new ArgumentException("XPath expression may not be null or empty.", "xpath");
			}
			XmlNodeList list = parent.SelectNodes(xpath);
			return (list.Count != 0);
		}

		/// <summary>
		/// Returns the inner text of a node selected via XPath.
		/// </summary>
		/// <param name="parent">
		/// The root <see cref="System.Xml.XmlNode"/> to select children of via XPath.
		/// </param>
		/// <param name="xpath">
		/// The XPath query to execute on the parent node.
		/// </param>
		/// <exception cref="System.ArgumentNullException">
		/// Thrown if <paramref name="parent" /> is <see langword="null" />.
		/// </exception>
		/// <exception cref="System.ArgumentException">
		/// Thrown if <paramref name="xpath" /> is <see langword="null" /> or <see cref="System.String.Empty" />.
		/// </exception>
		/// <returns>The encoded inner text of a selected node.</returns>
		public static string ValueOf(XmlNode parent, string xpath)
		{
			if (parent == null)
			{
				throw new ArgumentNullException("parent");
			}
			if (String.IsNullOrEmpty(xpath))
			{
				throw new ArgumentException("XPath expression may not be null or empty.", "xpath");
			}

			XmlNodeList list = parent.SelectNodes(xpath);
			if (list.Count == 1)
			{
				string value = list[0].InnerText;
				return value;
			}

			return string.Empty;
		}
	}
}
