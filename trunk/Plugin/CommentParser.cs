using System;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Xml;

using CR_Documentor.Xml;

using DevExpress.CodeRush.Core;

using SP = DevExpress.CodeRush.StructuralParser;

namespace CR_Documentor
{
	/// <summary>
	/// Provides <see langword="static" /> methods for common language functions.
	/// </summary>
	public static class CommentParser
	{

		#region CommentParser Variables

		#region Constants

		/// <summary>
		/// The name of an element containing an error message to be displayed.
		/// </summary>
		private const string ErrorNodeElementName = "CR_DOCUMENTOR_ERROR";

		#endregion

		#endregion



		#region CommentParser Properties

		/// <summary>
		/// Gets the XML document comment prefix used for the current language.
		/// </summary>
		/// <value>A <see cref="System.String"/> with the doc comment prefix.</value>
		private static string XmlCommentPrefix
		{
			get
			{
				LanguageExtensionBase lActiveLanguage = CodeRush.Language.ActiveExtension;
				string retVal = "/// ";
				if (lActiveLanguage != null)
				{
					retVal = lActiveLanguage.XMLDocCommentBegin + " ";
				}
				return retVal;
			}
		}

		#endregion



		#region CommentParser Implementation

		#region Methods

		/// <summary>
		/// Creates a document that only contains a single error node with a message.
		/// </summary>
		/// <param name="message">The error message the error node will contain.</param>
		/// <returns>A <see cref="System.Xml.XmlDocument"/> that is only a single error node.</returns>
		public static XmlDocument CreateErrorDocument(string message)
		{
			XmlDocument doc = new XmlDocument();
			doc.AppendChild(doc.CreateElement("member"));
			doc.DocumentElement.AppendChild(CreateErrorNode(doc, message));
			return doc;
		}

		/// <summary>
		/// Creates a node that will be displayed as an error.
		/// </summary>
		/// <param name="parentDoc">The document that will contain the node.</param>
		/// <param name="message">The message that will be displayed.</param>
		/// <returns>
		/// An <see cref="System.Xml.XmlElement"/> that can be inserted into a document
		/// and be rendered as an error.
		/// </returns>
		public static XmlElement CreateErrorNode(XmlDocument parentDoc, string message)
		{
			XmlElement retVal = parentDoc.CreateElement(ErrorNodeElementName);
			retVal.InnerText = message;
			return retVal;
		}

		/// <summary>
		/// Fixes up a comment line for reading into an XML document.
		/// </summary>
		/// <param name="lineToFixup">The line to fix up.</param>
		/// <param name="prefix">The XML doc comment prefix.</param>
		/// <param name="prefixTrim">A trimmed version of the XML doc comment prefix.</param>
		/// <returns>A fixed version of the comment line.</returns>
		/// <remarks>
		/// The <paramref name="prefix" /> and <paramref name="prefixTrim" /> get passed
		/// in so we don't have to recalculate them for every line.
		/// </remarks>
		private static string FixupCommentLine(string lineToFixup, string prefix, string prefixTrim)
		{
			string line = lineToFixup.TrimStart(new char[] { ' ', '\t' });

			if (line.StartsWith(prefix))
			{
				line = line.Substring(prefix.Length);
			}
			else if (line.StartsWith(prefixTrim))
			{
				line = line.Substring(prefixTrim.Length);
			}

			// This fixes a weird bug where the comment is
			// returned with two tabs at the beginning that replaces
			// a single tab.  We'll trim the leading tab off.
			if (line.StartsWith("\t\t"))
			{
				line = line.Substring(1);
			}

			return line;
		}

		/// <summary>
		/// Gets the set of error nodes inside a given node.
		/// </summary>
		/// <param name="parent">The <see cref="System.Xml.XmlNode"/> that contains errors.</param>
		/// <returns>
		/// The set of immediate children that are error nodes.
		/// </returns>
		/// <exception cref="System.ArgumentNullException">
		/// Thrown if <paramref name="parent" /> is <see langword="null" />.
		/// </exception>
		public static XmlNodeList GetChildErrorNodes(XmlNode parent)
		{
			if (parent == null)
			{
				throw new ArgumentNullException("parent");
			}
			return parent.SelectNodes(ErrorNodeElementName);
		}

		/// <summary>
		/// Gets an XML Doc Comment from a specified LanguageElement. If the LanguageElement
		/// is an XmlNode, that node's parents are checked until and XmlDocComment is found.
		/// If the specified LanguageElement is not an XML Doc Comment, and it's not parented
		/// by an XmlDocComment, then this method returns null.
		/// </summary>
		/// <param name="element">The LanguageElement to check.</param>
		/// <returns>An XmlDocComment if found; null otherwise.</returns>
		public static SP.XmlDocComment GetXmlDocComment(SP.LanguageElement element)
		{
			while (element is SP.XmlNode)
			{
				element = element.Parent;
			}
			SP.XmlDocComment retVal = element as SP.XmlDocComment;
			return retVal;
		}

		/// <summary>
		/// Determines if a given node is an error node.
		/// </summary>
		/// <param name="node">The node to check.</param>
		/// <returns>
		/// <see langword="true" /> if the <paramref name="node" />
		/// is an error node; <see langword="false" /> if not.
		/// </returns>
		/// <exception cref="System.ArgumentNullException">
		/// Thrown if <paramref name="node" /> is <see langword="null" />.
		/// </exception>
		public static bool IsErrorNode(XmlNode node)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			return node.Name == ErrorNodeElementName;
		}

		/// <summary>
		/// Parses through the XML comment in preparation for display.
		/// </summary>
		/// <param name="comment">The comment to parse.</param>
		/// <returns>An XML fragment with the comment, minus any line prefix.</returns>
		private static string ParseDocumentationToXmlString(string comment)
		{
			// Get the comment prefix (and trimmed version)
			string prefix = CommentParser.XmlCommentPrefix;
			string prefixTrim = prefix.Trim();

			// Fix up line breaks
			comment = comment.Replace("\r\n", "\n");

			// Process individual lines
			string[] lines = comment.Split(new char[] { '\n' });

			StringWriter writer = new StringWriter();
			writer.Write("<member>");
			foreach (string l in lines)
			{
				string line = FixupCommentLine(l, prefix, prefixTrim);
				writer.WriteLine(line);
			}
			writer.Write("</member>");

			return writer.ToString();
		}

		/// <summary>
		/// Converts an XML documentation comment to an XML document.
		/// </summary>
		/// <param name="comment">The comment to convert to an XML document.</param>
		/// <param name="parseErrorMsg">A string used in formatting parse errors.  {0} is line number, {1} is character position, {2} is error message.</param>
		/// <param name="generalErrorMsg">A string used in formatting general errors.  {0} is error message.</param>
		/// <returns>An XML document with the parsed comment.</returns>
		public static XmlDocument ParseXmlCommentToXmlDocument(SP.XmlDocComment comment, string parseErrorMsg, string generalErrorMsg)
		{
			try
			{
				XmlDocument document = new XmlDocument();
				document.LoadXml(CommentParser.ParseDocumentationToXmlString(comment.ToString()));
				return document;
			}
			catch (XmlException err)
			{
				// Get the line number of the source document having issues
				int errLineNumber = err.LineNumber + comment.StartLine - 1;

				// Calculate the indent
				string errLine = comment.Document.GetText(errLineNumber);

				// Get the comment prefix (and trimmed version)
				string prefix = CommentParser.XmlCommentPrefix;
				string prefixTrim = prefix.Trim();

				// Get the fixed up comment line (as was read into the XML)
				string fixedLine = CommentParser.FixupCommentLine(errLine, prefix, prefixTrim);

				// Calculate the difference in length - this will be the indent size
				int indent = errLine.Length - fixedLine.Length;

				// Get the line position of the error
				int errLinePosition = err.LinePosition + indent;

				string errMsg = String.Format(parseErrorMsg, errLineNumber, errLinePosition, err.Message);
				return CreateErrorDocument(errMsg);
			}
			catch (Exception err)
			{
				string errMsg = String.Format(generalErrorMsg, err.Message);
				return CreateErrorDocument(errMsg);
			}
		}


		/// <summary>
		/// Processes include links and resolves their contents.
		/// </summary>
		/// <param name="docToProcess">
		/// The <see cref="System.Xml.XmlDocument"/> to process the include links in.
		/// </param>
		/// <param name="sourceFilePath">
		/// The path to the source document the XML came from, for calculating
		/// relative paths to included doc. Ignored if <see langword="null" />
		/// or empty.
		/// </param>
		/// <remarks>
		/// <para>
		/// For every include node in the comment, the <c>file</c> node will be
		/// looked at for the location of the XML file containing documentation.
		/// The file path is first converted to an absolute path and if the document
		/// is found, that's what will be used. If it's not found, the file path
		/// will be converted to a path relative to the <paramref name="sourceFilePath" />
		/// and, if the document is found, it will be used.
		/// </para>
		/// </remarks>
		public static void ProcessIncludedDocumentation(XmlDocument docToProcess, string sourceFilePath)
		{
			if (docToProcess == null)
			{
				return;
			}
			XmlNodeList list = docToProcess.SelectNodes("//include");
			if (list == null || list.Count < 1)
			{
				// No included documentation
				return;
			}

			foreach (XmlNode includeItem in list)
			{
				XmlNode includeParent = includeItem.ParentNode;
				XmlNode replacement = null;
				string fileAtributeValue = Evaluator.ValueOf(includeItem, "@file");
				string pathAttributeValue = Evaluator.ValueOf(includeItem, "@path");

				// Check for empty values
				if (fileAtributeValue == "")
				{
					replacement = CommentParser.CreateErrorNode(docToProcess, "File path in INCLUDE element is empty; unable to render.");
				}
				else if (pathAttributeValue == "")
				{
					replacement = CommentParser.CreateErrorNode(docToProcess, "XPath in INCLUDE element is empty; unable to render.");
				}
				if (replacement != null)
				{
					// There was an empty value; sub in the error and continue
					includeParent.ReplaceChild(replacement, includeItem);
					continue;
				}

				// Map the file to the cache
				string file = Path.GetFullPath(fileAtributeValue);
				if (!File.Exists(file) && !String.IsNullOrEmpty(sourceFilePath))
				{
					file = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(sourceFilePath), fileAtributeValue));
				}
				string cacheKey = "CR_DOCUMENTOR_INCLUDE|" + file.ToUpper(System.Globalization.CultureInfo.InvariantCulture);

				// Get the document from cache
				XmlDocument cachedDoc = HttpRuntime.Cache[cacheKey] as XmlDocument;
				if (cachedDoc == null)
				{
					if (!File.Exists(file))
					{
						replacement = CommentParser.CreateErrorNode(docToProcess, String.Format("File [{0}] in INCLUDE element does not exist; unable to render.", file));
						includeParent.ReplaceChild(replacement, includeItem);
						continue;
					}

					try
					{
						cachedDoc = new XmlDocument();
						cachedDoc.Load(file);
					}
					catch (Exception err)
					{
						replacement = CommentParser.CreateErrorNode(docToProcess, String.Format("Error reading file [{0}] in INCLUDE element; unable to render.  Message: {1}", file, err.Message));
						includeParent.ReplaceChild(replacement, includeItem);
						continue;
					}

					HttpRuntime.Cache.Insert(cacheKey, cachedDoc, new CacheDependency(file));
				}

				if (cachedDoc == null)
				{
					replacement = CommentParser.CreateErrorNode(docToProcess, String.Format("Unknown error reading file [{0}] in INCLUDE element; unable to render.", file));
					includeParent.ReplaceChild(replacement, includeItem);
					continue;
				}

				// Get the list of nodes to insert
				XmlNodeList cachedDocNodes = cachedDoc.SelectNodes(pathAttributeValue);
				if (cachedDocNodes == null || cachedDocNodes.Count < 1)
				{
					replacement = CommentParser.CreateErrorNode(docToProcess, String.Format("Document [{0}] in INCLUDE element has no elements matching XPath [{1}]; unable to render.", file, pathAttributeValue));
					includeParent.ReplaceChild(replacement, includeItem);
					continue;
				}

				// Insert the nodes
				foreach (XmlNode cachedDocNode in cachedDocNodes)
				{
					XmlNode importedNode = docToProcess.ImportNode(cachedDocNode, true);
					includeParent.InsertBefore(importedNode, includeItem);
				}

				// Remove the original include node
				includeParent.RemoveChild(includeItem);
			}
		}

		#endregion

		#endregion

	}
}
