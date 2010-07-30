using System;
using System.Web.UI;
using DevExpress.CodeRush.StructuralParser;

namespace CR_Documentor.Transformation.Syntax
{
	/// <summary>
	/// Writes the contract information (abstract/overrides/virtual/etc.) for an element.
	/// </summary>
	public static class ContractWriter
	{
		/// <summary>
		/// Writes the contract (static/abstract/sealed/etc.) for the element.
		/// </summary>
		/// <param name="writer">
		/// The <see cref="System.Web.UI.HtmlTextWriter"/> to which the preview
		/// is being written.
		/// </param>
		/// <param name="element">
		/// The element for which the contract is being written.
		/// </param>
		/// <param name="language">
		/// The programming language for which the contract is being written.
		/// </param>
		/// <exception cref="System.ArgumentNullException">
		/// Thrown if <paramref name="writer" /> or <paramref name="element" /> is <see langword="null" />.
		/// </exception>
		public static void Write(HtmlTextWriter writer, AccessSpecifiedElement element, SupportedLanguageId language)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (element.IsStatic)
			{
				switch (language)
				{
					case SupportedLanguageId.Basic:
						HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "Shared");
						break;
					default:
						HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "static");
						break;
				}
			}
			else if (element.IsAbstract)
			{
				switch (language)
				{
					case SupportedLanguageId.Basic:
						if (element is Method)
						{
							HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "MustOverride");
						}
						else
						{
							HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "MustInherit");
						}
						break;
					default:
						HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "abstract");
						break;
				}
			}
			else if (element.IsSealed)
			{
				switch (language)
				{
					case SupportedLanguageId.Basic:
						HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "NotInheritable");
						break;
					default:
						HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "sealed");
						break;
				}
			}
			else if (element.IsConst)
			{
				switch (language)
				{
					case SupportedLanguageId.Basic:
						HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "Const");
						break;
					default:
						HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "const");
						break;
				}
			}
			else if (element.IsVirtual)
			{
				switch (language)
				{
					case SupportedLanguageId.Basic:
						HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "Overridable");
						break;
					default:
						HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "virtual");
						break;
				}
			}
			else if (element.IsOverride)
			{
				switch (language)
				{
					case SupportedLanguageId.Basic:
						HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "Overrides");
						break;
					default:
						HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "override");
						break;
				}
			}

			if (element.IsReadOnly)
			{
				switch (language)
				{
					case SupportedLanguageId.Basic:
						HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "ReadOnly");
						break;
					default:
						HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "readonly");
						break;
				}
			}
		}
	}
}
