using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.CodeRush.StructuralParser;
using System.IO;

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
		/// Generates the HTML syntax preview for an element.
		/// </summary>
		public virtual string Generate()
		{
			if (this.Language == SupportedLanguageId.None || !AccessSpecifiedElementExtensions.IsSupportedForPreview(this.Element))
			{
				return "";
			}
			using (var baseWriter = new StringWriter())
			{
				return this.Element.Name;
			}
		}
	}
}
