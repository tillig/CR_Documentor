using System;

namespace CR_Documentor.ContextMenu {
	/// <summary>
	/// Provides constants and helpers for evaluating context.
	/// </summary>
	public class DXCoreContext {

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
		/// Denotes the various selection contexts that must be satisfied for this replacement
		/// to be available.
		/// </summary>
		/// <remarks>
		/// "Or" the flags together to make a complete context.
		/// </remarks>
		[Flags]
		public enum SelectionContext{
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
