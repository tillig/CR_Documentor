using System;

using DevExpress.CodeRush.Core;
using DevExpress.CodeRush.Diagnostics.Menus;
using DevExpress.CodeRush.StructuralParser;

namespace CR_Documentor.ContextMenu.Button
{
	/// <summary>
	/// Expands or collapses all XML document comment sections in the current document.
	/// </summary>
	public class OutlineXmlDocSectionsButton : ContextMenuButton
	{
		/// <summary>
		/// Internal storage for the <see cref="ShouldCollapse"/> property.
		/// </summary>
		private bool _ShouldCollapse = false;

		/// <summary>
		/// Gets a <see cref="System.Boolean"/> indicating whether the button
		/// should expand or collapse the XML documentation
		/// </summary>
		/// <value>
		/// <see langword="true"/> to collapse the XML doc regions; <see langword="false" />
		/// to expand them.
		/// </value>
		public virtual bool ShouldCollapse{
			get {
				return _ShouldCollapse;
			}
			set {
				_ShouldCollapse = value;
			}
		}

		/// <summary>
		/// Overridden.  Toggles outlining on XML doc comment sections.
		/// </summary>
		public override void Execute() {
			Log.Enter(ImageType.Info, "Toggling visibility of XML doc comments.");

			try{
				Log.SendBool(ImageType.Info, "ShouldCollapse", this.ShouldCollapse);
				
				// Get the active text document
				TextDocument activeTextDocument = CodeRush.Documents.ActiveTextDocument;
				if(activeTextDocument == null){
					// The active text document wasn't retrievable.
					Log.SendWarning("Unable to retrieve active text document.");
					return;
				}

				if(activeTextDocument.FileNode == null || !(activeTextDocument.FileNode is SourceFile)){
					// We're not in a source file.
					Log.Send("Current document not a source file.  Not toggling visibility.");
					return;
				}
				
				// We're working with a source file - get a reference to that
				SourceFile sourceFile = (SourceFile)activeTextDocument.FileNode;
				CommentCollection xmlComments = CodeRush.Source.GetXmlDocComments(sourceFile);

				// Save the current caret position
				Log.Send("Saving original caret position.");
				SourcePoint origCaretPos = CodeRush.Caret.SourcePoint;

				// Get the current text view
				Log.Send("Getting active TextView.");
				TextView currentTextView = CodeRush.TextViews.Active;
				if(currentTextView == null){
					// The active text view wasn't retrievable.
					Log.SendWarning("Unable to retrieve active text view.");
					return;
				}

				// Mark the undo stack
				CodeRush.UndoStack.BeginUpdate("ToggleXmlDocOutline");
			
				// Iterate through the list of comments and expand/collapse as needed
				Log.Send("Iterating through XML doc comment blocks.");
				foreach(Comment c in xmlComments){
					CodeRush.Caret.MoveTo(c.StartLine, c.StartOffset);
					bool inCollapsedRegion = currentTextView.Lines.InCollapsedRegion(c.StartLine);
					if((this.ShouldCollapse && !inCollapsedRegion) || (!this.ShouldCollapse && inCollapsedRegion)){
						// We are either in an expanded region and need to collapse
						// or in a collapsed region and need to expand
						CodeRush.Outline.Toggle();
					}
				}

				// Restore the caret position
				Log.Send("Restoring original caret position.");
				CodeRush.Caret.MoveTo(origCaretPos.Line, origCaretPos.Offset);

				// Finish with the undo stack
				CodeRush.UndoStack.EndUpdate();

				Log.Send("Outlining toggle complete.");
			}
			catch(Exception err){
				Log.SendException(err);
			}
			finally{
				Log.Exit();
			}
		}
	}
}
