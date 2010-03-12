using System;
using System.Collections.Specialized;
using System.Xml;

namespace CR_Documentor.Xml
{
	/// <summary>
	/// Processes link definitions in XML documentation comments.
	/// </summary>
	public static class LinkProcessor
	{

		/// <summary>
		/// Processes duplicate "see" links.
		/// </summary>
		/// <param name="parent">The node whose children need to be processed.</param>
		/// <param name="xpath">An XPath statement to select which children to process.</param>
		/// <remarks>
		/// <para>
		/// The first link (<c>see</c> tag) with a given target will be left alone;
		/// subsequent links will have a <c>nolink</c> attribute added.  Transformation
		/// engines can use this attribute to determine whether or not to add a hyperlink.
		/// </para>
		/// </remarks>
		public static void RemoveDuplicates(XmlNode parent, string xpath)
		{
			RemoveDuplicates(parent.SelectNodes(xpath));
		}

		/// <summary>
		/// Processes duplicate "see" links.
		/// </summary>
		/// <param name="list">The list of nodes to process.</param>
		/// <remarks>
		/// <para>
		/// The first link (<c>see</c> tag) with a given target will be left alone;
		/// subsequent links will have a <c>nolink</c> attribute added.  Transformation
		/// engines can use this attribute to determine whether or not to add a hyperlink.
		/// </para>
		/// </remarks>
		private static void RemoveDuplicates(XmlNodeList list)
		{
			foreach (XmlNode node in list)
			{
				StringCollection seenLinks = new StringCollection();
				RemoveDuplicates(node, seenLinks);
			}
		}

		/// <summary>
		/// Processes duplicate "see" links.
		/// </summary>
		/// <param name="node">The node to process.</param>
		/// <param name="seenLinks">The collection of links that have been "seen"</param>
		/// <remarks>
		/// <para>
		/// The first link (<c>see</c> tag) with a given target will be left alone;
		/// subsequent links will have a <c>nolink</c> attribute added.  Transformation
		/// engines can use this attribute to determine whether or not to add a hyperlink.
		/// </para>
		/// </remarks>
		private static void RemoveDuplicates(XmlNode node, StringCollection seenLinks)
		{
			XmlElement element = node as XmlElement;
			if (element != null)
			{
				if (node.Name == "see")
				{
					if (Evaluator.Test(node, "@cref"))
					{
						string cref = Evaluator.ValueOf(element, "@cref");
						if (seenLinks.Contains(cref))
						{
							XmlAttribute seen = node.OwnerDocument.CreateAttribute("nolink");
							seen.Value = "true";
							node.Attributes.Append(seen);
						}
						else
						{
							seenLinks.Add(cref);
						}
					}
				}
			}
			foreach (XmlNode child in node.ChildNodes)
			{
				RemoveDuplicates(child, seenLinks);
			}
		}

	}
}
