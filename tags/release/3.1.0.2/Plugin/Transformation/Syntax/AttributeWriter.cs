using System;
using System.Web.UI;
using DevExpress.CodeRush.StructuralParser;

namespace CR_Documentor.Transformation.Syntax
{
	/// <summary>
	/// Writes the syntax preview for attributes.
	/// </summary>
	public static class AttributeWriter
	{
		/// <summary>
		/// Writes attribute argument information.
		/// </summary>
		/// <param name="writer">
		/// The <see cref="System.Web.UI.HtmlTextWriter"/> to which the preview
		/// is being written.
		/// </param>
		/// <param name="arguments">The attribute arguments to document.</param>
		/// <param name="language">
		/// The programming language for which the contract is being written.
		/// </param>
		private static void AttributeArguments(HtmlTextWriter writer, ExpressionCollection arguments, SupportedLanguageId language)
		{
			if (arguments == null || arguments.Count < 1)
			{
				return;
			}
			writer.AddAttribute(HtmlTextWriterAttribute.Class, PreviewCss.Parameters);
			writer.RenderBeginTag(HtmlTextWriterTag.Div);
			writer.Write("(");
			for (int j = 0; j < arguments.Count; j++)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Class, PreviewCss.Parameter);
				writer.RenderBeginTag(HtmlTextWriterTag.Div);
				Expression argument = arguments[j];
				if (argument is BinaryOperatorExpression)
				{
					var init = (BinaryOperatorExpression)argument;
					writer.WriteSpan(PreviewCss.Identifier, init.LeftSide.ToString());
					switch (language)
					{
						case SupportedLanguageId.Basic:
							writer.Write(":= ");
							break;
						default:
							writer.Write("= ");
							break;
					}
					writer.WriteSpan(PreviewCss.Literal, init.RightSide.ToString(), "", "");
				}
				else
				{
					writer.WriteSpan(PreviewCss.Literal, argument.ToString(), "", "");
				}
				if (j + 1 < arguments.Count)
				{
					writer.Write(", ");
				}
				writer.RenderEndTag();
			}
			writer.Write(")");
			writer.RenderEndTag();
		}

		/// <summary>
		/// Writes attribute information.
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
		public static void Write(HtmlTextWriter writer, AccessSpecifiedElement element, SupportedLanguageId language)
		{
			if (element.AttributeCount < 1)
			{
				return;
			}

			writer.AddAttribute(HtmlTextWriterAttribute.Class, PreviewCss.Attributes);
			writer.RenderBeginTag(HtmlTextWriterTag.Div);
			var attributes = element.Attributes;
			for (int i = 0; i < attributes.Count; i++)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Class, PreviewCss.Attribute);
				writer.RenderBeginTag(HtmlTextWriterTag.Div);
				switch (language)
				{
					case SupportedLanguageId.Basic:
						writer.Write("&lt;");
						break;
					default:
						writer.Write("[");
						break;
				}
				var attribute = (DevExpress.CodeRush.StructuralParser.Attribute)attributes[i];
				writer.WriteLink(attribute.Name, "", "");
				AttributeArguments(writer, attribute.Arguments, language);
				switch (language)
				{
					case SupportedLanguageId.Basic:
						writer.Write("&gt; _");
						break;
					default:
						writer.Write("]");
						break;
				}
				writer.RenderEndTag();
			}
			writer.RenderEndTag();
		}
	}
}
