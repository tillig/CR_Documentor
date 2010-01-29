using System;
using DevExpress.CodeRush.Core;
using DevExpress.CodeRush.StructuralParser;

namespace CR_Documentor
{
	/// <summary>
	/// Provides constants and helpers for evaluating context.
	/// </summary>
	public static class DXCoreContext
	{
		/// <summary>
		/// Context indicating the cursor is in an XML doc comment.
		/// </summary>
		public const string CTX_InXmlDocComment = @"Editor\Code\InXmlDocComment";

		/// <summary>
		/// Context indicating there is no selection.
		/// </summary>
		public const string CTX_SelectionNone = @"Editor\Selection\No Selection";

		/// <summary>
		/// Context indicating a line fragment is selected.
		/// </summary>
		public const string CTX_SelectionFragment = @"Editor\Selection\Any Selection\Line Fragment";

		/// <summary>
		/// Context indicating a whole line is selected.
		/// </summary>
		public const string CTX_SelectionWholeLine = @"Editor\Selection\Any Selection\Whole Line";

		/// <summary>
		/// Context indicating multiple lines are selected.
		/// </summary>
		public const string CTX_SelectionMultiLine = @"Editor\Selection\Any Selection\Multiple Lines";

		/// <summary>
		/// Context indicating the code editor is showing.
		/// </summary>
		public const string CTX_InCodeEditor = @"Focus\Documents\Source\Code Editor";


		/// <summary>
		/// Gets a value indicating whether a source file is currently being edited.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if the focus is currently in the code editor
		/// and if the active document is a <see cref="DevExpress.CodeRush.StructuralParser.SourceFile"/>;
		/// <see langword="false" /> otherwise.
		/// </value>
		public static bool EditingSourceFile
		{
			get
			{
				if (!CodeRush.Context.Satisfied(DXCoreContext.CTX_InCodeEditor, false))
				{
					return false;
				}
				TextDocument activeTextDocument = CodeRush.Documents.ActiveTextDocument;
				if (activeTextDocument == null || activeTextDocument.FileNode == null || !(activeTextDocument.FileNode is SourceFile))
				{
					return false;
				}
				return true;
			}
		}

		/// <summary>
		/// Gets a value indicating if there's an active text document and if there's
		/// a selection in that document.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if there is an active text document and if there
		/// is currently a selection made in that document; <see langword="false" />
		/// otherwise.
		/// </value>
		public static bool HasActiveSelection
		{
			get
			{
				if (CodeRush.Documents.ActiveTextDocument == null || !CodeRush.Selection.Exists)
				{
					return false;
				}
				return true;
			}
		}

		/// <summary>
		/// Denotes the various selection contexts that must be satisfied for this replacement
		/// to be available.
		/// </summary>
		/// <remarks>
		/// "Or" the flags together to make a complete context.
		/// </remarks>
		[Flags]
		public enum SelectionContext
		{
			/// <summary>
			/// Indicates NO selection must be selected.
			/// </summary>
			None = 1,

			/// <summary>
			/// Indicates a line fragment must be selected.
			/// </summary>
			LineFragment = 2,

			/// <summary>
			/// Indicates a whole line must be selected.
			/// </summary>
			WholeLine = 4,

			/// <summary>
			/// Indicates multiple lines must be selected.
			/// </summary>
			MultiLines = 8
		}

	}
}
