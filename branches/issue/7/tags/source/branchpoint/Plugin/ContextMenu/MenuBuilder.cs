using System;
using CR_Documentor.ContextMenu.Button;
using CR_Documentor.ContextMenu.Popup;
using CR_Documentor.Diagnostics;

namespace CR_Documentor.ContextMenu
{
	/// <summary>
	/// Provides assistance for building context menus.
	/// </summary>
	public static class MenuBuilder
	{
		/// <summary>
		/// Log entry handler.
		/// </summary>
		private static readonly ILog Log = LogManager.GetLogger(typeof(MenuBuilder));

		/// <summary>
		/// Creates a new context menu popup.
		/// </summary>
		/// <param name="parent">The parent of this popup.</param>
		/// <param name="tag">The tag associated with the context menu.</param>
		/// <param name="caption">The caption that will appear for the menu.</param>
		/// <returns>A <see cref="ContextMenuPopup"/> with the properties set.</returns>
		public static ContextMenuPopup NewPopup(ContextMenuPopup parent, string tag, string caption)
		{
			ContextMenuPopup popup = new ContextMenuPopup();
			popup.Tag = tag;
			popup.Caption = caption;
			popup.Parent = parent;
			if (parent != null)
			{
				parent.Children.Add(popup);
			}
			Log.Write(LogLevel.Info, String.Format("Created popup.  Tag: [{0}]; Full ID: [{1}].", popup.Tag, popup.FullID));
			return popup;
		}

		/// <summary>
		/// Creates a new template/embedding button.
		/// </summary>
		/// <param name="parent">The parent of this button.</param>
		/// <param name="tag">The tag associated with this button.</param>
		/// <param name="content">The template content.</param>
		/// <param name="caption">The text to display on the button.</param>
		/// <returns>A <see cref="TextTemplateContextMenuButton"/> with the properties set.</returns>
		public static TextTemplateContextMenuButton NewTemplateButton(ContextMenuPopup parent, string tag, string content, string caption)
		{
			TextTemplateContextMenuButton item = new TextTemplateContextMenuButton();
			item.Content = content;
			item.Caption = caption;
			item.Tag = tag;
			item.Parent = parent;
			if (parent != null)
			{
				parent.Children.Add(item);
			}
			Log.Write(LogLevel.Info, String.Format("Created TextTemplateContextMenuButton.  Tag: [{0}]; Full ID: [{1}].", item.Tag, item.FullID));
			return item;
		}

		/// <summary>
		/// Creates a new template/embedding button.
		/// </summary>
		/// <param name="parent">The parent of this button.</param>
		/// <param name="tag">The tag associated with this button.</param>
		/// <param name="content">The template content.</param>
		/// <returns>A <see cref="TextTemplateContextMenuButton"/> with the properties set.</returns>
		public static TextTemplateContextMenuButton NewTemplateButton(ContextMenuPopup parent, string tag, string content)
		{
			TextTemplateContextMenuButton item = NewTemplateButton(parent, tag, content, content);
			return item;
		}
	}
}
