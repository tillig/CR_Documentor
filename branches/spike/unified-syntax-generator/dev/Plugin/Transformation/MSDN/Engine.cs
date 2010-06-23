using System;
using System.Web;
using System.Xml;

using CR_Documentor.Reflector;
using CR_Documentor.Transformation.Syntax;
using CR_Documentor.Xml;

using SP = DevExpress.CodeRush.StructuralParser;

namespace CR_Documentor.Transformation.MSDN
{
	/// <summary>
	/// Renders documentation in the MSDN document style.
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
		protected const string ResourceBaseHtmlDocument = "CR_Documentor.Transformation.MSDN.BaseDocument.html";

		#endregion


		#region Engine Implementation

		#region Overrides

		/// <summary>
		/// Gets the base HTML document for the MSDN transform engine.
		/// </summary>
		/// <returns>The complete HTML required for initializing a browser to use the MSDN transform engine.</returns>
		protected override string GetHtmlPageTemplate()
		{
			if (this._baseHtml == null)
			{
				// Lazy-initialize the base HTML. Doesn't matter if it's thread-safe.
				this._baseHtml = AssemblyExtensions.ReadEmbeddedResourceString(System.Reflection.Assembly.GetExecutingAssembly(), ResourceBaseHtmlDocument);
			}
			return this._baseHtml;
		}

		/// <summary>
		/// Registers all tag handlers with the rendering event system.
		/// </summary>
		/// <seealso cref="CR_Documentor.Transformation.TransformEngine.RegisterCommentTagHandlers"/>
		protected override void RegisterCommentTagHandlers()
		{
			// TODO: Factor this into the base class - decorate handlers with attributes so we don't have to register manually.
			this.AddCommentTagHandler(DefaultCommentHandlerKey, new EventHandler<CommentMatchEventArgs>(this.HtmlPassThrough));
			this.AddCommentTagHandler("block", new EventHandler<CommentMatchEventArgs>(this.Block));
			this.AddCommentTagHandler("c", new EventHandler<CommentMatchEventArgs>(this.C));
			this.AddCommentTagHandler("code", new EventHandler<CommentMatchEventArgs>(this.Code));
			this.AddCommentTagHandler("event", new EventHandler<CommentMatchEventArgs>(this.Event));
			this.AddCommentTagHandler("exception", new EventHandler<CommentMatchEventArgs>(this.Exception));
			this.AddCommentTagHandler("exclude", new EventHandler<CommentMatchEventArgs>(this.IgnoreComment));
			this.AddCommentTagHandler("example", new EventHandler<CommentMatchEventArgs>(this.ApplyTemplates));
			this.AddCommentTagHandler("include", new EventHandler<CommentMatchEventArgs>(this.Include));
			this.AddCommentTagHandler("list", new EventHandler<CommentMatchEventArgs>(this.List));
			this.AddCommentTagHandler("member", new EventHandler<CommentMatchEventArgs>(this.Member));
			this.AddCommentTagHandler("note", new EventHandler<CommentMatchEventArgs>(this.Note));
			this.AddCommentTagHandler("obsolete", new EventHandler<CommentMatchEventArgs>(this.ApplyTemplates));
			this.AddCommentTagHandler("overloads", new EventHandler<CommentMatchEventArgs>(this.IgnoreComment));
			this.AddCommentTagHandler("para", new EventHandler<CommentMatchEventArgs>(this.Para));
			this.AddCommentTagHandler("param", new EventHandler<CommentMatchEventArgs>(this.Param));
			this.AddCommentTagHandler("paramref", new EventHandler<CommentMatchEventArgs>(this.Paramref));
			this.AddCommentTagHandler("permission", new EventHandler<CommentMatchEventArgs>(this.Permission));
			this.AddCommentTagHandler("preliminary", new EventHandler<CommentMatchEventArgs>(this.Preliminary));
			this.AddCommentTagHandler("remarks", new EventHandler<CommentMatchEventArgs>(this.Remarks));
			this.AddCommentTagHandler("returns", new EventHandler<CommentMatchEventArgs>(this.Returns));
			this.AddCommentTagHandler("see", new EventHandler<CommentMatchEventArgs>(this.See));
			this.AddCommentTagHandler("seealso", new EventHandler<CommentMatchEventArgs>(this.SeeAlso));
			this.AddCommentTagHandler("summary", new EventHandler<CommentMatchEventArgs>(this.ApplyTemplates));
			this.AddCommentTagHandler("threadsafety", new EventHandler<CommentMatchEventArgs>(this.ThreadSafety));
			this.AddCommentTagHandler("typeparam", new EventHandler<CommentMatchEventArgs>(this.TypeParam));
			this.AddCommentTagHandler("typeparamref", new EventHandler<CommentMatchEventArgs>(this.TypeParamref));
			this.AddCommentTagHandler("value", new EventHandler<CommentMatchEventArgs>(this.Value));
		}

		#endregion

		#region Methods

		#region Tags

		/// <summary>
		/// Matches and processes a 'block' element.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="CR_Documentor.Transformation.CommentMatchEventArgs"/> instance containing the event data.</param>
		private void Block(object sender, CommentMatchEventArgs e)
		{
			this.Writer.Write("<p>");
			string blockType = Evaluator.ValueOf(e.Element, "@type");
			switch (blockType)
			{
				case "note":
					this.Writer.Write("<b>Note: </b>");
					break;
				case "example":
					this.Writer.Write("<b>For example: </b>");
					break;
				case "behaviors":
					this.Writer.Write("<h4 class=\".dtH4\">Operation</h4>");
					break;
				case "overrides":
					this.Writer.Write("<h4 class=\".dtH4\">Note to Inheritors</h4>");
					break;
				case "usage":
					this.Writer.Write("<h4 class=\".dtH4\">Usage</h4>");
					break;
				case "default":
					this.Writer.Write("<h4 class=\".dtH4\">Default</h4>");
					break;
				default:
					// Do nothing in the default case.
					break;
			}
			this.ApplyTemplates(e.Element);
			this.Writer.Write("</p>");
		}

		/// <summary>
		/// Matches and processes a 'c' element.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="CR_Documentor.Transformation.CommentMatchEventArgs"/> instance containing the event data.</param>
		private void C(object sender, CommentMatchEventArgs e)
		{
			this.Writer.Write("<code>");
			this.ApplyTemplates(e.Element);
			this.Writer.Write("</code>");
		}

		/// <summary>
		/// Matches and processes a 'code' element.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="CR_Documentor.Transformation.CommentMatchEventArgs"/> instance containing the event data.</param>
		private void Code(object sender, CommentMatchEventArgs e)
		{
			this.Writer.Write("<pre class='code'>");
			if (Evaluator.Test(e.Element, "@lang"))
			{
				this.Writer.Write("<span class='lang'>[");
				this.Writer.Write(Evaluator.ValueOf(e.Element, "@lang"));
				this.Writer.Write("]</span>");
			}
			// TODO: Handle correct whitespace for the "escaped" attribute of the "code" element.
			string escaped = Evaluator.ValueOf(e.Element, "@escaped");
			string outputCode = "";
			if (escaped == "true")
			{
				outputCode = HttpUtility.HtmlEncode(e.Element.InnerXml);
			}
			else
			{
				outputCode = HttpUtility.HtmlEncode(e.Element.InnerText);
			}
			if (this.Options.ConvertCodeTabsToSpaces)
			{
				string replaceTab = new string(' ', this.Options.ConvertCodeTabsToSpacesNum);
				outputCode = outputCode.Replace("\t", replaceTab);
			}
			this.Writer.Write(outputCode);
			this.Writer.Write("</pre>");
		}

		/// <summary>
		/// Matches and processes an 'event' element.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="CR_Documentor.Transformation.CommentMatchEventArgs"/> instance containing the event data.</param>
		private void Event(object sender, CommentMatchEventArgs e)
		{
			this.StandardTableRow(e.Element);
		}

		/// <summary>
		/// Matches and processes an 'exception' element.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="CR_Documentor.Transformation.CommentMatchEventArgs"/> instance containing the event data.</param>
		private void Exception(object sender, CommentMatchEventArgs e)
		{
			this.StandardTableRow(e.Element);
		}

		/// <summary>
		/// Matches and processes an 'include' element.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="CR_Documentor.Transformation.CommentMatchEventArgs"/> instance containing the event data.</param>
		private void Include(object sender, CommentMatchEventArgs e)
		{
			// If includes haven't been processed out, put a placeholder
			this.Writer.Write("<i><b>[Insert documentation here: file = ");
			this.Writer.Write(Evaluator.ValueOf(e.Element, "@file"));
			this.Writer.Write(", path = ");
			this.Writer.Write(Evaluator.ValueOf(e.Element, "@path"));
			this.Writer.Write("]</b></i>");
		}

		/// <summary>
		/// Matches and processes a 'list' element.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="CR_Documentor.Transformation.CommentMatchEventArgs"/> instance containing the event data.</param>
		private void List(object sender, CommentMatchEventArgs e)
		{
			XmlElement itemTerm = null;
			XmlElement itemDescription = null;

			switch (Evaluator.ValueOf(e.Element, "@type"))
			{
				case "table":
					this.Writer.Write("<div class='tablediv'><table cellspacing='0' class='dtTABLE'>");

					foreach (XmlNode listHeader in e.Element.SelectNodes("listheader"))
					{
						itemTerm = listHeader.SelectSingleNode("term") as XmlElement;
						itemDescription = listHeader.SelectSingleNode("description") as XmlElement;
						if (itemTerm != null || itemDescription != null)
						{
							this.Writer.Write("<tr valign='top'><th width='50%'>");
							if (itemTerm != null)
							{
								this.ApplyTemplates(itemTerm);
								if (itemDescription != null)
								{
									this.Writer.Write("</th><th width='50%'>");
								}
							}
							if (itemDescription != null)
							{
								this.ApplyTemplates(itemDescription);
							}

							this.Writer.Write("</th></tr>");
						}
					}

					foreach (XmlNode item in e.Element.SelectNodes("item"))
					{
						itemTerm = item.SelectSingleNode("term") as XmlElement;
						itemDescription = item.SelectSingleNode("description") as XmlElement;
						if (itemTerm != null || itemDescription != null)
						{
							this.Writer.Write("<tr valign='top'><td width='50%'>");
							if (itemTerm != null)
							{
								this.ApplyTemplates(itemTerm);
								if (itemDescription != null)
								{
									this.Writer.Write("</td><td width='50%'>");
								}
							}
							if (itemDescription != null)
							{
								this.ApplyTemplates(itemDescription);
							}

							this.Writer.Write("</td></tr>");
						}
					}

					this.Writer.Write("</table></div>");
					break;

				case "bullet":
					this.Writer.Write("<ul type='disc'>");
					foreach (XmlNode item in e.Element.SelectNodes("item"))
					{
						itemTerm = item.SelectSingleNode("term") as XmlElement;
						itemDescription = item.SelectSingleNode("description") as XmlElement;
						if (itemTerm != null || itemDescription != null)
						{
							this.Writer.Write("<li>");
							if (itemTerm != null)
							{
								if (itemDescription != null)
								{
									this.Writer.Write("<b>");
								}
								this.ApplyTemplates(itemTerm);
								if (itemDescription != null)
								{
									this.Writer.Write(" - </b>");
								}
							}
							if (itemDescription != null)
							{
								this.ApplyTemplates(itemDescription);
							}
							this.Writer.Write("</li>");
						}
					}
					this.Writer.Write("</ul>");
					break;

				case "number":
					this.Writer.Write("<ol type='1'>");
					foreach (XmlNode item in e.Element.SelectNodes("item"))
					{
						itemTerm = item.SelectSingleNode("term") as XmlElement;
						itemDescription = item.SelectSingleNode("description") as XmlElement;
						if (itemTerm != null || itemDescription != null)
						{
							this.Writer.Write("<li>");
							if (itemTerm != null)
							{
								if (itemDescription != null)
								{
									this.Writer.Write("<b>");
								}
								this.ApplyTemplates(itemTerm);
								if (itemDescription != null)
								{
									this.Writer.Write(" - </b>");
								}
							}
							if (itemDescription != null)
							{
								this.ApplyTemplates(itemDescription);
							}
							this.Writer.Write("</li>");
						}
					}
					this.Writer.Write("</ol>");
					break;

				case "definition":
					this.Writer.Write("<dl>");
					foreach (XmlNode item in e.Element.SelectNodes("item"))
					{
						itemTerm = item.SelectSingleNode("term") as XmlElement;
						itemDescription = item.SelectSingleNode("description") as XmlElement;
						if (itemTerm != null || itemDescription != null)
						{
							this.Writer.Write("<item>");
							if (itemTerm != null)
							{
								this.Writer.Write("<dt>");
								this.ApplyTemplates(itemTerm);
								this.Writer.Write("</dt>");
							}
							if (itemDescription != null)
							{
								this.Writer.Write("<dd>");
								this.ApplyTemplates(itemDescription);
								this.Writer.Write("</dd>");
							}
							this.Writer.Write("</item>");
						}
					}
					this.Writer.Write("</dl>");
					break;
			}
		}

		/// <summary>
		/// Matches and processes the root 'member' element in a document.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="CR_Documentor.Transformation.CommentMatchEventArgs"/> instance containing the event data.</param>
		private void Member(object sender, CommentMatchEventArgs e)
		{
			// Stick the banner at the top
			this.Banner();

			// Start the text area
			this.Writer.Write("<div id='nstext'>");

			// Errors (at top level)
			this.ApplyTemplates(CommentParser.GetChildErrorNodes(e.Element));

			// Preliminary
			if (this.Options.ProcessDuplicateSeeLinks)
			{
				LinkProcessor.RemoveDuplicates(e.Element, "preliminary");
			}
			this.ApplyTemplates(e.Element, "preliminary");

			// Obsolete
			if (this.Options.RecognizedTags.Contains("obsolete") && e.Element.GetElementsByTagName("obsolete").Count != 0)
			{
				if (this.Options.ProcessDuplicateSeeLinks)
				{
					LinkProcessor.RemoveDuplicates(e.Element, "obsolete");
				}
				this.Writer.Write("<p><font color=\"red\"><b>NOTE: This class is now obsolete.</b></font></p>");
				this.ApplyTemplates(e.Element, "obsolete");
				this.Writer.Write("<hr />");
			}

			// Summary
			if (this.Options.ProcessDuplicateSeeLinks)
			{
				LinkProcessor.RemoveDuplicates(e.Element, "summary");
			}
			this.ApplyTemplates(e.Element, "summary");

			// Object Signature
			this.Syntax();

			// TypeParam
			if (this.Options.RecognizedTags.Contains("typeparam") && e.Element.GetElementsByTagName("typeparam").Count != 0)
			{
				if (this.Options.ProcessDuplicateSeeLinks)
				{
					LinkProcessor.RemoveDuplicates(e.Element, "typeparam");
				}
				this.Writer.Write("<h4 class='dtH4'>Generic Template Parameters</h4>");
				this.Writer.Write("<dl>");
				this.ApplyTemplates(e.Element, "typeparam");
				this.Writer.Write("</dl>");
			}

			// Param
			if (this.Options.RecognizedTags.Contains("param") && e.Element.GetElementsByTagName("param").Count != 0)
			{
				if (this.Options.ProcessDuplicateSeeLinks)
				{
					LinkProcessor.RemoveDuplicates(e.Element, "param");
				}
				this.Writer.Write("<h4 class='dtH4'>Parameters</h4>");
				this.Writer.Write("<dl>");
				this.ApplyTemplates(e.Element, "param");
				this.Writer.Write("</dl>");
			}

			// Returns
			if (this.Options.ProcessDuplicateSeeLinks)
			{
				LinkProcessor.RemoveDuplicates(e.Element, "returns");
			}
			this.ApplyTemplates(e.Element, "returns");

			// Value
			if (this.Options.ProcessDuplicateSeeLinks)
			{
				LinkProcessor.RemoveDuplicates(e.Element, "value");
			}
			this.ApplyTemplates(e.Element, "value");

			// Threadsafety
			if (this.Options.ProcessDuplicateSeeLinks)
			{
				LinkProcessor.RemoveDuplicates(e.Element, "threadsafety");
			}
			this.ApplyTemplates(e.Element, "threadsafety");

			// Remarks
			if (this.Options.ProcessDuplicateSeeLinks)
			{
				LinkProcessor.RemoveDuplicates(e.Element, "remarks");
			}
			this.ApplyTemplates(e.Element, "remarks");

			// Event
			if (this.Options.RecognizedTags.Contains("event") && e.Element.GetElementsByTagName("event").Count != 0)
			{
				if (this.Options.ProcessDuplicateSeeLinks)
				{
					LinkProcessor.RemoveDuplicates(e.Element, "event");
				}
				this.Writer.Write("<h4 class='dtH4'>Events</h4>");
				this.Writer.Write("<div class='tablediv'><table cellspacing='0' class='dtTABLE'>");
				this.Writer.Write("<tr valign='top'><th width='50%'>Event Type</th><th width='50%'>Reason</th></tr>");
				this.ApplyTemplates(e.Element, "event");
				this.Writer.Write("</table></div>");
			}

			// Exception
			if (this.Options.RecognizedTags.Contains("exception") && e.Element.GetElementsByTagName("exception").Count != 0)
			{
				if (this.Options.ProcessDuplicateSeeLinks)
				{
					LinkProcessor.RemoveDuplicates(e.Element, "exception");
				}
				this.Writer.Write("<h4 class='dtH4'>Exceptions</h4>");
				this.Writer.Write("<div class='tablediv'><table cellspacing='0' class='dtTABLE'>");
				this.Writer.Write("<tr valign='top'><th width='50%'>Exception Type</th><th width='50%'>Condition</th></tr>");
				this.ApplyTemplates(e.Element, "exception");
				this.Writer.Write("</table></div>");
			}

			// Example
			if (this.Options.RecognizedTags.Contains("example") && e.Element.GetElementsByTagName("example").Count != 0)
			{
				if (this.Options.ProcessDuplicateSeeLinks)
				{
					LinkProcessor.RemoveDuplicates(e.Element, "example");
				}
				this.Writer.Write("<h4 class='dtH4'>Example</h4>");
				this.ApplyTemplates(e.Element, "example");
			}

			// List enumeration members
			this.EnumMembers();

			// Permission
			if (this.Options.RecognizedTags.Contains("permission") && e.Element.GetElementsByTagName("permission").Count != 0)
			{
				if (this.Options.ProcessDuplicateSeeLinks)
				{
					LinkProcessor.RemoveDuplicates(e.Element, "permission");
				}
				this.Writer.Write("<h4 class='dtH4'>Requirements</h4><p><b>.NET Framework Security:</b>");
				this.Writer.Write("<ul>");
				this.ApplyTemplates(e.Element, "permission");
				this.Writer.Write("</ul></p>");

			}

			// Seealso
			if (this.Options.RecognizedTags.Contains("seealso") && e.Element.GetElementsByTagName("seealso").Count != 0)
			{
				if (this.Options.ProcessDuplicateSeeLinks)
				{
					LinkProcessor.RemoveDuplicates(e.Element, "seealso");
				}
				this.Writer.Write("<h4 class='dtH4'>See Also</h4>");
				this.ApplyTemplates(e.Element, "seealso");
			}

			// Include (at top level)
			if (this.Options.RecognizedTags.Contains("include") && e.Element.GetElementsByTagName("include").Count != 0)
			{
				if (this.Options.ProcessDuplicateSeeLinks)
				{
					LinkProcessor.RemoveDuplicates(e.Element, "include");
				}
				this.ApplyTemplates(e.Element, "include");
			}

			// End the text area
			this.Writer.Write("</div>");

			this.Writer.Write("<br/><br/>");
		}

		/// <summary>
		/// Matches and processes a 'note' element.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="CR_Documentor.Transformation.CommentMatchEventArgs"/> instance containing the event data.</param>
		private void Note(object sender, CommentMatchEventArgs e)
		{
			this.Writer.Write("<blockquote class='dtBlock'");
			TextProcessor.AttributePassThrough(this.Writer, e.Element, new string[] { "style", "align" });
			this.Writer.Write(">");
			switch (Evaluator.ValueOf(e.Element, "@type"))
			{
				case "":
					this.Writer.Write("<b>Note</b>   ");
					this.ApplyTemplates(e.Element);
					break;

				case "caution":
					this.Writer.Write("<b>CAUTION</b>   ");
					this.ApplyTemplates(e.Element);
					break;

				case "inheritinfo":
					this.Writer.Write("<b>Notes to Inheritors: </b>   ");
					this.ApplyTemplates(e.Element);
					break;

				case "implementnotes":
					this.Writer.Write("<b>Notes to Implementers: </b>   ");
					this.ApplyTemplates(e.Element);
					break;
			}
			this.Writer.Write("</blockquote>");
		}

		/// <summary>
		/// Matches and processes a 'para' element.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="CR_Documentor.Transformation.CommentMatchEventArgs"/> instance containing the event data.</param>
		private void Para(object sender, CommentMatchEventArgs e)
		{
			this.Writer.Write("<p");
			TextProcessor.AttributePassThrough(this.Writer, e.Element, new string[] { "style", "align" });
			this.Writer.Write(">");
			if (Evaluator.Test(e.Element, "@lang"))
			{
				this.Writer.Write("<span class='lang'>[");
				this.Writer.Write(Evaluator.ValueOf(e.Element, "@lang"));
				this.Writer.Write("]</span> ");
			}
			this.ApplyTemplates(e.Element);
			this.Writer.Write("</p>");
		}

		/// <summary>
		/// Matches and processes a 'param' element.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="CR_Documentor.Transformation.CommentMatchEventArgs"/> instance containing the event data.</param>
		private void Param(object sender, CommentMatchEventArgs e)
		{
			this.Writer.Write("<dt><i>");
			this.Writer.Write(Evaluator.ValueOf(e.Element, "@name"));
			this.Writer.Write("</i></dt>");
			this.Writer.Write("<dd>");
			this.ApplyTemplates(e.Element);
			this.Writer.Write("</dd>");
		}

		/// <summary>
		/// Matches and processes a 'paramref' element.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="CR_Documentor.Transformation.CommentMatchEventArgs"/> instance containing the event data.</param>
		private void Paramref(object sender, CommentMatchEventArgs e)
		{
			this.Writer.Write("<i>");
			this.Writer.Write(Evaluator.ValueOf(e.Element, "@name"));
			this.Writer.Write("</i>");
		}

		/// <summary>
		/// Matches and processes a 'permission' element.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="CR_Documentor.Transformation.CommentMatchEventArgs"/> instance containing the event data.</param>
		private void Permission(object sender, CommentMatchEventArgs e)
		{
			this.Writer.Write("<li><a href='urn:member:");
			this.Writer.Write(Evaluator.ValueOf(e.Element, "@cref"));
			this.Writer.Write("' title='");
			this.Writer.Write(MemberKey.GetFullName(Evaluator.ValueOf(e.Element, "@cref")));
			this.Writer.Write("'>");
			this.Writer.Write(MemberKey.GetName(Evaluator.ValueOf(e.Element, "@cref")));
			this.Writer.Write("</a> ");
			this.ApplyTemplates(e.Element);
			this.Writer.Write("</li>");
		}

		/// <summary>
		/// Matches and processes a 'preliminary' element.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="CR_Documentor.Transformation.CommentMatchEventArgs"/> instance containing the event data.</param>
		private void Preliminary(object sender, CommentMatchEventArgs e)
		{
			this.Writer.Write("<p class='topicstatus'>");
			string message = HttpUtility.HtmlEncode(e.Element.InnerText);
			if (message == "")
			{
				message = "[This is preliminary documentation and subject to change.]";
			}
			this.Writer.Write(message);
			this.Writer.Write("</p>");
		}

		/// <summary>
		/// Matches and processes a 'remarks' element.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="CR_Documentor.Transformation.CommentMatchEventArgs"/> instance containing the event data.</param>
		private void Remarks(object sender, CommentMatchEventArgs e)
		{
			this.Writer.Write("<h4 class='dtH4'>Remarks</h4>");
			this.ApplyTemplates(e.Element);
		}

		/// <summary>
		/// Matches and processes a 'returns' element.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="CR_Documentor.Transformation.CommentMatchEventArgs"/> instance containing the event data.</param>
		private void Returns(object sender, CommentMatchEventArgs e)
		{
			this.Writer.Write("<h4 class='dtH4'>Return Value</h4>");
			this.ApplyTemplates(e.Element);
		}

		/// <summary>
		/// Matches and processes a 'see' element.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="CR_Documentor.Transformation.CommentMatchEventArgs"/> instance containing the event data.</param>
		private void See(object sender, CommentMatchEventArgs e)
		{
			if (Evaluator.Test(e.Element, "@langword"))
			{
				string langword = Evaluator.ValueOf(e.Element, "@langword");
				switch (langword)
				{
					case "abstract":
						this.Writer.Write("abstract (<b>MustInherit</b> in Visual Basic)");
						break;
					case "null":
						this.Writer.Write("a null reference (<b>Nothing</b> in Visual Basic)");
						break;
					case "sealed":
						this.Writer.Write("sealed (<b>NotInheritable</b> in Visual Basic)");
						break;
					case "static":
						this.Writer.Write("static (<b>Shared</b> in Visual Basic)");
						break;
					case "virtual":
						this.Writer.Write("virtual (<b>CanOverride</b> in Visual Basic)");
						break;
					case "bold":
					case "false":
					case "true":
						this.Writer.Write(String.Format("<b>{0}</b>", langword));
						break;
					default:
						// Do nothing - not a valid langword
						break;
				}
			}
			else if (Evaluator.Test(e.Element, "@cref"))
			{
				string cref = Evaluator.ValueOf(e.Element, "@cref");
				string text = e.Element.InnerText;
				if (text != "")
				{
					text = HttpUtility.HtmlEncode(text);
				}
				else
				{
					text = this.ProcessCrefTypeParameters(MemberKey.GetName(cref));
				}
				if (Evaluator.Test(e.Element, "@nolink"))
				{
					this.Writer.Write("<b>");
					this.Writer.Write(text);
					this.Writer.Write("</b>");
				}
				else
				{
					this.Writer.Write("<a href='urn:member:");
					this.Writer.Write(cref);
					this.Writer.Write("' title='");
					this.Writer.Write(MemberKey.GetFullName(cref));
					this.Writer.Write("'>");
					this.Writer.Write(text);
					this.Writer.Write("</a>");
				}
			}
			else if (Evaluator.Test(e.Element, "@href"))
			{
				this.Writer.Write("<a href='");
				string href = Evaluator.ValueOf(e.Element, "@href");
				this.Writer.Write(href);
				this.Writer.Write("' title='");
				this.Writer.Write(href);
				this.Writer.Write("'>");
				string text = e.Element.InnerText;
				if (text != "")
				{
					this.Writer.Write(HttpUtility.HtmlEncode(text));
				}
				else
				{
					this.Writer.Write(HttpUtility.HtmlEncode(href));
				}
				this.Writer.Write("</a>");
			}
		}

		/// <summary>
		/// Matches and processes a 'seealso' element.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="CR_Documentor.Transformation.CommentMatchEventArgs"/> instance containing the event data.</param>
		private void SeeAlso(object sender, CommentMatchEventArgs e)
		{
			if (Evaluator.Test(e.Element, "@cref"))
			{
				this.Writer.Write("<a href='");
				this.Writer.Write("urn:member:" + Evaluator.ValueOf(e.Element, "@cref"));
				this.Writer.Write("' title='");
				this.Writer.Write(MemberKey.GetFullName(Evaluator.ValueOf(e.Element, "@cref")));
				this.Writer.Write("'>");
				string text = e.Element.InnerText;
				if (text != "")
				{
					this.Writer.Write(HttpUtility.HtmlEncode(text));
				}
				else
				{
					this.Writer.Write(this.ProcessCrefTypeParameters(MemberKey.GetName(Evaluator.ValueOf(e.Element, "@cref"))));
				}
				this.Writer.Write("</a>");
			}
			else if (Evaluator.Test(e.Element, "@href"))
			{
				this.Writer.Write("<a href='");
				string href = Evaluator.ValueOf(e.Element, "@href");
				this.Writer.Write(href);
				this.Writer.Write("' title='");
				this.Writer.Write(href);
				this.Writer.Write("'>");
				string text = e.Element.InnerText;
				if (text != "")
				{
					this.Writer.Write(HttpUtility.HtmlEncode(text));
				}
				else
				{
					this.Writer.Write(HttpUtility.HtmlEncode(href));
				}
				this.Writer.Write("</a>");
			}

			XmlNodeList list = e.Element.ParentNode.SelectNodes("seealso");
			for (int i = 0; i < list.Count - 1; i++)
			{
				if (list[i] == e.Element)
				{
					this.Writer.Write(" | ");
					break;
				}
			}
		}

		/// <summary>
		/// Matches and processes a 'threadsafety' element.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="CR_Documentor.Transformation.CommentMatchEventArgs"/> instance containing the event data.</param>
		private void ThreadSafety(object sender, CommentMatchEventArgs e)
		{
			this.Writer.Write("<h4 class='dtH4'>Thread Safety</h4><p>");
			string tsStatic = Evaluator.ValueOf(e.Element, "@static");
			string tsInstance = Evaluator.ValueOf(e.Element, "@instance");
			bool staticSafe = false;
			bool instanceSafe = false;
			if (tsStatic != null && tsStatic == "true")
			{
				staticSafe = true;
			}
			if (tsInstance != null && tsInstance == "true")
			{
				instanceSafe = true;
			}

			if (!instanceSafe && !staticSafe)
			{
				this.Writer.Write("This type is <b>not</b> safe for multithreaded operations.");
			}
			else if (!instanceSafe && staticSafe)
			{
				this.Writer.Write("Public static (<b>Shared</b> in Visual Basic) members of this type are safe for multithreaded operations. Instance members are <b>not</b> guaranteed to be thread-safe.");
			}
			else if (instanceSafe && !staticSafe)
			{
				this.Writer.Write("Public static (<b>Shared</b> in Visual Basic) members of this type are <b>not</b> guaranteed to be safe for multithreaded operations. Instance members are thread-safe.");
			}
			else
			{
				this.Writer.Write("This type is safe for multithreaded operations.");
			}

			this.Writer.Write("</p>");

			this.ApplyTemplates(e.Element);
		}

		/// <summary>
		/// Matches and processes a 'typeparam' element.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="CR_Documentor.Transformation.CommentMatchEventArgs"/> instance containing the event data.</param>
		private void TypeParam(object sender, CommentMatchEventArgs e)
		{
			this.Writer.Write("<dt><i>");
			this.Writer.Write(Evaluator.ValueOf(e.Element, "@name"));
			this.Writer.Write("</i></dt>");
			this.Writer.Write("<dd>");
			this.ApplyTemplates(e.Element);
			this.Writer.Write("</dd>");
		}

		/// <summary>
		/// Matches and processes a 'typeparamref' element.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="CR_Documentor.Transformation.CommentMatchEventArgs"/> instance containing the event data.</param>
		private void TypeParamref(object sender, CommentMatchEventArgs e)
		{
			this.Writer.Write("<i>");
			this.Writer.Write(Evaluator.ValueOf(e.Element, "@name"));
			this.Writer.Write("</i>");
		}

		/// <summary>
		/// Matches and processes a 'value' element.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="CR_Documentor.Transformation.CommentMatchEventArgs"/> instance containing the event data.</param>
		private void Value(object sender, CommentMatchEventArgs e)
		{
			this.Writer.Write("<h4 class='dtH4'>Value</h4>");
			this.ApplyTemplates(e.Element);
		}

		#endregion

		#region Helpers

		/// <summary>
		/// Produces the banner at the top of the preview
		/// </summary>
		private void Banner()
		{
			// Start banner
			this.Writer.Write("<div id='nsbanner'><div id='TitleRow'><h1 class='dtH1'>");

			// Output the member description
			SP.AccessSpecifiedElement el = this.CodeTargetToRender;
			if (el == null)
			{
				// The element can't be documented; put an empty header
				this.Writer.Write("&nbsp;");
			}
			else
			{
				// The first part of the header is the name of the element
				if (el is SP.Method)
				{
					this.Writer.Write(Lookup.MethodName((SP.Method)el));
				}
				else
				{
					this.Writer.Write(el.Name);
				}
				this.Writer.Write(" ");

				// The second part of the header is the type of element
				this.Writer.Write(Lookup.ElementTypeDescription(el));
			}

			// End banner
			this.Writer.Write("</h1></div></div>");
		}

		/// <summary>
		/// Lists members of enumeration objects
		/// </summary>
		/// <remarks>
		/// <para>
		/// Gets the list of members and associated documentation and renders as needed.
		/// </para>
		/// </remarks>
		private void EnumMembers()
		{
			if (!(this.CodeTarget is SP.Enumeration) || this.CodeTarget.NodeCount < 1)
			{
				return;
			}
			SP.Enumeration enumeration = (SP.Enumeration)this.CodeTarget;
			this.Writer.Write("<h4 class='dtH4'>Members</h4><div class='tablediv'><table class='dtTABLE' cellspacing='0'><tr valign='top'><th width='50%'>Member Name</th><th width='50%'>Description</th></tr>");
			// TODO: Handle enumerations that are flags (add "Value" column after "Description" with 40/40/20 widths)
			foreach (object node in enumeration.Nodes)
			{
				if (!(node is SP.EnumElement))
				{
					continue;
				}
				SP.EnumElement element = (SP.EnumElement)node;
				this.Writer.Write("<tr valign='top'><td><b>");
				this.Writer.Write(element.Name);
				this.Writer.Write("</b></td><td>");
				this.RenderElementSummary(element);
				this.Writer.Write("</td></tr>");
			}
			this.Writer.Write("</table></div>\n");
		}

		/// <summary>
		/// Processes cref-style type parameters (e.g., List{T}) into language-specific
		/// HTML-safe strings.
		/// </summary>
		/// <param name="cref">The reference text to process.</param>
		/// <returns>An HTML-safe language specific expanded cref.</returns>
		private string ProcessCrefTypeParameters(string cref)
		{
			string language = this.CodeTarget.Document.Language;
			string open = HttpUtility.HtmlEncode(Statement.TypeParamListOpener[language]);
			string close = HttpUtility.HtmlEncode(Statement.TypeParamListCloser[language]);
			string processed = cref.Replace("{", open).Replace("}", close);
			return processed;
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
				System.IO.StringWriter syntaxWriter = new System.IO.StringWriter(System.Globalization.CultureInfo.InvariantCulture);
				SyntaxGenerator generator = new SyntaxGenerator(asElement, syntaxWriter);
				generator.GenerateSyntaxPreview();
				this.MemberSyntax = syntaxWriter.ToString();
			}
			this.Writer.Write(this.MemberSyntax);
		}

		/// <summary>
		/// Writes a standard table row for an exception, event, or other standard table.
		/// </summary>
		/// <param name="element">The element containing the information to write.</param>
		private void StandardTableRow(XmlElement element)
		{
			this.Writer.Write("<tr valign='top'><td width='50%'><a href='urn:member:");
			this.Writer.Write(Evaluator.ValueOf(element, "@cref"));
			this.Writer.Write("' title='");
			this.Writer.Write(MemberKey.GetFullName(Evaluator.ValueOf(element, "@cref")));
			this.Writer.Write("'>");
			this.Writer.Write(MemberKey.GetName(Evaluator.ValueOf(element, "@cref")));
			this.Writer.Write("</a></td><td width='50%'>");
			this.ApplyTemplates(element);
			this.Writer.Write("</td></tr>");
		}

		#endregion

		#endregion

		#endregion

	}
}
