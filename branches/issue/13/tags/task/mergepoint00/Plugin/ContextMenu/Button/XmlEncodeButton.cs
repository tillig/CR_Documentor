using System;
using DevExpress.CodeRush.Core;
using DevExpress.CodeRush.Diagnostics.Menus;
using DevExpress.CodeRush.StructuralParser;

namespace CR_Documentor.ContextMenu.Button
{
	/// <summary>
	/// XML encodes a text selection.
	/// </summary>
	public class XmlEncodeButton : ContextMenuButton
	{
		/// <summary>
		/// Overridden.  Requires a selection.
		/// </summary>
		public override bool ContextSatisfied {
			get {
				if(!CodeRush.Selection.Exists){
					return false;
				}
				return base.ContextSatisfied;
			}
		}

		/// <summary>
		/// Overridden.  XML encodes a selection.
		/// </summary>
		public override void Execute() {
			Log.Enter(ImageType.Info, "Converting selected text to XML doc comment.");
			try{
				// Get the active text document
				TextDocument activeTextDocument = CodeRush.Documents.ActiveTextDocument;
				if(activeTextDocument == null){
					// The active text document wasn't retrievable.
					Log.SendWarning("Unable to retrieve active text document.");
					return;
				}

				if(!CodeRush.Selection.Exists){
					// There has to be a selection to XML encode
					Log.SendWarning("No selection to encode.");
					return;
				}

				// Get the full content to insert
				Log.Send("XML encoding selected text.");
				string replacement = this.XmlEncode(CodeRush.Selection.Text);

				// Insert the content (replace any selected text)
				CodeRush.UndoStack.BeginUpdate("XmlEncodeSelection");
				Log.Send("Deleting original text.");
				CodeRush.Selection.Delete();
				Log.Send("Inserting XML encoded text.");
				SourceRange insertedRange = activeTextDocument.InsertText(CodeRush.Caret.SourcePoint, replacement);
				CodeRush.UndoStack.EndUpdate();

				// Move to the end of the insertion
				CodeRush.Caret.MoveTo(insertedRange.Bottom);
				Log.Send("XML encode complete.");
			}
			catch(Exception err){
				Log.SendException(err);
			}
			finally{
				Log.Exit();
			}
		}

		/// <summary>
		/// XML encodes a string.
		/// </summary>
		/// <param name="toEncode"></param>
		/// <returns></returns>
		protected virtual string XmlEncode(string toEncode){
			return toEncode.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");
		}
	}
}
