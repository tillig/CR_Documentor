using System;
using CR_Documentor.Diagnostics;
using DevExpress.CodeRush.Core;
using DevExpress.CodeRush.StructuralParser;

namespace CR_Documentor.ContextMenu.Button
{
	/// <summary>
	/// XML encodes a text selection.
	/// </summary>
	public class XmlEncodeButton : ContextMenuButton
	{
		/// <summary>
		/// Log entry handler.
		/// </summary>
		private static readonly ILog Log = LogManager.GetLogger(typeof(XmlEncodeButton));

		/// <summary>
		/// Overridden.  Requires a selection.
		/// </summary>
		public override bool ContextSatisfied
		{
			get
			{
				if (!CodeRush.Selection.Exists)
				{
					return false;
				}
				return base.ContextSatisfied;
			}
		}

		/// <summary>
		/// Overridden.  XML encodes a selection.
		/// </summary>
		public override void Execute()
		{
			using (ActivityContext context = new ActivityContext(Log, "XML encoding selected text."))
			{
				try
				{
					// Get the active text document
					TextDocument activeTextDocument = CodeRush.Documents.ActiveTextDocument;
					if (activeTextDocument == null)
					{
						// The active text document wasn't retrievable.
						Log.Write(LogLevel.Warn, "Unable to retrieve active text document.");
						return;
					}

					if (!CodeRush.Selection.Exists)
					{
						// There has to be a selection to XML encode
						Log.Write(LogLevel.Info, "No selection to encode.");
						return;
					}

					// Get the full content to insert
					Log.Write(LogLevel.Info, "XML encoding selected text.");
					string replacement = this.XmlEncode(CodeRush.Selection.Text);

					// Insert the content (replace any selected text)
					CodeRush.UndoStack.BeginUpdate("XmlEncodeSelection");
					Log.Write(LogLevel.Info, "Deleting original text.");
					CodeRush.Selection.Delete();
					Log.Write(LogLevel.Info, "Inserting XML encoded text.");
					SourceRange insertedRange = activeTextDocument.InsertText(CodeRush.Caret.SourcePoint, replacement);
					CodeRush.UndoStack.EndUpdate();

					// Move to the end of the insertion
					CodeRush.Caret.MoveTo(insertedRange.Bottom);
					Log.Write(LogLevel.Info, "XML encode complete.");
				}
				catch (Exception err)
				{
					Log.Write(LogLevel.Error, "Error happened while XML encoding text.", err);
				}
			}
		}

		/// <summary>
		/// XML encodes a string.
		/// </summary>
		/// <param name="toEncode">The string to encode.</param>
		/// <returns>The string with XML entities encoded.</returns>
		protected virtual string XmlEncode(string toEncode)
		{
			return toEncode.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");
		}
	}
}
