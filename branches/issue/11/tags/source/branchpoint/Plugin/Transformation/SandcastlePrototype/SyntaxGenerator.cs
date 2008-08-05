using System;
using System.IO;
using System.Web;
using CR_Documentor.Transformation.Syntax;
using SP = DevExpress.CodeRush.StructuralParser;

namespace CR_Documentor.Transformation.SandcastlePrototype
{
	/// <summary>
	/// Writes object syntax for the <see cref="CR_Documentor.Transformation.SandcastlePrototype.Engine"/>.
	/// </summary>
	public class SyntaxGenerator : CR_Documentor.Transformation.Syntax.SyntaxGenerator
	{
		private const string CssClassKeyword = "keyword";
		private const string CssClassIdentifier = "identifier";
		private const string CssClassParameter = "parameter";

		#region SyntaxGenerator Abstract Implementations

		/// <summary>
		/// Initializes a new instance of the <see cref="CR_Documentor.Transformation.MSDN.SyntaxGenerator" /> class.
		/// </summary>
		/// <param name="element">The element to generate the syntax preview for.</param>
		/// <param name="writer">The writer to output the syntax preview to.</param>
		/// <exception cref="System.ArgumentNullException">
		/// Thrown if <paramref name="element" /> or <paramref name="writer" /> is <see langword="null" />.
		/// </exception>
		public SyntaxGenerator(SP.AccessSpecifiedElement element, StringWriter writer) : base(element, writer) { }

		/// <summary>
		/// Produces the syntax preview of the element.
		/// </summary>
		/// <remarks>
		/// This method starts the rendering of the Sandcastle Prototype documentation style
		/// syntax preview.
		/// </remarks>
		public override void GenerateSyntaxPreview()
		{
			// Start the signature box
			this.Writer.Write("<div id=\"syntaxBlocks\"><div class=\"code\"><pre>");

			// Write attributes
			this.Attributes();

			// Call the correct syntax this.Writer
			if (this.Element is SP.Enumeration)
			{
				this.Enumeration();
			}
			else if (this.Element is SP.Class)
			{
				this.Class();
			}
			else if (this.Element is SP.DelegateDefinition)
			{
				this.Delegate();
			}

			else if (this.Element is SP.Method)
			{
				SP.Method method = (SP.Method)this.Element;
				if (method.IsConstructor)
				{
					this.Constructor();
				}
				else if (method.IsClassOperator)
				{
					this.Operator();
				}
				else if (method.IsDestructor)
				{
					this.Destructor();
				}
				else
				{
					this.Method();
				}
			}
			else if (this.Element is SP.Property)
			{
				this.Property();
			}
			else if (this.Element is SP.Event)
			{
				this.Event();
			}
			else if (this.Element is SP.BaseVariable)
			{
				this.Field();
			}
			else
			{
				this.Writer.Write("[This object has no individual syntax.]");
			}

			// Done - close the signature box
			this.Writer.Write("</pre></div></div>");
		}

		#endregion

		#region Object Types

		/// <summary>
		/// Writes attribute information.
		/// </summary>
		private void Attributes()
		{
			if (this.Element.AttributeCount < 1)
			{
				return;
			}

			SP.NodeList attributes = this.Element.Attributes;
			for (int i = 0; i < attributes.Count; i++)
			{
				SP.Attribute attribute = (SP.Attribute)attributes[i];
				this.Writer.Write(Statement.AttributeOpen[this.DocumentLanguage]);
				this.WriteLink(attribute.Name, "", "");
				if (attribute.ArgumentCount > 0)
				{
					this.AttributeArguments(attribute.Arguments);
				}
				this.Writer.Write(Statement.AttributeClose[this.DocumentLanguage]);
				if (i + 1 < attributes.Count)
				{
					this.Writer.Write(Statement.Continue[this.DocumentLanguage]);
				}
				this.Writer.Write("\n");
			}
		}

		/// <summary>
		/// Writes class object signature information.
		/// </summary>
		private void Class()
		{
			if (this.DocumentLanguage == Language.C)
			{
				this.TypeParameters();
			}
			if (this.Element.IsNew)
			{
				this.WriteSpan(CssClassKeyword, Keyword.New[this.DocumentLanguage]);
			}
			this.WriteSpan(CssClassKeyword, Lookup.Visibility(this.Element));
			if (this.DocumentLanguage == Language.C)
			{
				this.WriteSpan(CssClassKeyword, Lookup.GCTypeQualifier(this.Element));
				this.WriteSpan(CssClassKeyword, Lookup.ElementType(this.Element));
				this.WriteSpan(CssClassIdentifier, this.Element.Name);
			}
			if (this.Element.IsStatic)
			{
				this.WriteSpan(CssClassKeyword, Keyword.StaticClass[this.DocumentLanguage]);
			}
			else
			{
				if (this.Element.IsAbstract)
				{
					this.WriteSpan(CssClassKeyword, Keyword.AbstractClass[this.DocumentLanguage]);
				}
				if (this.Element.IsSealed)
				{
					this.WriteSpan(CssClassKeyword, Keyword.Sealed[this.DocumentLanguage]);
				}
			}
			if (this.DocumentLanguage != Language.C)
			{
				this.WriteSpan(CssClassKeyword, Lookup.ElementType(this.Element));
				this.WriteSpan(CssClassIdentifier, this.Element.Name, "", "");
				this.TypeParameters();
				this.Writer.Write(" ");
			}
			// TODO: Write the inheritance/implements chain.
		}

		/// <summary>
		/// Writes constructor object signature information.
		/// </summary>
		private void Constructor()
		{
			this.MemberSyntaxProlog();
			string kw = Keyword.Constructor[this.DocumentLanguage];
			this.WriteSpan(CssClassKeyword, kw, "", "");
			if (this.DocumentLanguage != Language.Basic)
			{
				if (kw.Length > 0)
				{
					this.Writer.Write(" ");
				}
				this.WriteSpan(CssClassIdentifier, this.Element.Name, "", "");
			}
			this.Parameters("(", ")");
		}

		/// <summary>
		/// Writes delegate object signature information.
		/// </summary>
		private void Delegate()
		{
			this.WriteSpan(CssClassKeyword, Lookup.Visibility(this.Element));
			this.WriteSpan(CssClassKeyword, Lookup.ElementType(this.Element));
			if (this.DocumentLanguage != Language.Basic)
			{
				if (TypeInfo.TypeIsVoid(this.ElementMemberType))
				{
					this.WriteSpan(CssClassKeyword, this.ElementMemberType);
				}
				else
				{
					this.WriteLink(this.ElementMemberType);
				}
			}
			this.WriteSpan(CssClassIdentifier, this.Element.Name, "", "");
			this.Parameters("(", ")");
			if (this.DocumentLanguage == Language.C && this.ElementMemberType.IndexOf("[") >= 0)
			{
				this.Writer.Write(" []");
			}
			if (this.DocumentLanguage == Language.Basic)
			{
				this.ReturnType();
			}
		}

		/// <summary>
		/// Writes destructor object signature information.
		/// </summary>
		private void Destructor()
		{
			SP.AccessSpecifiers access = new SP.AccessSpecifiers();
			access.IsOverride = true;
			this.MemberSyntaxProlog(SP.MemberVisibility.Protected, access, false);
			if (this.DocumentLanguage == Language.Basic)
			{
				this.WriteSpan(CssClassKeyword, "Sub");
			}
			else
			{
				this.WriteSpan(CssClassKeyword, "void");
			}
			this.WriteSpan(CssClassIdentifier, Keyword.Destructor[this.DocumentLanguage], "", "");
			this.Parameters("(", ")");
		}

		/// <summary>
		/// Writes enumeration object signature information.
		/// </summary>
		private void Enumeration()
		{
			this.WriteSpan(CssClassKeyword, Lookup.Visibility(this.Element));
			this.WriteSpan(CssClassKeyword, Lookup.ElementType(this.Element));
			this.WriteSpan(CssClassIdentifier, this.Element.Name, "", "");
			string underlyingType = ((SP.Enumeration)this.Element).UnderlyingType;
			if (!String.IsNullOrEmpty(underlyingType))
			{
				if (this.DocumentLanguage == Language.Basic)
				{
					this.WriteSpan(CssClassKeyword, "As", " ", " ");
				}
				else
				{
					this.Writer.Write(" : ");
				}
				this.WriteLink(HttpUtility.HtmlEncode(underlyingType), "", "");
			}
		}

		/// <summary>
		/// Writes event object signature information.
		/// </summary>
		private void Event()
		{
			this.WriteSpan(CssClassKeyword, Lookup.Visibility(this.Element));
			this.WriteSpan(CssClassKeyword, Lookup.Contract(this.Element));
			this.WriteSpan(CssClassKeyword, Lookup.ElementType(this.Element));
			if (this.DocumentLanguage != Language.Basic)
			{
				this.WriteLink(HttpUtility.HtmlEncode(this.ElementMemberType));
			}
			this.WriteSpan(CssClassIdentifier, this.Element.Name);
			if (this.DocumentLanguage == Language.Basic)
			{
				// TODO: Handle VB parameterized events with generated event handler types.
				this.ReturnType();
			}
			else if (this.DocumentLanguage == Language.C)
			{
				// TODO: Figure a better way to write the C++ add/remove methods for events.
				this.Writer.WriteLine("{<br />");
				this.Writer.Write("\t");
				this.WriteSpan(CssClassKeyword, "void add");
				this.Writer.Write("(");
				this.WriteLink(HttpUtility.HtmlEncode(this.ElementMemberType));
				this.WriteSpan(CssClassParameter, "value", "", "");
				this.Writer.WriteLine(");<br />");
				this.WriteSpan(CssClassKeyword, "void remove");
				this.Writer.Write("(");
				this.WriteLink(HttpUtility.HtmlEncode(this.ElementMemberType));
				this.WriteSpan(CssClassParameter, "value", "", "");
				this.Writer.WriteLine(");<br />");
				this.Writer.Write("}");
			}
		}

		/// <summary>
		/// Writes field object signature information.
		/// </summary>
		private void Field()
		{
			this.WriteSpan(CssClassKeyword, Lookup.Visibility(this.Element));
			this.WriteSpan(CssClassKeyword, Lookup.Contract(this.Element));
			this.WriteSpan(CssClassKeyword, Lookup.ElementType(this.Element));
			if (this.DocumentLanguage != Language.Basic)
			{
				this.WriteLink(HttpUtility.HtmlEncode(this.ElementMemberType));
			}
			this.WriteSpan(CssClassIdentifier, this.Element.Name);
			if (this.DocumentLanguage == Language.Basic)
			{
				this.ReturnType();
			}
		}

		/// <summary>
		/// Writes the portion before a member signature (visibility, contract)
		/// for the current element.
		/// </summary>
		private void MemberSyntaxProlog()
		{
			this.MemberSyntaxProlog(this.Element.Visibility, this.Element.AccessSpecifiers, (this.Element is SP.Interface || this.Element.Parent is SP.Interface));
		}

		/// <summary>
		/// Writes the portion before a member signature (visibility, contract).
		/// </summary>
		/// <param name="visibility">Visibility of the member.</param>
		/// <param name="access">Accessibility of the member.</param>
		/// <param name="isInterface"><see langword="true" /> if the member is an interface or part of an interface.</param>
		private void MemberSyntaxProlog(SP.MemberVisibility visibility, SP.AccessSpecifiers access, bool isInterface)
		{
			if (isInterface)
			{
				// No prolog for interfaces or interface members
				return;
			}

			this.WriteSpan(CssClassKeyword, Lookup.Visibility(this.DocumentLanguage, visibility));
			this.WriteSpan(CssClassKeyword, Lookup.Contract(this.DocumentLanguage, access));
		}

		/// <summary>
		/// Writes method object signature information.
		/// </summary>
		private void Method()
		{
			this.MemberSyntaxProlog();
			if (this.DocumentLanguage == Language.Basic)
			{
				if (TypeInfo.TypeIsVoid(this.ElementMemberType))
				{
					this.WriteSpan(CssClassKeyword, "Sub");
				}
				else
				{
					this.WriteSpan(CssClassKeyword, "Function");
				}
			}
			if (this.DocumentLanguage != Language.Basic)
			{
				this.ReturnType();
			}

			this.WriteSpan(CssClassIdentifier, this.Element.Name, "", "");
			this.TypeParameters();
			this.Parameters("(", ")");

			SP.Method method = (SP.Method)this.Element;
			if (this.DocumentLanguage == Language.Basic)
			{
				int count = method.Implements.Count;
				if (count > 0)
				{
					this.Writer.Write("_<br/>");
					this.WriteSpan(CssClassKeyword, "Implements");
					for (int i = 0; i < count; i++)
					{
						this.WriteLink(method.Implements[i]);
						if (i + 1 < count)
						{
							this.WriteSpan("", ",");
						}
					}
				}

				this.ReturnType();
			}
		}

		/// <summary>
		/// Writes operator object signature information.
		/// </summary>
		private void Operator()
		{
			this.MemberSyntaxProlog();

			SP.Method method = (SP.Method)this.Element;
			string memberType = HttpUtility.HtmlEncode(method.MemberType);
			bool isCast = method.IsImplicitCast || method.IsExplicitCast;

			if (isCast)
			{
				if (method.IsImplicitCast)
				{
					this.WriteSpan(CssClassKeyword, Keyword.ImplicitConversion[this.DocumentLanguage]);
				}
				else if (method.IsExplicitCast)
				{
					this.WriteSpan(CssClassKeyword, Keyword.ExplicitConversion[this.DocumentLanguage]);
				}
			}
			else
			{
				if (this.DocumentLanguage != Language.Basic)
				{
					this.WriteLink(memberType);
				}
				if (this.DocumentLanguage == Language.C)
				{
					this.Writer.Write("^");
				}
			}
			this.WriteSpan(CssClassKeyword, Lookup.ElementType(this.Element));
			if (isCast)
			{
				if (this.DocumentLanguage != Language.Basic)
				{
					this.WriteLink(memberType);
				}
				else
				{
					this.WriteSpan(CssClassIdentifier, "CType");
				}
				if (this.DocumentLanguage == Language.C)
				{
					this.Writer.Write("^");
				}
			}
			else
			{
				this.WriteSpan(CssClassIdentifier, this.Element.Name);
			}
			this.Parameters("(", ")");
			if (this.DocumentLanguage == Language.Basic)
			{
				this.WriteSpan(CssClassKeyword, "As", " ", " ");
				this.WriteLink(memberType);
			}
		}

		/// <summary>
		/// Writes parameter information to the object signature.
		/// </summary>
		/// <param name="openParen">The open parenthesis (usually "(" but sometimes "[").</param>
		/// <param name="closeParen">The close parenthesis (usually ")" but sometimes "]").</param>
		private void Parameters(string openParen, string closeParen)
		{
			SP.LanguageElementCollection parameters = ((SP.MemberWithParameters)this.Element).Parameters;
			int count = parameters.Count;
			bool isBasic = this.DocumentLanguage == Language.Basic;
			if (isBasic && count == 0)
			{
				// If there aren't any parameters in VB, no parens get
				// rendered or anything.
				return;
			}

			this.Writer.Write(openParen);
			for (int i = 0; i < count; i++)
			{
				// Get vars
				SP.Param parameter = (SP.Param)parameters[i];

				// Write the newline
				this.Writer.Write(Statement.Continue[this.DocumentLanguage]);
				this.Writer.Write("<br />\t");

				// Optional, ref, out, params, etc.
				if (isBasic && parameter.IsOptional)
				{
					this.WriteSpan(CssClassKeyword, "Optional");
				}
				if (parameter.IsOutParam)
				{
					this.WriteSpan(CssClassKeyword, Keyword.Out[this.DocumentLanguage]);
				}
				else if (parameter.IsReferenceParam)
				{
					this.WriteSpan(CssClassKeyword, Keyword.Ref[this.DocumentLanguage]);
				}
				else if (isBasic)
				{
					this.WriteSpan(CssClassKeyword, "ByVal");
				}
				if (parameter.IsParamArray)
				{
					this.WriteSpan(CssClassKeyword, Keyword.Params[this.DocumentLanguage]);
				}

				// Name and type
				if (isBasic)
				{
					this.WriteSpan(CssClassParameter, parameter.Name);
					this.WriteSpan(CssClassKeyword, "As");
				}
				this.WriteLink(HttpUtility.HtmlEncode(parameter.ParamType), "", "");
				if (this.DocumentLanguage == Language.C)
				{
					this.Writer.Write("^");
				}
				if (!isBasic)
				{
					this.Writer.Write(" ");
					this.WriteSpan(CssClassParameter, parameter.Name, "", "");
				}

				// Default value and other language-specifics
				if (isBasic && parameter.IsOptional)
				{
					this.Writer.Write(" = ");
					this.Writer.Write(parameter.DefaultValue);
				}

				// Next param
				if (i + 1 < count)
				{
					this.Writer.Write(",");
				}
			}

			// Final newline if there were params
			if (count > 0)
			{
				this.Writer.Write(Statement.Continue[this.DocumentLanguage]);
				this.Writer.Write("<br />");
			}

			// Finished
			this.Writer.Write(closeParen);
		}

		/// <summary>
		/// Writes property object signature information.
		/// </summary>
		private void Property()
		{
			this.MemberSyntaxProlog();
			this.WriteSpan(CssClassKeyword, Keyword.Property[this.DocumentLanguage]);

			if (this.DocumentLanguage != Language.Basic)
			{
				this.ReturnType();
			}

			SP.Property property = (SP.Property)this.Element;

			if (property.ParameterCount > 0)
			{
				if (this.DocumentLanguage == Language.CSharp)
				{
					this.WriteSpan(CssClassKeyword, "this", "", "");
				}
				else if (this.DocumentLanguage == Language.C)
				{
					this.WriteSpan(CssClassKeyword, "default", "", "");
				}
				else
				{
					this.WriteSpan(CssClassIdentifier, this.Element.Name, "", "");
				}
			}
			else
			{
				this.WriteSpan(CssClassIdentifier, this.Element.Name, "", "");
			}

			if (this.DocumentLanguage == Language.Basic)
			{
				this.Parameters("(", ")");
			}
			else
			{
				this.Parameters("[", "]");
			}

			if (this.DocumentLanguage != Language.Basic)
			{
				// Get/Set
				this.Writer.Write("{ ");
				if (property.Getter != null)
				{
					if (this.DocumentLanguage == Language.C)
					{
						this.Writer.Write("<br />\t");
					}
					this.WriteSpan(CssClassKeyword, "get", "", "");
					// TODO: C++ lists parameters inline for the getter.
					this.Writer.Write("; ");
				}
				if (property.Setter != null)
				{
					if (this.DocumentLanguage == Language.C)
					{
						this.Writer.Write("<br />\t");
					}
					this.WriteSpan(CssClassKeyword, "set", "", "");
					// TODO: C++ lists parameters inline for the setter.
					this.Writer.Write("; ");
				}
				if (this.DocumentLanguage == Language.C)
				{
					this.Writer.Write("<br />");
				}
				this.Writer.Write("}");
			}

			if (this.DocumentLanguage == Language.Basic)
			{
				this.ReturnType();
			}
		}

		/// <summary>
		/// Writes return type information for method signatures.
		/// </summary>
		private void ReturnType()
		{
			if (this.DocumentLanguage == Language.Basic)
			{
				if (!TypeInfo.TypeIsVoid(this.ElementMemberType))
				{
					this.WriteSpan(CssClassKeyword, Statement.ParamSeparator[this.DocumentLanguage], " ", " ");
					this.WriteLink(HttpUtility.HtmlEncode(this.ElementMemberType));
				}
			}
			else
			{
				if (!TypeInfo.TypeIsVoid(this.ElementMemberType))
				{
					this.WriteLink(HttpUtility.HtmlEncode(this.ElementMemberType));
				}
				else
				{
					this.WriteSpan(CssClassKeyword, HttpUtility.HtmlEncode(this.ElementMemberType));
				}
			}
		}

		/// <summary>
		/// Writes the type parameter information to the object signature.
		/// </summary>
		private void TypeParameters()
		{
			if (!this.Element.IsGeneric)
			{
				return;
			}

			SP.TypeParameterCollection typeParams = this.Element.GenericModifier.TypeParameters;
			int parameterCount = typeParams.Count;
			if (parameterCount == 0)
			{
				return;
			}

			bool isBasic = this.DocumentLanguage == Language.Basic;
			bool isC = this.DocumentLanguage == Language.C;

			if (isC)
			{
				this.WriteSpan(CssClassKeyword, "generic", "", "");
			}

			this.Writer.Write(HttpUtility.HtmlEncode(Statement.TypeParamListOpener[this.DocumentLanguage]));
			bool constraintsExist = false;
			for (int i = 0; i < parameterCount; i++)
			{
				SP.TypeParameter parameter = typeParams[i];
				this.Writer.Write(parameter.Name);
				if (parameter.Constraints.Count > 0)
				{
					// Track if there are constraints to output for later.
					constraintsExist = true;

					if (isBasic)
					{
						// ...but Basic constraints are inline.
						this.TypeParameterConstraints(parameter);
					}
				}
				if (i + 1 < parameterCount)
				{
					this.Writer.Write(", ");
				}
			}
			this.Writer.Write(HttpUtility.HtmlEncode(Statement.TypeParamListCloser[this.DocumentLanguage]));

			if (!isBasic && constraintsExist)
			{
				if (isC)
				{
					this.Writer.Write("<br />");
				}
				else
				{
					this.Writer.Write(" ");
				}
				this.WriteSpan(CssClassKeyword, "where");

				// Constraints for non-basic languages.
				for (int i = 0; i < parameterCount; i++)
				{
					SP.TypeParameter parameter = typeParams[i];
					this.TypeParameterConstraints(parameter);
				}
				if (isC)
				{
					this.Writer.Write("<br />");
				}
			}
		}

		/// <summary>
		/// Writes out type parameter constraints for a given type parameter.
		/// </summary>
		/// <param name="typeParameter">The type parameter to write constraints for.</param>
		private void TypeParameterConstraints(SP.TypeParameter typeParameter)
		{
			SP.TypeParameterConstraintCollection typeParamConstraints = typeParameter.Constraints;
			int parameterConstraintCount = typeParamConstraints.Count;
			if (parameterConstraintCount == 0)
			{
				return;
			}
			bool isBasic = this.DocumentLanguage == Language.Basic;
			if (!isBasic)
			{
				this.Writer.Write(typeParameter.Name);
				this.Writer.Write(" : ");
			}
			else
			{
				this.WriteSpan(CssClassKeyword, "As", " ", " ");
			}
			if (isBasic && parameterConstraintCount > 1)
			{
				this.Writer.Write("{");
			}
			for (int j = 0; j < parameterConstraintCount; j++)
			{
				SP.TypeParameterConstraint constraint = typeParamConstraints[j];
				this.WriteLink(constraint.Name, "", "");
				if (j + 1 < parameterConstraintCount)
				{
					this.Writer.Write(", ");
				}
			}
			if (isBasic && parameterConstraintCount > 1)
			{
				this.Writer.Write("}");
			}

		}

		/// <summary>
		/// Writes a non-functional link with the given content, followed by a space.
		/// </summary>
		/// <param name="content">The content that should appear inside the link.</param>
		/// <remarks>
		/// <para>
		/// If <paramref name="content" /> is <see langword="null" /> or
		/// <see cref="System.String.Empty" />, nothing is written.
		/// </para>
		/// </remarks>
		private void WriteLink(string content)
		{
			this.WriteLink(content, "", " ");
		}

		/// <summary>
		/// Writes a non-functional link with the given content, wrapped by specified text.
		/// </summary>
		/// <param name="content">The content that should appear inside the link.</param>
		/// <param name="pre">The content that should appear before the link.</param>
		/// <param name="post">The content that should appear after the link.</param>
		/// <remarks>
		/// <para>
		/// If <paramref name="content" /> is <see langword="null" /> or
		/// <see cref="System.String.Empty" />, nothing is written.
		/// </para>
		/// </remarks>
		private void WriteLink(string content, string pre, string post)
		{
			if (String.IsNullOrEmpty(content))
			{
				return;
			}
			if (!String.IsNullOrEmpty(pre))
			{
				this.Writer.Write(pre);
			}
			this.Writer.Write("<a href=\"#\">");
			this.Writer.Write(content);
			this.Writer.Write("</a>");
			if (!String.IsNullOrEmpty(post))
			{
				this.Writer.Write(post);
			}
		}

		/// <summary>
		/// Writes a span tag with a CSS class and specific contents, followed by a space.
		/// </summary>
		/// <param name="cssClass">The CSS class for the span (<c>keyword</c>, <c>identifier</c>).</param>
		/// <param name="content">The content that should appear inside the span.</param>
		/// <remarks>
		/// <para>
		/// If <paramref name="cssClass" /> is <see langword="null" /> or <see cref="System.String.Empty" />,
		/// no class information is written. If <paramref name="content" /> is
		/// <see langword="null" /> or <see cref="System.String.Empty" />, nothing
		/// is written.
		/// </para>
		/// </remarks>
		private void WriteSpan(string cssClass, string content)
		{
			this.WriteSpan(cssClass, content, "", " ");
		}

		/// <summary>
		/// Writes a span tag with a CSS class and specific contents, wrapped by specified text.
		/// </summary>
		/// <param name="cssClass">The CSS class for the span (<c>keyword</c>, <c>identifier</c>).</param>
		/// <param name="content">The content that should appear inside the span.</param>
		/// <param name="pre">The content that should appear before the span.</param>
		/// <param name="post">The content that should appear after the span.</param>
		/// <remarks>
		/// <para>
		/// If <paramref name="cssClass" /> is <see langword="null" /> or <see cref="System.String.Empty" />,
		/// no class information is written. If <paramref name="content" /> is
		/// <see langword="null" /> or <see cref="System.String.Empty" />, nothing
		/// is written.
		/// </para>
		/// </remarks>
		private void WriteSpan(string cssClass, string content, string pre, string post)
		{
			if (String.IsNullOrEmpty(content))
			{
				return;
			}
			if (!String.IsNullOrEmpty(pre))
			{
				this.Writer.Write(pre);
			}
			this.Writer.Write("<span");
			if (!String.IsNullOrEmpty(cssClass))
			{
				this.Writer.Write(" class=\"");
				this.Writer.Write(cssClass);
				this.Writer.Write("\"");
			}
			this.Writer.Write(">");
			this.Writer.Write(content);
			this.Writer.Write("</span>");
			if (!String.IsNullOrEmpty(post))
			{
				this.Writer.Write(post);
			}
		}

		#endregion

	}
}
