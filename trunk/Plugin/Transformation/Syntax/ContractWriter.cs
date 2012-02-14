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
						writer.WriteSpan(PreviewCss.Keyword, "Shared");
						break;
					default:
						writer.WriteSpan(PreviewCss.Keyword, "static");
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
							writer.WriteSpan(PreviewCss.Keyword, "MustOverride");
						}
						else
						{
							writer.WriteSpan(PreviewCss.Keyword, "MustInherit");
						}
						break;
					default:
						writer.WriteSpan(PreviewCss.Keyword, "abstract");
						break;
				}
			}
			else if (element.IsSealed)
			{
				switch (language)
				{
					case SupportedLanguageId.Basic:
						writer.WriteSpan(PreviewCss.Keyword, "NotInheritable");
						break;
					default:
						writer.WriteSpan(PreviewCss.Keyword, "sealed");
						break;
				}
			}
			else if (element.IsConst)
			{
				switch (language)
				{
					case SupportedLanguageId.Basic:
						writer.WriteSpan(PreviewCss.Keyword, "Const");
						break;
					default:
						writer.WriteSpan(PreviewCss.Keyword, "const");
						break;
				}
			}
			else if (element.IsVirtual)
			{
				switch (language)
				{
					case SupportedLanguageId.Basic:
						writer.WriteSpan(PreviewCss.Keyword, "Overridable");
						break;
					default:
						writer.WriteSpan(PreviewCss.Keyword, "virtual");
						break;
				}
			}
			else if (element.IsOverride)
			{
				switch (language)
				{
					case SupportedLanguageId.Basic:
						writer.WriteSpan(PreviewCss.Keyword, "Overrides");
						break;
					default:
						writer.WriteSpan(PreviewCss.Keyword, "override");
						break;
				}
			}

			if (element.IsReadOnly)
			{
				switch (language)
				{
					case SupportedLanguageId.Basic:
						writer.WriteSpan(PreviewCss.Keyword, "ReadOnly");
						break;
					default:
						writer.WriteSpan(PreviewCss.Keyword, "readonly");
						break;
				}
			}
		}
	}
}
