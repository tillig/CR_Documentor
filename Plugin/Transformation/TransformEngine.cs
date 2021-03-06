using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security;
using System.Text;
using System.Xml;
using CR_Documentor.Diagnostics;
using CR_Documentor.Options;
using CR_Documentor.Xml;
using SP = DevExpress.CodeRush.StructuralParser;

namespace CR_Documentor.Transformation
{
	/// <summary>
	/// Base class for transforming XML documentation into HTML for preview.
	/// </summary>
	public abstract class TransformEngine
	{
		/// <summary>
		/// Log entry handler.
		/// </summary>
		private static readonly ILog Log = LogManager.GetLogger(typeof(TransformEngine));

		/// <summary>
		/// Text that appears in the base HTML document that will be replaced with the documentation preview body.
		/// </summary>
		protected const string BodyReplacementMacro = "!!BODY!!";

		/// <summary>
		/// Contains the signature of the current code target to render.
		/// </summary>
		/// <seealso cref="CR_Documentor.Transformation.TransformEngine" />
		private string _codeTargetSignature = null;

		/// <summary>
		/// Internal storage for the
		/// <see cref="CR_Documentor.Transformation.TransformEngine.CodeTarget" />
		/// property.
		/// </summary>
		/// <seealso cref="CR_Documentor.Transformation.TransformEngine" />
		private SP.LanguageElement _codeTarget;

		/// <summary>
		/// Internal storage for the
		/// <see cref="CR_Documentor.Transformation.TransformEngine.Document" />
		/// property.
		/// </summary>
		/// <seealso cref="CR_Documentor.Transformation.TransformEngine" />
		private XmlDocument _document;

		/// <summary>
		/// Internal storage for the
		/// <see cref="CR_Documentor.Transformation.TransformEngine.MemberSyntax" />
		/// property.
		/// </summary>
		/// <seealso cref="CR_Documentor.Transformation.TransformEngine" />
		private string _memberSyntax = null;

		/// <summary>
		/// Internal storage for the
		/// <see cref="CR_Documentor.Transformation.TransformEngine.Options" />
		/// property.
		/// </summary>
		/// <seealso cref="CR_Documentor.Transformation.TransformEngine" />
		private OptionSet _options;

		/// <summary>
		/// Internal storage for the
		/// <see cref="CR_Documentor.Transformation.TransformEngine.Writer" />
		/// property.
		/// </summary>
		/// <seealso cref="CR_Documentor.Transformation.TransformEngine" />
		private StringWriter _writer = null;

		/// <summary>
		/// Gets or sets the <see cref="DevExpress.CodeRush.StructuralParser.LanguageElement"/>
		/// corresponding to the XML comment.
		/// </summary>
		/// <value>
		/// A <see cref="DevExpress.CodeRush.StructuralParser.LanguageElement"/> that corresponds
		/// to the XML doc comment.
		/// </value>
		/// <remarks>
		/// <para>
		/// Clears the cached member syntax information when an XML doc comment
		/// is left or when the code target is set to null.
		/// </para>
		/// </remarks>
		/// <seealso cref="CR_Documentor.Transformation.TransformEngine" />
		public SP.LanguageElement CodeTarget
		{
			get
			{
				return _codeTarget;
			}
			set
			{
				string sig = null;
				if (value != null)
				{
					sig = this.GenerateLanguageElementSignature(value);
				}
				bool codeTargetSignatureChanged = !DevExpress.CodeRush.Core.CodeRush.Source.InsideXMLDocComment || value == null || sig != this._codeTargetSignature;
				if (codeTargetSignatureChanged)
				{
					this._codeTargetSignature = sig;
					this.MemberSyntax = null;
				}
				_codeTarget = value;
			}
		}

		/// <summary>
		/// Gets the element that signature/banner documentation should be generated based on.
		/// </summary>
		/// <value>
		/// If the <see cref="CR_Documentor.Transformation.TransformEngine.CodeTarget"/>
		/// can be documented, it is returned as a <see cref="DevExpress.CodeRush.StructuralParser.AccessSpecifiedElement"/>;
		/// otherwise, <see langword="null" /> is returned.
		/// </value>
		/// <seealso cref="CR_Documentor.Transformation.TransformEngine" />
		protected SP.AccessSpecifiedElement CodeTargetToRender
		{
			get
			{
				if (this.CodeTarget == null || !this.CodeTarget.CanBeDocumented)
				{
					return null;
				}

				return this.CodeTarget as SP.AccessSpecifiedElement;
			}
		}

		/// <summary>
		/// Gets a dictionary of tag/handler mappings.
		/// </summary>
		/// <value>
		/// A <see cref="System.Collections.Generic.Dictionary{K,V}"/>
		/// where the key is the tag being matched and the value is the handler
		/// for the tag. The key <see cref="System.String.Empty"/> should be used
		/// as the "default" handler tag.
		/// </value>
		/// <seealso cref="CR_Documentor.Transformation.TransformEngine" />
		protected Dictionary<string, Action<XmlElement>> CommentMatchHandlers { get; private set; }

		/// <summary>
		/// Gets or sets the <see cref="System.Xml.XmlDocument"/>
		/// containing the XML doc comment to transform.
		/// </summary>
		/// <value>
		/// A <see cref="System.Xml.XmlDocument"/> with the current
		/// document to be transformed.
		/// </value>
		/// <seealso cref="CR_Documentor.Transformation.TransformEngine" />
		public XmlDocument Document
		{
			get
			{
				return _document;
			}
			set
			{
				_document = value;
				if (this.Options.ProcessIncludes != IncludeProcessing.None)
				{
					// If we allow relative file paths, pass the source file location
					// to the processor; if we don't, pass null so the location
					// of the source won't be accounted for.
					string sourcefile = null;
					if (this.Options.ProcessIncludes == IncludeProcessing.Relative && this.CodeTarget != null && this.CodeTarget.Document != null)
					{
						sourcefile = this.CodeTarget.Document.FullName;
					}
					CommentParser.ProcessIncludedDocumentation(_document, sourcefile);
				}
			}
		}

		/// <summary>
		/// Gets or sets the generated syntax of the code target to render.
		/// </summary>
		/// <value>
		/// A <see cref="System.String"/> containing the HTML representing
		/// the syntax of the current code target.  If there is no current
		/// code target being rendered, this value will be <see langword="null" />.
		/// </value>
		/// <seealso cref="CR_Documentor.Transformation.TransformEngine" />
		protected string MemberSyntax
		{
			get
			{
				return this._memberSyntax;
			}
			set
			{
				this._memberSyntax = value;
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="OptionSet"/> associated with this transform.
		/// </summary>
		/// <value>A <see cref="OptionSet"/> with rendering options.</value>
		/// <seealso cref="CR_Documentor.Transformation.TransformEngine" />
		public OptionSet Options
		{
			get
			{
				return _options;
			}
			set
			{
				if (value != null)
				{
					_options = value;
					this.ValidateRegisteredHandlers();
				}
			}
		}

		/// <summary>
		/// Gets the writer that HTML should be set on when generating previews.
		/// </summary>
		/// <value>
		/// A <see cref="System.IO.StringWriter"/> that will be written to by various methods.
		/// </value>
		/// <remarks>
		/// <para>
		/// When <see cref="CR_Documentor.Transformation.TransformEngine.ToString"/>
		/// is called, the contents of this writer are returned.  Methods that process
		/// tags should write their HTML to this writer.
		/// </para>
		/// </remarks>
		/// <seealso cref="CR_Documentor.Transformation.TransformEngine" />
		protected StringWriter Writer
		{
			get
			{
				if (_writer == null)
				{
					_writer = new StringWriter(CultureInfo.InvariantCulture);
				}
				return this._writer;
			}
			set
			{
				_writer = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CR_Documentor.Transformation.TransformEngine" /> class.
		/// </summary>
		/// <seealso cref="CR_Documentor.Transformation.TransformEngine" />
		protected TransformEngine()
		{
			this.CommentMatchHandlers = new Dictionary<string, Action<XmlElement>>();
			this.RegisterCommentTagHandlers();
		}

		/// <summary>
		/// Gets the base HTML document that the browser preview window will
		/// be instantiated with.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> containing a complete HTML document including
		/// CSS, script, etc., that will be the initial document used in the
		/// browser preview window.
		/// </returns>
		/// <remarks>
		/// <para>
		/// Only the body of the document will be substituted out when the preview
		/// renders; the rest of the document should contain any supporting items
		/// required for proper preview.
		/// </para>
		/// </remarks>
		/// <seealso cref="CR_Documentor.Transformation.TransformEngine" />
		protected abstract string GetHtmlPageTemplate();

		/// <summary>
		/// Retrieves an HTML page based on the base transform engine template with
		/// the body substituted in the correct place.
		/// </summary>
		/// <param name="body">The body content to place in the page.</param>
		/// <returns>
		/// A <see cref="System.String"/> containing a complete HTML document including
		/// CSS, script, etc., to be rendered based on the base HTML template.
		/// </returns>
		/// <seealso cref="CR_Documentor.Transformation.TransformEngine" />
		/// <seealso cref="CR_Documentor.Transformation.TransformEngine.GetHtmlPageTemplate" />
		public virtual string GetHtmlPage(string body)
		{
			return this.GetHtmlPageTemplate().Replace(BodyReplacementMacro, body);
		}

		/// <summary>
		/// Registers all tag handlers with the rendering event system.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Derived classes should use this to register all of the known tag handlers
		/// with the event system so when it comes time to render the comment,
		/// all recognized tags will properly raise the appropriate handler.
		/// </para>
		/// <para>
		/// At a minimum, you should register a handler for the <c>member</c> tag
		/// as this is the root of the document and will always be called.
		/// </para>
		/// </remarks>
		/// <seealso cref="CR_Documentor.Transformation.TransformEngine" />
		protected abstract void RegisterCommentTagHandlers();

		/// <summary>
		/// Executes the translation process and creates a string with the final HTML for
		/// the rendered help page.
		/// </summary>
		/// <returns>A <see cref="System.String"/> with the complete HTML for preview.</returns>
		/// <seealso cref="CR_Documentor.Transformation.TransformEngine" />
		public override string ToString()
		{
			string body;
			try
			{
				this.Writer = new StringWriter(CultureInfo.InvariantCulture);
				this.Writer.Write("<body>");
				this.ApplyTemplates(this.Document, "member");
				this.Writer.Write("</body>");
				body = this.Writer.ToString();
			}
			catch (Exception err)
			{
				body = err.Message;
			}
			return this.GetHtmlPage(body);
		}

		/// <summary>
		/// Applies the transformation to the passed in set of <see cref="System.Xml.XmlNode"/> objects.
		/// </summary>
		/// <param name="list">The nodes to apply the transform to.</param>
		/// <remarks>
		/// <para>
		/// For each node in the list, if it's an <see cref="System.Xml.XmlElement"/>
		/// and is recognized in the set of <see cref="CR_Documentor.Options.OptionSet.RecognizedTags"/>
		/// in <see cref="CR_Documentor.Transformation.TransformEngine.Options"/>
		/// and is registered with a handler (<see cref="CR_Documentor.Transformation.TransformEngine.CommentMatchHandlers"/>),
		/// the handler will be called with the element.
		/// </para>
		/// <para>
		/// Register a handler using the key <see cref="System.String.Empty"/>
		/// to provide a default handler for tags that are recognized but don't
		/// have a specific handler registered.
		/// </para>
		/// </remarks>
		protected void ApplyTemplates(XmlNodeList list)
		{
			foreach (XmlNode node in list)
			{
				XmlElement element = node as XmlElement;
				if (element != null)
				{
					if (this.Options.RecognizedTags.Contains(element.Name))
					{
						// The tag is recognized - call the appropriate handler
						Action<XmlElement> handler = null;
						if (!this.CommentMatchHandlers.TryGetValue(element.Name, out handler))
						{
							if (!this.CommentMatchHandlers.TryGetValue("", out handler))
							{
								Log.Write(LogLevel.Warn, String.Format("No handler found for tag '{0}' found. Verify there is a default tag handler registered.", element.Name));
							}
						}
						handler(element);
					}
					else if (CommentParser.IsErrorNode(element))
					{
						// Display errors
						this.Writer.Write("<p><font color=\"red\"><b>");
						this.ApplyTemplates(element);
						this.Writer.Write("</b></font></p>");
					}
					else
					{
						// We don't recognize the tag and it's not an error
						this.UnrecognizedTag(element);
					}
				}
				else
				{
					// It's a text node - write the contents.
					TextProcessor.TextNode(this.Writer, node);
				}
			}
		}

		/// <summary>
		/// Applies the transformation to the children of a <see cref="System.Xml.XmlNode"/>.
		/// </summary>
		/// <param name="parent">
		/// The <see cref="System.Xml.XmlNode"/> whose <see cref="System.Xml.XmlNode.ChildNodes"/>
		/// you want to process.
		/// </param>
		/// <seealso cref="CR_Documentor.Transformation.TransformEngine" />
		protected void ApplyTemplates(XmlNode parent)
		{
			if (parent == null || !parent.HasChildNodes)
			{
				return;
			}
			this.ApplyTemplates(parent.ChildNodes);
		}

		/// <summary>
		/// Applies the transformation to a set of <see cref="System.Xml.XmlNode"/> objects
		/// selected by XPath.
		/// </summary>
		/// <param name="parent">
		/// The root <see cref="System.Xml.XmlNode"/> to select children of via XPath.
		/// </param>
		/// <param name="xpath">
		/// The XPath query to execute on the parent node.
		/// </param>
		/// <seealso cref="CR_Documentor.Transformation.TransformEngine" />
		protected void ApplyTemplates(XmlNode parent, string xpath)
		{
			if (parent == null)
			{
				return;
			}
			this.ApplyTemplates(parent.SelectNodes(xpath));
		}

		/// <summary>
		/// Generates a unique signature path for a language element.
		/// </summary>
		/// <param name="element">The language element to build a signature for.</param>
		/// <returns>A unique signature path that works with overloaded method names.</returns>
		private string GenerateLanguageElementSignature(SP.LanguageElement element)
		{
			if (element == null)
			{
				return null;
			}
			SP.Member member = element as SP.Member;
			StringBuilder sigBuilder = new StringBuilder(member == null ? element.ParentPath + "/" + element.PathSegment : member.Signature);
			SP.MemberWithParameters withParams = element as SP.MemberWithParameters;
			if (withParams != null)
			{
				sigBuilder.Append("(");
				foreach (SP.Param parameter in withParams.AllParameters)
				{
					sigBuilder.Append(parameter.FullTypeName);
					sigBuilder.Append(",");
				}
				sigBuilder.Append(")");
			}
			return sigBuilder.ToString();
		}

		/// <summary>
		/// Passes through HTML comment tags.
		/// </summary>
		/// <param name="element">
		/// The <see cref="System.Xml.XmlElement"/> to process.
		/// </param>
		protected void HtmlPassThrough(XmlElement element)
		{
			this.Writer.Write("<");
			this.Writer.Write(element.Name);
			TextProcessor.AttributePassThrough(this.Writer, element);
			if (element.InnerXml == "")
			{
				// If the tag is empty (like "<br />") just close it
				// Otherwise we get odd things like "<br><br>"
				this.Writer.Write("/>");
			}
			else
			{
				this.Writer.Write(">");
				this.ApplyTemplates(element);
				this.Writer.Write("</" + element.Name + ">");
			}
		}

		/// <summary>
		/// Ignores the comment match and renders nothing.
		/// </summary>
		/// <param name="element">
		/// The <see cref="System.Xml.XmlElement"/> to process.
		/// </param>
		protected void IgnoreComment(XmlElement element)
		{
			// Do nothing - the comment is being ignored.
		}

		/// <summary>
		/// Renders the "summary" node for a given language element. Useful when rendering tables
		/// of type members.
		/// </summary>
		/// <param name="element">The member element to render the summary information for.</param>
		/// <remarks>
		/// <para>
		/// If <paramref name="element" /> is <see langword="null" /> or if there
		/// is no summary information, this method renders a non-breaking space.
		/// </para>
		/// </remarks>
		protected void RenderElementSummary(SP.LanguageElement element)
		{
			SP.CodeElement codeElement = element as SP.CodeElement;
			if (codeElement == null || !codeElement.CanBeDocumented)
			{
				this.Writer.Write("&nbsp;");
				return;
			}

			SP.XmlDocComment comment = codeElement.DocComment;
			if (comment != null)
			{
				System.Xml.XmlDocument document = CommentParser.ParseXmlCommentToXmlDocument(comment, "Parse error at line {0}, character {1} of source document.  XML Parse Error Message: {2}", "Error: {0}");
				this.ApplyTemplates(document.DocumentElement, "summary");
			}
			else
			{
				this.Writer.Write("&nbsp;");
			}
		}

		/// <summary>
		/// Handles unrecognized tags.
		/// </summary>
		/// <param name="element">The unrecognized tag</param>
		/// <remarks>
		/// <para>
		/// Based on the selected option for handling unrecognized tags, the
		/// default behavior is as follows:
		/// </para>
		/// <list type="table">
		/// <listheader>
		/// <term>Option</term>
		/// <description>Effect</description>
		/// </listheader>
		/// <item>
		/// <term><see cref="CR_Documentor.Options.UnrecognizedTagHandlingMethod.HideTagAndContents"/></term>
		/// <description>No action.</description>
		/// </item>
		/// <item>
		/// <term><see cref="CR_Documentor.Options.UnrecognizedTagHandlingMethod.StripTagShowContents"/></term>
		/// <description>The tag is ignored but the contents are processed.</description>
		/// </item>
		/// <item>
		/// <term><see cref="CR_Documentor.Options.UnrecognizedTagHandlingMethod.HighlightTagAndContents"/></term>
		/// <description>The tag and contents are encoded and wrapped in a span with a class "unknowntag."  Highlighting can be done through CSS.</description>
		/// </item>
		/// <item>
		/// <term><see cref="CR_Documentor.Options.UnrecognizedTagHandlingMethod.RenderContents"/></term>
		/// <description>The content is rendered as it is with internal tags processed.</description>
		/// </item>
		/// </list>
		/// </remarks>
		/// <seealso cref="CR_Documentor.Transformation.TransformEngine" />
		protected void UnrecognizedTag(XmlElement element)
		{
			switch (this.Options.UnrecognizedTagHandlingMethod)
			{
				case UnrecognizedTagHandlingMethod.HideTagAndContents:
					// Don't do anything - eat the tag and everything in it.
					break;
				case UnrecognizedTagHandlingMethod.StripTagShowContents:
					// Don't do anything with the tag, but render the contents.
					this.ApplyTemplates(element);
					break;
				case UnrecognizedTagHandlingMethod.HighlightTagAndContents:
					// Write out the highlight and the encoded version
					// of the tag contents.
					this.Writer.Write("<span class='unknowntag'>");
					this.Writer.Write(SecurityElement.Escape(element.OuterXml));
					this.Writer.Write("</span>");
					break;
				case UnrecognizedTagHandlingMethod.RenderContents:
					// Write out the tag and the contents.
					this.Writer.Write("<");
					this.Writer.Write(element.Name);
					TextProcessor.AttributePassThrough(this.Writer, element);
					if (element.InnerXml == "")
					{
						// If the tag is empty (like "<br />") just close it
						// Otherwise we get odd things like "<br><br>"
						this.Writer.Write("/>");
					}
					else
					{
						this.Writer.Write(">");
						this.ApplyTemplates(element);
						this.Writer.Write("</" + element.Name + ">");
					}
					break;
			}
		}

		/// <summary>
		/// Reconciles the selected options with the set of registered tag handlers.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This method runs when the set of <see cref="CR_Documentor.Transformation.TransformEngine.Options"/>
		/// changes. It checks to see if there is a tag handler for every supported
		/// tag in the list and logs a warning if not. Helpful in debugging why
		/// something didn't render correctly.
		/// </para>
		/// </remarks>
		private void ValidateRegisteredHandlers()
		{
			List<String> defaulted = new List<string>();
			foreach (string tag in this.Options.RecognizedTags)
			{
				if (!this.CommentMatchHandlers.ContainsKey(tag))
				{
					defaulted.Add(tag);
				}
			}
			if (defaulted.Count == 0)
			{
				Log.Write(LogLevel.Info, "All recognized tags have handlers.");
			}
			else
			{
				Log.Write(LogLevel.Info, String.Format("The following recognized tags will be passed through or are handled implicitly: {0}", String.Join(", ", defaulted.ToArray())));
			}
		}
	}
}
