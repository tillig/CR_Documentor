using System;
using CR_Documentor.Diagnostics;

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
				if (!DXCoreContext.HasActiveWholeOrMultiLineSelection)
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
			DocumentorActions.ConvertSelectionToXmlDocComment();
		}
	}
}
