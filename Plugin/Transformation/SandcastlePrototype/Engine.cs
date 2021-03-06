using System;
using System.Xml;
using System.Web;

using CR_Documentor.Reflector;
using CR_Documentor.Transformation.Syntax;
using CR_Documentor.Xml;

using SP = DevExpress.CodeRush.StructuralParser;

namespace CR_Documentor.Transformation.SandcastlePrototype
{
	/// <summary>
	/// Renders documentation in the Sandcastle "Prototype" document style.
	/// </summary>
	public class Engine : TransformEngine
	{
		#region Engine Variables

		/// <summary>
		/// Cached copy of the base HTML so it doesn't get retrieved repeatedly.
		/// </summary>
		private string _baseHtml = null;

		/// <summary>
		/// Path to the embedded resource containing the base HTML document for this transform engine.
		/// </summary>
		protected const string ResourceBaseHtmlDocument = "CR_Documentor.Transformation.SandcastlePrototype.BaseDocument.html";

		#endregion



		#region Engine Implementation

		#region Overrides

		/// <summary>
		/// Gets the base HTML document for the MSDN transform engine.
		/// </summary>
		/// <returns>The complete HTML required for initializing a browser to use the Sandcastle "prototype" transform engine.</returns>
		protected override string GetHtmlPageTemplate()
		{
			// TODO: Figure out how to handle images/assets that appear in the HTML. Extract to a temporary location?
			// TODO: When images/assets are available, pull the CSS and scripts out of the base HTML document.
			if (this._baseHtml == null)
			{
				// Lazy-initialize the base HTML. Doesn't matter if it's thread-safe.
				var asm = System.Reflection.Assembly.GetExecutingAssembly();
				this._baseHtml = asm.ReadEmbeddedResourceString(ResourceBaseHtmlDocument);
			}
			return this._baseHtml;
		}

		/// <summary>
		/// Registers all tag handlers with the rendering event system.
		/// </summary>
		/// <seealso cref="CR_Documentor.Transformation.TransformEngine.RegisterCommentTagHandlers"/>
		protected override void RegisterCommentTagHandlers()
		{
			this.CommentMatchHandlers[""] = this.HtmlPassThrough;
			this.CommentMatchHandlers["c"] = this.C;
			this.CommentMatchHandlers["code"] = this.Code;
			this.CommentMatchHandlers["example"] = this.ApplyTemplates;
			this.CommentMatchHandlers["exclude"] = this.IgnoreComment;
			this.CommentMatchHandlers["exception"] = this.Exception;
			this.CommentMatchHandlers["include"] = this.Include;
			this.CommentMatchHandlers["list"] = this.List;
			this.CommentMatchHandlers["member"] = this.Member;
			this.CommentMatchHandlers["note"] = this.Note;
			this.CommentMatchHandlers["overloads"] = this.IgnoreComment;
			this.CommentMatchHandlers["para"] = this.Para;
			this.CommentMatchHandlers["param"] = this.Param;
			this.CommentMatchHandlers["paramref"] = this.Paramref;
			this.CommentMatchHandlers["permission"] = this.Permission;
			this.CommentMatchHandlers["preliminary"] = this.Preliminary;
			this.CommentMatchHandlers["remarks"] = this.Remarks;
			this.CommentMatchHandlers["returns"] = this.Returns;
			this.CommentMatchHandlers["see"] = this.See;
			this.CommentMatchHandlers["seealso"] = this.SeeAlso;
			this.CommentMatchHandlers["summary"] = this.Summary;
			this.CommentMatchHandlers["threadsafety"] = this.ThreadSafety;
			this.CommentMatchHandlers["typeparam"] = this.TypeParam;
			this.CommentMatchHandlers["typeparamref"] = this.TypeParamref;
			this.CommentMatchHandlers["value"] = this.Value;
		}

		#endregion

		#region Methods

		#region Tags

		/// <summary>
		/// Matches the 'c' tag.
		/// </summary>
		/// <param name="element">
		/// The <see cref="System.Xml.XmlElement"/> to process.
		/// </param>
		private void C(XmlElement element)
		{
			this.Writer.Write("<span class=\"code\">");
			this.ApplyTemplates(element);
			this.Writer.Write("</span>");
		}

		/// <summary>
		/// Matches and processes a 'code' element.
		/// </summary>
		/// <param name="element">
		/// The <see cref="System.Xml.XmlElement"/> to process.
		/// </param>
		private void Code(XmlElement element)
		{
			this.Writer.Write("<div class=\"code\"><pre>");
			// Sandcastle doesn't support the lang attribute, the escaped attribute, or converting tabs to spaces
			this.Writer.Write(HttpUtility.HtmlEncode(element.InnerText));
			this.Writer.Write("</pre></div>");
		}

		/// <summary>
		/// Matches and processes an 'exception' element.
		/// </summary>
		/// <param name="element">
		/// The <see cref="System.Xml.XmlElement"/> to process.
		/// </param>
		private void Exception(XmlElement element)
		{
			this.StandardTableRow(element);
		}

		/// <summary>
		/// Matches and processes an 'include' element.
		/// </summary>
		/// <param name="element">
		/// The <see cref="System.Xml.XmlElement"/> to process.
		/// </param>
		private void Include(XmlElement element)
		{
			// If includes haven't been processed out, put a placeholder
			this.Writer.Write("<p><i><b>[Insert documentation here: file = ");
			this.Writer.Write(Evaluator.ValueOf(element, "@file"));
			this.Writer.Write(", path = ");
			this.Writer.Write(Evaluator.ValueOf(element, "@path"));
			this.Writer.Write("]</b></i></p>");
		}

		/// <summary>
		/// Matches and processes a 'list' element.
		/// </summary>
		/// <param name="element">
		/// The <see cref="System.Xml.XmlElement"/> to process.
		/// </param>
		private void List(XmlElement element)
		{
			switch (Evaluator.ValueOf(element, "@type"))
			{
				case "table":
					this.Writer.Write("<table class=\"authoredTable\">");

					foreach (XmlNode listHeader in element.SelectNodes("listheader"))
					{
						this.Writer.Write("<tr>");
						foreach (XmlNode column in listHeader.ChildNodes)
						{
							if (!(column is XmlElement))
							{
								continue;
							}
							this.Writer.Write("<th>");
							this.ApplyTemplates(column.ChildNodes);
							this.Writer.Write("</th>");
						}
						this.Writer.Write("</tr>");
					}

					foreach (XmlNode item in element.SelectNodes("item"))
					{
						this.Writer.Write("<tr>");
						foreach (XmlNode column in item.ChildNodes)
						{
							if (!(column is XmlElement))
							{
								continue;
							}
							this.Writer.Write("<td>");
							this.ApplyTemplates(column.ChildNodes);
							this.Writer.Write("<br/></td>");
						}
						this.Writer.Write("</tr>");
					}

					this.Writer.Write("</table>");
					break;

				case "bullet":
					this.Writer.Write("<ul>");
					foreach (XmlNode item in element.SelectNodes("item"))
					{
						this.Writer.Write("<li>");
						foreach (XmlNode itemChild in item.ChildNodes)
						{
							this.ApplyTemplates(itemChild);
						}
						this.Writer.Write("</li>");
					}
					this.Writer.Write("</ul>");
					break;

				case "number":
					this.Writer.Write("<ol>");
					foreach (XmlNode item in element.SelectNodes("item"))
					{
						this.Writer.Write("<li>");
						foreach (XmlNode itemChild in item.ChildNodes)
						{
							this.ApplyTemplates(itemChild);
						}
						this.Writer.Write("</li>");
					}
					this.Writer.Write("</ol>");
					break;
				default:
					this.Writer.Write(element.InnerText);
					break;
			}
		}

		/// <summary>
		/// Matches and processes the root 'member' element in a document.
		/// </summary>
		/// <param name="element">
		/// The <see cref="System.Xml.XmlElement"/> to process.
		/// </param>
		private void Member(XmlElement element)
		{
			// Note that Sandcastle doesn't process out redundant "see" links

			// Stick the banner at the top
			this.Banner();

			// Open the "main" (content) div.
			this.Writer.Write("<div id=\"main\"><div id=\"header\">This is experimental documentation.</div>");

			// Errors (at top level)
			this.ApplyTemplates(CommentParser.GetChildErrorNodes(element));

			// Preliminary
			this.ApplyTemplates(element, "preliminary");

			// Summary
			this.ApplyTemplates(element, "summary");

			// Syntax
			this.Syntax();

			// Type Parameters
			if (this.Options.RecognizedTags.Contains("typeparam") && element.GetElementsByTagName("typeparam").Count != 0)
			{
				this.SectionOpen("Generic Template Parameters");
				this.Writer.Write("<dl>");
				this.ApplyTemplates(element, "typeparam");
				this.Writer.Write("</dl>");
				this.SectionClose();
			}

			// Parameters
			if (this.Options.RecognizedTags.Contains("param") && element.GetElementsByTagName("param").Count != 0)
			{
				this.SectionOpen("Parameters");
				this.Writer.Write("<dl>");
				this.ApplyTemplates(element, "param");
				this.Writer.Write("</dl>");
				this.SectionClose();
			}

			// Value
			this.ApplyTemplates(element, "value");

			// Returns
			this.ApplyTemplates(element, "returns");

			// Members (Applies to type declarations including enum values)
			this.Members();

			// Remarks
			this.ApplyTemplates(element, "remarks");

			// Threadsafety
			this.ApplyTemplates(element, "threadsafety");

			// Example
			if (this.Options.RecognizedTags.Contains("example") && element.GetElementsByTagName("example").Count != 0)
			{
				this.SectionOpen("Examples");
				this.ApplyTemplates(element, "example");
				this.SectionClose();
			}

			// Permissions
			if (this.Options.RecognizedTags.Contains("permission") && element.GetElementsByTagName("permission").Count != 0)
			{
				this.SectionOpen("Permissions");
				this.Writer.Write("<table class=\"permissions\"><tr><th class=\"permissionNameColumn\">Permission</th><th class=\"permissionDescriptionColumn\">Description</th></tr>");
				this.ApplyTemplates(element, "permission");
				this.Writer.Write("</table>");
				this.SectionClose();
			}

			// Exceptions
			if (this.Options.RecognizedTags.Contains("exception") && element.GetElementsByTagName("exception").Count != 0)
			{
				this.SectionOpen("Exceptions");
				this.Writer.Write("<table class=\"exceptions\"><tr><th class=\"exceptionNameColumn\">Exception</th><th class=\"exceptionConditionColumn\">Condition</th></tr>");
				this.ApplyTemplates(element, "exception");
				this.Writer.Write("</table>");
				this.SectionClose();
			}

			// Usually classes would display "Inheritance Hierarchy" information here, like:
			// Object
			// +-SomeClass
			//   +-SomeDerivedClass
			// But that doesn't really add anything to a preview, so we'll omit it.

			// SeeAlso
			if (this.Options.RecognizedTags.Contains("seealso") && element.GetElementsByTagName("seealso").Count != 0)
			{
				this.SectionOpen("See Also");
				this.ApplyTemplates(element, "seealso");
				this.SectionClose();
			}

			// Usually "Assembly" information will be shown here, like:
			// Assembly: Foo (Module: Bar)
			// But that doesn't really add anything to a preview, so we'll omit it.

			// Close the "main" div.
			this.Writer.Write("</div>");

			// Activate the visible language elements.
			if (this.CodeTargetToRender != null)
			{
				string languageValue = null;
				if (Lookup.LanguageValue.TryGetValue(this.CodeTargetToRender.Document.Language, out languageValue))
				{
					this.Writer.Write("<script type=\"text/javascript\">var sd = getStyleDictionary();setLanguage('");
					this.Writer.Write(languageValue);
					this.Writer.Write("');</script>");
				}
			}
		}

		/// <summary>
		/// Matches and processes a 'note' element.
		/// </summary>
		/// <param name="element">
		/// The <see cref="System.Xml.XmlElement"/> to process.
		/// </param>
		private void Note(XmlElement element)
		{
			// Sandcastle doesn't support note "types" (inheritinfo, etc.). It's all just "note."
			this.Writer.Write("<div class=\"alert\"><img src=\"SandcastlePrototype.alert_note.gif\"/> <b>Note:</b>");
			this.ApplyTemplates(element);
			this.Writer.Write("</div>");
		}

		/// <summary>
		/// Matches and processes a 'para' element.
		/// </summary>
		/// <param name="element">
		/// The <see cref="System.Xml.XmlElement"/> to process.
		/// </param>
		private void Para(XmlElement element)
		{
			// Sandcastle doesn't support passing through style or align attributes.
			// Sandcastle doesn't support the lang attribute.
			this.Writer.Write("<p>");
			this.ApplyTemplates(element);
			this.Writer.Write("</p>");
		}

		/// <summary>
		/// Matches and processes a 'param' element.
		/// </summary>
		/// <param name="element">
		/// The <see cref="System.Xml.XmlElement"/> to process.
		/// </param>
		private void Param(XmlElement element)
		{
			string paramName = Evaluator.ValueOf(element, "@name");
			string paramType = this.ParamType(paramName);
			this.Writer.Write("<dt><span class=\"parameter\">");
			this.Writer.Write(paramName);
			if (paramType != null)
			{
				this.Writer.Write(" (<a href=\"#\">");
				this.Writer.Write(paramType);
				this.Writer.Write("</a>)");
			}
			this.Writer.Write("</span>");
			this.Writer.Write("</dt>");
			this.Writer.Write("<dd>");
			this.ApplyTemplates(element);
			this.Writer.Write("</dd>");
		}

		/// <summary>
		/// Matches and processes a 'paramref' element.
		/// </summary>
		/// <param name="element">
		/// The <see cref="System.Xml.XmlElement"/> to process.
		/// </param>
		private void Paramref(XmlElement element)
		{
			this.Writer.Write("<span class=\"parameter\">");
			this.Writer.Write(Evaluator.ValueOf(element, "@name"));
			this.Writer.Write("</span>");
		}

		/// <summary>
		/// Matches and processes a 'permission' element.
		/// </summary>
		/// <param name="element">
		/// The <see cref="System.Xml.XmlElement"/> to process.
		/// </param>
		private void Permission(XmlElement element)
		{
			this.StandardTableRow(element);
		}

		/// <summary>
		/// Matches and processes a 'preliminary' element.
		/// </summary>
		/// <param name="element">
		/// The <see cref="System.Xml.XmlElement"/> to process.
		/// </param>
		private void Preliminary(XmlElement element)
		{
			// December 2006 CTP does not pay attention to specified text for the preliminary message.
			this.Writer.Write("<div class=\"preliminary\">This API is preliminary and subject to change.</div>");
		}

		/// <summary>
		/// Matches and processes a 'remarks' element.
		/// </summary>
		/// <param name="element">
		/// The <see cref="System.Xml.XmlElement"/> to process.
		/// </param>
		private void Remarks(XmlElement element)
		{
			this.SectionOpen("Remarks");
			this.ApplyTemplates(element);
			this.SectionClose();
		}

		/// <summary>
		/// Matches and processes a 'returns' element.
		/// </summary>
		/// <param name="element">
		/// The <see cref="System.Xml.XmlElement"/> to process.
		/// </param>
		private void Returns(XmlElement element)
		{
			this.SectionOpen("Return Value");
			this.ApplyTemplates(element);
			this.SectionClose();
		}

		/// <summary>
		/// Matches and processes a 'see' element.
		/// </summary>
		/// <param name="element">
		/// The <see cref="System.Xml.XmlElement"/> to process.
		/// </param>
		private void See(XmlElement element)
		{
			if (Evaluator.Test(element, "@langword"))
			{
				string langword = Evaluator.ValueOf(element, "@langword");
				this.Writer.Write("<span class=\"keyword\">");
				switch (langword)
				{
					case "null":
					case "Nothing":
					case "nullptr":
						this.Writer.Write("<span class=\"cs\">null</span><span class=\"vb\">Nothing</span><span class=\"cpp\">nullptr</span>");
						break;
					case "static":
					case "Shared":
						this.Writer.Write("<span class=\"cs\">static</span><span class=\"vb\">Shared</span><span class=\"cpp\">static</span>");
						break;
					case "virtual":
					case "Overridable":
						this.Writer.Write("<span class=\"cs\">virtual</span><span class=\"vb\">Overridable</span><span class=\"cpp\">virtual</span>");
						break;
					case "True":
					case "true":
						this.Writer.Write("<span class=\"cs\">true</span><span class=\"vb\">True</span><span class=\"cpp\">true</span>");
						break;
					case "false":
					case "False":
						this.Writer.Write("<span class=\"cs\">false</span><span class=\"vb\">False</span><span class=\"cpp\">false</span>");
						break;
					case "abstract":
					case "MustInherit":
						this.Writer.Write("<span class=\"cs\">abstract</span><span class=\"vb\">MustInherit</span><span class=\"cpp\">abstract</span>");
						break;
					default:
						this.Writer.Write(langword);
						break;
				}
				this.Writer.Write("</span>");
			}
			else if (Evaluator.Test(element, "@cref"))
			{
				string cref = Evaluator.ValueOf(element, "@cref");
				string text = element.InnerText;
				if (!String.IsNullOrEmpty(text))
				{
					text = HttpUtility.HtmlEncode(text);
				}
				else
				{
					text = MemberKey.GetName(cref);
					// This handles the type parameters in the member name.
					text = text.Replace("{", "<span class=\"cs\">&lt;</span><span class=\"vb\">(Of </span><span class=\"cpp\">&lt;</span>");
					text = text.Replace("}", "<span class=\"cs\">&gt;</span><span class=\"vb\">)</span><span class=\"cpp\">&gt;</span>");
				}
				this.Writer.Write("<a href=\"urn:member:");
				this.Writer.Write(cref);
				this.Writer.Write("\">");
				this.Writer.Write(text);
				this.Writer.Write("</a>");
			}
			else
			{
				this.Writer.Write(HttpUtility.HtmlEncode(element.InnerText));
			}
		}

		/// <summary>
		/// Matches and processes a 'seealso' element.
		/// </summary>
		/// <param name="element">
		/// The <see cref="System.Xml.XmlElement"/> to process.
		/// </param>
		private void SeeAlso(XmlElement element)
		{
			string linkDestination = null;
			string text = element.InnerText;
			if (Evaluator.Test(element, "@cref"))
			{
				linkDestination = "urn:member:" + Evaluator.ValueOf(element, "@cref");
				if (text != "")
				{
					text = HttpUtility.HtmlEncode(text);
				}
				else
				{
					text = MemberKey.GetName(Evaluator.ValueOf(element, "@cref"));
					// This handles the type parameters in the member name.
					text = text.Replace("{", "<span class=\"cs\">&lt;</span><span class=\"vb\">(Of </span><span class=\"cpp\">&lt;</span>");
					text = text.Replace("}", "<span class=\"cs\">&gt;</span><span class=\"vb\">)</span><span class=\"cpp\">&gt;</span>");
				}
			}

			if (linkDestination != null)
			{
				this.Writer.Write("<a href=\"");
				this.Writer.Write(linkDestination);
				this.Writer.Write("\">");
			}
			else
			{
				this.Writer.Write("<span class=\"nolink\">");
			}

			this.Writer.Write(text);

			if (linkDestination != null)
			{
				this.Writer.Write("</a>");
			}
			else
			{
				this.Writer.Write("</span>");
			}

			this.Writer.Write("<br />");
		}

		/// <summary>
		/// Matches and processes a 'summary' element.
		/// </summary>
		/// <param name="element">
		/// The <see cref="System.Xml.XmlElement"/> to process.
		/// </param>
		private void Summary(XmlElement element)
		{
			this.Writer.Write("<div class=\"summary\">");
			this.ApplyTemplates(element);
			this.Writer.Write("</div>");
		}

		/// <summary>
		/// Matches and processes a 'remarks' element.
		/// </summary>
		/// <param name="element">
		/// The <see cref="System.Xml.XmlElement"/> to process.
		/// </param>
		private void ThreadSafety(XmlElement element)
		{
			this.SectionOpen("Thread Safety");

			bool staticSafe = false;
			bool instanceSafe = false;
			Boolean.TryParse(Evaluator.ValueOf(element, "@static"), out staticSafe);
			Boolean.TryParse(Evaluator.ValueOf(element, "@instance"), out instanceSafe);

			this.Writer.Write("Static members of this type are {0}safe for multi-threaded operations. ", staticSafe ? "" : "not ");
			this.Writer.Write("Instance members of this type are {0}safe for multi-threaded operations. ", instanceSafe ? "" : "not ");

			this.SectionClose();
		}

		/// <summary>
		/// Matches and processes a 'typeparam' element.
		/// </summary>
		/// <param name="element">
		/// The <see cref="System.Xml.XmlElement"/> to process.
		/// </param>
		private void TypeParam(XmlElement element)
		{
			string paramName = Evaluator.ValueOf(element, "@name");
			this.Writer.Write("<dt><span class=\"parameter\">");
			this.Writer.Write(paramName);
			this.Writer.Write("</span>");
			this.Writer.Write("</dt>");
			this.Writer.Write("<dd>");
			this.ApplyTemplates(element);
			this.Writer.Write("</dd>");
		}

		/// <summary>
		/// Matches and processes a 'typeparamref' element.
		/// </summary>
		/// <param name="element">
		/// The <see cref="System.Xml.XmlElement"/> to process.
		/// </param>
		private void TypeParamref(XmlElement element)
		{
			this.Writer.Write("<span class=\"typeparameter\">");
			this.Writer.Write(Evaluator.ValueOf(element, "@name"));
			this.Writer.Write("</span>");
		}

		/// <summary>
		/// Matches and processes a 'value' element.
		/// </summary>
		/// <param name="element">
		/// The <see cref="System.Xml.XmlElement"/> to process.
		/// </param>
		private void Value(XmlElement element)
		{
			this.SectionOpen("Value");
			this.ApplyTemplates(element);
			this.SectionClose();
		}

		#endregion

		#region Helpers

		/// <summary>
		/// Produces the banner at the top of the preview
		/// </summary>
		private void Banner()
		{
			this.Writer.Write("<script type=\"text/javascript\">registerEventHandler(window, 'load', function() { var ss = new SplitScreen('control', 'main'); });</script>");
			this.Writer.Write("<div id=\"control\">");
			this.Writer.Write("<span class=\"productTitle\">Reference Library</span><br />");

			// Output the member description/title
			this.Writer.Write("<span class=\"topicTitle\">");
			SP.AccessSpecifiedElement el = this.CodeTargetToRender;
			if (el == null)
			{
				// The element can't be documented; put an empty title
				this.Writer.Write("&nbsp;");
			}
			else
			{
				// The first part of the header is the name of the element
				if (el is SP.Method)
				{
					this.Writer.Write(((SP.Method)el).DisplayName());
				}
				else
				{
					this.Writer.Write(el.Name);
				}
				this.Writer.Write(" ");

				// The second part of the header is the type of element
				this.Writer.Write(el.ElementTypeDescription());
			}
			this.Writer.Write("</span><br />"); // topicTitle

			// Output the 'toolbar'
			this.Writer.Write("<div id=\"toolbar\">");
			this.Writer.Write("<span id=\"chickenFeet\">");
			if (el == null)
			{
				this.Writer.Write("&nbsp;");
			}
			else
			{
				this.Writer.Write("<a href=\"#\">Namespaces</a>");
				SP.Namespace memberNamespace = el.GetNamespace();
				if (memberNamespace != null)
				{
					this.Writer.Write(" &#x25ba; <a href=\"#\">");
					this.Writer.Write(memberNamespace.FullName);
					this.Writer.Write("</a> ");
				}
				this.Writer.Write("&#x25ba; ");
				if (el is SP.Member)
				{
					// Write the parent's name before the item name
					this.Writer.Write("<a href=\"#\">");
					this.Writer.Write(el.Parent.Name);
					this.Writer.Write("</a> &#x25ba; ");
				}
				this.Writer.Write("<span class=\"nolink\">");
				this.Writer.Write(el.Name);
				this.Writer.Write("</span>"); // nolink
			}
			this.Writer.Write("</span>"); // chickenFeet
			this.Writer.Write("<span id=\"languageFilter\">");
			this.Writer.Write("<select id=\"languageSelector\">");
			if (el == null)
			{
				this.Writer.Write("<option value=\"x\">--</option>");
			}
			else
			{
				string languageOption = "x";
				string languageName = "--";
				Lookup.LanguageValue.TryGetValue(el.Document.Language, out languageOption);
				Lookup.LanguageName.TryGetValue(el.Document.Language, out languageName);
				this.Writer.Write("<option value=\"");
				this.Writer.Write(languageOption);
				this.Writer.Write("\">");
				this.Writer.Write(HttpUtility.HtmlEncode(languageName));
				this.Writer.Write("</option>");
			}
			this.Writer.Write("</select>");
			this.Writer.Write("</span>"); // languageFilter
			this.Writer.Write("</div>"); // toolbar
			this.Writer.Write("</div>"); // control
		}

		/// <summary>
		/// Writes out the "Members" section for type declarations.
		/// </summary>
		private void Members()
		{
			SP.Class targetClass = this.CodeTarget as SP.Class;
			SP.Enumeration targetEnum = this.CodeTarget as SP.Enumeration;
			if ((targetClass == null && targetEnum == null) || this.CodeTarget.NodeCount < 1)
			{
				return;
			}
			this.SectionOpen("Members");
			if (targetClass != null)
			{
				this.Writer.Write("<table class=\"filter\"><tr class=\"tabs\" id=\"memberTabs\">");
				this.Writer.Write("<td class=\"tab\" value=\"all\">All Members</td><td class=\"tab\" value=\"constructor\">Constructors</td><td class=\"tab\" value=\"method\">Methods</td><td class=\"tab\" value=\"property\">Properties</td><td class=\"tab\" value=\"field\">Fields</td><td class=\"tab\" value=\"event\">Events</td>");
				this.Writer.Write("</tr><tr>");
				this.Writer.Write("<td class=\"line\" colspan=\"2\"><label for=\"public\"><input id=\"public\" type=\"checkbox\" checked=\"true\" disabled=\"true\" />Public</label><br /><label for=\"protected\"><input id=\"protected\" type=\"checkbox\" checked=\"true\" disabled=\"true\" />Protected</label></td>");
				this.Writer.Write("<td class=\"line\" colspan=\"2\"><label for=\"instance\"><input id=\"instance\" type=\"checkbox\" checked=\"true\" disabled=\"true\" />Instance</label><br /><label for=\"static\"><input id=\"static\" type=\"checkbox\" checked=\"true\" disabled=\"true\" />Static</label></td>");
				this.Writer.Write("<td class=\"line\" colspan=\"2\"><label for=\"declared\"><input id=\"declared\" type=\"checkbox\" checked=\"true\" disabled=\"true\" />Declared</label><br /><label for=\"inherited\"><input id=\"inherited\" type=\"checkbox\" checked=\"true\" disabled=\"true\" />Inherited</label></td>");
				this.Writer.Write("</tr></table>");
			}
			this.Writer.Write("<table class=\"members\" id=\"memberList\"><tr>");
			if (targetClass != null)
			{
				this.Writer.Write("<th class=\"iconColumn\">Icon</th>");
			}
			this.Writer.Write("<th class=\"nameColumn\">Member</th><th class=\"descriptionColumn\">Description</th></tr>");
			if (targetEnum != null)
			{
				// Write out the enumeration members
				foreach (object node in targetEnum.Nodes)
				{
					SP.EnumElement element = node as SP.EnumElement;
					if (element == null)
					{
						continue;
					}
					this.Writer.Write("<tr><td><b><span class=\"selflink\">");
					this.Writer.Write(element.Name);
					this.Writer.Write("</span></b></td><td>");
					this.RenderElementSummary(element);
					this.Writer.Write("<br /></td></tr>");
				}
			}
			else if (targetClass != null)
			{
				var language = targetClass.Document.Language.ToLanguageId();
				foreach (SP.LanguageElement classMember in targetClass.AllMembers)
				{
					var ase = classMember as SP.AccessSpecifiedElement;
					if (ase == null || ase is SP.DelegateDefinition || ase.Visibility == SP.MemberVisibility.Private || ase.Visibility == SP.MemberVisibility.Internal)
					{
						// Delegate definitions, private members, and internal members
						// don't show up in top-level class doc.
						continue;
					}
					this.Writer.Write("<tr><td>");
					this.MemberIcon(ase);
					this.Writer.Write("</td><td><a href=\"#\">");
					var generator = new SimpleSignatureGenerator(ase, language);
					this.Writer.Write(HttpUtility.HtmlEncode(generator.Generate()));
					this.Writer.Write("</a></td><td>");
					this.RenderElementSummary(ase);
					this.Writer.Write("</td></tr>");
				}
			}
			this.Writer.Write("</table>");
			this.SectionClose();
		}

		/// <summary>
		/// Writes the icon HTML for a given class member.
		/// </summary>
		/// <param name="classMember">The member for which the icon HTML should be written.</param>
		private void MemberIcon(SP.AccessSpecifiedElement classMember)
		{
			if (classMember == null)
			{
				this.Writer.Write("&nbsp;");
				return;
			}
			string name = "SandcastlePrototype.";
			switch (classMember.Visibility)
			{
				case SP.MemberVisibility.Public:
				case SP.MemberVisibility.Published:
					name += "pub";
					break;
				case SP.MemberVisibility.Protected:
				case SP.MemberVisibility.ProtectedFriend:
				case SP.MemberVisibility.ProtectedInternal:
					name += "prot";
					break;
				case SP.MemberVisibility.Private:
					name += "priv";
					break;
				default:
					// We don't recognize it or it otherwise doesn't have an icon.
					this.Writer.Write("&nbsp;");
					return;
			}
			if (classMember is SP.Enumeration)
			{
				name += "enum.gif";
			}
			else if (classMember is SP.Class)
			{
				if (classMember is SP.Interface)
				{
					name += "interface.gif";
				}
				else if (classMember is SP.Struct)
				{
					name += "structure.gif";
				}
				else
				{
					name += "class.gif";
				}
			}
			else if (classMember is SP.DelegateDefinition)
			{
				name += "delegate.gif";
			}
			else if (classMember is SP.Method)
			{
				if (((SP.Method)classMember).IsClassOperator)
				{
					name += "operator.gif";
				}
				else
				{
					name += "method.gif";
				}
			}
			else if (classMember is SP.Property)
			{
				name += "property.gif";
			}
			else if (classMember is SP.Event)
			{
				name += "event.gif";
			}
			else if (classMember is SP.BaseVariable)
			{
				name += "field.gif";
			}

			this.Writer.Write("<img src=\"");
			this.Writer.Write(name);
			this.Writer.Write("\" />");

			if (classMember.IsConst || classMember.IsStatic)
			{
				this.Writer.Write("<img src=\"SandcastlePrototype.static.gif\" />");
			}
		}

		/// <summary>
		/// Retrieves the type name for a parameter on the current element being documented.
		/// </summary>
		/// <param name="paramName">The name of the parameter to look up the type for.</param>
		/// <returns>
		/// A <see cref="System.String"/> containing the parameter's type name,
		/// or <see langword="null" /> if not found.
		/// </returns>
		/// <remarks>
		/// <para>
		/// If <paramref name="paramName" /> is <see langword="null" /> or
		/// <see cref="System.String.Empty"/>, this method returns <see langword="null" />.
		/// </para>
		/// <para>
		/// If the current code target (<see cref="CR_Documentor.Transformation.TransformEngine.CodeTargetToRender"/>)
		/// is not a <see cref="DevExpress.CodeRush.StructuralParser.MemberWithParameters"/>
		/// or if the code target has no parameters, this method returns <see langword="null" />.
		/// </para>
		/// </remarks>
		private string ParamType(string paramName)
		{
			if (paramName == null || paramName.Length == 0)
			{
				return null;
			}
			SP.MemberWithParameters mwpElement = this.CodeTargetToRender as SP.MemberWithParameters;
			if (mwpElement == null || mwpElement.ParameterCount == 0)
			{
				return null;
			}
			SP.LanguageElementCollection parameters = mwpElement.Parameters;
			foreach (SP.LanguageElement parameter in parameters)
			{
				if (parameter.Name == paramName)
				{
					return System.Web.HttpUtility.HtmlEncode(((SP.Param)parameter).ParamType);
				}
			}
			return null;
		}

		/// <summary>
		/// Opens a standard section block with a title.
		/// </summary>
		/// <param name="sectionTitle">The title of the section block.</param>
		private void SectionOpen(string sectionTitle)
		{
			this.Writer.Write("<div class=\"section\"><div class=\"sectionTitle\">");
			this.Writer.Write(sectionTitle);
			this.Writer.Write("</div><div class=\"sectionContent\">");
		}

		/// <summary>
		/// Closes a standard section block.
		/// </summary>
		private void SectionClose()
		{
			this.Writer.Write("</div></div>");
		}

		/// <summary>
		/// Writes a standard table row for an exception, event, or other standard table.
		/// </summary>
		/// <param name="element">The element containing the information to write.</param>
		private void StandardTableRow(XmlElement element)
		{
			this.Writer.Write("<tr><td><a href=\"urn:member:");
			this.Writer.Write(Evaluator.ValueOf(element, "@cref"));
			this.Writer.Write("\">");
			this.Writer.Write(MemberKey.GetName(Evaluator.ValueOf(element, "@cref")));
			this.Writer.Write("</a></td><td>");
			this.ApplyTemplates(element);
			this.Writer.Write("</td></tr>");
		}

		/// <summary>
		/// Produces the object signature preview.
		/// </summary>
		/// <remarks>
		/// <para>
		/// If the current code target (<see cref="CR_Documentor.Transformation.TransformEngine.CodeTargetToRender"/>)
		/// is not an <see cref="DevExpress.CodeRush.StructuralParser.AccessSpecifiedElement"/>,
		/// this method exits.
		/// </para>
		/// <para>
		/// Otherwise, the member syntax signature cache is checked.  If the cache is empty,
		/// the member syntax signature is generated.  If it's not, generation is skipped
		/// as the currently active member has already been "rendered."  The member syntax
		/// signature is then written to the output document.
		/// </para>
		/// </remarks>
		private void Syntax()
		{
			// If we don't have a target or if the target can't be documented, we
			// Everything that has a signature has an access specifier; don't try
			// to render anything that doesn't
			SP.AccessSpecifiedElement asElement = this.CodeTargetToRender;
			if (asElement == null)
			{
				return;
			}

			if (this.MemberSyntax == null)
			{
				// Refresh the member syntax signature cache if it's empty
				PreviewGenerator generator = new PreviewGenerator(asElement, asElement.Document.Language.ToLanguageId());
				this.MemberSyntax = generator.Generate();
			}
			this.SectionOpen("Declaration Syntax");
			this.Writer.Write(this.MemberSyntax);
			this.SectionClose();
		}

		#endregion

		#endregion

		#endregion

	}
}
