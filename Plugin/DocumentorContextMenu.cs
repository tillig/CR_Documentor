using System;
using System.Collections.ObjectModel;
using System.Resources;
using System.Windows.Forms;
using CR_Documentor.ContextMenu;
using CR_Documentor.ContextMenu.Button;
using CR_Documentor.ContextMenu.Popup;
using CR_Documentor.Diagnostics;
using CR_Documentor.Options;
using DevExpress.CodeRush.Core;
using DevExpress.CodeRush.Menus;
using DevExpress.CodeRush.PlugInCore;
using System.Threading;

namespace CR_Documentor
{
	/// <summary>
	/// Provides context-sensitive help for creating and editing XML document comments.
	/// </summary>
	public class DocumentorContextMenu : StandardPlugIn
	{
		/// <summary>
		/// Log entry handler.
		/// </summary>
		private static readonly ILog Log = LogManager.GetLogger(typeof(DocumentorContextMenu));

		/// <summary>
		/// Standard context requiring "Any Selection."
		/// </summary>
		protected const DXCoreContext.SelectionContext AnySelection = DXCoreContext.SelectionContext.LineFragment | DXCoreContext.SelectionContext.WholeLine | DXCoreContext.SelectionContext.MultiLines;

		/// <summary>
		/// Object providing VS events to act on.
		/// </summary>
		private DevExpress.DXCore.PlugInCore.DXCoreEvents crEvents;

		/// <summary>
		/// Component collection used by designer.
		/// </summary>
		private System.ComponentModel.IContainer components;

		/// <summary>
		/// Rendering options from the option page.
		/// </summary>
		private OptionSet options;

		/// <summary>
		/// The context menu this plugin will be working with.
		/// </summary>
		private IMenuPopup contextMenu = null;

		/// <summary>
		/// A resource manager allowing us to internationalize strings.
		/// </summary>
		private ResourceManager resourceManager = null;

		/// <summary>
		/// The list of context menu items.
		/// </summary>
		private Collection<ContextMenuItem> children = new Collection<ContextMenuItem>();

		/// <summary>
		/// Gets or sets the <see cref="OptionSet"/> associated with this plugin.
		/// </summary>
		/// <value>A <see cref="OptionSet"/> with rendering options.</value>
		public virtual OptionSet Options
		{
			get
			{
				return options;
			}
			set
			{
				if (value != null)
				{
					options = value;
				}
			}
		}

		/// <summary>
		/// Gets a <see cref="System.Collections.ObjectModel.Collection{T}"/> containing all of the
		/// items that show up in the context menus.
		/// </summary>
		/// <value>A <see cref="System.Collections.ObjectModel.Collection{T}"/> with the menu items.</value>
		public virtual Collection<ContextMenuItem> Children
		{
			get
			{
				return children;
			}
		}

		/// <summary>
		/// Gets the XML document comment prefix used for the current language (not including any whitespace).
		/// </summary>
		/// <value>A <see cref="System.String"/> with the doc comment prefix.</value>
		public static string CommentPrefix
		{
			get
			{
				LanguageExtensionBase lActiveLanguage = CodeRush.Language.ActiveExtension;
				string retVal = "///";
				if (lActiveLanguage != null)
				{
					retVal = lActiveLanguage.XMLDocCommentBegin;
				}
				return retVal;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DocumentorContextMenu"/> class.
		/// </summary>
		public DocumentorContextMenu()
		{
			// Required for Windows.Forms Class Composition Designer support
			InitializeComponent();

			// Refresh settings from storage.
			RefreshSettings();
		}

		/// <summary>
		/// Performs initialization functions.
		/// </summary>
		public override void InitializePlugIn()
		{
			base.InitializePlugIn();
			Log.Write(LogLevel.Info, "Initializing CR_Documentor context menu plugin.");

			// Create resource manager for string localization.
			resourceManager = new ResourceManager("CR_Documentor.Resources.Strings", typeof(DocumentorContextMenu).Assembly);

			// Add event handlers
			this.crEvents.OptionsChanged += crEvents_OptionsChanged;
			this.crEvents.EditorMouseDown += crEvents_EditorMouseDown;

			// Refresh the settings/options
			this.RefreshSettings();
		}


		/// <summary>
		/// Performs finalization functions.
		/// </summary>
		public override void FinalizePlugIn()
		{
			base.FinalizePlugIn();
			Log.Write(LogLevel.Info, "Finalizing CR_Documentor context menu plugin.");

			// Remove event handlers
			this.crEvents.OptionsChanged -= crEvents_OptionsChanged;
			this.crEvents.EditorMouseDown -= crEvents_EditorMouseDown;
		}

		/// <summary>
		/// Handles the options changed event.  Refreshes options from storage.
		/// </summary>
		/// <param name="ea">Event args.</param>
		private void crEvents_OptionsChanged(OptionsChangedEventArgs ea)
		{
			this.RefreshSettings();
		}

		/// <summary>
		/// Handles a mouse down event in the code editor.
		/// </summary>
		/// <param name="ea">Event args.</param>
		private void crEvents_EditorMouseDown(EditorMouseEventArgs ea)
		{
			// Check to see if this is a right-mouse-click - don't handle non-right buttons
			if (ea.Button != MouseButtons.Right)
			{
				return;
			}

			// It's the right mouse button - rebuild the context menu
			// Get the editor context menu
			Log.Write(LogLevel.Info, "CR_Documentor context menu handling right-click event.");
			MenuBar editorContextMenu = DevExpress.CodeRush.VSCore.Manager.Menus.Bars[VsCommonBar.EditorContext];

			// Clear the XML doc context menu
			if (this.contextMenu != null)
			{
				Log.Write(LogLevel.Info, "CR_Documentor context menu popup exists; removing in preparation for refresh.");

				// Clear all items
				foreach (ContextMenuItem item in this.Children)
				{
					if (item is ContextMenuPopup)
					{
						((ContextMenuPopup)item).ClearItems();
					}
				}

				// Remove the items in descending order so the item
				// collection doesn't reorder on you mid-removal.
				for (int i = this.contextMenu.Count - 1; i >= 0; i--)
				{
					if (this.contextMenu[i] != null)
					{
						this.contextMenu[i].Delete();
					}
				}

				// Delete the menu itself
				this.contextMenu.Delete();
				this.contextMenu = null;
			}

			// Add the context menu to the editor context menu.
			Log.Write(LogLevel.Info, "Adding CR_Documentor context menu popup to editor context menu.");
			this.contextMenu = editorContextMenu.AddPopup();
			this.contextMenu.Caption = resourceManager.GetString("CR_Documentor.DocumentorContextMenu.ContextMenuCaption");

			// Rebuild the context menu
			foreach (ContextMenuItem item in this.Children)
			{
				item.Render(this.contextMenu, new MenuButtonClickEventHandler(this.contextMenuButton_Click));
			}
		}

		/// <summary>
		/// Handles the click event for context menu buttons.
		/// </summary>
		/// <param name="sender">The button being clicked.</param>
		/// <param name="e">Event arguments.</param>
		private void contextMenuButton_Click(object sender, MenuButtonClickEventArgs e)
		{
			// Find the item with the corresponding tag
			ContextMenuButton selected = null;
			string tagToFind = e.Button.Tag;
			Log.Write(LogLevel.Info, String.Format("Handling button [{0}].", tagToFind));
			foreach (ContextMenuItem item in this.Children)
			{
				if (item is ContextMenuButton)
				{
					if (((ContextMenuButton)item).Tag == tagToFind)
					{
						selected = (ContextMenuButton)item;
						break;
					}
				}
				else if (item is ContextMenuPopup)
				{
					selected = ((ContextMenuPopup)item).GetButtonByTag(tagToFind);
					if (selected != null)
					{
						break;
					}
				}
			}

			// Execute the item
			if (selected != null)
			{
				Log.Write(LogLevel.Info, String.Format("Found button of type [{0}]; executing.", selected.GetType().Name));
				selected.Execute();
			}
		}

		/// <summary>
		/// Adds the primary template blocks to the parent popup.
		/// </summary>
		/// <param name="popup">The parent to add the submenu to.</param>
		/// <param name="tag">The tag for the added submenu.</param>
		/// <param name="caption">The caption for the added submenu.</param>
		/// <param name="embed"><see langword="true" /> to embed a selection; <see langword="false"/> to replace the selection.</param>
		protected virtual void AddPrimaryBlocks(ContextMenuPopup popup, string tag, string caption, bool embed)
		{
			ContextMenuPopup subPopup = null;
			ContextMenuItem item = null;

			string replacement = "";
			if (embed)
			{
				replacement = TextTemplateContextMenuButton.TEXT_REPLACEMENT;
			}

			// Primary Blocks
			subPopup = MenuBuilder.NewPopup(popup, tag, caption);
			if (this.Options.RecognizedTags.Contains("summary"))
			{
				// <summary>
				item = MenuBuilder.NewTemplateButton(subPopup,
					tag + "Summary",
					String.Format("\n<summary>\n{0}\n</summary>\n", replacement),
					"<summary />");
				if (embed)
				{
					this.SetSelectionRequirements(item, AnySelection);
				}
			}
			if (!embed)
			{
				// Threadsafety doesn't take any content
				if (this.Options.RecognizedTags.Contains("threadsafety"))
				{
					// <threadsafety>
					item = MenuBuilder.NewTemplateButton(subPopup,
						tag + "ThreadSafety",
						"\n<threadsafety static=\"true\" instance=\"false\" />\n",
						"<threadsafety />");
				}
			}
			if (this.Options.RecognizedTags.Contains("param"))
			{
				// <param>
				item = MenuBuilder.NewTemplateButton(subPopup,
					tag + "Param",
					String.Format("\n<param name=\"\">{0}</param>\n", replacement),
					"<param />");
				if (embed)
				{
					this.SetSelectionRequirements(item, AnySelection);
				}
			}
			if (this.Options.RecognizedTags.Contains("returns"))
			{
				// <returns>
				item = MenuBuilder.NewTemplateButton(subPopup,
					tag + "Returns",
					String.Format("\n<returns>\n{0}\n</returns>\n", replacement),
					"<returns />");
				if (embed)
				{
					this.SetSelectionRequirements(item, AnySelection);
				}
			}
			if (this.Options.RecognizedTags.Contains("value"))
			{
				// <value>
				item = MenuBuilder.NewTemplateButton(subPopup,
					tag + "Value",
					String.Format("\n<value>\n{0}\n</value>\n", replacement),
					"<value />");
				if (embed)
				{
					this.SetSelectionRequirements(item, AnySelection);
				}
			}
			if (this.Options.RecognizedTags.Contains("remarks"))
			{
				// <remarks>
				item = MenuBuilder.NewTemplateButton(subPopup,
					tag + "Remarks",
					String.Format("\n<remarks>\n{0}\n</remarks>\n", replacement),
					"<remarks />");
				if (embed)
				{
					this.SetSelectionRequirements(item, AnySelection);
				}
			}
			if (this.Options.RecognizedTags.Contains("event"))
			{
				// <event>
				item = MenuBuilder.NewTemplateButton(subPopup,
					tag + "Event",
					String.Format("\n<event cref=\"\">\n{0}\n</event>\n", replacement),
					"<event />");
				if (embed)
				{
					this.SetSelectionRequirements(item, AnySelection);
				}
			}
			if (this.Options.RecognizedTags.Contains("exception"))
			{
				// <exception>
				item = MenuBuilder.NewTemplateButton(subPopup,
					tag + "Exception",
					String.Format("\n<exception cref=\"\">\n{0}\n</exception>\n", replacement),
					"<exception />");
				if (embed)
				{
					this.SetSelectionRequirements(item, AnySelection);
				}
			}
			if (this.Options.RecognizedTags.Contains("example") && this.Options.RecognizedTags.Contains("code"))
			{
				// <example><code /></example>
				item = MenuBuilder.NewTemplateButton(subPopup,
					tag + "Example",
					String.Format("\n<example>\n{0}\n<code>\n</code>\n</example>\n", replacement),
					"<example />");
				if (embed)
				{
					this.SetSelectionRequirements(item, AnySelection);
				}
			}
			if (this.Options.RecognizedTags.Contains("permission"))
			{
				// <permission>
				item = MenuBuilder.NewTemplateButton(subPopup,
					tag + "Permission",
					String.Format("\n<permission cref=\"\">\n{0}\n</permission>\n", replacement),
					"<permission />");
				if (embed)
				{
					this.SetSelectionRequirements(item, AnySelection);
				}
			}
			if (this.Options.RecognizedTags.Contains("seealso"))
			{
				// <seealso cref="" />
				item = MenuBuilder.NewTemplateButton(subPopup,
					tag + "SeeAlsoCref",
					String.Format("\n<seealso cref=\"{0}\" />\n", replacement),
					"<seealso cref=\"\" />");
				if (embed)
				{
					this.SetSelectionRequirements(item, DXCoreContext.SelectionContext.LineFragment);
				}
				// <seealso href="" />
				item = MenuBuilder.NewTemplateButton(subPopup,
					tag + "SeeAlsoHref",
					String.Format("\n<seealso href=\"{0}\" />\n", replacement),
					"<seealso href=\"\" />");
				if (embed)
				{
					this.SetSelectionRequirements(item, DXCoreContext.SelectionContext.LineFragment);
				}
			}
			if (!embed)
			{
				// Obsolete, Preliminary and Include don't take content
				if (this.Options.RecognizedTags.Contains("preliminary"))
				{
					// <preliminary>
					item = MenuBuilder.NewTemplateButton(subPopup,
						tag + "Preliminary",
						"\n<preliminary />\n",
						"<preliminary />");
				}
				if (this.Options.RecognizedTags.Contains("obsolete"))
				{
					// <obsolete>
					item = MenuBuilder.NewTemplateButton(subPopup,
						tag + "Obsolete",
						"\n<obsolete />\n",
						"<obsolete />");
				}
				if (this.Options.RecognizedTags.Contains("include"))
				{
					// <include>
					item = MenuBuilder.NewTemplateButton(subPopup,
						tag + "Include",
						"\n<include file='' path='' />\n",
						"<include />");
				}
			}
		}

		/// <summary>
		/// Creates the list of items available in the context menu.
		/// </summary>
		protected virtual void CreateContextMenuItems()
		{
			// TODO: Refactor this method. It's far too long.
			Log.Write(LogLevel.Info, "Creating context menu items.");
			this.Children.Clear();

			ContextMenuPopup popup = null;
			ContextMenuPopup subPopup = null;
			ContextMenuItem item = null;

			// ------------------------------------------------------
			// TEMPLATES
			// ------------------------------------------------------
			popup = MenuBuilder.NewPopup(null, "Templates", resourceManager.GetString("CR_Documentor.DocumentorContextMenu.Templates"));
			popup.Context.Add(DXCoreContext.CTX_InXmlDocComment);

			// <see ...>
			if (this.Options.RecognizedTags.Contains("see"))
			{
				subPopup = MenuBuilder.NewPopup(popup, "See", "<see ... />");
				item = MenuBuilder.NewTemplateButton(subPopup, "TemplatesSeeCref", "<see cref=\"\" />");
				item = MenuBuilder.NewTemplateButton(subPopup, "TemplatesSeeLangwordTrue", "<see langword=\"true\" />");
				item = MenuBuilder.NewTemplateButton(subPopup, "TemplatesSeeLangwordFalse", "<see langword=\"false\" />");
				item = MenuBuilder.NewTemplateButton(subPopup, "TemplatesSeeLangwordNull", "<see langword=\"null\" />");
				item = MenuBuilder.NewTemplateButton(subPopup, "TemplatesSeeLangwordAbstract", "<see langword=\"abstract\" />");
				item = MenuBuilder.NewTemplateButton(subPopup, "TemplatesSeeLangwordSealed", "<see langword=\"sealed\" />");
				item = MenuBuilder.NewTemplateButton(subPopup, "TemplatesSeeLangwordStatic", "<see langword=\"static\" />");
				item = MenuBuilder.NewTemplateButton(subPopup, "TemplatesSeeLangwordVirtual", "<see langword=\"virtual\" />");
				item = MenuBuilder.NewTemplateButton(subPopup, "TemplatesSeeHref", "<see href=\"\" />");
			}

			if (this.Options.RecognizedTags.Contains("list"))
			{
				// <list ...>
				subPopup = MenuBuilder.NewPopup(popup, "List", "<list ... />");
				// <list type="bullet">
				item = MenuBuilder.NewTemplateButton(subPopup,
					"TemplatesListBullet",
					"\n<list type=\"bullet\">\n<item>\n<term></term>\n<description></description>\n</item>\n</list>\n",
					"<list type=\"bullet\" />");
				// <list type="table">
				item = MenuBuilder.NewTemplateButton(subPopup,
					"TemplatesListTable",
					"\n<list type=\"table\">\n<listheader>\n<term></term>\n<description></description>\n</listheader>\n<item>\n<term></term>\n<description></description>\n</item>\n</list>\n",
					"<list type=\"table\" />");
				// <list type="number">
				item = MenuBuilder.NewTemplateButton(subPopup,
					"TemplatesListNumber",
					"\n<list type=\"number\">\n<item>\n<term></term>\n<description></description>\n</item>\n</list>\n",
					"<list type=\"number\" />");
				// <list type="definition">
				item = MenuBuilder.NewTemplateButton(subPopup,
					"TemplatesListDefinition",
					"\n<list type=\"definition\">\n<item>\n<term></term>\n<description></description>\n</item>\n</list>\n",
					"<list type=\"definition\" />");
				// <listheader>
				item = MenuBuilder.NewTemplateButton(subPopup,
					"TemplatesListHeader",
					"\n<listheader>\n<term></term>\n<description></description>\n</listheader>\n",
					"<listheader />");
				// <item>
				item = MenuBuilder.NewTemplateButton(subPopup,
					"TemplatesListItem",
					"\n<item>\n<term></term>\n<description></description>\n</item>\n",
					"<item />");
			}

			// Primary Blocks
			AddPrimaryBlocks(popup, "TemplatesPrimaryBlocks", resourceManager.GetString("CR_Documentor.DocumentorContextMenu.Templates.PrimaryBlocks"), false);

			// Add the templates menu to the main list of popups
			this.Children.Add(popup);

			// ------------------------------------------------------
			// EMBEDDINGS
			// ------------------------------------------------------
			// Create embeddings menu
			popup = new ContextMenuPopupSelectExists();
			popup.Tag = "Embed";
			popup.Caption = resourceManager.GetString("CR_Documentor.DocumentorContextMenu.Embed");
			popup.Parent = null;
			popup.Context.Add(DXCoreContext.CTX_InXmlDocComment);

			// Standard items
			if (this.Options.RecognizedTags.Contains("para"))
			{
				// <para>
				item = MenuBuilder.NewTemplateButton(popup,
					"EmbedPara",
					String.Format("\n<para>\n{0}\n</para>\n", TextTemplateContextMenuButton.TEXT_REPLACEMENT),
					"<para />");
				this.SetSelectionRequirements(item, AnySelection);
			}
			if (this.Options.RecognizedTags.Contains("see") &&
				this.Options.RecognizedTags.Contains("seealso"))
			{
				subPopup = MenuBuilder.NewPopup(popup, "See", "<see/seealso />");

				// <see cref="">
				item = MenuBuilder.NewTemplateButton(subPopup,
					"EmbedSeeCref",
					String.Format("<see cref=\"{0}\" />", TextTemplateContextMenuButton.TEXT_REPLACEMENT),
					"<see cref=\"\" />");
				this.SetSelectionRequirements(item, DXCoreContext.SelectionContext.LineFragment);

				// <seealso cref="">
				item = MenuBuilder.NewTemplateButton(subPopup,
					"EmbedSeeAlsoCref",
					String.Format("<seealso cref=\"{0}\" />", TextTemplateContextMenuButton.TEXT_REPLACEMENT),
					"<seealso cref=\"\" />");
				this.SetSelectionRequirements(item, DXCoreContext.SelectionContext.LineFragment);

				// <see langword="">
				// TODO: Add context so langword only shows up for valid selected words
				// TODO: Add conversion so VB equivalents work for VB and convert the langword appropriately
				item = MenuBuilder.NewTemplateButton(subPopup,
					"EmbedSeeLangword",
					String.Format("<see langword=\"{0}\" />", TextTemplateContextMenuButton.TEXT_REPLACEMENT),
					"<see langword=\"\" />");
				((TextTemplateContextMenuButton)item).ConvertSelectionToLower = true;
				this.SetSelectionRequirements(item, DXCoreContext.SelectionContext.LineFragment);
			}
			if (this.Options.RecognizedTags.Contains("code") &&
				this.Options.RecognizedTags.Contains("c"))
			{
				subPopup = MenuBuilder.NewPopup(popup, "Code", "<code />");

				// <code>
				item = MenuBuilder.NewTemplateButton(subPopup,
					"EmbedCode",
					String.Format("\n<code>\n{0}\n</code>\n", TextTemplateContextMenuButton.TEXT_REPLACEMENT),
					"<code />");
				this.SetSelectionRequirements(item, AnySelection);

				// <c>
				item = MenuBuilder.NewTemplateButton(subPopup,
					"EmbedC",
					String.Format("<c>{0}</c>", TextTemplateContextMenuButton.TEXT_REPLACEMENT),
					"<c />");
				this.SetSelectionRequirements(item, DXCoreContext.SelectionContext.LineFragment);
			}
			if (this.Options.RecognizedTags.Contains("paramref"))
			{
				// <paramref>
				item = MenuBuilder.NewTemplateButton(popup,
					"EmbedParamref",
					String.Format("<paramref name=\"{0}\" />", TextTemplateContextMenuButton.TEXT_REPLACEMENT),
					"<paramref name=\"\" />");
				this.SetSelectionRequirements(item, DXCoreContext.SelectionContext.LineFragment);
			}

			// Add listheader, item
			if (this.Options.RecognizedTags.Contains("list") &&
				this.Options.RecognizedTags.Contains("listheader") &&
				this.Options.RecognizedTags.Contains("item"))
			{
				subPopup = MenuBuilder.NewPopup(popup, "List", resourceManager.GetString("CR_Documentor.DocumentorContextMenu.Embed.List"));
				// <listheader>
				item = MenuBuilder.NewTemplateButton(subPopup,
					"EmbedListHeader",
					String.Format("\n<listheader>\n<term>{0}</term>\n</listheader>\n", TextTemplateContextMenuButton.TEXT_REPLACEMENT),
					"<listheader />");
				this.SetSelectionRequirements(item, AnySelection);
				// <item>
				item = MenuBuilder.NewTemplateButton(subPopup,
					"EmbedListItem",
					String.Format("\n<item>\n<term>{0}</term>\n</item>\n", TextTemplateContextMenuButton.TEXT_REPLACEMENT),
					"<item />");
				this.SetSelectionRequirements(item, AnySelection);
			}

			// Add primary blocks
			AddPrimaryBlocks(popup, "EmbedPrimaryBlocks", resourceManager.GetString("CR_Documentor.DocumentorContextMenu.Embed.PrimaryBlocks"), true);

			// Add embeddings menu
			this.Children.Add(popup);


			// Add "Expand all XML document sections"
			OutlineXmlDocSectionsButton outlineButton = new OutlineXmlDocSectionsButton();
			outlineButton.Caption = resourceManager.GetString("CR_Documentor.DocumentorContextMenu.OutlineExpand");
			outlineButton.ShouldCollapse = false;
			outlineButton.Tag = "ExpandXmlDocSections";
			outlineButton.BeginGroup = true;
			Log.Write(LogLevel.Info, String.Format("Created OutlineXmlDocSectionsButton.  Tag: [{0}].", outlineButton.Tag));
			this.Children.Add(outlineButton);

			// Add "Collapse all XML document sections"
			outlineButton = new OutlineXmlDocSectionsButton();
			outlineButton.Caption = resourceManager.GetString("CR_Documentor.DocumentorContextMenu.OutlineCollapse");
			outlineButton.ShouldCollapse = true;
			outlineButton.Tag = "CollapseXmlDocSections";
			Log.Write(LogLevel.Info, String.Format("Created OutlineXmlDocSectionsButton.  Tag: [{0}].", outlineButton.Tag));
			this.Children.Add(outlineButton);

			// Add "XML Encode" for selection
			XmlEncodeButton xmlEncodeButton = new XmlEncodeButton();
			xmlEncodeButton.Caption = resourceManager.GetString("CR_Documentor.DocumentorContextMenu.XmlEncode");
			xmlEncodeButton.Tag = "XmlEncode";
			Log.Write(LogLevel.Info, String.Format("Created XmlEncodeButton.  Tag: [{0}].", xmlEncodeButton.Tag));
			this.Children.Add(xmlEncodeButton);

			// Add "Convert to XML doc comment" for selection
			ConvertSelectionToCommentButton convertToXmlDocCommentButton = new ConvertSelectionToCommentButton();
			convertToXmlDocCommentButton.Caption = resourceManager.GetString("CR_Documentor.DocumentorContextMenu.ConvertSelectionToXmlDocComment");
			convertToXmlDocCommentButton.Tag = "ConvertSelectionToXmlDocComment";
			convertToXmlDocCommentButton.Context.Add("!" + DXCoreContext.CTX_InXmlDocComment);
			Log.Write(LogLevel.Info, String.Format("Created ConvertSelectionToCommentButton.  Tag: [{0}].", convertToXmlDocCommentButton.Tag));
			this.Children.Add(convertToXmlDocCommentButton);

			// Add "Show/Hide CR_Documentor window" option
			DocumentorVisibilityToggleButton visibleToggle = new DocumentorVisibilityToggleButton();
			visibleToggle.ResourceManager = this.resourceManager;
			visibleToggle.Caption = "CR_Documentor.DocumentorContextMenu.ToggleVisibility";
			visibleToggle.Tag = "ToggleVisibility";
			visibleToggle.BeginGroup = true;
			Log.Write(LogLevel.Info, String.Format("Created DocumentorVisibilityToggleButton.  Tag: [{0}].", visibleToggle.Tag));
			this.Children.Add(visibleToggle);

			Log.Write(LogLevel.Info, "Completed adding context menu items.");
		}

		/// <summary>
		/// Sets the context on a text template button.
		/// </summary>
		/// <param name="item">The menu item to set the context on.</param>
		/// <param name="context">The context the button will require.</param>
		protected virtual void SetSelectionRequirements(ContextMenuItem item, DXCoreContext.SelectionContext context)
		{
			if (item is TextTemplateContextMenuButton)
			{
				((TextTemplateContextMenuButton)item).SelectionRequirements = context;
			}
		}

		/// <summary>
		/// Refreshes the settings from the options window.
		/// </summary>
		protected virtual void RefreshSettings()
		{
			Log.Write(LogLevel.Info, "Refreshing settings.");
			this.Options = OptionSet.GetOptionSetFromStorage(DocumentorOptions.Storage);
			CreateContextMenuItems();
		}

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.crEvents = new DevExpress.DXCore.PlugInCore.DXCoreEvents(this.components);
			((System.ComponentModel.ISupportInitialize)(this.crEvents)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.crEvents)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();

		}
	}
}