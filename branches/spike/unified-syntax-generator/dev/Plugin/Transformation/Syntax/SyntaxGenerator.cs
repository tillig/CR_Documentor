using System;
using System.IO;
using DevExpress.CodeRush.StructuralParser;

namespace CR_Documentor.Transformation.Syntax
{
	/// <summary>
	/// Defines the base behavior for a class that generates a syntax preview.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The creation of member signature inforamtion is expensive - it involves a lot of
	/// programmatic traversal of the code tree.  Whenever possible, computed values should
	/// be cached so the signature is not being generated repeatedly.
	/// </para>
	/// </remarks>
	public abstract class SyntaxGenerator
	{
		/// <summary>
		/// Internal storage for the
		/// <see cref="CR_Documentor.Transformation.Syntax.SyntaxGenerator.Element" />
		/// property.
		/// </summary>
		/// <seealso cref="CR_Documentor.Transformation.Syntax.SyntaxGenerator" />
		private AccessSpecifiedElement _element;

		/// <summary>
		/// Internal storage for the
		/// <see cref="CR_Documentor.Transformation.Syntax.SyntaxGenerator.Writer" />
		/// property.
		/// </summary>
		/// <seealso cref="CR_Documentor.Transformation.Syntax.SyntaxGenerator" />
		private StringWriter _writer;

		/// <summary>
		/// Gets the language the current element's document is written in.
		/// </summary>
		/// <value>
		/// A <see cref="System.String"/> with the language identifier of the
		/// language the <see cref="CR_Documentor.Transformation.Syntax.SyntaxGenerator.Element"/>
		/// document is written in.
		/// </value>
		protected virtual string DocumentLanguage
		{
			get
			{
				return this.Element.Document.Language;
			}
		}

		/// <summary>
		/// Gets the element that the syntax preview will be generated for.
		/// </summary>
		/// <value>
		/// A <see cref="DevExpress.CodeRush.StructuralParser.AccessSpecifiedElement"/>
		/// that is the element for which a syntax preview will be rendered.
		/// </value>
		protected virtual AccessSpecifiedElement Element
		{
			get { return _element; }
		}

		/// <summary>
		/// Gets the member type of the current element, assuming the current element
		/// is a <see cref="DevExpress.CodeRush.StructuralParser.Member"/>.
		/// </summary>
		/// <value>
		/// A <see cref="System.String"/> containing the <see cref="DevExpress.CodeRush.StructuralParser.Member.MemberType"/>
		/// of the <see cref="CR_Documentor.Transformation.Syntax.SyntaxGenerator.Element"/>.
		/// If <see cref="CR_Documentor.Transformation.Syntax.SyntaxGenerator.Element"/>
		/// is not a <see cref="DevExpress.CodeRush.StructuralParser.Member"/>,
		/// this property returns <see langword="null" />.
		/// </value>
		protected virtual string ElementMemberType
		{
			get
			{
				Member member = this.Element as Member;
				if (member == null)
				{
					return null;
				}
				return member.MemberType;
			}
		}

		/// <summary>
		/// Gets the writer that the syntax preview will be written to.
		/// </summary>
		/// <value>
		/// A <see cref="System.IO.StringWriter"/> that the syntax preview for the
		/// current element will be written to.
		/// </value>
		protected virtual StringWriter Writer
		{
			get { return _writer; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CR_Documentor.Transformation.Syntax.SyntaxGenerator"/> class.
		/// </summary>
		/// <param name="element">The element to generate the syntax preview for.</param>
		/// <param name="writer">The writer to output the syntax preview to.</param>
		/// <exception cref="System.ArgumentNullException">
		/// Thrown if <paramref name="element" /> or <paramref name="writer" /> is <see langword="null" />.
		/// </exception>
		protected SyntaxGenerator(AccessSpecifiedElement element, StringWriter writer)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}

			this._element = element;
			this._writer = writer;
		}

		/// <summary>
		/// Writes attribute argument information.
		/// </summary>
		/// <param name="arguments">The attribute arguments to document.</param>
		protected void AttributeArguments(ExpressionCollection arguments)
		{
			this.Writer.Write("(");
			for (int j = 0; j < arguments.Count; j++)
			{
				Expression argument = arguments[j];
				if (argument is AttributeVariableInitializer)
				{
					AttributeVariableInitializer init = (AttributeVariableInitializer)argument;
					this.Writer.Write(init.LeftSide);
					if (this.DocumentLanguage == Language.Basic)
					{
						this.Writer.Write(":");
					}
					this.Writer.Write("=");
					this.Writer.Write(init.RightSide);
				}
				else
				{
					this.Writer.Write(argument.ToString());
				}
				if (j + 1 < arguments.Count)
				{
					this.Writer.Write(", ");
				}
			}
			this.Writer.Write(")");
		}

		/// <summary>
		/// Produces the syntax preview of the element.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Implementations of this method should inspect the <see cref="CR_Documentor.Transformation.Syntax.SyntaxGenerator.Element"/>
		/// to generate a proper syntax preview in HTML and write it to the
		/// <see cref="CR_Documentor.Transformation.Syntax.SyntaxGenerator.Writer"/>.
		/// </para>
		/// </remarks>
		public abstract void GenerateSyntaxPreview();
	}
}
