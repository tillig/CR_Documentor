using System;

using DevExpress.CodeRush.Core;
using DevExpress.CodeRush.Menus;
using DevExpress.CodeRush.StructuralParser;

namespace CR_Documentor.ContextMenu.Button
{
	/// <summary>
	/// Describes a button/item that appears in a context menu.
	/// </summary>
	public abstract class ContextMenuButton : ContextMenuItem
	{
		
		#region ContextMenuButton Implementation
  
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ContextMenuButton"/> class.
		/// </summary>
		public ContextMenuButton() {
		}
  
		#endregion
  
		#region Overrides
		
		/// <summary>
		/// Renders this button to the context menu.
		/// </summary>
		/// <param name="parentPopup">The parent item to render this button in.</param>
		/// <param name="buttonClickHandler">The event handler that will be called when this button is clicked.</param>
		public override void Render(DevExpress.CodeRush.Menus.IMenuPopup parentPopup, DevExpress.CodeRush.Menus.MenuButtonClickEventHandler buttonClickHandler) {
			if(!this.IsAvailable){
				return;
			}

			IMenuButton newButton = parentPopup.AddButton();
			newButton.Caption = this.Caption;
			newButton.BeginGroup = this.BeginGroup;
			newButton.Enabled = this.Enabled;
			newButton.Tag = this.Tag;
			newButton.Click += buttonClickHandler;
		}
  
		#endregion
  
		#region Methods

		/// <summary>
		/// Executes whatever action the context menu button performs.
		/// </summary>
		public abstract void Execute();

		#endregion

		#endregion
  
	}
}
