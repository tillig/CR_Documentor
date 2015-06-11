using System;
using System.IO;
using System.Web;
using System.Xml;

namespace CR_Documentor.Xml
{
	/// <summary>
	/// Processes <see cref="System.Xml.XmlNode"/> values into text values.
	/// </summary>
	public static class TextProcessor
	{
		/// <summary>
		/// Echoes attributes that are found on a given element to a <see cref="System.IO.TextWriter"/>.
		/// </summary>
		/// <param name="writer">The writer to pass the attributes to.</param>
		/// <param name="parent">The element containing the attributes.</param>
		public static void AttributePassThrough(TextWriter writer, XmlNode parent)
		{
			foreach (XmlAttribute attrib in parent.Attributes)
			{
				writer.Write(String.Format(" {0}=\"{1}\"", attrib.Name, attrib.Value));
			}
		}

		/// <summary>
		/// Echoes attributes that are found on a given element to a <see cref="System.IO.TextWriter"/>.
		/// </summary>
		/// <param name="writer">The writer to pass the attributes to.</param>
		/// <param name="parent">The element containing the attributes.</param>
		/// <param name="attribs">The list of attributes that should pass through.</param>
		public static void AttributePassThrough(TextWriter writer, XmlNode parent, string[] attribs)
		{
			foreach (string attrib in attribs)
			{
				string attribXpath = "@" + attrib;
				if (Evaluator.Test(parent, attribXpath))
				{
					writer.Write(String.Format(" {0}=\"{1}\"", attrib, Evaluator.ValueOf(parent, attribXpath)));
				}
			}
		}

		/// <summary>
		/// Writes a text node value to a <see cref="System.IO.TextWriter"/>.
		/// </summary>
		/// <param name="writer">The writer to write the text value to.</param>
		/// <param name="node">The <see cref="System.Xml.XmlNode"/> containing text or CDATA.</param>
		public static void TextNode(TextWriter writer, XmlNode node)
		{
			// If it's a straight text block, encode, then dump it.
			// (Must encode because InnerText gets decoded "for us.")
			XmlText text = node as XmlText;
			if (text != null)
			{
				writer.Write(HttpUtility.HtmlEncode(text.InnerText));
				return;
			}

			// If it's a CData block, encode, then dump it.
			// (We don't encode twice because CData InnerText
			// doesn't automatically decode when we access.)
			XmlCDataSection cdatasection = node as XmlCDataSection;
			if (cdatasection != null)
			{
				writer.Write(HttpUtility.HtmlEncode(cdatasection.InnerText));
			}
		}
	}
}
