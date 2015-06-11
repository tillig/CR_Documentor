using System;
using CR_Documentor.Diagnostics;
using DevExpress.CodeRush.Core;
using DevExpress.CodeRush.StructuralParser;

namespace CR_Documentor.ContextMenu.Button
{
	/// <summary>
	/// Expands or collapses all XML document comment sections in the current document.
	/// </summary>
	public class OutlineXmlDocSectionsButton : ContextMenuButton
	{
		/// <summary>
		/// Log entry handler.
		/// </summary>
		private static readonly ILog Log = LogManager.GetLogger(typeof(OutlineXmlDocSectionsButton));

		/// <summary>
		/// Gets a <see cref="System.Boolean"/> indicating whether the button
		/// should expand or collapse the XML documentation
		/// </summary>
		/// <value>
		/// <see langword="true"/> to collapse the XML doc regions; <see langword="false" />
		/// to expand them.
		/// </value>
		public virtual bool ShouldCollapse { get; set; }

		/// <summary>
		/// Overridden.  Toggles outlining on XML doc comment sections.
		/// </summary>
		public override void Execute()
		{
			DocumentorActions.OutlineXmlDocSections(this.ShouldCollapse);
		}
	}
}
