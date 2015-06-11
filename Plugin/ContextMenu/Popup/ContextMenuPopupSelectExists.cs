using System;
using DevExpress.CodeRush.Core;

namespace CR_Documentor.ContextMenu.Popup
{
	/// <summary>
	/// A context menu popup which is only available if a selection exists.
	/// </summary>
	public class ContextMenuPopupSelectExists : ContextMenuPopup
	{
		/// <summary>
		/// Overridden.  Requires a selection to exist.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if the context is satisfied or if there is no context
		/// to be met; <see langword="false" /> otherwise.
		/// </value>
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
		/// Initializes a new instance of the <see cref="ContextMenuPopupSelectExists"/> class.
		/// </summary>
		public ContextMenuPopupSelectExists()
		{
		}
	}
}
