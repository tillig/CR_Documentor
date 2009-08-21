using System;
using CR_Documentor.Diagnostics;

namespace CR_Documentor.ContextMenu.Button
{
	/// <summary>
	/// A button used specifically to toggle the visibility of the CR_Documentor window.
	/// </summary>
	public class DocumentorVisibilityToggleButton : ContextMenuButton
	{
		/// <summary>
		/// Log entry handler.
		/// </summary>
		private static readonly ILog Log = LogManager.GetLogger(typeof(DocumentorVisibilityToggleButton));

		/// <summary>
		/// Gets or sets a resource manager for string lookup.
		/// </summary>
		/// <value>
		/// A <see cref="System.Resources.ResourceManager"/> used for looking up strings.
		/// </value>
		public virtual System.Resources.ResourceManager ResourceManager { get; set; }

		/// <summary>
		/// Dynamically calculates the caption based on the visibility of
		/// the Documentor window.
		/// </summary>
		/// <value>
		/// A <see cref="System.String"/> with the results of looking up
		/// the base caption string and appending "Visible" or "Hidden" to
		/// that, using it as a resource ID.
		/// </value>
		public override string Caption
		{
			get
			{
				string baseResourceID = base.Caption;
				string qualifier = "Hidden";
				if (DocumentorWindow.CurrentlyVisible)
				{
					qualifier = "Visible";
				}
				return this.ResourceManager.GetString(String.Format("{0}.{1}", baseResourceID, qualifier));
			}
			set
			{
				base.Caption = value;
			}
		}

		/// <summary>
		/// Toggles the visibility of the Documentor tool window.
		/// </summary>
		public override void Execute()
		{
			using (ActivityContext context = new ActivityContext(Log, "Toggling visibility of CR_Documentor window."))
			{
				try
				{
					if (DocumentorWindow.CurrentlyVisible)
					{
						Log.Write(LogLevel.Info, "Hiding window.");
						DocumentorWindow.HideWindow();
					}
					else
					{
						Log.Write(LogLevel.Info, "Showing window.");
						DocumentorWindow.ShowWindow();
					}
					Log.Write(LogLevel.Info, "Visibility toggle complete.");
				}
				catch (Exception err)
				{
					Log.Write(LogLevel.Error, "Error while toggling window visibility.", err);
				}
			}
		}
	}
}
