using System;
using System.Windows.Forms;
using DevExpress.CodeRush.Core;
using DevExpress.CodeRush.Diagnostics.Menus;

namespace CR_Documentor.ContextMenu.Button
{
	/// <summary>
	/// A button used specifically to toggle the visibility of the CR_Documentor window.
	/// </summary>
	public class DocumentorVisibilityToggleButton : ContextMenuButton
	{
		
		#region DocumentorVisibilityToggleButton Variables

		/// <summary>
		/// Internal storage for the <see cref="ResourceManager"/> property.
		/// </summary>
		private System.Resources.ResourceManager resMgr = null;

		#endregion
  
  
  
		#region DocumentorVisibilityToggleButton Properties
		
		/// <summary>
		/// Gets or sets a resource manager for string lookup.
		/// </summary>
		/// <value>
		/// A <see cref="System.Resources.ResourceManager"/> used for looking up strings.
		/// </value>
		public virtual System.Resources.ResourceManager ResourceManager{
			get {
				return resMgr;
			}
			set {
				resMgr = value;
			}
		}

		/// <summary>
		/// Dynamically calculates the caption based on the visibility of
		/// the Documentor window.
		/// </summary>
		/// <value>
		/// A <see cref="System.String"/> with the results of looking up
		/// the base caption string and appending "Visible" or "Hidden" to
		/// that, using it as a resource ID.
		/// </value>
		public override string Caption {
			get {
				string baseResourceID = base.Caption;
				string qualifier = "Hidden";
				if(DocumentorWindow.CurrentlyVisible){
					qualifier = "Visible";
				}
				return resMgr.GetString(String.Format("{0}.{1}", baseResourceID, qualifier));
			}
			set {
				base.Caption = value;
			}
		}
  
		#endregion
  
  
  
		#region DocumentorVisibilityToggleButton Implementation
  
		#region Constructors
  
		/// <summary>
		/// Initializes a new instance of the <see cref="DocumentorVisibilityToggleButton"/> class.
		/// </summary>
		public DocumentorVisibilityToggleButton() {
		}
  
		#endregion
  
		#region Overrides

		/// <summary>
		/// Toggles the visibility of the Documentor tool window.
		/// </summary>
		public override void Execute() {
			Log.Enter(ImageType.Info, "Toggling visibility of CR_Documentor window.");
			try{
				if(DocumentorWindow.CurrentlyVisible){
					Log.Send("Hiding CR_Documentor window.");
					DocumentorWindow.HideWindow();
				}
				else{
					Log.Send("Showing CR_Documentor window.");
					DocumentorWindow.ShowWindow();
				}
				Log.Send("Visibility toggle complete.");
			}
			catch(Exception err){
				Log.SendException(err);
			}
			finally{
				Log.Exit();
			}
		}

		#endregion
  
		#endregion
	}
}