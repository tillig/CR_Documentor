using System;
using CR_Documentor.Diagnostics;
using DevExpress.CodeRush.Core;
using DevExpress.CodeRush.StructuralParser;

namespace CR_Documentor.ContextMenu.Button
{
	/// <summary>
	/// Expands or collapses all XML document comment sections in the current document.
	/// </summary>
	public class OutlineXmlDocSectionsButton : ContextMenuButton
	{
		/// <summary>
		/// Log entry handler.
		/// </summary>
		private static readonly ILog Log = LogManager.GetLogger(typeof(OutlineXmlDocSectionsButton));

		/// <summary>
		/// Gets a <see cref="System.Boolean"/> indicating whether the button
		/// should expand or collapse the XML documentation
		/// </summary>
		/// <value>
		/// <see langword="true"/> to collapse the XML doc regions; <see langword="false" />
		/// to expand them.
		/// </value>
		public virtual bool ShouldCollapse { get; set; }

		/// <summary>
		/// Overridden.  Toggles outlining on XML doc comment sections.
		/// </summary>
		public override void Execute()
		{
			using (ActivityContext context = new ActivityContext(Log, "Toggling visibility of XML doc comments."))
			{
				try
				{
					Log.Write(LogLevel.Info, String.Format("Setting comments to be {0}.", this.ShouldCollapse ? "collapsed" : "expanded"));

					// Get the active text document
					TextDocument activeTextDocument = CodeRush.Documents.ActiveTextDocument;
					if (activeTextDocument == null)
					{
						// The active text document wasn't retrievable.
						Log.Write(LogLevel.Warn, "Unable to retrieve active text document.");
						return;
					}

					if (activeTextDocument.FileNode == null || !(activeTextDocument.FileNode is SourceFile))
					{
						// We're not in a source file.
						Log.Write(LogLevel.Info, "Current document not a source file.  Not toggling visibility.");
						return;
					}

					// We're working with a source file - get a reference to that
					SourceFile sourceFile = (SourceFile)activeTextDocument.FileNode;
					CommentCollection xmlComments = CodeRush.Source.GetXmlDocComments(sourceFile);

					// Save the current caret position
					Log.Write(LogLevel.Info, "Saving original caret position.");
					SourcePoint origCaretPos = CodeRush.Caret.SourcePoint;

					// Get the current text view
					Log.Write(LogLevel.Info, "Getting active TextView.");
					TextView currentTextView = CodeRush.TextViews.Active;
					if (currentTextView == null)
					{
						// The active text view wasn't retrievable.
						Log.Write(LogLevel.Warn, "Unable to retrieve active text view.");
						return;
					}

					// Mark the undo stack
					CodeRush.UndoStack.BeginUpdate("ToggleXmlDocOutline");

					// Iterate through the list of comments and expand/collapse as needed
					Log.Write(LogLevel.Info, "Iterating through XML doc comment blocks.");
					foreach (Comment c in xmlComments)
					{
						CodeRush.Caret.MoveTo(c.StartLine, c.StartOffset);
						bool inCollapsedRegion = currentTextView.Lines.InCollapsedRegion(c.StartLine);
						if ((this.ShouldCollapse && !inCollapsedRegion) || (!this.ShouldCollapse && inCollapsedRegion))
						{
							// We are either in an expanded region and need to collapse
							// or in a collapsed region and need to expand
							CodeRush.Outline.Toggle();
						}
					}

					// Restore the caret position
					Log.Write(LogLevel.Info, "Restoring original caret position.");
					CodeRush.Caret.MoveTo(origCaretPos.Line, origCaretPos.Offset);

					// Finish with the undo stack
					CodeRush.UndoStack.EndUpdate();

					Log.Write(LogLevel.Info, "Outlining toggle complete.");
				}
				catch (Exception err)
				{
					Log.Write(LogLevel.Error, "Error while toggling XML doc visibility.", err);
				}
			}
		}
	}
}
