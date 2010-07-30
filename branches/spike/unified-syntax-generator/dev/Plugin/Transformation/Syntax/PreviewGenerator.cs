using System;
using System.IO;
using System.Web;
using System.Web.UI;
using CR_Documentor.Properties;
using DevExpress.CodeRush.StructuralParser;

namespace CR_Documentor.Transformation.Syntax
{
	/// <summary>
	/// Syntax preview generator. Converts a langauge element into an HTML snippet
	/// that can be inserted into a documentation preview.
	/// </summary>
	/// <remarks>
	/// <para>
	/// CSS classes for common syntax generator:
	/// </para>
	/// <para>
	/// <c>.code</c> - wrapper around code blocks
	/// </para>
	/// <para>
	/// Language constructs:
	/// </para>
	/// <list type="bullet">
	/// <item><term>.attribute</term></item>
	/// <item><term>.attributes</term></item>
	/// <item><term>.comment</term></item>
	/// <item><term>.constraint</term></item>
	/// <item><term>.constraints</term></item>
	/// <item><term>.getset</term></item>
	/// <item><term>.identifier</term></item>
	/// <item><term>.keyword</term></item>
	/// <item><term>.literal</term></item>
	/// <item><term>.member</term></item>
	/// <item><term>.parameter</term></item>
	/// <item><term>.parameters</term></item>
	/// <item><term>.typeparameter</term></item>
	/// <item><term>.typeparameters</term></item>
	/// </list>
	/// <para>
	/// There will be line continuations and line breaks in places. That may
	/// not quite be identical to how the finished product actually renders
	/// but there's not much we can do about that since we can't get CSS
	/// to add the special line-continuation characters in VB.
	/// </para>
	/// <para>
	/// Since we're not generating the preview for every language all at once,
	/// there is no need for CSS classes that are language-specific.
	/// </para>
	/// </remarks>
	public class PreviewGenerator
	{
		// TODO: Look at options to refactor this class - smaller methods, smaller classes (one per preview type?), etc.

		/// <summary>
		/// Gets the element being documented.
		/// </summary>
		/// <value>
		/// The <see cref="DevExpress.CodeRush.StructuralParser.AccessSpecifiedElement"/>
		/// for which a preview will be generated.
		/// </value>
		public AccessSpecifiedElement Element { get; private set; }

		/// <summary>
		/// Gets the member type of the current element, assuming the current element
		/// is a <see cref="DevExpress.CodeRush.StructuralParser.Member"/>.
		/// </summary>
		/// <value>
		/// A <see cref="System.String"/> containing the <see cref="DevExpress.CodeRush.StructuralParser.Member.MemberType"/>
		/// of the <see cref="CR_Documentor.Transformation.Syntax.PreviewGenerator.Element"/>.
		/// If <see cref="CR_Documentor.Transformation.Syntax.PreviewGenerator.Element"/>
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
		/// Gets a value indicating if the HTML output has newlines in it.
		/// </summary>
		/// <value>
		/// <see langword="true" /> to allow newlines (easier for testing);
		/// <see langword="false" /> to disable newlines (for production).
		/// </value>
		public bool EnableNewlines { get; private set; }

		/// <summary>
		/// Gets the preview language.
		/// </summary>
		/// <value>
		/// The <see cref="CR_Documentor.Transformation.Syntax.SupportedLanguageId"/>
		/// for which a preview will be generated.
		/// </value>
		public SupportedLanguageId Language { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="CR_Documentor.Transformation.Syntax.PreviewGenerator" /> class.
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
		public PreviewGenerator(AccessSpecifiedElement element, SupportedLanguageId language)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			this.Element = element;
			this.Language = language;
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="CR_Documentor.Transformation.Syntax.PreviewGenerator" /> class.
		/// </summary>
		/// <param name="element">
		/// The element for which the syntax preview should be generated.
		/// </param>
		/// <param name="language">
		/// The programming language in which the preview should be rendered.
		/// </param>
		/// <param name="enableNewlines">
		/// <see langword="true" /> to enable newlines in the output (helpful for
		/// testing); <see langword="false" /> to disable newlines (production).
		/// </param>
		/// <exception cref="System.ArgumentNullException">
		/// Thrown if <paramref name="element" /> is <see langword="null" />.
		/// </exception>
		public PreviewGenerator(AccessSpecifiedElement element, SupportedLanguageId language, bool enableNewlines)
			: this(element, language)
		{
			this.EnableNewlines = enableNewlines;
		}

		/// <summary>
		/// Writes attribute argument information.
		/// </summary>
		/// <param name="writer">
		/// The <see cref="System.Web.UI.HtmlTextWriter"/> to which the preview
		/// is being written.
		/// </param>
		/// <param name="arguments">The attribute arguments to document.</param>
		protected virtual void AttributeArguments(HtmlTextWriter writer, ExpressionCollection arguments)
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
					HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Identifier, init.LeftSide.ToString());
					switch (this.Language)
					{
						case SupportedLanguageId.Basic:
							writer.Write(":= ");
							break;
						default:
							writer.Write("= ");
							break;
					}
					HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Literal, init.RightSide.ToString(), "", "");
				}
				else
				{
					HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Literal, argument.ToString(), "", "");
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
		protected virtual void Attributes(HtmlTextWriter writer)
		{
			if (this.Element.AttributeCount < 1)
			{
				return;
			}

			writer.AddAttribute(HtmlTextWriterAttribute.Class, PreviewCss.Attributes);
			writer.RenderBeginTag(HtmlTextWriterTag.Div);
			var attributes = this.Element.Attributes;
			for (int i = 0; i < attributes.Count; i++)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Class, PreviewCss.Attribute);
				writer.RenderBeginTag(HtmlTextWriterTag.Div);
				switch (this.Language)
				{
					case SupportedLanguageId.Basic:
						writer.Write("&lt;");
						break;
					default:
						writer.Write("[");
						break;
				}
				var attribute = (DevExpress.CodeRush.StructuralParser.Attribute)attributes[i];
				HtmlTextWriterExtensions.WriteLink(writer, attribute.Name, "", "");
				this.AttributeArguments(writer, attribute.Arguments);
				switch (this.Language)
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

		/// <summary>
		/// Writes the preview for a class.
		/// </summary>
		/// <param name="writer">
		/// The <see cref="System.Web.UI.HtmlTextWriter"/> to which the preview
		/// is being written.
		/// </param>
		protected virtual void Class(HtmlTextWriter writer)
		{
			if (this.Language == SupportedLanguageId.CSharp && this.Element.IsNew)
			{
				HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "new");
			}
			HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, Lookup.Visibility(this.Language, this.Element.Visibility));
			if (this.Language == SupportedLanguageId.Basic && this.Element.IsNew)
			{
				HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "Shadows");
			}
			ContractWriter.Write(writer, this.Element, this.Language);
			if (this.Element is Interface)
			{
				switch (this.Language)
				{
					case SupportedLanguageId.Basic:
						HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "Interface");
						break;
					default:
						HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "interface");
						break;
				}
			}
			else if (this.Element is Struct)
			{
				switch (this.Language)
				{
					case SupportedLanguageId.Basic:
						HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "Structure");
						break;
					default:
						HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "struct");
						break;
				}
			}
			else
			{
				switch (this.Language)
				{
					case SupportedLanguageId.Basic:
						HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "Class");
						break;
					default:
						HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "class");
						break;
				}
			}
			HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Identifier, this.Element.Name, null, "");
			TypeParameterWriter.WriteTypeParameters(writer, this.Element, this.Language);
			// TODO: Write the inheritance/implements chain.
			TypeParameterWriter.WriteConstraintsPostSignature(writer, this.Element, this.Language);
		}

		/// <summary>
		/// Writes the preview for a class operator (cast, overload, etc.).
		/// </summary>
		/// <param name="writer">
		/// The <see cref="System.Web.UI.HtmlTextWriter"/> to which the preview
		/// is being written.
		/// </param>
		protected virtual void ClassOperator(HtmlTextWriter writer)
		{
			var method = (Method)this.Element;
			HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, Lookup.Visibility(this.Language, this.Element.Visibility));
			ContractWriter.Write(writer, this.Element, this.Language);
			bool isCast = method.IsImplicitCast || method.IsExplicitCast;

			switch (this.Language)
			{
				case SupportedLanguageId.Basic:
					if (method.IsImplicitCast)
					{
						HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "Widening");
					}
					else if (method.IsExplicitCast)
					{
						HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "Narrowing");
					}
					HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "Operator");
					break;
				default:
					if (method.IsImplicitCast)
					{
						HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "implicit");
					}
					else if (method.IsExplicitCast)
					{
						HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "explicit");
					}
					else
					{
						HtmlTextWriterExtensions.WriteLink(writer, this.ElementMemberType, "", " ");
					}
					HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "operator");
					if (isCast)
					{
						HtmlTextWriterExtensions.WriteLink(writer, this.ElementMemberType, "", "");
					}
					break;
			}
			if (!isCast)
			{
				HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Identifier, this.Element.Name, "", "");
			}
			else if (this.Language == SupportedLanguageId.Basic)
			{
				HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Identifier, "CType", "", "");
			}
			ParameterWriter.Write(writer, this.Element as MemberWithParameters, this.Language, "(", ")");
			if (this.Language == SupportedLanguageId.Basic)
			{
				HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "As", "&nbsp;", " ");
				HtmlTextWriterExtensions.WriteLink(writer, this.ElementMemberType, "", "");
			}
		}

		/// <summary>
		/// Writes the preview for a constructor.
		/// </summary>
		/// <param name="writer">
		/// The <see cref="System.Web.UI.HtmlTextWriter"/> to which the preview
		/// is being written.
		/// </param>
		protected virtual void Constructor(HtmlTextWriter writer)
		{
			HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, Lookup.Visibility(this.Language, this.Element.Visibility));
			ContractWriter.Write(writer, this.Element, this.Language);
			switch (this.Language)
			{
				case SupportedLanguageId.Basic:
					HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "Sub");
					HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Identifier, "New", "", "");
					break;
				default:
					HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Identifier, this.Element.Name, "", "");
					break;
			}
			ParameterWriter.Write(writer, this.Element as MemberWithParameters, this.Language, "(", ")");
		}

		/// <summary>
		/// Writes the preview for a destructor/finalizer.
		/// </summary>
		/// <param name="writer">
		/// The <see cref="System.Web.UI.HtmlTextWriter"/> to which the preview
		/// is being written.
		/// </param>
		protected virtual void Destructor(HtmlTextWriter writer)
		{
			// Destructor preview always renders as protected.
			HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, Lookup.Visibility(this.Language, MemberVisibility.Protected));
			switch (this.Language)
			{
				case SupportedLanguageId.Basic:
					HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "Overrides");
					HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "Sub");
					break;
				default:
					HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "override");
					HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "void");
					break;
			}
			HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Identifier, "Finalize", "", "");
			ParameterWriter.Write(writer, this.Element as MemberWithParameters, this.Language, "(", ")");
		}

		/// <summary>
		/// Writes the preview for a delegate.
		/// </summary>
		/// <param name="writer">
		/// The <see cref="System.Web.UI.HtmlTextWriter"/> to which the preview
		/// is being written.
		/// </param>
		protected virtual void Delegate(HtmlTextWriter writer)
		{
			HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, Lookup.Visibility(this.Language, this.Element.Visibility));
			switch (this.Language)
			{
				case SupportedLanguageId.Basic:
					HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "Delegate");
					break;
				default:
					HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "delegate");
					break;
			}
			string elementMemberType = this.ElementMemberType;
			if (TypeInfo.TypeIsVoid(elementMemberType))
			{
				switch (this.Language)
				{
					case SupportedLanguageId.Basic:
						HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "Sub");
						break;
					default:
						HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "void");
						break;
				}
			}
			else
			{
				switch (this.Language)
				{
					case SupportedLanguageId.Basic:
						HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "Function");
						break;
					default:
						HtmlTextWriterExtensions.WriteLink(writer, elementMemberType, null, null);
						break;
				}
			}
			HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Identifier, this.Element.Name, "", "");
			ParameterWriter.Write(writer, this.Element as MemberWithParameters, this.Language, "(", ")");
			if (this.Language == SupportedLanguageId.Basic && !TypeInfo.TypeIsVoid(elementMemberType))
			{
				HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "As", "&nbsp;", " ");
				HtmlTextWriterExtensions.WriteLink(writer, elementMemberType, "", "");
			}
		}

		/// <summary>
		/// Writes the preview for an enumeration.
		/// </summary>
		/// <param name="writer">
		/// The <see cref="System.Web.UI.HtmlTextWriter"/> to which the preview
		/// is being written.
		/// </param>
		protected virtual void Enumeration(HtmlTextWriter writer)
		{
			HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, Lookup.Visibility(this.Language, this.Element.Visibility));
			switch (this.Language)
			{
				case SupportedLanguageId.Basic:
					HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "Enum");
					break;
				default:
					HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "enum");
					break;
			}
			HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Identifier, this.Element.Name, null, "");
			string underlyingType = ((Enumeration)this.Element).UnderlyingType;
			if (!String.IsNullOrEmpty(underlyingType))
			{
				switch (this.Language)
				{
					case SupportedLanguageId.Basic:
						HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "As", "&nbsp;", " ");
						break;
					default:
						writer.Write(" : ");
						break;
				}
				HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, underlyingType, null, "");
			}
		}

		/// <summary>
		/// Writes the preview for an event.
		/// </summary>
		/// <param name="writer">
		/// The <see cref="System.Web.UI.HtmlTextWriter"/> to which the preview
		/// is being written.
		/// </param>
		protected virtual void Event(HtmlTextWriter writer)
		{
			HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, Lookup.Visibility(this.Language, this.Element.Visibility));
			ContractWriter.Write(writer, this.Element, this.Language);
			switch (this.Language)
			{
				case SupportedLanguageId.Basic:
					HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "Event");
					break;
				default:
					HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "event");
					break;
			}

			// Get the generated parameterized event handler type name if it exists.
			// e.g., Public Event EventParameterized As TestClass.EventParameterizedEventHandler
			var eventElement = (Event)this.Element;
			string memberType = this.ElementMemberType;
			if (eventElement.ParameterCount > 0)
			{
				var parent = eventElement.GetParentClassInterfaceStructOrModule();
				memberType = "";
				if (parent != null)
				{
					memberType = parent.Name + ".";
				}
				memberType += eventElement.Name + "EventHandler";
			}

			if (this.Language != SupportedLanguageId.Basic)
			{
				HtmlTextWriterExtensions.WriteLink(writer, memberType, null, null);
			}
			HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Identifier, this.Element.Name, "", "");
			if (this.Language == SupportedLanguageId.Basic)
			{
				HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "As", "&nbsp;", " ");
				HtmlTextWriterExtensions.WriteLink(writer, memberType, "", "");
			}
		}

		/// <summary>
		/// Writes the preview for a field.
		/// </summary>
		/// <param name="writer">
		/// The <see cref="System.Web.UI.HtmlTextWriter"/> to which the preview
		/// is being written.
		/// </param>
		protected virtual void Field(HtmlTextWriter writer)
		{
			HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, Lookup.Visibility(this.Language, this.Element.Visibility));
			ContractWriter.Write(writer, this.Element, this.Language);
			if (this.Language != SupportedLanguageId.Basic)
			{
				HtmlTextWriterExtensions.WriteLink(writer, this.ElementMemberType, null, null);
			}
			HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Identifier, this.Element.Name, "", "");
			if (this.Language == SupportedLanguageId.Basic)
			{
				HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "As", "&nbsp;", " ");
				HtmlTextWriterExtensions.WriteLink(writer, this.ElementMemberType, "", "");
			}
		}

		/// <summary>
		/// Generates the HTML syntax preview for an element.
		/// </summary>
		public virtual string Generate()
		{
			using (StringWriter baseWriter = new StringWriter())
			using (XhtmlTextWriter writer = new XhtmlTextWriter(baseWriter, ""))
			{
				if (!this.EnableNewlines)
				{
					writer.NewLine = "";
				}
				string cssClass = PreviewCss.Code;
				switch (this.Language)
				{
					case SupportedLanguageId.Basic:
						cssClass += " " + PreviewCss.Language_Basic;
						break;
					case SupportedLanguageId.CSharp:
						cssClass += " " + PreviewCss.Language_CSharp;
						break;
				}
				writer.AddAttribute(HtmlTextWriterAttribute.Class, cssClass);
				writer.RenderBeginTag(HtmlTextWriterTag.Div);
				if (this.Language == SupportedLanguageId.None)
				{
					writer.Write(HttpUtility.HtmlEncode(Resources.PreviewGenerator_LanguageNotSupported));
				}
				else if (!IsSupportedElement(this.Element))
				{
					writer.Write(HttpUtility.HtmlEncode(Resources.PreviewGenerator_LanguageElementNotSupported));
				}
				else
				{
					this.Attributes(writer);
					writer.AddAttribute(HtmlTextWriterAttribute.Class, PreviewCss.Member);
					writer.RenderBeginTag(HtmlTextWriterTag.Div);
					if (this.Element is Enumeration)
					{
						this.Enumeration(writer);
					}
					else if (this.Element is Class)
					{
						this.Class(writer);
					}
					else if (this.Element is DelegateDefinition)
					{
						this.Delegate(writer);
					}
					else if (this.Element is Method)
					{
						this.Method(writer);
					}
					else if (this.Element is Property)
					{
						this.Property(writer);
					}
					else if (this.Element is Event)
					{
						this.Event(writer);
					}
					else if (this.Element is BaseVariable)
					{
						this.Field(writer);
					}
					writer.RenderEndTag();
				}
				writer.RenderEndTag();
				writer.Flush();
				return baseWriter.ToString();
			}
		}

		/// <summary>
		/// Determines if a langauge element is supported for preview.
		/// </summary>
		/// <returns>
		/// <see langword="true" /> if a syntax preview can be generated
		/// for the element, otherwise <see langword="false" />.
		/// </returns>
		public static bool IsSupportedElement(AccessSpecifiedElement element)
		{
			return
				element is Enumeration ||
				element is Class ||
				element is DelegateDefinition ||
				element is Method ||
				element is Property ||
				element is Event ||
				element is BaseVariable;
		}

		/// <summary>
		/// Writes the preview for a method.
		/// </summary>
		/// <param name="writer">
		/// The <see cref="System.Web.UI.HtmlTextWriter"/> to which the preview
		/// is being written.
		/// </param>
		protected virtual void Method(HtmlTextWriter writer)
		{
			var method = (Method)this.Element;
			if (method.IsConstructor)
			{
				this.Constructor(writer);
				return;
			}
			else if (method.IsClassOperator)
			{
				this.ClassOperator(writer);
				return;
			}
			else if (method.IsDestructor)
			{
				this.Destructor(writer);
				return;
			}

			if (this.Element.Parent as Interface == null &&
				!(this.Language == SupportedLanguageId.CSharp && method.ImplementsCount > 0))
			{
				// For members that belong to an interface (methods in an interface definition), no visibility is written.
				// For explicit interface implementations in C#, no visibility is written.
				HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, Lookup.Visibility(this.Language, this.Element.Visibility));
			}

			ContractWriter.Write(writer, this.Element, this.Language);
			string elementMemberType = this.ElementMemberType;
			if (TypeInfo.TypeIsVoid(elementMemberType))
			{
				switch (this.Language)
				{
					case SupportedLanguageId.Basic:
						HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "Sub");
						break;
					default:
						HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "void");
						break;
				}
			}
			else
			{
				switch (this.Language)
				{
					case SupportedLanguageId.Basic:
						HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "Function");
						break;
					default:
						HtmlTextWriterExtensions.WriteLink(writer, elementMemberType, null, null);
						break;
				}
			}
			HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Identifier, this.Element.Name, "", "");
			TypeParameterWriter.WriteTypeParameters(writer, this.Element, this.Language);
			ParameterWriter.Write(writer, this.Element as MemberWithParameters, this.Language, "(", ")");
			TypeParameterWriter.WriteConstraintsPostSignature(writer, this.Element, this.Language);
			if (this.Language == SupportedLanguageId.Basic)
			{
				if (!TypeInfo.TypeIsVoid(elementMemberType))
				{
					HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "As", "&nbsp;", " ");
					HtmlTextWriterExtensions.WriteLink(writer, elementMemberType, "", "");
				}
				int count = method.ImplementsCount;
				if (count > 0)
				{
					writer.Write(" ");
					HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "Implements");
					for (int i = 0; i < count; i++)
					{
						HtmlTextWriterExtensions.WriteLink(writer, method.Implements[i], "", "");
						if (i + 1 < count)
						{
							writer.Write(", ");
						}
					}
				}
			}
		}

		/// <summary>
		/// Writes the preview for a property.
		/// </summary>
		/// <param name="writer">
		/// The <see cref="System.Web.UI.HtmlTextWriter"/> to which the preview
		/// is being written.
		/// </param>
		protected virtual void Property(HtmlTextWriter writer)
		{
			HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, Lookup.Visibility(this.Language, this.Element.Visibility));
			ContractWriter.Write(writer, this.Element, this.Language);
			var property = (Property)this.Element;
			switch (this.Language)
			{
				case SupportedLanguageId.Basic:
					if (property.ParameterCount > 0)
					{
						HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "Default");
					}
					// In VB, properties with the ReadOnly keyword parse with IsReadOnly
					// set so we don't have to check for property.HasGetter && !property.HasSetter
					// because the ElementContract method will handle ReadOnly.
					else if (!property.HasGetter && property.HasSetter)
					{
						HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "WriteOnly");
					}
					HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "Property");
					break;
				default:
					break;
			}

			if (this.Language != SupportedLanguageId.Basic)
			{
				HtmlTextWriterExtensions.WriteLink(writer, this.ElementMemberType, null, null);
			}
			if (property.ParameterCount > 0)
			{
				switch (this.Language)
				{
					case SupportedLanguageId.Basic:
						HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Identifier, this.Element.Name, "", "");
						ParameterWriter.Write(writer, this.Element as MemberWithParameters, this.Language, "(", ")");
						break;
					default:
						HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "this", "", "");
						ParameterWriter.Write(writer, this.Element as MemberWithParameters, this.Language, "[", "]");
						break;
				}
			}
			else
			{
				HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Identifier, this.Element.Name, "", "");
			}
			if (this.Language == SupportedLanguageId.Basic)
			{
				HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, "As", "&nbsp;", " ");
				HtmlTextWriterExtensions.WriteLink(writer, this.ElementMemberType, "", "");
			}
			else
			{
				writer.Write(" {");
			}
			if (property.HasGetter)
			{
				switch (this.Language)
				{
					case SupportedLanguageId.Basic:
						this.GetSet(writer, "Get");
						break;
					default:
						this.GetSet(writer, "get");
						break;
				}
			}
			if (property.HasSetter)
			{
				switch (this.Language)
				{
					case SupportedLanguageId.Basic:
						this.GetSet(writer, "Set");
						break;
					default:
						this.GetSet(writer, "set");
						break;
				}
			}
			if (this.Language != SupportedLanguageId.Basic)
			{
				writer.Write("}");
			}
		}

		/// <summary>
		/// Writes the "get" or "set" keyword for a property.
		/// </summary>
		/// <param name="writer">
		/// The <see cref="System.Web.UI.HtmlTextWriter"/> to which the preview
		/// is being written.
		/// </param>
		/// <param name="keyword">
		/// The "get" or "set" keyword being written.
		/// </param>
		private void GetSet(HtmlTextWriter writer, string keyword)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Class, PreviewCss.GetSet);
			writer.RenderBeginTag(HtmlTextWriterTag.Div);
			HtmlTextWriterExtensions.WriteSpan(writer, PreviewCss.Keyword, keyword, "", "");
			if (this.Language != SupportedLanguageId.Basic)
			{
				writer.Write(";");
			}
			writer.RenderEndTag();
		}
	}
}
