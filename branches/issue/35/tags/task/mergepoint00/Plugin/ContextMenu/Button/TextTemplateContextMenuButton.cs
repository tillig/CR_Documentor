using System;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using CR_Documentor.Diagnostics;
using DevExpress.CodeRush.Core;
using DevExpress.CodeRush.StructuralParser;

namespace CR_Documentor.ContextMenu.Button
{
	/// <summary>
	/// A context menu button that provides template insertion and embedding ability in
	/// the code editor.
	/// </summary>
	public class TextTemplateContextMenuButton : ContextMenuButton
	{
		/// <summary>
		/// Log entry handler.
		/// </summary>
		private static readonly ILog Log = LogManager.GetLogger(typeof(TextTemplateContextMenuButton));

		/// <summary>
		/// The string used in <see cref="Content"/> for inserting text into the item.
		/// </summary>
		public const string TEXT_REPLACEMENT = @"\^";

		/// <summary>
		/// The string used in <see cref="Content"/> for placing the caret.
		/// </summary>
		public const string CARET_PLACEMENT = @"\@";

		/// <summary>
		/// Gets or sets the string that will be inserted when this item is
		/// activated.
		/// </summary>
		/// <value>A <see cref="System.String"/> with the insertion value.</value>
		/// <remarks>
		/// <para>
		/// If the item is used for replacing an existing selection or inserting
		/// new text, this content is the string that gets inserted or replaces the
		/// selection.
		/// </para>
		/// <para>
		/// If the content is meant to embed a selection, use the string <c>\^</c> as the
		/// point at which any selected text should end up.
		/// </para>
		/// </remarks>
		public virtual string Content { get; set; }

		/// <summary>
		/// Gets or sets a <see cref="System.Boolean"/> indicating any selected text that
		/// gets included in the replacement should be converted to lower case.
		/// </summary>
		/// <value>
		/// <see langword="true"/> to indicate the selection should be converted to lower
		/// case on replacement; <see langword="false" /> otherwise.
		/// </value>
		public virtual bool ConvertSelectionToLower { get; set; }

		/// <summary>
		/// Indicates the list of selection contexts that must be satisfied for this button
		/// to be available.
		/// </summary>
		/// <value>
		/// A combination of <see cref="DXCoreContext.SelectionContext"/> flags indicating the possible
		/// selection contexts for the button.
		/// </value>
		public virtual DXCoreContext.SelectionContext SelectionRequirements { get; set; }

		/// <summary>
		/// Overridden. Requires a selection if necessary.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if the context is satisfied or if there is no context
		/// to be met; <see langword="false" /> otherwise.
		/// </value>
		public override bool ContextSatisfied
		{
			get
			{
				StringCollection coll = new StringCollection();
				if ((this.SelectionRequirements & DXCoreContext.SelectionContext.None) == DXCoreContext.SelectionContext.None)
				{
					coll.Add(DXCoreContext.CTX_SelectionNone);
				}
				if ((this.SelectionRequirements & DXCoreContext.SelectionContext.LineFragment) == DXCoreContext.SelectionContext.LineFragment)
				{
					coll.Add(DXCoreContext.CTX_SelectionFragment);
				}
				if ((this.SelectionRequirements & DXCoreContext.SelectionContext.WholeLine) == DXCoreContext.SelectionContext.WholeLine)
				{
					coll.Add(DXCoreContext.CTX_SelectionWholeLine);
				}
				if ((this.SelectionRequirements & DXCoreContext.SelectionContext.MultiLines) == DXCoreContext.SelectionContext.MultiLines)
				{
					coll.Add(DXCoreContext.CTX_SelectionMultiLine);
				}
				if (coll.Count > 0 && !CodeRush.Context.Satisfied(coll, true))
				{
					return false;
				}
				return base.ContextSatisfied;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TextTemplateContextMenuButton"/> class.
		/// </summary>
		public TextTemplateContextMenuButton()
		{
			this.Content = "";
			this.SelectionRequirements = DXCoreContext.SelectionContext.None;
		}

		/// <summary>
		/// Executes the text insertion/replacement for the item.
		/// </summary>
		public override void Execute()
		{
			// TODO: Refactor this nightmare huge method. It's too big.
			Log.Write(LogLevel.Info, "Executing text insertion/replacement.");
			// Mark the undo stack
			CodeRush.UndoStack.BeginUpdate("InsertTextTemplate");

			try
			{
				// Get the original selection values
				string originalSelectedText = "";
				SourceRange originalRange;
				ISavedSelection originalSelection = TextView.Active.Selection.Save();

				if (CodeRush.Selection.Exists)
				{
					// Save the original selection values
					Log.Write(LogLevel.Info, "Selection exists.  Saving selected text and active range.");
					originalSelectedText = CodeRush.Selection.Text;
					originalRange = CodeRush.Documents.ActiveTextView.GetSelectionRange();
				}
				else
				{
					// Set up an empty range at the point of insertion
					Log.Write(LogLevel.Info, "No selection to save.");
					originalRange = new SourceRange(CodeRush.Caret.Line, CodeRush.Caret.Offset);
				}

				// Get the replacement text
				string replacement = originalSelectedText;
				if (replacement != "")
				{
					Log.Write(LogLevel.Info, "Formatting replacement text in preparation for embedding.");

					// Convert any newlines to \n (we'll put that back later)
					replacement = replacement.Replace(System.Environment.NewLine, "\n");

					// Convert replacement text to lower case, if necessary
					if (this.ConvertSelectionToLower)
					{
						replacement = replacement.ToLower(System.Globalization.CultureInfo.InvariantCulture);
					}

					// Strip out any XML doc comment prefixes - we'll put them back later
					replacement = this.StripXmlDocCommentPrefix(replacement);
				}

				// Remove double-newlines at the end of any replacement text
				if ((this.Content.IndexOf(TEXT_REPLACEMENT + "\n") >= 0) &&
					(replacement.EndsWith("\n"))
					)
				{
					Log.Write(LogLevel.Info, "Removing double-newlines at end of replacement.");
					replacement = replacement.Substring(0, replacement.Length - 1);
				}

				// Get the full content to insert
				// This will be a string without the XML doc comment prefixes in it
				string fullContent = this.Content.Replace(TEXT_REPLACEMENT, replacement);

				// Get the active text document
				TextDocument activeTextDocument = CodeRush.Documents.ActiveTextDocument;
				if (activeTextDocument == null)
				{
					// The active text document wasn't retrievable.
					Log.Write(LogLevel.Warn, "Unable to retrieve active text document.");
					return;
				}

				// If there's a \n at the beginning of the template, make sure it starts
				// on its own line.  If it's the first line in the XML comment, leave it there.
				// Also handle single-line insertions to make sure selections containing the
				// beginning of the line are dealt with.

				Log.Write(LogLevel.Info, "Processing beginning of insertion line.");

				// Get the beginning of the insertion line
				CodeRush.Selection.SelectRange(originalRange.Start.Line, 1, originalRange.Start.Line, originalRange.Start.Offset);

				if (CodeRush.Selection.Exists)
				{
					// If there's a selection, see if this will be the first thing on the line.
					string startText = CodeRush.Selection.Text.Trim();

					if (startText.Length == 0)
					{
						// This is totally the beginning of the line -
						// we even need to insert the XML doc comment prefix
						// (but no newline)
						if (fullContent.StartsWith("\n"))
						{
							fullContent = DocumentorContextMenu.CommentPrefix + " " + fullContent.Substring(1);
						}
						else
						{
							fullContent = DocumentorContextMenu.CommentPrefix + " " + fullContent;
						}
					}
					else
					{
						// Strip any XML doc comment prefixes
						startText = this.StripXmlDocCommentPrefix(CodeRush.Selection.Text);

						// Trim any whitespace (which is all that should be left)
						startText = startText.Trim();

						// If that's it, we don't need to insert a newline.
						if (startText.Length == 0 && fullContent.StartsWith("\n"))
						{
							fullContent = fullContent.Substring(1);
						}
					}
				}
				else
				{
					// There's NO selection so the entire beginning of the line was
					// originally selected and we're going to want to put in the comment
					// prefix but NOT add a newline.
					if (fullContent.StartsWith("\n"))
					{
						fullContent = DocumentorContextMenu.CommentPrefix + " " + fullContent.Substring(1);
					}
					else
					{
						fullContent = DocumentorContextMenu.CommentPrefix + " " + fullContent;
					}
				}
				originalSelection.Restore();

				Log.Write(LogLevel.Info, "Processing end of insertion line.");

				// If there's a \n at the end of the template, make sure it ends on its own line.
				// If it's the last line in the XML comment, leave it there.
				if (fullContent.EndsWith("\n"))
				{
					// Get the end of the insertion line
					CodeRush.Selection.SelectRange(
						originalRange.End.Line,
						originalRange.End.Offset,
						originalRange.End.Line,
						activeTextDocument.GetLineLength(originalRange.End.Line) + 1);

					if (CodeRush.Selection.Exists)
					{
						// There's something at the end of this line

						// If there's a selection, see if this will be the last thing on the line.
						string endText = CodeRush.Selection.Text.Trim();
						if (endText.Length == 0)
						{
							// This is the end of the line, not counting the whitespace.
							// Nuke the extra newline.
							fullContent = fullContent.Substring(0, fullContent.Length - 1);
						}
						else if (endText.StartsWith(DocumentorContextMenu.CommentPrefix))
						{
							// We've selected all the way to the next line.
							// Replace the trailing standard newline with just a
							// return (which won't get any comment prefix)
							fullContent = fullContent.Substring(0, fullContent.Length - 1) + "\r";
						}
					}
					else
					{
						// There's nothing at the end of this line, so remove
						// any trailing newline.
						fullContent = fullContent.Substring(0, fullContent.Length - 1);
					}

					originalSelection.Restore();
				}

				// For each \n in the middle of the template, add the comment prefix.
				Log.Write(LogLevel.Info, "Adding in XML comment prefixes.");
				fullContent = fullContent.Replace("\n", "\n" + DocumentorContextMenu.CommentPrefix + " ");

				Log.Write(LogLevel.Info, "Converting newlines back to system value.");

				// For each \r in the middle of the template, convert to a regular newline.
				fullContent = fullContent.Replace("\r", "\n");

				// Put the system newline back in
				fullContent = fullContent.Replace("\n", System.Environment.NewLine);

				// Insert the content (replace any selected text)
				Log.Write(LogLevel.Info, "Removing any existing selection in preparation for insert/embed.");
				CodeRush.Selection.Delete();

				Log.Write(LogLevel.Info, "Inserting/embedding text.");
				SourceRange insertedRange = activeTextDocument.InsertText(CodeRush.Caret.SourcePoint, fullContent);

				// Format the inserted area of the document if we inserted newlines
				Log.Write(LogLevel.Info, "Formatting final insertion.");
				SourceRange formattedRange = insertedRange;
				formattedRange = activeTextDocument.Format(insertedRange);

				Log.Write(LogLevel.Info, "Positioning caret.");
				// TODO: Figure out how to place the caret in the spot with CARET_PLACEMENT

				// Move to the end of the insertion
				CodeRush.Caret.MoveTo(formattedRange.Bottom);

				Log.Write(LogLevel.Info, "Insert/embed complete.");
			}
			catch (Exception err)
			{
				Log.Write(LogLevel.Error, "Error while inserting/embedding text.", err);
			}
			finally
			{
				// End the undo stack mark
				CodeRush.UndoStack.EndUpdate();
			}
		}

		/// <summary>
		/// Strips the XML Doc Comment prefixes from any lines in a string.
		/// </summary>
		/// <param name="toStrip">The string to strip the comment prefixes from.</param>
		/// <returns>A stripped version of the string.</returns>
		protected string StripXmlDocCommentPrefix(string toStrip)
		{
			string xmlDocCommentStartExpression = String.Format(@"^\s*{0}[ ]?", DocumentorContextMenu.CommentPrefix.Replace("\\", "\\\\"));
			string replaced = Regex.Replace(toStrip, xmlDocCommentStartExpression, "", RegexOptions.Multiline);
			return replaced;
		}
	}
}
