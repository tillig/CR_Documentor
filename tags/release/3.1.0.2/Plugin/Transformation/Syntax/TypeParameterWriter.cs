using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using DevExpress.CodeRush.StructuralParser;

namespace CR_Documentor.Transformation.Syntax
{
	/// <summary>
	/// Writes type parameter syntax previews.
	/// </summary>
	public static class TypeParameterWriter
	{
		/// <summary>
		/// Gets a value indicating if type parameter constraints get shown
		/// inline with the parameters.
		/// </summary>
		/// <returns>
		/// <see langword="true" /> if type parameters get shown inline
		/// like in VB; <see langword="false" /> if they appear after the member
		/// signature like in C#.
		/// </returns>
		public static bool ShowTypeParameterConstraintsInline(SupportedLanguageId language)
		{
			return language == SupportedLanguageId.Basic;
		}

		/// <summary>
		/// Writes the info for all type parameter constraints to the object signature.
		/// Used when the constraints are not written inline during the type parameter
		/// rendering.
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
		public static void WriteConstraintsPostSignature(HtmlTextWriter writer, AccessSpecifiedElement element, SupportedLanguageId language)
		{
			if (ShowTypeParameterConstraintsInline(language) || !element.HasGenericParameters())
			{
				return;
			}
			var typeParams = element.GenericModifier.TypeParameters;
			int parameterCount = typeParams.Count;
			bool constraintsExist = false;
			for (int i = 0; i < parameterCount; i++)
			{
				var parameter = typeParams[i];
				var constraints = parameter.Constraints;
				var constraintCount = constraints.Count;
				if (constraintCount < 1)
				{
					continue;
				}
				if (!constraintsExist)
				{
					// Write the block for the constraints when the first one is encountered
					writer.AddAttribute(HtmlTextWriterAttribute.Class, PreviewCss.Constraints);
					writer.RenderBeginTag(HtmlTextWriterTag.Div);
					constraintsExist = true;
				}
				writer.AddAttribute(HtmlTextWriterAttribute.Class, PreviewCss.Constraint);
				writer.RenderBeginTag(HtmlTextWriterTag.Div);
				writer.WriteSpan(PreviewCss.Keyword, "where");
				writer.WriteSpan(PreviewCss.TypeParameter, parameter.Name);
				writer.Write(": ");
				for (int j = 0; j < constraintCount; j++)
				{
					WriteConstraintValue(writer, constraints[j]);
					if (j + 1 < constraintCount)
					{
						writer.Write(", ");
					}
				}
				writer.RenderEndTag();
			}
			if (constraintsExist)
			{
				// Close the constraint block if it was opened.
				writer.RenderEndTag();
			}
		}

		/// <summary>
		/// Writes the info for an individual type parameter constraint to the object signature
		/// for inline constraints.
		/// </summary>
		/// <param name="writer">
		/// The <see cref="System.Web.UI.HtmlTextWriter"/> to which the preview
		/// is being written.
		/// </param>
		/// <param name="parameter">
		/// The type parameter constraint for which the information is being written.
		/// </param>
		/// <param name="language">
		/// The langauge for which parametes are being written.
		/// </param>
		private static void WriteInlineConstraint(HtmlTextWriter writer, TypeParameter parameter, SupportedLanguageId language)
		{
			if (!ShowTypeParameterConstraintsInline(language))
			{
				return;
			}
			var constraints = parameter.Constraints;
			int constraintCount = constraints.Count;
			if (constraintCount == 0)
			{
				return;
			}

			writer.AddAttribute(HtmlTextWriterAttribute.Class, PreviewCss.Constraints);
			writer.RenderBeginTag(HtmlTextWriterTag.Div);
			writer.WriteSpan(PreviewCss.Keyword, "As", "&nbsp;", " ");
			if (constraintCount > 1)
			{
				writer.Write("{");
			}
			writer.AddAttribute(HtmlTextWriterAttribute.Class, PreviewCss.Constraint);
			writer.RenderBeginTag(HtmlTextWriterTag.Div);
			for (int j = 0; j < constraintCount; j++)
			{
				WriteConstraintValue(writer, constraints[j]);
				if (j + 1 < constraintCount)
				{
					writer.Write(", ");
				}
			}
			writer.RenderEndTag();
			if (constraintCount > 1)
			{
				writer.Write("}");
			}
			writer.RenderEndTag();
		}

		private static void WriteConstraintValue(HtmlTextWriter writer, TypeParameterConstraint constraint)
		{
			if (constraint is NamedTypeParameterConstraint)
			{
				// TODO: Consider rendering the complete signature of the type in the constraint.
				// While NamedTypeParameterConstraint.TypeReference gives you a
				// reference to a Type in a constraint, if that type is generic
				// we'd have to recursively process it and render that, too. For
				// now, if there's a constraint like...
				// where T : IList<Something>
				// it will render as...
				// where T : IList
				// because rendering the full type in a language-specific fashion is non-trivial.
				writer.WriteLink(((NamedTypeParameterConstraint)constraint).TypeReference.ToString(), "", "");
			}
			else
			{
				writer.WriteSpan(PreviewCss.Keyword, constraint.Name, "", "");
			}
		}

		/// <summary>
		/// Writes the type parameter information to the object signature.
		/// </summary>
		/// <param name="writer">
		/// The <see cref="System.Web.UI.HtmlTextWriter"/> to which the preview
		/// is being written.
		/// </param>
		/// <param name="element">
		/// The element for which parameters are being written.
		/// </param>
		/// <param name="language">
		/// The language for which parameters are being written.
		/// </param>
		public static void WriteTypeParameters(HtmlTextWriter writer, AccessSpecifiedElement element, SupportedLanguageId language)
		{
			if (!element.HasGenericParameters())
			{
				return;
			}

			writer.AddAttribute(HtmlTextWriterAttribute.Class, PreviewCss.TypeParameters);
			writer.RenderBeginTag(HtmlTextWriterTag.Div);
			switch (language)
			{
				case SupportedLanguageId.Basic:
					writer.Write("(");
					writer.WriteSpan(PreviewCss.Keyword, "Of");
					break;
				default:
					writer.Write("&lt;");
					break;
			}

			var typeParams = element.GenericModifier.TypeParameters;
			int parameterCount = typeParams.Count;
			for (int i = 0; i < parameterCount; i++)
			{
				var parameter = typeParams[i];
				writer.WriteSpan(PreviewCss.TypeParameter, parameter.Name, "", "");
				if (parameter.Constraints.Count > 0)
				{
					WriteInlineConstraint(writer, parameter, language);
				}
				if (i + 1 < parameterCount)
				{
					writer.Write(", ");
				}
			}

			switch (language)
			{
				case SupportedLanguageId.Basic:
					writer.Write(")");
					break;
				default:
					writer.Write("&gt;");
					break;
			}
			writer.RenderEndTag();
		}
	}
}
