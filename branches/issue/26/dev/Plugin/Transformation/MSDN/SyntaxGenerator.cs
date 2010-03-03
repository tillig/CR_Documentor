using System;
using System.IO;
using System.Web;
using CR_Documentor.Transformation.Syntax;
using SP = DevExpress.CodeRush.StructuralParser;

namespace CR_Documentor.Transformation.MSDN
{
	/// <summary>
	/// Writes object syntax for the <see cref="CR_Documentor.Transformation.MSDN.Engine"/>.
	/// </summary>
	public class SyntaxGenerator : CR_Documentor.Transformation.Syntax.SyntaxGenerator
	{
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
		/// This method starts the rendering of the MSDN documentation style
		/// syntax preview.
		/// </remarks>
		public override void GenerateSyntaxPreview()
		{
			// Start the signature box
			this.Writer.Write("<div class='syntax'>");
			if (this.DocumentLanguage != Language.Basic && this.DocumentLanguage != Language.CSharp)
			{
				this.Writer.Write("[Language not supported for syntax preview.]");
			}
			else
			{
				// Write pre-syntax info
				this.PreSyntax();

				// Can't write unsafe doc for Basic
				if (this.DocumentLanguage == Language.Basic && this.Element.IsUnsafe)
				{
					this.Writer.Write("<p>Unsafe elements cannot be documented in VB.</p>");
					this.Writer.Write("</div>");
					return;
				}

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
			}

			// Done - close the signature box
			this.Writer.Write("</div>");
		}

		/// <summary>
		/// Writes pre-syntax object signature information.
		/// </summary>
		private void PreSyntax()
		{
			// Pre-signature
			if (this.Element is SP.Property && ((SP.Property)this.Element).ParameterCount > 0)
			{
				this.Writer.Write(String.Format("<p><span class=\"lang\">[C#]</span> In C#, this property is the indexer for the <b>{0}</b> class.</p>\n", this.Element.Parent.Name));
			}
			else if (this.Element is SP.Method && ((SP.Method)this.Element).MethodType == SP.MethodTypeEnum.Destructor)
			{
				string destructorPreSyntax = "<p><span class=\"lang\">[{0}]</span> In {0}, finalizers are expressed using destructor syntax.</p>\n";
				this.Writer.Write(String.Format(destructorPreSyntax, "C#"));
				this.Writer.Write(String.Format(destructorPreSyntax, "C++"));
			}
		}

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
			if (this.DocumentLanguage == Language.Basic)
			{
				this.Writer.Write("<div class='attribute'>");
				this.Writer.Write(Statement.AttributeOpen[this.DocumentLanguage]);
				for (int i = 0; i < attributes.Count; i++)
				{
					SP.Attribute attribute = (SP.Attribute)attributes[i];
					this.Writer.Write(attribute.Name);
					if (attribute.ArgumentCount > 0)
					{
						this.AttributeArguments(attribute.Arguments);
					}
					if (i + 1 < attributes.Count)
					{
						this.Writer.Write(", _<br /> ");
					}
				}
				this.Writer.Write(Statement.AttributeClose[this.DocumentLanguage]);
				this.Writer.Write("</div>");
			}
			else
			{
				for (int i = 0; i < attributes.Count; i++)
				{
					this.Writer.Write("<div class='attribute'>");
					this.Writer.Write(Statement.AttributeOpen[this.DocumentLanguage]);
					SP.Attribute attribute = (SP.Attribute)attributes[i];
					this.Writer.Write(attribute.Name);
					if (attribute.ArgumentCount > 0)
					{
						this.AttributeArguments(attribute.Arguments);
					}
					this.Writer.Write(Statement.AttributeClose[this.DocumentLanguage]);
					this.Writer.Write("</div>");
				}
			}
		}

		/// <summary>
		/// Writes class object signature information.
		/// </summary>
		private void Class()
		{
			this.Writer.Write("<b>");
			if (this.Element.IsNew)
			{
				this.Writer.Write(Keyword.New[this.DocumentLanguage]);
				this.Writer.Write(" ");
			}
			this.Writer.Write(Lookup.Visibility(this.Element));
			this.Writer.Write(" ");
			if (this.Element.IsStatic)
			{
				this.Writer.Write(Keyword.StaticClass[this.DocumentLanguage]);
				this.Writer.Write(" ");
			}
			else
			{
				if (this.Element.IsAbstract)
				{
					this.Writer.Write(Keyword.AbstractClass[this.DocumentLanguage]);
					this.Writer.Write(" ");
				}
				if (this.Element.IsSealed)
				{
					this.Writer.Write(Keyword.Sealed[this.DocumentLanguage]);
					this.Writer.Write(" ");
				}
			}

			this.Writer.Write(Lookup.ElementType(this.Element));
			this.Writer.Write(" ");
			this.Writer.Write(this.Element.Name);
			this.TypeParameters();
			// TODO: Write the inheritance/implements chain.
			// <xsl:template match="structure | interface | class" mode="derivation">
			this.Writer.Write("</b>");
		}

		/// <summary>
		/// Writes constructor object signature information.
		/// </summary>
		private void Constructor()
		{
			this.Writer.Write("<b>");
			this.MemberSyntaxProlog();
			this.Writer.Write(" ");
			string kw = Keyword.Constructor[this.DocumentLanguage];
			this.Writer.Write(kw);
			if (this.DocumentLanguage != Language.Basic)
			{
				if (kw.Length > 0)
				{
					this.Writer.Write(" ");
				}
				this.Writer.Write(this.Element.Name);
			}
			this.Parameters("(", ")");
			this.Writer.Write(Statement.End[this.DocumentLanguage]);
			this.Writer.Write("</b>");
		}

		/// <summary>
		/// Writes delegate object signature information.
		/// </summary>
		private void Delegate()
		{
			this.Writer.Write("<b>");
			this.Writer.Write(Lookup.Visibility(this.Element));
			this.Writer.Write(" ");
			this.Writer.Write(Lookup.ElementType(this.Element));
			this.Writer.Write(" ");

			if (this.DocumentLanguage != Language.Basic)
			{
				this.Writer.Write(HttpUtility.HtmlEncode(this.ElementMemberType));
				this.Writer.Write(" ");
			}
			this.Writer.Write(this.Element.Name);
			this.Parameters("(", ")");
			if (this.DocumentLanguage == Language.Basic)
			{
				this.ReturnType();
			}
			this.Writer.Write("</b>");
		}

		/// <summary>
		/// Writes enumeration object signature information.
		/// </summary>
		private void Enumeration()
		{
			this.Writer.Write("<b>");
			this.Writer.Write(Lookup.Visibility(this.Element));
			this.Writer.Write(" ");
			this.Writer.Write(Lookup.ElementType(this.Element));
			this.Writer.Write(" ");
			this.Writer.Write(this.Element.Name);
			string underlyingType = ((SP.Enumeration)this.Element).UnderlyingType;
			if (!String.IsNullOrEmpty(underlyingType))
			{
				if (this.DocumentLanguage == Language.Basic)
				{
					this.Writer.Write(" As ");
				}
				else
				{
					this.Writer.Write(" : ");
				}
				this.Writer.Write(HttpUtility.HtmlEncode(underlyingType));
			}
			this.Writer.Write("</b>");
		}

		/// <summary>
		/// Writes event object signature information.
		/// </summary>
		private void Event()
		{
			this.Writer.Write("<b>");
			this.Writer.Write(Lookup.Visibility(this.Element));
			this.Writer.Write(" ");
			string contract = Lookup.Contract(this.Element);
			if (contract.Length > 0)
			{
				this.Writer.Write(contract);
				this.Writer.Write(" ");
			}
			this.Writer.Write(Lookup.ElementType(this.Element));
			this.Writer.Write(" ");
			if (this.DocumentLanguage != Language.Basic)
			{
				this.Writer.Write(HttpUtility.HtmlEncode(this.ElementMemberType));
				this.Writer.Write(" ");
			}
			this.Writer.Write(this.Element.Name);
			if (this.DocumentLanguage == Language.Basic)
			{
				this.ReturnType();
				this.Writer.Write(" ");
			}
			this.Writer.Write(Statement.End[this.DocumentLanguage]);
			this.Writer.Write("</b>");
		}

		/// <summary>
		/// Writes field object signature information.
		/// </summary>
		private void Field()
		{
			this.Writer.Write("<b>");
			if (!(this.Element.Parent is SP.Interface))
			{
				this.Writer.Write(Lookup.Visibility(this.Element));
				this.Writer.Write(" ");
			}
			string contract = Lookup.Contract(this.Element);
			if (contract.Length > 0)
			{
				this.Writer.Write(contract);
				this.Writer.Write(" ");
			}
			this.Writer.Write(Lookup.ElementType(this.Element));
			this.Writer.Write(" ");
			if (this.DocumentLanguage != Language.Basic)
			{
				this.Writer.Write(HttpUtility.HtmlEncode(this.ElementMemberType));
				this.Writer.Write(" ");
			}
			this.Writer.Write(this.Element.Name);
			if (this.DocumentLanguage == Language.Basic)
			{
				this.ReturnType();
				this.Writer.Write(" ");
			}

			SP.Expression initializer = null;
			if (this.Element is SP.Const)
			{
				initializer = ((SP.Const)this.Element).Expression;
			}
			else if (this.Element is SP.InitializedVariable)
			{
				initializer = ((SP.InitializedVariable)this.Element).Expression;
			}
			if (initializer != null)
			{
				this.Writer.Write(" = ");
				this.Writer.Write(initializer.ToString());
			}
			this.Writer.Write(Statement.End[this.DocumentLanguage]);
			this.Writer.Write("</b>");
		}

		/// <summary>
		/// Writes the portion before a member signature (visibility, contract).
		/// </summary>
		private void MemberSyntaxProlog()
		{
			if (this.Element is SP.Interface || this.Element.Parent is SP.Interface)
			{
				// No prolog for interfaces or interface members
				return;
			}

			string prolog = Lookup.Contract(this.Element);
			string visibility = Lookup.Visibility(this.Element);
			bool writeVisibility = (!this.Element.IsStatic && !(this.Element is SP.Method && ((SP.Method)this.Element).IsConstructor));

			if (this.DocumentLanguage == Language.Basic && prolog.Length > 0)
			{
				// VB: Contract then visibility
				this.Writer.Write(prolog);
				this.Writer.Write(" ");
			}
			if (writeVisibility && visibility.Length > 0)
			{
				this.Writer.Write(visibility);
				this.Writer.Write(" ");
			}
			if (this.DocumentLanguage != Language.Basic && prolog.Length > 0)
			{
				// Non-VB: Visibility then contract
				this.Writer.Write(prolog);
				this.Writer.Write(" ");
			}
		}

		/// <summary>
		/// Writes method object signature information.
		/// </summary>
		private void Method()
		{
			SP.Method method = (SP.Method)this.Element;
			if (method.IsDestructor && this.DocumentLanguage == Language.CSharp)
			{
				// Handle destructors differently than other methods
				this.Writer.Write("<b>");
				this.Writer.Write("~");
				this.Writer.Write(this.Element.Parent.Name);
				this.Writer.Write("();");
				this.Writer.Write("</b>");
				return;
			}

			// The method is not a destructor

			this.Writer.Write("<b>");
			this.MemberSyntaxProlog();

			// method-start
			if (this.DocumentLanguage == Language.Basic)
			{
				if (TypeInfo.TypeIsVoid(this.ElementMemberType))
				{
					this.Writer.Write("Sub");
				}
				else
				{
					this.Writer.Write("Function");
				}
				this.Writer.Write(" ");
			}
			if (this.DocumentLanguage != Language.Basic)
			{
				this.ReturnType();
			}

			this.Writer.Write(this.Element.Name);

			this.TypeParameters();
			this.Parameters("(", ")");

			// method-end, member-implements
			if (this.DocumentLanguage == Language.Basic)
			{
				int count = method.Implements.Count;
				if (count > 0)
				{
					this.Writer.Write(" _ <br />    Implements ");
					for (int i = 0; i < count; i++)
					{
						this.Writer.Write(method.Implements[i]);
						if (i + 1 < count)
						{
							this.Writer.Write(", ");
						}
					}
				}

				this.ReturnType();
			}
			this.Writer.Write(Statement.End[this.DocumentLanguage]);
			this.Writer.Write("</b>");
		}

		/// <summary>
		/// Writes operator object signature information.
		/// </summary>
		private void Operator()
		{
			SP.Method method = (SP.Method)this.Element;

			string parentType = HttpUtility.HtmlEncode(this.Element.GetParentClassInterfaceOrStruct().Name);
			if (this.DocumentLanguage == Language.CSharp)
			{
				this.Writer.Write("<b>");
				this.MemberSyntaxProlog();
				this.Writer.Write(" ");
				if (this.DocumentLanguage == Language.CSharp)
				{
					if (method.IsImplicitCast)
					{
						this.Writer.Write("implicit ");
					}
					else if (method.IsExplicitCast)
					{
						this.Writer.Write("explicit ");
					}
				}
				this.Writer.Write(parentType);
				this.Writer.Write(" ");
				this.Writer.Write(Lookup.ElementType(this.Element));
				this.Writer.Write(" ");
				this.Writer.Write(this.Element.Name);
				this.Parameters("(", ")");
				this.Writer.Write("</b>");
			}
			else if (this.DocumentLanguage == Language.Basic)
			{
				this.Writer.Write("<i>returnValue</i> = <b>");
				this.Writer.Write(parentType);
				this.Writer.Write(".");
				this.Writer.Write(this.Element.Name);
				this.Writer.Write("(</b>");
				int paramCount = method.ParameterCount;
				for (int i = 0; i < paramCount; i++)
				{
					this.Writer.Write(method.Parameters[i]);
					if (i + 1 < paramCount)
					{
						this.Writer.Write(", ");
					}
				}
				this.Writer.Write("<b>)</b>");
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

			this.Writer.Write(openParen);
			for (int i = 0; i < count; i++)
			{
				// Get vars
				SP.Param parameter = (SP.Param)parameters[i];

				// Write the newline
				this.Writer.Write(Statement.Continue[this.DocumentLanguage]);
				this.Writer.Write("<br />&#160;&#160;&#160;");

				// Optional, ref, out, params, etc.
				if (isBasic && parameter.IsOptional)
				{
					this.Writer.Write("Optional ");
				}
				if (parameter.IsOutParam)
				{
					this.Writer.Write(Keyword.Out[this.DocumentLanguage]);
					this.Writer.Write(" ");
				}
				else if (parameter.IsReferenceParam)
				{
					this.Writer.Write(Keyword.Ref[this.DocumentLanguage]);
					this.Writer.Write(" ");
				}
				else if (isBasic)
				{
					this.Writer.Write("ByVal ");
				}
				if (parameter.IsParamArray)
				{
					this.Writer.Write(Keyword.Params[this.DocumentLanguage]);
					this.Writer.Write(" ");
				}

				// Name and type
				if (isBasic)
				{
					this.Writer.Write("<i>");
					this.Writer.Write(parameter.Name);
					this.Writer.Write("</i> As ");
				}
				// TODO: Handle type parameters (e.g., public void Foo(IEnumerable<T> bar)).
				this.Writer.Write(HttpUtility.HtmlEncode(parameter.ParamType));
				if (this.DocumentLanguage != Language.Basic)
				{
					this.Writer.Write(" <i>");
					this.Writer.Write(parameter.Name);
					this.Writer.Write("</i>");
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
			SP.Property property = (SP.Property)this.Element;
			SP.Get getter = property.Getter;
			SP.Set setter = property.Setter;
			int parameterCount = property.ParameterCount;

			if (this.DocumentLanguage == Language.Basic)
			{
				// syntax-map line 187
				this.Writer.Write("<b>");
				this.MemberSyntaxProlog();
				this.Writer.Write(" ");
				if (property.ParameterCount > 0)
				{
					this.Writer.Write("Default ");
				}
				this.Writer.Write("Property ");
				if (getter == null && setter != null)
				{
					this.Writer.Write("WriteOnly ");
				}
				else if (getter != null && setter == null)
				{
					this.Writer.Write("ReadOnly ");
				}
				this.Writer.Write(this.Element.Name);
				if (parameterCount > 0)
				{
					this.Parameters("(", ")");
				}
				this.ReturnType();
				this.Writer.Write("</b>");
			}
			else if (this.DocumentLanguage == Language.CSharp)
			{
				// syntax-map line 229
				this.Writer.Write("<b>");
				this.MemberSyntaxProlog();
				this.Writer.Write(" ");
				this.ReturnType();
				if (parameterCount > 0)
				{
					this.Writer.Write("this");
					this.Parameters("[", "]");
				}
				else
				{
					this.Writer.Write(this.Element.Name);
				}
				this.Writer.Write(" { ");
				if (getter == null && setter != null)
				{
					this.Writer.Write("set;");
				}
				else if (getter != null && setter == null)
				{
					this.Writer.Write("get;");
				}
				else
				{
					this.Writer.Write("get; set;");
				}
				this.Writer.Write(" } ");
				this.Writer.Write("</b>");
			}
			else
			{
				this.Writer.Write("[Language not supported.]");
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
					this.Writer.Write(" ");
					this.Writer.Write(Statement.ParamSeparator[this.DocumentLanguage]);
					this.Writer.Write(" ");
					this.Writer.Write(HttpUtility.HtmlEncode(this.ElementMemberType));
				}
			}
			else
			{
				this.Writer.Write(HttpUtility.HtmlEncode(this.ElementMemberType));
				this.Writer.Write(" ");
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
				this.Writer.Write(" where ");

				// Constraints for non-basic languages.
				for (int i = 0; i < parameterCount; i++)
				{
					SP.TypeParameter parameter = typeParams[i];
					this.TypeParameterConstraints(parameter);
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
				this.Writer.Write(" As ");
			}
			if (isBasic && parameterConstraintCount > 1)
			{
				this.Writer.Write("{");
			}
			for (int j = 0; j < parameterConstraintCount; j++)
			{
				SP.TypeParameterConstraint constraint = typeParamConstraints[j];
				this.Writer.Write("<a href=\"#\">");
				this.Writer.Write(constraint.Name);
				this.Writer.Write("</a>");
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
	}
}
