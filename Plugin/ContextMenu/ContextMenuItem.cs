using System;
using System.Collections.Specialized;

using DevExpress.CodeRush.Core;
using DevExpress.CodeRush.Menus;
using DevExpress.CodeRush.StructuralParser;

using CR_Documentor.ContextMenu.Popup;

namespace CR_Documentor.ContextMenu
{
	/// <summary>
	/// An individual item in a context menu.  Serves as the base functionality for
	/// other items on the context menu, like buttons and popups.
	/// </summary>
	public abstract class ContextMenuItem
	{
		
		#region ContextMenuItem Variables
  
		/// <summary>
		/// Internal storage for the <see cref="Caption"/> property.
		/// </summary>
		private string _Caption = "";
 
		/// <summary>
		/// Internal storage for the <see cref="Context"/> property.
		/// </summary>
		private StringCollection _Context = new StringCollection();
 
		/// <summary>
		/// Internal storage for the <see cref="BeginGroup"/> property.
		/// </summary>
		private bool _BeginGroup = false;

		/// <summary>
		/// Internal storage for the <see cref="Enabled"/> property.
		/// </summary>
		private bool _Enabled = true;
  
		/// <summary>
		/// Internal storage for the <see cref="Tag"/> property.
		/// </summary>
		private string _Tag = "";
 
		/// <summary>
		/// Internal storage for the <see cref="Parent"/> property.
		/// </summary>
		private ContextMenuPopup _Parent = null;

		#endregion
  
  
  
		#region ContextMenuItem Properties

		/// <summary>
		/// Gets or sets a <see cref="System.Boolean"/> indicating if the item is available.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if the item is enabled; <see langword="false" /> otherwise.
		/// </value>
		public virtual bool Enabled{
			get {
				return _Enabled;
			}
			set {
				_Enabled = value;
			}
		}

		/// <summary>
		/// Gets or sets a <see cref="System.Boolean"/> indicating whether a group should
		/// be started before this item.
		/// </summary>
		/// <value>
		/// <see langword="true" /> to start a new group; <see langword="false" /> otherwise.
		/// </value>
		public virtual bool BeginGroup{
			get {
				return _BeginGroup;
			}
			set {
				_BeginGroup = value;
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="System.String"/> to be displayed as the caption
		/// for this item.
		/// </summary>
		/// <value>
		/// A <see cref="System.String"/> with the caption.
		/// </value>
		public virtual string Caption {
			get {
				return _Caption;
			}
			set {
				_Caption = value;
			}
		}
 
		/// <summary>
		/// Gets the set of contexts that must be satisfied for this item to render.
		/// </summary>
		/// <value>
		/// A <see cref="System.Collections.Specialized.StringCollection"/> with the
		/// contexts that must be satisfied for this item to render.
		/// </value>
		public virtual StringCollection Context{
			get {
				return _Context;
			}
		}

		/// <summary>
		/// Gets a <see cref="System.Boolean"/> indicating if the context has been satisfied
		/// for this item to be shown.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if the context is satisfied or if there is no context
		/// to be met; <see langword="false" /> otherwise.
		/// </value>
		public virtual bool ContextSatisfied{
			get{
				if(this.Context.Count < 1){
					return true;
				}
				foreach(string ctx in this.Context){
					if(ctx.StartsWith("!")){
						// Handle "not" context
						string adjustedCtx = ctx.Substring(1);
						if(CodeRush.Context.Satisfied(adjustedCtx, false)){
							return false;
						}
					}
					else if(!(CodeRush.Context.Satisfied(ctx, true))){
						// Handle context
						return false;
					}
#if(DEBUG)
					System.Diagnostics.Debug.WriteLine("Satisfied context: " + ctx);
#endif
				}
				return true;
			}
		}

		/// <summary>
		/// Gets a <see cref="System.Boolean"/> indicating if this item is available to be
		/// rendered.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if the item is avaiable to be rendered; <see langword="false" /> otherwise.
		/// </value>
		public virtual bool IsAvailable{
			get {
				return this.ContextSatisfied;
			}
		}
   
		/// <summary>
		/// Gets or sets the tag associated with this item.
		/// </summary>
		/// <value>A <see cref="System.String"/> with the item tag.</value>
		public virtual string Tag {
			get {
				return _Tag;
			}
			set {
				_Tag = value;
			}
		}
 
		/// <summary>
		/// Gets or sets a link back to the parent popup of this item.
		/// </summary>
		/// <value>
		/// A <see cref="ContextMenuPopup"/> containing the parent of this node.
		/// <see langword="null" /> if there is no parent.
		/// </value>
		public ContextMenuPopup Parent{
			get {
				return _Parent;
			}
			set{
				_Parent = value;
			}
		}

		/// <summary>
		/// The full ID for the item.
		/// </summary>
		/// <value>
		/// A "/" delimited <see cref="System.String"/> containing the full path to
		/// this popup.
		/// </value>
		public string FullID {
			get{
				if(this.Parent == null){
					return this.Tag;
				}
				else{
					return this.Parent.FullID + "/" + this.Tag;
				}
			}
		}

		#endregion
  
  
  
		#region ContextMenuItem Implementation
  
		#region Constructors
   
		/// <summary>
		/// Initializes a new instance of the <see cref="ContextMenuItem"/> class.
		/// </summary>
		public ContextMenuItem() {
		}
 
		#endregion
  
		#region Methods
		
		/// <summary>
		/// Renders the current item in the context menu.
		/// </summary>
		/// <param name="parentPopup">The parent item to which this item should be added.</param>
		/// <param name="buttonClickHandler">A click event handler that should be raised if this item is clicked.</param>
		public abstract void Render(IMenuPopup parentPopup, MenuButtonClickEventHandler buttonClickHandler);
		
		#endregion
  
		#endregion
	}
}
