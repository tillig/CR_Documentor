using System;
using CR_Documentor.Diagnostics;
using DevExpress.CodeRush.Core;
using DevExpress.CodeRush.StructuralParser;

namespace CR_Documentor.ContextMenu.Button
{
	/// <summary>
	/// XML encodes a text selection.
	/// </summary>
	public class XmlEncodeButton : ContextMenuButton
	{
		/// <summary>
		/// Log entry handler.
		/// </summary>
		private static readonly ILog Log = LogManager.GetLogger(typeof(XmlEncodeButton));

		/// <summary>
		/// Overridden.  Requires a selection.
		/// </summary>
		public override bool ContextSatisfied
		{
			get
			{
				if (!DXCoreContext.HasActiveSelection)
				{
					return false;
				}
				return base.ContextSatisfied;
			}
		}

		/// <summary>
		/// Overridden.  XML encodes a selection.
		/// </summary>
		public override void Execute()
		{
			DocumentorActions.XmlEncodeSelection();
		}
	}
}
