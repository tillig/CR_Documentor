using System;
using System.IO;
using DevExpress.CodeRush.StructuralParser;

namespace CR_Documentor.Transformation.Syntax
{
	/// <summary>
	/// Simple syntax generator that just creates a text-only item signature.
	/// Primarily used in class summary doc.
	/// </summary>
	public class SimpleSignatureGenerator
	{
		/// <summary>
		/// Gets the element being documented.
		/// </summary>
		/// <value>
		/// The <see cref="DevExpress.CodeRush.StructuralParser.AccessSpecifiedElement"/>
		/// for which a preview will be generated.
		/// </value>
		public AccessSpecifiedElement Element { get; private set; }

		/// <summary>
		/// Gets the preview language.
		/// </summary>
		/// <value>
		/// The <see cref="CR_Documentor.Transformation.Syntax.SupportedLanguageId"/>
		/// for which a preview will be generated.
		/// </value>
		public SupportedLanguageId Language { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="CR_Documentor.Transformation.Syntax.SimpleSignatureGenerator" /> class.
		/// </summary>
		/// <param name="element">
		/// The element for which the syntax preview should be generated.
		/// </param>
		/// <param name="language">
		/// The programming language in which the preview should be rendered.
		/// </param>
		/// <exception cref="System.ArgumentNullException">
		/// Thrown if <paramref name="element" /> is <see langword="null" />.
		/// </exception>
		public SimpleSignatureGenerator(AccessSpecifiedElement element, SupportedLanguageId language)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			this.Element = element;
			this.Language = language;
		}

		/// <summary>
		/// Writes the preview for a class.
		/// </summary>
		protected virtual string Class()
		{
			using (StringWriter writer = new StringWriter())
			{
				writer.Write(this.Element.Name);
				this.TypeParameters(writer);
				return writer.ToString();
			}
		}

		/// <summary>
		/// Generates the simple signature syntax preview for an element.
		/// </summary>
		public virtual string Generate()
		{
			if (this.Language == SupportedLanguageId.None || !AccessSpecifiedElementExtensions.IsSupportedForPreview(this.Element))
			{
				return "";
			}

			// Only methods/delegates get signature info generated beyond the name.
			if (this.Element is Class)
			{
				return this.Class();
			}
			else if (this.Element is DelegateDefinition || this.Element is Method)
			{
				return this.Method();
			}
			else if (this.Element is Property)
			{
				return this.Property();
			}
			else
			{
				return this.Element.Name;
			}
		}

		/// <summary>
		/// Writes the preview for a method or delegate.
		/// </summary>
		protected virtual string Method()
		{
			using (StringWriter writer = new StringWriter())
			{
				writer.Write(this.Element.Name);
				this.TypeParameters(writer);
				this.Parameters(writer, "(", ")");
				return writer.ToString();
			}
		}

		/// <summary>
		/// Writes parameter information to the object signature.
		/// </summary>
		/// <param name="writer">
		/// The <see cref="System.IO.StringWriter"/> to which the preview
		/// is being written.
		/// </param>
		/// <param name="openParen">The open parenthesis (usually "(" but sometimes "[").</param>
		/// <param name="closeParen">The close parenthesis (usually ")" but sometimes "]").</param>
		protected virtual void Parameters(StringWriter writer, string openParen, string closeParen)
		{
			var element = this.Element as MemberWithParameters;
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
			bool isBasic = this.Language == SupportedLanguageId.Basic;
			if (isBasic && count == 0)
			{
				// If there aren't any parameters in VB, no parens get
				// rendered or anything.
				return;
			}

			writer.Write(openParen);
			for (int i = 0; i < count; i++)
			{
				var parameter = (Param)parameters[i];
				writer.Write(parameter.ParamType);

				// Sandcastle doesn't render the default value settings, but if
				// that changes, we'll have to put the default value here.

				if (i + 1 < count)
				{
					writer.Write(", ");
				}
			}
			writer.Write(closeParen);
		}

		/// <summary>
		/// Writes the preview for a property.
		/// </summary>
		protected virtual string Property()
		{
			var property = (Property)this.Element;
			using (StringWriter writer = new StringWriter())
			{
				if (property.ParameterCount > 0)
				{
					// Indexed properties always get documented at the top
					// level as 'Item'
					writer.Write("Item");
					switch (this.Language)
					{
						case SupportedLanguageId.Basic:
							this.Parameters(writer, "(", ")");
							break;
						default:
							this.Parameters(writer, "[", "]");
							break;
					}
				}
				else
				{
					writer.Write(this.Element.Name);
				}
				return writer.ToString();
			}
		}

		/// <summary>
		/// Writes the type parameters for a simple signature.
		/// </summary>
		/// <param name="writer">
		/// The <see cref="System.IO.StringWriter"/> to which the preview
		/// is being written.
		/// </param>
		protected virtual void TypeParameters(StringWriter writer)
		{
			if (!AccessSpecifiedElementExtensions.HasGenericParameters(this.Element))
			{
				return;
			}

			switch (this.Language)
			{
				case SupportedLanguageId.Basic:
					writer.Write("(Of ");
					break;
				default:
					writer.Write("<");
					break;
			}

			var typeParams = this.Element.GenericModifier.TypeParameters;
			int parameterCount = typeParams.Count;
			for (int i = 0; i < parameterCount; i++)
			{
				var parameter = typeParams[i];
				writer.Write(parameter.Name);
				if (i + 1 < parameterCount)
				{
					writer.Write(", ");
				}
			}

			switch (this.Language)
			{
				case SupportedLanguageId.Basic:
					writer.Write(")");
					break;
				default:
					writer.Write(">");
					break;
			}
		}
	}
}
