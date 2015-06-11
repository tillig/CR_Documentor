using System;
using System.Web.UI;
using DevExpress.CodeRush.StructuralParser;

namespace CR_Documentor.Transformation.Syntax
{
	/// <summary>
	/// Writes parameter syntax previews.
	/// </summary>
	public static class ParameterWriter
	{
		/// <summary>
		/// Writes parameter information to the object signature.
		/// </summary>
		/// <param name="writer">
		/// The <see cref="System.Web.UI.HtmlTextWriter"/> to which the preview
		/// is being written.
		/// </param>
		/// <param name="element">
		/// The element for which parameters are being written.
		/// </param>
		/// <param name="language">
		/// The langauge for which parametes are being written.
		/// </param>
		/// <param name="openParen">The open parenthesis (usually "(" but sometimes "[").</param>
		/// <param name="closeParen">The close parenthesis (usually ")" but sometimes "]").</param>
		public static void Write(HtmlTextWriter writer, MemberWithParameters element, SupportedLanguageId language, string openParen, string closeParen)
		{
			if (element == null)
			{
				return;
			}
			var parameters = element.Parameters;
			if (parameters == null)
			{
				return;
			}
			int count = parameters.Count;
			bool isBasic = language == SupportedLanguageId.Basic;
			if (isBasic && count == 0)
			{
				// If there aren't any parameters in VB, no parens get
				// rendered or anything.
				return;
			}

			writer.AddAttribute(HtmlTextWriterAttribute.Class, PreviewCss.Parameters);
			writer.RenderBeginTag(HtmlTextWriterTag.Div);
			writer.Write(openParen);
			if (isBasic)
			{
				writer.Write(" _");
			}
			for (int i = 0; i < count; i++)
			{
				var parameter = (Param)parameters[i];
				writer.AddAttribute(HtmlTextWriterAttribute.Class, PreviewCss.Parameter);
				writer.RenderBeginTag(HtmlTextWriterTag.Div);

				// Sandcastle does not render anything around optional parameters
				// but if that changes, we'll need to do something here.
				//if (isBasic && parameter.IsOptional)
				//{
				//    HtmlTextWriterExtensions.WriteSpan(CssClassKeyword, "Optional");
				//}

				if (parameter.IsOutParam)
				{
					switch (language)
					{
						case SupportedLanguageId.Basic:
							writer.WriteLink("OutAttribute", "&lt;", "&gt; ");
							writer.WriteSpan(PreviewCss.Keyword, "ByRef");
							break;
						default:
							writer.WriteSpan(PreviewCss.Keyword, "out");
							break;
					}
				}
				else if (parameter.IsReferenceParam)
				{
					switch (language)
					{
						case SupportedLanguageId.Basic:
							writer.WriteSpan(PreviewCss.Keyword, "ByRef");
							break;
						default:
							writer.WriteSpan(PreviewCss.Keyword, "ref");
							break;
					}
				}

				if (parameter.IsParamArray)
				{
					switch (language)
					{
						case SupportedLanguageId.Basic:
							writer.WriteSpan(PreviewCss.Keyword, "ParamArray");
							break;
						default:
							writer.WriteSpan(PreviewCss.Keyword, "params");
							break;
					}
				}

				if (isBasic)
				{
					writer.WriteSpan(PreviewCss.Identifier, parameter.Name);
					writer.WriteSpan(PreviewCss.Keyword, "As");
				}
				writer.WriteLink(parameter.ParamType, "", "");
				if (!isBasic)
				{
					writer.Write(" ");
					writer.WriteSpan(PreviewCss.Identifier, parameter.Name, "", "");
				}

				// Sandcastle doesn't render the default value settings, but if
				// that changes, we'll have to put the default value here.
				//if (isBasic && parameter.IsOptional)
				//{
				//    this.Writer.Write(" = ");
				//    this.Writer.Write(parameter.DefaultValue);
				//}

				if (i + 1 < count)
				{
					writer.Write(",");
				}
				if (isBasic)
				{
					writer.Write(" _");
				}
				writer.RenderEndTag();

			}

			writer.Write(closeParen);
			writer.RenderEndTag();
		}
	}
}
