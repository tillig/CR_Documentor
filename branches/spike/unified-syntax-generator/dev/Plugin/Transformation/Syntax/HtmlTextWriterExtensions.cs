using System;
using System.Web;
using System.Web.UI;

namespace CR_Documentor.Transformation.Syntax
{
	/// <summary>
	/// Extension methods for the <see cref="System.Web.UI.HtmlTextWriter"/> class.
	/// </summary>
	public static class HtmlTextWriterExtensions
	{
		// TODO: When we target .NET 3.5, convert these to real extension methods.

		/// <summary>
		/// Writes a hyperlink with HTML-encoded contents.
		/// </summary>
		/// <param name="writer">The writer to which the content should be written.</param>
		/// <param name="contents">The contents to appear inside the tag.</param>
		/// <param name="before">Text that should appear before the tag. If <see langword="null" />, nothing will be written before the tag.</param>
		/// <param name="after">Text that should appear after the tag. If <see langword="null" />, a space will be written after the tag.</param>
		public static void WriteLink(HtmlTextWriter writer, string contents, string before, string after)
		{
			if (string.IsNullOrEmpty(contents))
			{
				return;
			}
			if (before != null)
			{
				writer.Write(before);
			}
			writer.Write("<a href=\"#\">");
			writer.Write(HttpUtility.HtmlEncode(contents));
			writer.Write("</a>");
			if (after != null)
			{
				writer.Write(after);
			}
			else
			{
				writer.Write(" ");
			}
		}

		/// <summary>
		/// Writes a DIV with HTML-encoded contents. The tag will not be preceded
		/// by any text and will be followed by a space.
		/// </summary>
		/// <param name="writer">The writer to which the content should be written.</param>
		/// <param name="cssClass">The CSS class to be applied to the tag.</param>
		/// <param name="contents">The contents to appear inside the tag.</param>
		public static void WriteDiv(HtmlTextWriter writer, string cssClass, string contents)
		{
			WriteDiv(writer, cssClass, contents, null, null);
		}

		/// <summary>
		/// Writes a DIV with HTML-encoded contents.
		/// </summary>
		/// <param name="writer">The writer to which the content should be written.</param>
		/// <param name="cssClass">The CSS class to be applied to the tag.</param>
		/// <param name="contents">The contents to appear inside the tag.</param>
		/// <param name="before">Text that should appear before the tag. If <see langword="null" />, nothing will be written before the tag.</param>
		/// <param name="after">Text that should appear after the tag. If <see langword="null" />, a space will be written after the tag.</param>
		public static void WriteDiv(HtmlTextWriter writer, string cssClass, string contents, string before, string after)
		{
			WriteTag(writer, HtmlTextWriterTag.Div, cssClass, contents, before, after);
		}

		/// <summary>
		/// Writes a SPAN with HTML-encoded contents. The tag will not be preceded
		/// by any text and will be followed by a space.
		/// </summary>
		/// <param name="writer">The writer to which the content should be written.</param>
		/// <param name="cssClass">The CSS class to be applied to the tag.</param>
		/// <param name="contents">The contents to appear inside the tag.</param>
		public static void WriteSpan(HtmlTextWriter writer, string cssClass, string contents)
		{
			WriteSpan(writer, cssClass, contents, null, null);
		}

		/// <summary>
		/// Writes a SPAN with HTML-encoded contents.
		/// </summary>
		/// <param name="writer">The writer to which the content should be written.</param>
		/// <param name="cssClass">The CSS class to be applied to the tag.</param>
		/// <param name="contents">The contents to appear inside the tag.</param>
		/// <param name="before">Text that should appear before the tag. If <see langword="null" />, nothing will be written before the tag.</param>
		/// <param name="after">Text that should appear after the tag. If <see langword="null" />, a space will be written after the tag.</param>
		public static void WriteSpan(HtmlTextWriter writer, string cssClass, string contents, string before, string after)
		{
			WriteTag(writer, HtmlTextWriterTag.Span, cssClass, contents, before, after);
		}

		/// <summary>
		/// Writes a tag with HTML-encoded contents.
		/// </summary>
		/// <param name="writer">The writer to which the content should be written.</param>
		/// <param name="tagKey">The name of the tag to write.</param>
		/// <param name="cssClass">The CSS class to be applied to the tag.</param>
		/// <param name="contents">The contents to appear inside the tag.</param>
		/// <param name="before">Text that should appear before the tag. If <see langword="null" />, nothing will be written before the tag.</param>
		/// <param name="after">Text that should appear after the tag. If <see langword="null" />, a space will be written after the tag.</param>
		public static void WriteTag(HtmlTextWriter writer, HtmlTextWriterTag tagKey, string cssClass, string contents, string before, string after)
		{
			if (string.IsNullOrEmpty(contents))
			{
				return;
			}
			if (before != null)
			{
				writer.Write(before);
			}
			writer.AddAttribute(HtmlTextWriterAttribute.Class, cssClass);
			writer.RenderBeginTag(tagKey);
			writer.Write(HttpUtility.HtmlEncode(contents));
			writer.RenderEndTag();
			if (after != null)
			{
				writer.Write(after);
			}
			else
			{
				writer.Write(" ");
			}
		}
	}
}
