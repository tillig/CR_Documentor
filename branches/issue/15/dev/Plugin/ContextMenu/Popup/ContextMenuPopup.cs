using System;
using System.Collections.ObjectModel;

using DevExpress.CodeRush.Core;
using DevExpress.CodeRush.Menus;
using DevExpress.CodeRush.StructuralParser;

using CR_Documentor.ContextMenu.Button;

namespace CR_Documentor.ContextMenu.Popup
{
	/// <summary>
	/// Describes a popup that appears in a context menu.
	/// </summary>
	public class ContextMenuPopup : ContextMenuItem
	{
		
		#region ContextMenuPopup Variables
  
		#region Constants
  
		#endregion
  
		#region Statics
  
		#endregion
  
		#region Instance
 
		/// <summary>
		/// Internal storage for the <see cref="Popup" /> property.
		/// </summary>
		private IMenuPopup _Popup = null;

		/// <summary>
		/// Internal storage for the <see cref="Children"/> property.
		/// </summary>
		private Collection<ContextMenuItem> _Children = new Collection<ContextMenuItem>();
 
		#endregion
  
		#endregion
  
  
  
		#region ContextMenuPopup Events
  
		#endregion
  
  
  
		#region ContextMenuPopup Properties

		/// <summary>
		/// Gets or sets the <see cref="DevExpress.CodeRush.Menus.IMenuPopup"/> representing
		/// this popup context menu item.
		/// </summary>
		public virtual IMenuPopup Popup{
			get {
				return _Popup;
			}
			set {
				_Popup = value;
			}
		}

		/// <summary>
		/// Gets the set of children of this menu item.
		/// </summary>
		/// <value>A <see cref="System.Collections.ObjectModel.Collection{T}"/> with the children of this item.</value>
		public virtual Collection<ContextMenuItem> Children
		{
			get {
				return _Children;
			}
		}
 
		#endregion
  
  
  
		#region ContextMenuPopup Implementation
  
		#region Constructors
  
		/// <summary>
		/// Initializes a new instance of the <see cref="ContextMenuPopup"/> class.
		/// </summary>
		public ContextMenuPopup() {
		}
  
		#endregion
  
		#region Overrides
  
		#endregion
  
		#region Event Handlers
  
		#endregion
  
		#region Methods
  
		#region Static
  
		#endregion
  
		#region Instance

		/// <summary>
		/// Recursively traverses the popup hierarchy to find the <see cref="ContextMenuButton"/>
		/// with the specified tag.
		/// </summary>
		/// <param name="tag">The button tag to search for.</param>
		/// <returns>
		/// A <see cref="ContextMenuButton"/> that has the specified tag, or <see langword="null" />
		/// if the button wasn't found.
		/// </returns>
		public virtual ContextMenuButton GetButtonByTag(string tag){
			ContextMenuButton retVal = null;
			
			// Search the buttons in the current menu first
			foreach(ContextMenuItem item in this.Children){
				if(item is ContextMenuButton){
					ContextMenuButton button = (ContextMenuButton)item;
					if(button.Tag == tag){
						retVal = button;
						break;
					}
				}
			}

			if(retVal == null){
				// Search the subpopups in the current menu
				foreach(ContextMenuItem item in this.Children){
					if(item is ContextMenuPopup){
						retVal = ((ContextMenuPopup)item).GetButtonByTag(tag);
						if(retVal != null){
							break;
						}
					}
				}
			}

			return retVal;
		}

		/// <summary>
		/// Deletes all of the items in this popup.
		/// </summary>
		public virtual void ClearItems(){
			// Remove all subpopups first
			foreach(ContextMenuItem item in this.Children){
				if(item is ContextMenuPopup){
					((ContextMenuPopup)item).ClearItems();
				}
			}

			// Remove this popup
			if(this.Popup != null){
				// Delete the controls in the popup
				for(int i = this.Popup.Count - 1; i >= 0; i--){
					this.Popup[i].Delete();
				}

				// Delete the menu itself
				this.Popup.Delete();
				this.Popup = null;
			}
#if(DEBUG)
					System.Diagnostics.Debug.WriteLine("Cleared: " + this.FullID);
#endif
		}

		/// <summary>
		/// Renders this popup into a context menu.
		/// </summary>
		/// <param name="parentPopup">The parent to attach this popup to.</param>
		/// <param name="buttonClickHandler">The click handler to be used when contained buttons are clicked.</param>
		public override void Render(IMenuPopup parentPopup, MenuButtonClickEventHandler buttonClickHandler) {
			if(!this.IsAvailable){
				return;
			}

			// Add this popup
			this.Popup = parentPopup.AddPopup();
			this.Popup.Caption = this.Caption;
			this.Popup.BeginGroup = this.BeginGroup;
			this.Popup.Enabled = this.Enabled;
			this.Popup.Tag = this.Tag;
#if(DEBUG)
			System.Diagnostics.Debug.WriteLine("Rendered: " + this.FullID);
#endif

			// Render menu children
			foreach(ContextMenuItem item in this.Children){
				item.Render(this.Popup, buttonClickHandler);
				item.Parent = this;
			}

			// If nothing below this ends up being available, remove it
			if(this.Popup.IsEmpty){
				this.ClearItems();
			}
		}

		#endregion
  
		#endregion
  
		#endregion
	}
}
