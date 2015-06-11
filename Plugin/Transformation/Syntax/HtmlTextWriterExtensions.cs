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
		/// <summary>
		/// Writes a hyperlink with HTML-encoded contents.
		/// </summary>
		/// <param name="writer">The writer to which the content should be written.</param>
		/// <param name="contents">The contents to appear inside the tag.</param>
		/// <param name="before">Text that should appear before the tag. If <see langword="null" />, nothing will be written before the tag.</param>
		/// <param name="after">Text that should appear after the tag. If <see langword="null" />, a space will be written after the tag.</param>
		/// <exception cref="System.ArgumentNullException">
		/// Thrown if <paramref name="writer" /> is <see langword="null" />.
		/// </exception>
		public static void WriteLink(this HtmlTextWriter writer, string contents, string before, string after)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
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
		/// <exception cref="System.ArgumentNullException">
		/// Thrown if <paramref name="writer" /> is <see langword="null" />.
		/// </exception>
		public static void WriteDiv(this HtmlTextWriter writer, string cssClass, string contents)
		{
			writer.WriteDiv(cssClass, contents, null, null);
		}

		/// <summary>
		/// Writes a DIV with HTML-encoded contents.
		/// </summary>
		/// <param name="writer">The writer to which the content should be written.</param>
		/// <param name="cssClass">The CSS class to be applied to the tag.</param>
		/// <param name="contents">The contents to appear inside the tag.</param>
		/// <param name="before">Text that should appear before the tag. If <see langword="null" />, nothing will be written before the tag.</param>
		/// <param name="after">Text that should appear after the tag. If <see langword="null" />, a space will be written after the tag.</param>
		/// <exception cref="System.ArgumentNullException">
		/// Thrown if <paramref name="writer" /> is <see langword="null" />.
		/// </exception>
		public static void WriteDiv(this HtmlTextWriter writer, string cssClass, string contents, string before, string after)
		{
			writer.WriteTag(HtmlTextWriterTag.Div, cssClass, contents, before, after);
		}

		/// <summary>
		/// Writes a SPAN with HTML-encoded contents. The tag will not be preceded
		/// by any text and will be followed by a space.
		/// </summary>
		/// <param name="writer">The writer to which the content should be written.</param>
		/// <param name="cssClass">The CSS class to be applied to the tag.</param>
		/// <param name="contents">The contents to appear inside the tag.</param>
		/// <exception cref="System.ArgumentNullException">
		/// Thrown if <paramref name="writer" /> is <see langword="null" />.
		/// </exception>
		public static void WriteSpan(this HtmlTextWriter writer, string cssClass, string contents)
		{
			writer.WriteSpan(cssClass, contents, null, null);
		}

		/// <summary>
		/// Writes a SPAN with HTML-encoded contents.
		/// </summary>
		/// <param name="writer">The writer to which the content should be written.</param>
		/// <param name="cssClass">The CSS class to be applied to the tag.</param>
		/// <param name="contents">The contents to appear inside the tag.</param>
		/// <param name="before">Text that should appear before the tag. If <see langword="null" />, nothing will be written before the tag.</param>
		/// <param name="after">Text that should appear after the tag. If <see langword="null" />, a space will be written after the tag.</param>
		/// <exception cref="System.ArgumentNullException">
		/// Thrown if <paramref name="writer" /> is <see langword="null" />.
		/// </exception>
		public static void WriteSpan(this HtmlTextWriter writer, string cssClass, string contents, string before, string after)
		{
			writer.WriteTag(HtmlTextWriterTag.Span, cssClass, contents, before, after);
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
		/// <exception cref="System.ArgumentNullException">
		/// Thrown if <paramref name="writer" /> is <see langword="null" />.
		/// </exception>
		public static void WriteTag(this HtmlTextWriter writer, HtmlTextWriterTag tagKey, string cssClass, string contents, string before, string after)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
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
