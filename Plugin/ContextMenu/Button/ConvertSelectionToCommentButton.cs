using System;
using System.Text.RegularExpressions;
using CR_Documentor.Diagnostics;
using DevExpress.CodeRush.Core;

namespace CR_Documentor.ContextMenu.Button
{
	/// <summary>
	/// A context menu button that converts the selection to an XML comment.
	/// </summary>
	public class ConvertSelectionToCommentButton : XmlEncodeButton
	{
		/// <summary>
		/// Log entry handler.
		/// </summary>
		private static readonly ILog Log = LogManager.GetLogger(typeof(ConvertSelectionToCommentButton));

		/// <summary>
		/// Overridden.  Requires either a 1+ whole lines be selected.
		/// </summary>
		public override bool ContextSatisfied
		{
			get
			{
				if (!CodeRush.Context.Satisfied(DXCoreContext.CTX_SelectionWholeLine, true) &&
					!CodeRush.Context.Satisfied(DXCoreContext.CTX_SelectionMultiLine, true))
				{
					return false;
				}
				return base.ContextSatisfied;
			}
		}

		/// <summary>
		/// Overridden.  Converts the selected text to an XML doc comment.
		/// </summary>
		public override void Execute()
		{
			using (ActivityContext context = new ActivityContext(Log, "Converting selected text to XML doc comment."))
			{
				try
				{
					// Get the active text document
					if (TextView.Active == null)
					{
						// The active text document wasn't retrievable.
						Log.Write(LogLevel.Warn, "Unable to retrieve active text document.");
						return;
					}

					if (!TextView.Active.Selection.Exists)
					{
						// There has to be a selection to convert
						Log.Write(LogLevel.Warn, "No selection to convert.");
						return;
					}

					// Mark the undo stack
					CodeRush.UndoStack.BeginUpdate("ConvertSelectionToComment");

					// Get the full content to insert
					Log.Write(LogLevel.Info, "XML encoding selected text.");
					TextView.Active.Selection.ExtendToWholeLines();
					TextView.Active.Selection.Text = this.XmlEncode(TextView.Active.Selection.Text);

					// Tabify the selection, figure out how many tabs are at the start of
					// each line and calculate the minimum.  Remove that many tabs from
					// each line.
					Log.Write(LogLevel.Info, "Tabifying insertion.");

					// The TextView.Active.Selection.Tabify() call doesn't work because
					// it always tabifies the NEXT LINE AFTER THE END. We have to do
					// it manually.
					for (int i = TextView.Active.Selection.StartLine; i < TextView.Active.Selection.EndLine; i++)
					{
						TextView.Active.Selection.TextDocument.TabifyLine(i);
					}
					string[] lines = TextView.Active.Selection.Lines;
					int minTabs = Int32.MaxValue;
					foreach (string line in lines)
					{
						Match tabMatch = Regex.Match(line, @"^(\t*)[^\t]", RegexOptions.None);
						if (tabMatch.Success && tabMatch.Groups[1].Captures[0].Length < minTabs)
						{
							minTabs = tabMatch.Groups[1].Captures[0].Length;
						}
					}
					for (int i = 0; i < minTabs; i++)
					{
						TextView.Active.Selection.Unindent();
					}

					// Add the XML Doc Comment prefix to each line
					Log.Write(LogLevel.Info, "Adding XML doc comment prefix to each line.");
					string prefix = DocumentorContextMenu.CommentPrefix + " ";
					TextView.Active.Selection.Text = Regex.Replace(TextView.Active.Selection.Text, "^", prefix, RegexOptions.Multiline);
					// Remove the one at the end because it will accidentally change the line after the selection into a doc comment.
					TextView.Active.Selection.Text = Regex.Replace(TextView.Active.Selection.Text, prefix + "$", "", RegexOptions.Singleline);

					// Formatting will re-indent the new comment into the proper location
					// using correct spaces/tabs based on user settings.
					Log.Write(LogLevel.Info, "Formatting the new XML doc comment.");
					TextView.Active.Selection.Format();

					// Finish with the undo stack
					CodeRush.UndoStack.EndUpdate();

					// Move to the end of the insertion
					Log.Write(LogLevel.Info, "Positioning caret.");
					CodeRush.Caret.MoveTo(TextView.Active.Selection.EndSourcePoint);

					Log.Write(LogLevel.Info, "Comment conversion complete.");
				}
				catch (Exception err)
				{
					Log.Write(LogLevel.Error, "Error while converting text.", err);
				}
			}
		}

	}
}
