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
		// TODO: Create CSS for Sandcastle syntax preview.
		// TODO: Create CSS for MSDN syntax preview.

		/// <summary>
		/// Gets the element being documented.
		/// </summary>
		/// <value>
		/// The <see cref="DevExpress.CodeRush.StructuralParser.AccessSpecifiedElement"/>
		/// for which a preview will be generated.
		/// </value>
		public AccessSpecifiedElement Element { get; private set; }

		/// <summary>
		/// Gets a flag indicating if there are generic type parameters
		/// to render to the preview.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if <see cref="CR_Documentor.Transformation.Syntax.PreviewGenerator.Element"/>
		/// has generic type parameters to render; <see langword="false" /> if not.
		/// </value>
		protected bool ElementHasGenericParameters
		{
			get
			{
				if (!this.Element.IsGeneric || this.Element.GenericModifier.TypeParameters == null || this.Element.GenericModifier.TypeParameters.Count == 0)
				{
					return false;
				}
				return true;
			}
		}

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
		/// Gets a value indicating if type parameter constraints get shown
		/// inline with the parameters.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if type parameters get shown inline
		/// like in VB; <see langword="false" /> if they appear after the member
		/// signature like in C#.
		/// </value>
		protected virtual bool ShowTypeParameterConstraintsInline
		{
			get
			{
				return this.Language == SupportedLanguageId.Basic;
			}
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
					this.WriteSpan(writer, PreviewCss.Identifier, init.LeftSide.ToString());
					switch (this.Language)
					{
						case SupportedLanguageId.Basic:
							writer.Write(":= ");
							break;
						default:
							writer.Write("= ");
							break;
					}
					this.WriteSpan(writer, PreviewCss.Literal, init.RightSide.ToString(), "", "");
				}
				else
				{
					this.WriteSpan(writer, PreviewCss.Literal, argument.ToString(), "", "");
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
				this.WriteLink(writer, attribute.Name, "", "");
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
				this.WriteSpan(writer, PreviewCss.Keyword, "new");
			}
			this.WriteSpan(writer, PreviewCss.Keyword, Lookup.Visibility(this.Language, this.Element.Visibility));
			if (this.Language == SupportedLanguageId.Basic && this.Element.IsNew)
			{
				this.WriteSpan(writer, PreviewCss.Keyword, "Shadows");
			}
			this.ElementContract(writer);
			if (this.Element is Interface)
			{
				switch (this.Language)
				{
					case SupportedLanguageId.Basic:
						this.WriteSpan(writer, PreviewCss.Keyword, "Interface");
						break;
					default:
						this.WriteSpan(writer, PreviewCss.Keyword, "interface");
						break;
				}
			}
			else if (this.Element is Struct)
			{
				switch (this.Language)
				{
					case SupportedLanguageId.Basic:
						this.WriteSpan(writer, PreviewCss.Keyword, "Structure");
						break;
					default:
						this.WriteSpan(writer, PreviewCss.Keyword, "struct");
						break;
				}
			}
			else
			{
				switch (this.Language)
				{
					case SupportedLanguageId.Basic:
						this.WriteSpan(writer, PreviewCss.Keyword, "Class");
						break;
					default:
						this.WriteSpan(writer, PreviewCss.Keyword, "class");
						break;
				}
			}
			this.WriteSpan(writer, PreviewCss.Identifier, this.Element.Name, null, "");
			this.TypeParameters(writer);
			// TODO: Write the inheritance/implements chain.
			this.TypeParameterConstraintsPostSignature(writer);
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
			this.WriteSpan(writer, PreviewCss.Keyword, Lookup.Visibility(this.Language, this.Element.Visibility));
			this.ElementContract(writer);
			bool isCast = method.IsImplicitCast || method.IsExplicitCast;

			switch (this.Language)
			{
				case SupportedLanguageId.Basic:
					if (method.IsImplicitCast)
					{
						this.WriteSpan(writer, PreviewCss.Keyword, "Widening");
					}
					else if (method.IsExplicitCast)
					{
						this.WriteSpan(writer, PreviewCss.Keyword, "Narrowing");
					}
					this.WriteSpan(writer, PreviewCss.Keyword, "Operator");
					break;
				default:
					if (method.IsImplicitCast)
					{
						this.WriteSpan(writer, PreviewCss.Keyword, "implicit");
					}
					else if (method.IsExplicitCast)
					{
						this.WriteSpan(writer, PreviewCss.Keyword, "explicit");
					}
					else
					{
						this.WriteLink(writer, this.ElementMemberType, "", " ");
					}
					this.WriteSpan(writer, PreviewCss.Keyword, "operator");
					if (isCast)
					{
						this.WriteLink(writer, this.ElementMemberType, "", "");
					}
					break;
			}
			if (!isCast)
			{
				this.WriteSpan(writer, PreviewCss.Identifier, this.Element.Name, "", "");
			}
			else if (this.Language == SupportedLanguageId.Basic)
			{
				this.WriteSpan(writer, PreviewCss.Identifier, "CType", "", "");
			}
			this.Parameters(writer, "(", ")");
			if (this.Language == SupportedLanguageId.Basic)
			{
				this.WriteSpan(writer, PreviewCss.Keyword, "As", "&nbsp;", " ");
				this.WriteLink(writer, this.ElementMemberType, "", "");
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
			this.WriteSpan(writer, PreviewCss.Keyword, Lookup.Visibility(this.Language, this.Element.Visibility));
			this.ElementContract(writer);
			switch (this.Language)
			{
				case SupportedLanguageId.Basic:
					this.WriteSpan(writer, PreviewCss.Keyword, "Sub");
					this.WriteSpan(writer, PreviewCss.Identifier, "New", "", "");
					break;
				default:
					this.WriteSpan(writer, PreviewCss.Identifier, this.Element.Name, "", "");
					break;
			}
			this.Parameters(writer, "(", ")");
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
			this.WriteSpan(writer, PreviewCss.Keyword, Lookup.Visibility(this.Language, MemberVisibility.Protected));
			switch (this.Language)
			{
				case SupportedLanguageId.Basic:
					this.WriteSpan(writer, PreviewCss.Keyword, "Overrides");
					this.WriteSpan(writer, PreviewCss.Keyword, "Sub");
					break;
				default:
					this.WriteSpan(writer, PreviewCss.Keyword, "override");
					this.WriteSpan(writer, PreviewCss.Keyword, "void");
					break;
			}
			this.WriteSpan(writer, PreviewCss.Identifier, "Finalize", "", "");
			this.Parameters(writer, "(", ")");
		}

		/// <summary>
		/// Writes the contract (static/abstract/sealed/etc.) for the element.
		/// </summary>
		/// <param name="writer">
		/// The <see cref="System.Web.UI.HtmlTextWriter"/> to which the preview
		/// is being written.
		/// </param>
		protected virtual void ElementContract(HtmlTextWriter writer)
		{
			if (this.Element.IsStatic)
			{
				switch (this.Language)
				{
					case SupportedLanguageId.Basic:
						this.WriteSpan(writer, PreviewCss.Keyword, "Shared");
						break;
					default:
						this.WriteSpan(writer, PreviewCss.Keyword, "static");
						break;
				}
			}
			else if (this.Element.IsAbstract)
			{
				switch (this.Language)
				{
					case SupportedLanguageId.Basic:
						this.WriteSpan(writer, PreviewCss.Keyword, "MustInherit");
						break;
					default:
						this.WriteSpan(writer, PreviewCss.Keyword, "abstract");
						break;
				}
			}
			else if (this.Element.IsSealed)
			{
				switch (this.Language)
				{
					case SupportedLanguageId.Basic:
						this.WriteSpan(writer, PreviewCss.Keyword, "NotInheritable");
						break;
					default:
						this.WriteSpan(writer, PreviewCss.Keyword, "sealed");
						break;
				}
			}
			else if (this.Element.IsConst)
			{
				switch (this.Language)
				{
					case SupportedLanguageId.Basic:
						this.WriteSpan(writer, PreviewCss.Keyword, "Const");
						break;
					default:
						this.WriteSpan(writer, PreviewCss.Keyword, "const");
						break;
				}
			}
			else if (this.Element.IsVirtual)
			{
				switch (this.Language)
				{
					case SupportedLanguageId.Basic:
						this.WriteSpan(writer, PreviewCss.Keyword, "Overridable");
						break;
					default:
						this.WriteSpan(writer, PreviewCss.Keyword, "virtual");
						break;
				}
			}
			else if (this.Element.IsOverride)
			{
				switch (this.Language)
				{
					case SupportedLanguageId.Basic:
						this.WriteSpan(writer, PreviewCss.Keyword, "Overrides");
						break;
					default:
						this.WriteSpan(writer, PreviewCss.Keyword, "override");
						break;
				}
			}

			if (this.Element.IsReadOnly)
			{
				switch (this.Language)
				{
					case SupportedLanguageId.Basic:
						this.WriteSpan(writer, PreviewCss.Keyword, "ReadOnly");
						break;
					default:
						this.WriteSpan(writer, PreviewCss.Keyword, "readonly");
						break;
				}
			}
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
			this.WriteSpan(writer, PreviewCss.Keyword, Lookup.Visibility(this.Language, this.Element.Visibility));
			switch (this.Language)
			{
				case SupportedLanguageId.Basic:
					this.WriteSpan(writer, PreviewCss.Keyword, "Delegate");
					break;
				default:
					this.WriteSpan(writer, PreviewCss.Keyword, "delegate");
					break;
			}
			string elementMemberType = this.ElementMemberType;
			if (TypeInfo.TypeIsVoid(elementMemberType))
			{
				switch (this.Language)
				{
					case SupportedLanguageId.Basic:
						this.WriteSpan(writer, PreviewCss.Keyword, "Sub");
						break;
					default:
						this.WriteSpan(writer, PreviewCss.Keyword, "void");
						break;
				}
			}
			else
			{
				switch (this.Language)
				{
					case SupportedLanguageId.Basic:
						this.WriteSpan(writer, PreviewCss.Keyword, "Function");
						break;
					default:
						this.WriteLink(writer, elementMemberType, null, null);
						break;
				}
			}
			this.WriteSpan(writer, PreviewCss.Identifier, this.Element.Name, "", "");
			this.Parameters(writer, "(", ")");
			if (this.Language == SupportedLanguageId.Basic && !TypeInfo.TypeIsVoid(elementMemberType))
			{
				this.WriteSpan(writer, PreviewCss.Keyword, "As", "&nbsp;", " ");
				this.WriteLink(writer, elementMemberType, "", "");
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
			this.WriteSpan(writer, PreviewCss.Keyword, Lookup.Visibility(this.Language, this.Element.Visibility));
			switch (this.Language)
			{
				case SupportedLanguageId.Basic:
					this.WriteSpan(writer, PreviewCss.Keyword, "Enum");
					break;
				default:
					this.WriteSpan(writer, PreviewCss.Keyword, "enum");
					break;
			}
			this.WriteSpan(writer, PreviewCss.Identifier, this.Element.Name, null, "");
			string underlyingType = ((Enumeration)this.Element).UnderlyingType;
			if (!String.IsNullOrEmpty(underlyingType))
			{
				switch (this.Language)
				{
					case SupportedLanguageId.Basic:
						this.WriteSpan(writer, PreviewCss.Keyword, "As", "&nbsp;", " ");
						break;
					default:
						writer.Write(" : ");
						break;
				}
				this.WriteSpan(writer, PreviewCss.Keyword, underlyingType, null, "");
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
			this.WriteSpan(writer, PreviewCss.Keyword, Lookup.Visibility(this.Language, this.Element.Visibility));
			this.ElementContract(writer);
			switch (this.Language)
			{
				case SupportedLanguageId.Basic:
					this.WriteSpan(writer, PreviewCss.Keyword, "Event");
					break;
				default:
					this.WriteSpan(writer, PreviewCss.Keyword, "event");
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
				this.WriteLink(writer, memberType, null, null);
			}
			this.WriteSpan(writer, PreviewCss.Identifier, this.Element.Name, "", "");
			if (this.Language == SupportedLanguageId.Basic)
			{
				this.WriteSpan(writer, PreviewCss.Keyword, "As", "&nbsp;", " ");
				this.WriteLink(writer, memberType, "", "");
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
			this.WriteSpan(writer, PreviewCss.Keyword, Lookup.Visibility(this.Language, this.Element.Visibility));
			this.ElementContract(writer);
			if (this.Language != SupportedLanguageId.Basic)
			{
				this.WriteLink(writer, this.ElementMemberType, null, null);
			}
			this.WriteSpan(writer, PreviewCss.Identifier, this.Element.Name, "", "");
			if (this.Language == SupportedLanguageId.Basic)
			{
				this.WriteSpan(writer, PreviewCss.Keyword, "As", "&nbsp;", " ");
				this.WriteLink(writer, this.ElementMemberType, "", "");
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

			// TODO: For members that belong to an interface (methods in an interface definition), no visibility is written.
			// TODO: For explicit interface implementations in C#, no visibility is written.
			this.WriteSpan(writer, PreviewCss.Keyword, Lookup.Visibility(this.Language, this.Element.Visibility));
			// TODO: VB abstract methods should be MustOverride, not MustInherit.
			this.ElementContract(writer);
			string elementMemberType = this.ElementMemberType;
			if (TypeInfo.TypeIsVoid(elementMemberType))
			{
				switch (this.Language)
				{
					case SupportedLanguageId.Basic:
						this.WriteSpan(writer, PreviewCss.Keyword, "Sub");
						break;
					default:
						this.WriteSpan(writer, PreviewCss.Keyword, "void");
						break;
				}
			}
			else
			{
				switch (this.Language)
				{
					case SupportedLanguageId.Basic:
						this.WriteSpan(writer, PreviewCss.Keyword, "Function");
						break;
					default:
						this.WriteLink(writer, elementMemberType, null, null);
						break;
				}
			}
			this.WriteSpan(writer, PreviewCss.Identifier, this.Element.Name, "", "");
			this.TypeParameters(writer);
			this.Parameters(writer, "(", ")");
			this.TypeParameterConstraintsPostSignature(writer);
			if (this.Language == SupportedLanguageId.Basic)
			{
				if (!TypeInfo.TypeIsVoid(elementMemberType))
				{
					this.WriteSpan(writer, PreviewCss.Keyword, "As", "&nbsp;", " ");
					this.WriteLink(writer, elementMemberType, "", "");
				}
				int count = method.ImplementsCount;
				if (count > 0)
				{
					writer.Write(" ");
					this.WriteSpan(writer, PreviewCss.Keyword, "Implements");
					for (int i = 0; i < count; i++)
					{
						this.WriteLink(writer, method.Implements[i], "", "");
						if (i + 1 < count)
						{
							writer.Write(", ");
						}
					}
				}
			}
		}

		/// <summary>
		/// Writes parameter information to the object signature.
		/// </summary>
		/// <param name="writer">
		/// The <see cref="System.Web.UI.HtmlTextWriter"/> to which the preview
		/// is being written.
		/// </param>
		/// <param name="openParen">The open parenthesis (usually "(" but sometimes "[").</param>
		/// <param name="closeParen">The close parenthesis (usually ")" but sometimes "]").</param>
		protected virtual void Parameters(HtmlTextWriter writer, string openParen, string closeParen)
		{
			var parameters = ((MemberWithParameters)this.Element).Parameters;
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
				//    this.WriteSpan(CssClassKeyword, "Optional");
				//}

				if (parameter.IsOutParam)
				{
					switch (this.Language)
					{
						case SupportedLanguageId.Basic:
							this.WriteLink(writer, "OutAttribute", "&lt;", "&gt; ");
							this.WriteSpan(writer, PreviewCss.Keyword, "ByRef");
							break;
						default:
							this.WriteSpan(writer, PreviewCss.Keyword, "out");
							break;
					}
				}
				else if (parameter.IsReferenceParam)
				{
					switch (this.Language)
					{
						case SupportedLanguageId.Basic:
							this.WriteSpan(writer, PreviewCss.Keyword, "ByRef");
							break;
						default:
							this.WriteSpan(writer, PreviewCss.Keyword, "ref");
							break;
					}
				}

				if (parameter.IsParamArray)
				{
					switch (this.Language)
					{
						case SupportedLanguageId.Basic:
							this.WriteSpan(writer, PreviewCss.Keyword, "ParamArray");
							break;
						default:
							this.WriteSpan(writer, PreviewCss.Keyword, "params");
							break;
					}
				}

				if (isBasic)
				{
					this.WriteSpan(writer, PreviewCss.Identifier, parameter.Name);
					this.WriteSpan(writer, PreviewCss.Keyword, "As");
				}
				this.WriteLink(writer, parameter.ParamType, "", "");
				if (!isBasic)
				{
					writer.Write(" ");
					this.WriteSpan(writer, PreviewCss.Identifier, parameter.Name, "", "");
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

		/// <summary>
		/// Writes the preview for a property.
		/// </summary>
		/// <param name="writer">
		/// The <see cref="System.Web.UI.HtmlTextWriter"/> to which the preview
		/// is being written.
		/// </param>
		protected virtual void Property(HtmlTextWriter writer)
		{
			this.WriteSpan(writer, PreviewCss.Keyword, Lookup.Visibility(this.Language, this.Element.Visibility));
			this.ElementContract(writer);
			var property = (Property)this.Element;
			switch (this.Language)
			{
				case SupportedLanguageId.Basic:
					if (property.ParameterCount > 0)
					{
						this.WriteSpan(writer, PreviewCss.Keyword, "Default");
					}
					// In VB, properties with the ReadOnly keyword parse with IsReadOnly
					// set so we don't have to check for property.HasGetter && !property.HasSetter
					// because the ElementContract method will handle ReadOnly.
					else if (!property.HasGetter && property.HasSetter)
					{
						this.WriteSpan(writer, PreviewCss.Keyword, "WriteOnly");
					}
					this.WriteSpan(writer, PreviewCss.Keyword, "Property");
					break;
				default:
					break;
			}

			if (this.Language != SupportedLanguageId.Basic)
			{
				this.WriteLink(writer, this.ElementMemberType, null, null);
			}
			if (property.ParameterCount > 0)
			{
				switch (this.Language)
				{
					case SupportedLanguageId.Basic:
						this.WriteSpan(writer, PreviewCss.Identifier, this.Element.Name, "", "");
						this.Parameters(writer, "(", ")");
						break;
					default:
						this.WriteSpan(writer, PreviewCss.Keyword, "this", "", "");
						this.Parameters(writer, "[", "]");
						break;
				}
			}
			else
			{
				this.WriteSpan(writer, PreviewCss.Identifier, this.Element.Name, "", "");
			}
			if (this.Language == SupportedLanguageId.Basic)
			{
				this.WriteSpan(writer, PreviewCss.Keyword, "As", "&nbsp;", " ");
				this.WriteLink(writer, this.ElementMemberType, "", "");
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

		private void GetSet(HtmlTextWriter writer, string keyword)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Class, PreviewCss.GetSet);
			writer.RenderBeginTag(HtmlTextWriterTag.Div);
			this.WriteSpan(writer, PreviewCss.Keyword, keyword, "", "");
			if (this.Language != SupportedLanguageId.Basic)
			{
				writer.Write(";");
			}
			writer.RenderEndTag();
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
		protected virtual void TypeParameterConstraintsPostSignature(HtmlTextWriter writer)
		{
			if (this.ShowTypeParameterConstraintsInline || !this.ElementHasGenericParameters)
			{
				return;
			}
			var typeParams = this.Element.GenericModifier.TypeParameters;
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
				this.WriteSpan(writer, PreviewCss.Keyword, "where");
				this.WriteSpan(writer, PreviewCss.TypeParameter, parameter.Name);
				writer.Write(": ");
				for (int j = 0; j < constraintCount; j++)
				{
					this.TypeParameterConstraintValue(writer, constraints[j]);
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
		protected virtual void TypeParameterConstraintsInline(HtmlTextWriter writer, TypeParameter parameter)
		{
			if (!this.ShowTypeParameterConstraintsInline)
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
			this.WriteSpan(writer, PreviewCss.Keyword, "As", "&nbsp;", " ");
			if (constraintCount > 1)
			{
				writer.Write("{");
			}
			writer.AddAttribute(HtmlTextWriterAttribute.Class, PreviewCss.Constraint);
			writer.RenderBeginTag(HtmlTextWriterTag.Div);
			for (int j = 0; j < constraintCount; j++)
			{
				this.TypeParameterConstraintValue(writer, constraints[j]);
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

		private void TypeParameterConstraintValue(HtmlTextWriter writer, TypeParameterConstraint constraint)
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
				this.WriteLink(writer, ((NamedTypeParameterConstraint)constraint).TypeReference.ToString(), "", "");
			}
			else
			{
				this.WriteSpan(writer, PreviewCss.Keyword, constraint.Name, "", "");
			}
		}

		/// <summary>
		/// Writes the type parameter information to the object signature.
		/// </summary>
		/// <param name="writer">
		/// The <see cref="System.Web.UI.HtmlTextWriter"/> to which the preview
		/// is being written.
		/// </param>
		protected virtual void TypeParameters(HtmlTextWriter writer)
		{
			if (!this.ElementHasGenericParameters)
			{
				return;
			}

			writer.AddAttribute(HtmlTextWriterAttribute.Class, PreviewCss.TypeParameters);
			writer.RenderBeginTag(HtmlTextWriterTag.Div);
			switch (this.Language)
			{
				case SupportedLanguageId.Basic:
					writer.Write("(");
					this.WriteSpan(writer, PreviewCss.Keyword, "Of");
					break;
				default:
					writer.Write("&lt;");
					break;
			}

			var typeParams = this.Element.GenericModifier.TypeParameters;
			int parameterCount = typeParams.Count;
			for (int i = 0; i < parameterCount; i++)
			{
				var parameter = typeParams[i];
				this.WriteSpan(writer, PreviewCss.TypeParameter, parameter.Name, "", "");
				if (parameter.Constraints.Count > 0)
				{
					this.TypeParameterConstraintsInline(writer, parameter);
				}
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
					writer.Write("&gt;");
					break;
			}
			writer.RenderEndTag();
		}

		private void WriteLink(HtmlTextWriter writer, string contents, string before, string after)
		{
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

		private void WriteDiv(HtmlTextWriter writer, string cssClass, string contents)
		{
			this.WriteDiv(writer, cssClass, contents, null, null);
		}

		private void WriteDiv(HtmlTextWriter writer, string cssClass, string contents, string before, string after)
		{
			this.WriteTag(writer, HtmlTextWriterTag.Div, cssClass, contents, before, after);
		}

		private void WriteSpan(HtmlTextWriter writer, string cssClass, string contents)
		{
			this.WriteSpan(writer, cssClass, contents, null, null);
		}

		private void WriteSpan(HtmlTextWriter writer, string cssClass, string contents, string before, string after)
		{
			this.WriteTag(writer, HtmlTextWriterTag.Span, cssClass, contents, before, after);
		}

		private void WriteTag(HtmlTextWriter writer, HtmlTextWriterTag tagKey, string cssClass, string contents, string before, string after)
		{
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
