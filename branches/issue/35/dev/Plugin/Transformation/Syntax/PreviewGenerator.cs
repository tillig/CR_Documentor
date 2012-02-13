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
				writer.WriteSpan(PreviewCss.Keyword, "new");
			}
			writer.WriteSpan(PreviewCss.Keyword, this.Element.Visibility.ForLanguage(this.Language));
			if (this.Language == SupportedLanguageId.Basic && this.Element.IsNew)
			{
				writer.WriteSpan(PreviewCss.Keyword, "Shadows");
			}
			ContractWriter.Write(writer, this.Element, this.Language);
			if (this.Element is Interface)
			{
				switch (this.Language)
				{
					case SupportedLanguageId.Basic:
						writer.WriteSpan(PreviewCss.Keyword, "Interface");
						break;
					default:
						writer.WriteSpan(PreviewCss.Keyword, "interface");
						break;
				}
			}
			else if (this.Element is Struct)
			{
				switch (this.Language)
				{
					case SupportedLanguageId.Basic:
						writer.WriteSpan(PreviewCss.Keyword, "Structure");
						break;
					default:
						writer.WriteSpan(PreviewCss.Keyword, "struct");
						break;
				}
			}
			else
			{
				switch (this.Language)
				{
					case SupportedLanguageId.Basic:
						writer.WriteSpan(PreviewCss.Keyword, "Class");
						break;
					default:
						writer.WriteSpan(PreviewCss.Keyword, "class");
						break;
				}
			}
			writer.WriteSpan(PreviewCss.Identifier, this.Element.Name, null, "");
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
			writer.WriteSpan(PreviewCss.Keyword, this.Element.Visibility.ForLanguage(this.Language));
			ContractWriter.Write(writer, this.Element, this.Language);
			bool isCast = method.IsImplicitCast || method.IsExplicitCast;

			switch (this.Language)
			{
				case SupportedLanguageId.Basic:
					if (method.IsImplicitCast)
					{
						writer.WriteSpan(PreviewCss.Keyword, "Widening");
					}
					else if (method.IsExplicitCast)
					{
						writer.WriteSpan(PreviewCss.Keyword, "Narrowing");
					}
					writer.WriteSpan(PreviewCss.Keyword, "Operator");
					break;
				default:
					if (method.IsImplicitCast)
					{
						writer.WriteSpan(PreviewCss.Keyword, "implicit");
					}
					else if (method.IsExplicitCast)
					{
						writer.WriteSpan(PreviewCss.Keyword, "explicit");
					}
					else
					{
						writer.WriteLink(this.Element.MemberType(), "", " ");
					}
					writer.WriteSpan(PreviewCss.Keyword, "operator");
					if (isCast)
					{
						writer.WriteLink(this.Element.MemberType(), "", "");
					}
					break;
			}
			if (!isCast)
			{
				writer.WriteSpan(PreviewCss.Identifier, this.Element.Name, "", "");
			}
			else if (this.Language == SupportedLanguageId.Basic)
			{
				writer.WriteSpan(PreviewCss.Identifier, "CType", "", "");
			}
			ParameterWriter.Write(writer, this.Element as MemberWithParameters, this.Language, "(", ")");
			if (this.Language == SupportedLanguageId.Basic)
			{
				writer.WriteSpan(PreviewCss.Keyword, "As", "&nbsp;", " ");
				writer.WriteLink(this.Element.MemberType(), "", "");
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
			writer.WriteSpan(PreviewCss.Keyword, this.Element.Visibility.ForLanguage(this.Language));
			ContractWriter.Write(writer, this.Element, this.Language);
			switch (this.Language)
			{
				case SupportedLanguageId.Basic:
					writer.WriteSpan(PreviewCss.Keyword, "Sub");
					writer.WriteSpan(PreviewCss.Identifier, "New", "", "");
					break;
				default:
					writer.WriteSpan(PreviewCss.Identifier, this.Element.Name, "", "");
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
			writer.WriteSpan(PreviewCss.Keyword, MemberVisibility.Protected.ForLanguage(this.Language));
			switch (this.Language)
			{
				case SupportedLanguageId.Basic:
					writer.WriteSpan(PreviewCss.Keyword, "Overrides");
					writer.WriteSpan(PreviewCss.Keyword, "Sub");
					break;
				default:
					writer.WriteSpan(PreviewCss.Keyword, "override");
					writer.WriteSpan(PreviewCss.Keyword, "void");
					break;
			}
			writer.WriteSpan(PreviewCss.Identifier, "Finalize", "", "");
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
			writer.WriteSpan(PreviewCss.Keyword, this.Element.Visibility.ForLanguage(this.Language));
			switch (this.Language)
			{
				case SupportedLanguageId.Basic:
					writer.WriteSpan(PreviewCss.Keyword, "Delegate");
					break;
				default:
					writer.WriteSpan(PreviewCss.Keyword, "delegate");
					break;
			}
			string elementMemberType = this.Element.MemberType();
			if (TypeInfo.TypeIsVoid(elementMemberType))
			{
				switch (this.Language)
				{
					case SupportedLanguageId.Basic:
						writer.WriteSpan(PreviewCss.Keyword, "Sub");
						break;
					default:
						writer.WriteSpan(PreviewCss.Keyword, "void");
						break;
				}
			}
			else
			{
				switch (this.Language)
				{
					case SupportedLanguageId.Basic:
						writer.WriteSpan(PreviewCss.Keyword, "Function");
						break;
					default:
						writer.WriteLink(elementMemberType, null, null);
						break;
				}
			}
			writer.WriteSpan(PreviewCss.Identifier, this.Element.Name, "", "");
			ParameterWriter.Write(writer, this.Element as MemberWithParameters, this.Language, "(", ")");
			if (this.Language == SupportedLanguageId.Basic && !TypeInfo.TypeIsVoid(elementMemberType))
			{
				writer.WriteSpan(PreviewCss.Keyword, "As", "&nbsp;", " ");
				writer.WriteLink(elementMemberType, "", "");
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
			writer.WriteSpan(PreviewCss.Keyword, this.Element.Visibility.ForLanguage(this.Language));
			switch (this.Language)
			{
				case SupportedLanguageId.Basic:
					writer.WriteSpan(PreviewCss.Keyword, "Enum");
					break;
				default:
					writer.WriteSpan(PreviewCss.Keyword, "enum");
					break;
			}
			writer.WriteSpan(PreviewCss.Identifier, this.Element.Name, null, "");
			string underlyingType = ((Enumeration)this.Element).UnderlyingType;
			if (!String.IsNullOrEmpty(underlyingType))
			{
				switch (this.Language)
				{
					case SupportedLanguageId.Basic:
						writer.WriteSpan(PreviewCss.Keyword, "As", "&nbsp;", " ");
						break;
					default:
						writer.Write(" : ");
						break;
				}
				writer.WriteSpan(PreviewCss.Keyword, underlyingType, null, "");
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
			writer.WriteSpan(PreviewCss.Keyword, this.Element.Visibility.ForLanguage(this.Language));
			ContractWriter.Write(writer, this.Element, this.Language);
			switch (this.Language)
			{
				case SupportedLanguageId.Basic:
					writer.WriteSpan(PreviewCss.Keyword, "Event");
					break;
				default:
					writer.WriteSpan(PreviewCss.Keyword, "event");
					break;
			}

			// Get the generated parameterized event handler type name if it exists.
			// e.g., Public Event EventParameterized As TestClass.EventParameterizedEventHandler
			var eventElement = (Event)this.Element;
			string memberType = this.Element.MemberType();
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
				writer.WriteLink(memberType, null, null);
			}
			writer.WriteSpan(PreviewCss.Identifier, this.Element.Name, "", "");
			if (this.Language == SupportedLanguageId.Basic)
			{
				writer.WriteSpan(PreviewCss.Keyword, "As", "&nbsp;", " ");
				writer.WriteLink(memberType, "", "");
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
			writer.WriteSpan(PreviewCss.Keyword, this.Element.Visibility.ForLanguage(this.Language));
			ContractWriter.Write(writer, this.Element, this.Language);
			if (this.Language != SupportedLanguageId.Basic)
			{
				writer.WriteLink(this.Element.MemberType(), null, null);
			}
			writer.WriteSpan(PreviewCss.Identifier, this.Element.Name, "", "");
			if (this.Language == SupportedLanguageId.Basic)
			{
				writer.WriteSpan(PreviewCss.Keyword, "As", "&nbsp;", " ");
				writer.WriteLink(this.Element.MemberType(), "", "");
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
					writer.Write(HttpUtility.HtmlEncode(Strings.PreviewGenerator_LanguageNotSupported));
				}
				else if (!this.Element.IsSupportedForPreview())
				{
					writer.Write(HttpUtility.HtmlEncode(Strings.PreviewGenerator_LanguageElementNotSupported));
				}
				else
				{
					AttributeWriter.Write(writer, this.Element, this.Language);
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
				writer.WriteSpan(PreviewCss.Keyword, this.Element.Visibility.ForLanguage(this.Language));
			}

			ContractWriter.Write(writer, this.Element, this.Language);
			string elementMemberType = this.Element.MemberType();
			if (TypeInfo.TypeIsVoid(elementMemberType))
			{
				switch (this.Language)
				{
					case SupportedLanguageId.Basic:
						writer.WriteSpan(PreviewCss.Keyword, "Sub");
						break;
					default:
						writer.WriteSpan(PreviewCss.Keyword, "void");
						break;
				}
			}
			else
			{
				switch (this.Language)
				{
					case SupportedLanguageId.Basic:
						writer.WriteSpan(PreviewCss.Keyword, "Function");
						break;
					default:
						writer.WriteLink(elementMemberType, null, null);
						break;
				}
			}
			writer.WriteSpan(PreviewCss.Identifier, this.Element.Name, "", "");
			TypeParameterWriter.WriteTypeParameters(writer, this.Element, this.Language);
			ParameterWriter.Write(writer, this.Element as MemberWithParameters, this.Language, "(", ")");
			TypeParameterWriter.WriteConstraintsPostSignature(writer, this.Element, this.Language);
			if (this.Language == SupportedLanguageId.Basic)
			{
				if (!TypeInfo.TypeIsVoid(elementMemberType))
				{
					writer.WriteSpan(PreviewCss.Keyword, "As", "&nbsp;", " ");
					writer.WriteLink(elementMemberType, "", "");
				}
				int count = method.ImplementsCount;
				if (count > 0)
				{
					writer.Write(" ");
					writer.WriteSpan(PreviewCss.Keyword, "Implements");
					for (int i = 0; i < count; i++)
					{
						writer.WriteLink(method.Implements[i], "", "");
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
			writer.WriteSpan(PreviewCss.Keyword, this.Element.Visibility.ForLanguage(this.Language));
			ContractWriter.Write(writer, this.Element, this.Language);
			var property = (Property)this.Element;
			switch (this.Language)
			{
				case SupportedLanguageId.Basic:
					if (property.ParameterCount > 0)
					{
						writer.WriteSpan(PreviewCss.Keyword, "Default");
					}
					// In VB, properties with the ReadOnly keyword parse with IsReadOnly
					// set so we don't have to check for property.HasGetter && !property.HasSetter
					// because the ElementContract method will handle ReadOnly.
					else if (!property.HasGetter && property.HasSetter)
					{
						writer.WriteSpan(PreviewCss.Keyword, "WriteOnly");
					}
					writer.WriteSpan(PreviewCss.Keyword, "Property");
					break;
				default:
					break;
			}

			if (this.Language != SupportedLanguageId.Basic)
			{
				writer.WriteLink(this.Element.MemberType(), null, null);
			}
			if (property.ParameterCount > 0)
			{
				switch (this.Language)
				{
					case SupportedLanguageId.Basic:
						writer.WriteSpan(PreviewCss.Identifier, this.Element.Name, "", "");
						ParameterWriter.Write(writer, this.Element as MemberWithParameters, this.Language, "(", ")");
						break;
					default:
						writer.WriteSpan(PreviewCss.Keyword, "this", "", "");
						ParameterWriter.Write(writer, this.Element as MemberWithParameters, this.Language, "[", "]");
						break;
				}
			}
			else
			{
				writer.WriteSpan(PreviewCss.Identifier, this.Element.Name, "", "");
			}
			if (this.Language == SupportedLanguageId.Basic)
			{
				writer.WriteSpan(PreviewCss.Keyword, "As", "&nbsp;", " ");
				writer.WriteLink(this.Element.MemberType(), "", "");
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
			writer.WriteSpan(PreviewCss.Keyword, keyword, "", "");
			if (this.Language != SupportedLanguageId.Basic)
			{
				writer.Write(";");
			}
			writer.RenderEndTag();
		}
	}
}
