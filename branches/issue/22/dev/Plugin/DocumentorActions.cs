using System;
using CR_Documentor.Diagnostics;
using DevExpress.CodeRush.Core;
using DevExpress.CodeRush.PlugInCore;
using DevExpress.CodeRush.StructuralParser;

namespace CR_Documentor
{
	/// <summary>
	/// Defines common actions that are available via context menu or shortcut key.
	/// </summary>
	public partial class DocumentorActions : StandardPlugIn
	{
		/// <summary>
		/// Log entry handler.
		/// </summary>
		private static readonly ILog Log = LogManager.GetLogger(typeof(DocumentorActions));

		/// <summary>
		/// Toggles the expansion/collapse of XML doc comments in the current source
		/// document.
		/// </summary>
		/// <param name="collapse">
		/// <see langword="true" /> to collapse the doc comments,
		/// <see langword="false" /> to expand them.
		/// </param>
		public static void OutlineXmlDocSections(bool collapse)
		{
			using (ActivityContext context = new ActivityContext(Log, "Toggling visibility of XML doc comments."))
			{
				try
				{
					if (!DXCoreContext.EditingSourceFile)
					{
						Log.Write(LogLevel.Info, "Current document not a source file. Not toggling visibility.");
						return;
					}

					Log.Write(LogLevel.Info, String.Format("Setting comments to be {0}.", collapse ? "collapsed" : "expanded"));

					// We're working with a source file - get a reference to that
					SourceFile sourceFile = (SourceFile)CodeRush.Documents.ActiveTextDocument.FileNode;
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
						if ((collapse && !inCollapsedRegion) || (!collapse && inCollapsedRegion))
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

		/// <summary>
		/// Toggles the visibility of the CR_Documentor window.
		/// </summary>
		public static void ToggleDocumentorVisibility()
		{
			using (ActivityContext context = new ActivityContext(Log, "Toggling visibility of CR_Documentor window."))
			{
				try
				{
					if (DocumentorWindow.CurrentlyVisible)
					{
						Log.Write(LogLevel.Info, "Hiding window.");
						DocumentorWindow.HideWindow();
					}
					else
					{
						Log.Write(LogLevel.Info, "Showing window.");
						DocumentorWindow.ShowWindow();
					}
					Log.Write(LogLevel.Info, "Visibility toggle complete.");
				}
				catch (Exception err)
				{
					Log.Write(LogLevel.Error, "Error while toggling window visibility.", err);
				}
			}
		}

		/// <summary>
		/// XML encodes the currently selected text.
		/// </summary>
		public static void XmlEncodeSelection()
		{
			using (ActivityContext context = new ActivityContext(Log, "XML encoding selected text."))
			{
				try
				{
					if (!DXCoreContext.HasActiveSelection)
					{
						Log.Write(LogLevel.Info, "No selection to encode.");
						return;
					}

					// Get the full content to insert
					Log.Write(LogLevel.Info, "XML encoding selected text.");
					string replacement = XmlEncode(CodeRush.Selection.Text);

					// Insert the content (replace any selected text)
					CodeRush.UndoStack.BeginUpdate("XmlEncodeSelection");
					Log.Write(LogLevel.Info, "Deleting original text.");
					CodeRush.Selection.Delete();
					Log.Write(LogLevel.Info, "Inserting XML encoded text.");
					SourceRange insertedRange = CodeRush.Documents.ActiveTextDocument.InsertText(CodeRush.Caret.SourcePoint, replacement);
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
		public static string XmlEncode(string toEncode)
		{
			return toEncode.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");
		}

		/// <summary>
		/// Handles execution of the "Collapse XML Doc Comments" action.
		/// </summary>
		/// <param name="ea">
		/// The <see cref="DevExpress.CodeRush.Core.ExecuteEventArgs"/> instance
		/// containing the event data.
		/// </param>
		private void collapseXmlDocComments_Execute(ExecuteEventArgs ea)
		{
			OutlineXmlDocSections(true);
		}

		/// <summary>
		/// Handles execution of the "Expand XML Doc Comments" action.
		/// </summary>
		/// <param name="ea">
		/// The <see cref="DevExpress.CodeRush.Core.ExecuteEventArgs"/> instance
		/// containing the event data.
		/// </param>
		private void expandXmlDocComments_Execute(ExecuteEventArgs ea)
		{
			OutlineXmlDocSections(false);
		}

		/// <summary>
		/// Handles execution of the "Toggle CR_Documentor Window Visibility" action.
		/// </summary>
		/// <param name="ea">
		/// The <see cref="DevExpress.CodeRush.Core.ExecuteEventArgs"/> instance
		/// containing the event data.
		/// </param>
		private void toggleDocumentorVisibility_Execute(ExecuteEventArgs ea)
		{
			ToggleDocumentorVisibility();
		}

		/// <summary>
		/// Determines the availability of the expand/collapse XML doc comment actions.
		/// </summary>
		/// <param name="ea">
		/// The <see cref="DevExpress.CodeRush.Core.QueryStatusEventArgs"/> instance
		/// containing the event data.
		/// </param>
		private void toggleXmlDocComments_QueryStatus(QueryStatusEventArgs ea)
		{
			if (!DXCoreContext.EditingSourceFile)
			{
				ea.Status = EnvDTE.vsCommandStatus.vsCommandStatusUnsupported;
			}
			else
			{
				ea.Status = EnvDTE.vsCommandStatus.vsCommandStatusEnabled | EnvDTE.vsCommandStatus.vsCommandStatusSupported;
			}
		}

		/// <summary>
		/// Handles execution of the "XML Encode Selection" action.
		/// </summary>
		/// <param name="ea">
		/// The <see cref="DevExpress.CodeRush.Core.ExecuteEventArgs"/> instance
		/// containing the event data.
		/// </param>
		private void xmlEncodeSelection_Execute(ExecuteEventArgs ea)
		{
			XmlEncodeSelection();
		}
	}
}