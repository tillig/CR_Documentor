using System;
using System.Collections.Specialized;
using CR_Documentor.Diagnostics;
using DevExpress.CodeRush.Core;

namespace CR_Documentor.Options
{
	/// <summary>
	/// Provides a structure where Documentor rendering options can be stored.
	/// </summary>
	public class OptionSet
	{
		/// <summary>
		/// Log entry handler.
		/// </summary>
		private static readonly ILog Log = LogManager.GetLogger(typeof(OptionSet));

		#region Constants

		#region Section: Document Tag Compatibility

		/// <summary>
		/// The section that will contain the set of documentation tag compatibility
		/// settings.
		/// </summary>
		public const string SECTION_DOCTAGCOMPAT = "DocumentationTagCompatibility";

		/// <summary>
		/// The key storing the documentation compatibility level setting.
		/// </summary>
		public const string KEY_DOCTAGCOMPAT_COMPATLEVEL = "CompatibilityLevel";

		/// <summary>
		/// The default value for the documentation compatibility level.
		/// </summary>
		public const TagCompatibilityLevel DEFAULT_DOCTAGCOMPAT_COMPATLEVEL = TagCompatibilityLevel.Sandcastle;

		#endregion

		#region Section: Unrecognized Tag Handling

		/// <summary>
		/// The section that will contain the set of unrecognized tag handling
		/// settings.
		/// </summary>
		public const string SECTION_UNRECOGNIZEDTAGS = "UnrecognizedTagHandling";

		/// <summary>
		/// The key storing the unrecognized tag handling method.
		/// </summary>
		public const string KEY_UNRECOGNIZEDTAGS_HANDLING = "HandlerMethod";

		/// <summary>
		/// The default value for the unrecognized tag handling method.
		/// </summary>
		public const UnrecognizedTagHandlingMethod DEFAULT_UNRECOGNIZEDTAGS_HANDLING = UnrecognizedTagHandlingMethod.StripTagShowContents;

		#endregion

		#region Section: Formatting Options

		/// <summary>
		/// The section that will contain the set of formatting options.
		/// </summary>
		public const string SECTION_FORMATOPTIONS = "Formatting";

		/// <summary>
		/// The key storing whether or not tabs in code blocks should be replaced by spaces.
		/// </summary>
		public const string KEY_FORMATOPTIONS_CODETABSTOSPACES = "CodeTabsToSpaces";

		/// <summary>
		/// The default value for whether we replace tabs in code sections with spaces.
		/// </summary>
		public const bool DEFAULT_FORMATOPTIONS_CODETABSTOSPACES = true;

		/// <summary>
		/// The key storing the number of spaces that replace tabs in code blocks.
		/// </summary>
		public const string KEY_FORMATOPTIONS_CODETABSTOSPACESNUM = "CodeTabsToSpacesNum";

		/// <summary>
		/// The default number of spaces to use when replacing tabs with spaces.
		/// </summary>
		public const UInt16 DEFAULT_FORMATOPTIONS_CODETABSTOSPACESNUM = 4;

		/// <summary>
		/// The key storing whether or not duplicate "see cref" links should be removed.
		/// </summary>
		public const string KEY_FORMATOPTIONS_PROCESSDUPLICATESEELINKS = "ProcessDuplicateSeeLinks";

		/// <summary>
		/// The default value for whether or not duplicate "see cref" links should be removed.
		/// </summary>
		public const bool DEFAULT_FORMATOPTIONS_PROCESSDUPLICATESEELINKS = true;

		/// <summary>
		/// The key storing whether or not included documentation should be processed.
		/// </summary>
		public const string KEY_FORMATOPTIONS_PROCESSINCLUDES = "ProcessIncludes";

		/// <summary>
		/// The default value for whether or not included documentation should be processed.
		/// </summary>
		public const IncludeProcessing DEFAULT_FORMATOPTIONS_PROCESSINCLUDES = IncludeProcessing.None;

		#endregion

		#region Section: Display Options

		/// <summary>
		/// The section that will contain the set of display options.
		/// </summary>
		public const string SECTION_DISPLAYOPTIONS = "Display";

		/// <summary>
		/// The key storing whether or not the documentor window toolbar should display.
		/// </summary>
		public const string KEY_DISPLAYOPTIONS_SHOWTOOLBAR = "ShowToolbar";

		/// <summary>
		/// The default value for whether or not the documentor window toolbar should display.
		/// </summary>
		public const bool DEFAULT_DISPLAYOPTIONS_SHOWTOOLBAR = true;

		#endregion

		#region Section: Preview Style

		// TODO: Consider using GUIDs or something other than type name for the preview style option value.

		/// <summary>
		/// The section that will contain the preview style options.
		/// </summary>
		public const string SECTION_PREVIEWSTYLE = "PreviewStyle";

		/// <summary>
		/// The key storing the style descriptor used to determine how to render the preview.
		/// </summary>
		public const string KEY_PREVIEWSTYLE_STYLEDESCRIPTOR = "StyleDescriptor";

		/// <summary>
		/// The default value for the style descriptor.
		/// </summary>
		public static readonly string DEFAULT_PREVIEWSTYLE_STYLEDESCRIPTOR = OptionSet.BuildStoredTypeName(typeof(CR_Documentor.Transformation.SandcastlePrototype.Engine));

		#endregion

		#region Section: Server Options

		/// <summary>
		/// The section that will contain the server options.
		/// </summary>
		public const string SECTION_SERVEROPTIONS = "ServerOptions";

		/// <summary>
		/// The key storing the port number the preview server listens on.
		/// </summary>
		public const string KEY_SERVEROPTIONS_PORT = "Port";

		/// <summary>
		/// The default value for the server port.
		/// </summary>
		public const UInt16 DEFAULT_SERVEROPTIONS_PORT = 11235;

		#endregion

		#endregion

		/// <summary>
		/// Internal storage for the
		/// <see cref="CR_Documentor.Options.OptionSet.TagCompatibilityLevel" />
		/// property.
		/// </summary>
		/// <seealso cref="CR_Documentor.Options.OptionSet" />
		private TagCompatibilityLevel _tagCompatibilityLevel;

		/// <summary>
		/// Gets or sets a <see cref="Boolean"/> indicating whether tabs in code
		/// blocks get replaced with spaces.
		/// </summary>
		/// <value>True if tabs in code blocks should be replaced with spaces; false otherwise.</value>
		public virtual bool ConvertCodeTabsToSpaces { get; set; }

		/// <summary>
		/// Gets or sets a <see cref="System.UInt16"/> with the number of spaces that replace
		/// tabs in code blocks.
		/// </summary>
		/// <value>
		/// The number of spaces that replace tabs in code blocks.
		/// </value>
		/// <remarks>
		/// <para>
		/// <see cref="ConvertCodeTabsToSpaces"/> must be set to true for this to be active.
		/// </para>
		/// <para>
		/// This property may not be set to less than zero.
		/// </para>
		/// </remarks>
		public virtual UInt16 ConvertCodeTabsToSpacesNum { get; set; }

		/// <summary>
		/// Gets or sets the style the preview window should render in.
		/// </summary>
		/// <value>
		/// A <see cref="System.String"/> with the preview window style descriptor.
		/// </value>
		public virtual string PreviewStyle { get; set; }

		/// <summary>
		/// Gets or sets a <see cref="System.Boolean"/> indicating whether duplicate
		/// "see cref" links should be removed.
		/// </summary>
		/// <value><see langword="true"/> to remove duplicate links; <see langword="false"/> to leave them.</value>
		/// <remarks>
		/// <para>
		/// If duplicate cref links are removed, they are replaced by bold text.
		/// </para>
		/// </remarks>
		public virtual bool ProcessDuplicateSeeLinks { get; set; }

		/// <summary>
		/// Gets or sets a <see cref="Boolean"/> indicating whether included documentation
		/// gets processed.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if included doc should actually be read in and
		/// included; <see langword="false" /> otherwise.
		/// </value>
		public virtual IncludeProcessing ProcessIncludes { get; set; }

		/// <summary>
		/// Gets the current collection of recognized tags based on the tag compatibility
		/// level.
		/// </summary>
		/// <value>A <see cref="System.Collections.Specialized.StringCollection"/> with the list of recognized tags.</value>
		/// <seealso cref="CR_Documentor.Options.OptionSet.TagCompatibilityLevel" />
		public virtual StringCollection RecognizedTags { get; private set; }

		/// <summary>
		/// Gets or sets the port the web server listens on.
		/// </summary>
		/// <value>
		/// A <see cref="System.UInt16"/> with the port the internal preview web
		/// server should listen on.
		/// </value>
		public virtual UInt16 ServerPort { get; set; }

		/// <summary>
		/// Gets or sets the indicator of whether the documentor window toolbar should be
		/// visible.
		/// </summary>
		/// <value>
		/// A <see cref="System.Boolean"/> indicating the visibility preference for the
		/// documentor window toolbar.
		/// </value>
		public virtual bool ShowToolbar { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="CR_Documentor.Options.TagCompatibilityLevel"/>
		/// for the transform.
		/// </summary>
		/// <value>
		/// A <see cref="CR_Documentor.Options.TagCompatibilityLevel"/> with the current
		/// tag compatibility level.
		/// </value>
		public virtual TagCompatibilityLevel TagCompatibilityLevel
		{
			get
			{
				return _tagCompatibilityLevel;
			}
			set
			{
				_tagCompatibilityLevel = value;
				RebuildRecognizedTags();
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="CR_Documentor.Options.UnrecognizedTagHandlingMethod"/>
		/// for the transform.
		/// </summary>
		/// <value>
		/// A <see cref="CR_Documentor.Options.UnrecognizedTagHandlingMethod"/> with the current
		/// unrecognized tag handling method.
		/// </value>
		public virtual UnrecognizedTagHandlingMethod UnrecognizedTagHandlingMethod { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="OptionSet"/> class.
		/// </summary>
		public OptionSet()
		{
			this.ConvertCodeTabsToSpaces = DEFAULT_FORMATOPTIONS_CODETABSTOSPACES;
			this.ConvertCodeTabsToSpacesNum = DEFAULT_FORMATOPTIONS_CODETABSTOSPACESNUM;
			this.PreviewStyle = DEFAULT_PREVIEWSTYLE_STYLEDESCRIPTOR;
			this.ProcessDuplicateSeeLinks = DEFAULT_FORMATOPTIONS_PROCESSDUPLICATESEELINKS;
			this.ProcessIncludes = DEFAULT_FORMATOPTIONS_PROCESSINCLUDES;
			this.RecognizedTags = new StringCollection();
			this.ServerPort = DEFAULT_SERVEROPTIONS_PORT;
			this.ShowToolbar = DEFAULT_DISPLAYOPTIONS_SHOWTOOLBAR;
			this.UnrecognizedTagHandlingMethod = DEFAULT_UNRECOGNIZEDTAGS_HANDLING;

			// Set the tag compatibility level which will automatically rebuild the recognized tag list.
			this.TagCompatibilityLevel = DEFAULT_DOCTAGCOMPAT_COMPATLEVEL;
		}

		/// <summary>
		/// Builds a type name that can be stored and used in a <see cref="System.Type.GetType(string)"/> call.
		/// </summary>
		/// <param name="type">The type to get the name for.</param>
		/// <returns>
		/// A string similar to <see cref="System.Type.AssemblyQualifiedName"/>
		/// but omitting the assembly's version information.
		/// </returns>
		/// <remarks>
		/// <para>
		/// This is needed so rendering engine type names can be stored and won't
		/// have issues when a user upgrades the plugin to a new version.
		/// </para>
		/// </remarks>
		public static string BuildStoredTypeName(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			return type.FullName + ", " + type.Assembly.FullName;
		}

		/// <summary>
		/// Reads an option set in from decoupled storage.
		/// </summary>
		/// <param name="storage">The storage set to read the data from.</param>
		/// <returns>An <see cref="OptionSet"/> with the settings from storage.</returns>
		public static OptionSet GetOptionSetFromStorage(DecoupledStorage storage)
		{
			OptionSet retVal = new OptionSet();
			retVal.TagCompatibilityLevel = storage.SafeReadEnum<TagCompatibilityLevel>(SECTION_DOCTAGCOMPAT, KEY_DOCTAGCOMPAT_COMPATLEVEL, DEFAULT_DOCTAGCOMPAT_COMPATLEVEL, Log);
			retVal.UnrecognizedTagHandlingMethod = storage.SafeReadEnum<UnrecognizedTagHandlingMethod>(SECTION_UNRECOGNIZEDTAGS, KEY_UNRECOGNIZEDTAGS_HANDLING, DEFAULT_UNRECOGNIZEDTAGS_HANDLING, Log);
			retVal.ConvertCodeTabsToSpaces = storage.SafeReadBoolean(SECTION_FORMATOPTIONS, KEY_FORMATOPTIONS_CODETABSTOSPACES, DEFAULT_FORMATOPTIONS_CODETABSTOSPACES, Log);
			retVal.ConvertCodeTabsToSpacesNum = Convert.ToUInt16(storage.SafeReadInt32(SECTION_FORMATOPTIONS, KEY_FORMATOPTIONS_CODETABSTOSPACESNUM, DEFAULT_FORMATOPTIONS_CODETABSTOSPACESNUM, Log));
			retVal.ProcessDuplicateSeeLinks = storage.SafeReadBoolean(SECTION_FORMATOPTIONS, KEY_FORMATOPTIONS_PROCESSDUPLICATESEELINKS, DEFAULT_FORMATOPTIONS_PROCESSDUPLICATESEELINKS, Log);
			retVal.ProcessIncludes = storage.SafeReadEnum<IncludeProcessing>(SECTION_FORMATOPTIONS, KEY_FORMATOPTIONS_PROCESSINCLUDES, DEFAULT_FORMATOPTIONS_PROCESSINCLUDES, Log);
			retVal.ServerPort = Convert.ToUInt16(storage.SafeReadInt32(SECTION_SERVEROPTIONS, KEY_SERVEROPTIONS_PORT, DEFAULT_SERVEROPTIONS_PORT, Log));
			retVal.ShowToolbar = storage.SafeReadBoolean(SECTION_DISPLAYOPTIONS, KEY_DISPLAYOPTIONS_SHOWTOOLBAR, DEFAULT_DISPLAYOPTIONS_SHOWTOOLBAR, Log);
			retVal.PreviewStyle = storage.SafeReadString(SECTION_PREVIEWSTYLE, KEY_PREVIEWSTYLE_STYLEDESCRIPTOR, DEFAULT_PREVIEWSTYLE_STYLEDESCRIPTOR, Log);
			return retVal;
		}

		// TODO: Should I even bother with tag compatibility?  Is it even used?  Maybe it should be entirely tied to the rendering engine?
		/// <summary>
		/// Rebuilds the list of recognized tags based on the current tag compatibility level.
		/// </summary>
		protected virtual void RebuildRecognizedTags()
		{
			this.RecognizedTags.Clear();

			// Add the common MSDN tags
			this.RecognizedTags.AddRange(new string[]{
													 "c",
													 "code",
													 "description",
													 "example",
													 "exception",
													 "include",
													 "item",
													 "list",
													 "listheader",
													 "member",
													 "para",
													 "param",
													 "paramref",
													 "permission",
													 "remarks",
													 "returns",
													 "see",
													 "seealso",
													 "summary",
													 "term",
													 "typeparam",
													 "typeparamref",
													 "value"});

			// Add the NDoc tags
			if (_tagCompatibilityLevel == TagCompatibilityLevel.NDoc1_3)
			{
				this.RecognizedTags.AddRange(new string[]{
														 "block",
														 "event",
														 "exclude",
														 "note",
														 "obsolete",
														 "overloads",
														 "preliminary",
														 "threadsafety"
													 });
			}

			// Add the Sandcastle tags (Sandcastle explicitly recognizes HTML)
			else if (_tagCompatibilityLevel == TagCompatibilityLevel.Sandcastle)
			{
				this.RecognizedTags.AddRange(new string[]{
														 "exclude",
														 "note",
														 "overloads",
														 "preliminary",
														 "threadsafety",
														 "a",
														 "abbr",
														 "acronym",
														 "b",
														 "blockquote",
														 "dd",
														 "del",
														 "div",
														 "dl",
														 "dt",
														 "em",
														 "h1",
														 "h2",
														 "h3",
														 "h4",
														 "h5",
														 "h6",
														 "i",
														 "img",
														 "li",
														 "ol",
														 "p",
														 "pre",
														 "span",
														 "strong",
														 "sub",
														 "sup",
														 "table",
														 "td",
														 "th",
														 "tr",
														 "ul"
				});
			}
		}

		/// <summary>
		/// Saves the options to decoupled storage.
		/// </summary>
		/// <param name="storage">The storage set to save the options to.</param>
		public virtual void Save(DecoupledStorage storage)
		{
			storage.LanguageID = "";
			storage.WriteEnum(SECTION_DOCTAGCOMPAT, KEY_DOCTAGCOMPAT_COMPATLEVEL, this.TagCompatibilityLevel);
			storage.WriteEnum(SECTION_UNRECOGNIZEDTAGS, KEY_UNRECOGNIZEDTAGS_HANDLING, this.UnrecognizedTagHandlingMethod);
			storage.WriteBoolean(SECTION_FORMATOPTIONS, KEY_FORMATOPTIONS_CODETABSTOSPACES, this.ConvertCodeTabsToSpaces);
			storage.WriteInt32(SECTION_FORMATOPTIONS, KEY_FORMATOPTIONS_CODETABSTOSPACESNUM, this.ConvertCodeTabsToSpacesNum);
			storage.WriteBoolean(SECTION_FORMATOPTIONS, KEY_FORMATOPTIONS_PROCESSDUPLICATESEELINKS, this.ProcessDuplicateSeeLinks);
			storage.WriteEnum(SECTION_FORMATOPTIONS, KEY_FORMATOPTIONS_PROCESSINCLUDES, this.ProcessIncludes);
			storage.WriteBoolean(SECTION_DISPLAYOPTIONS, KEY_DISPLAYOPTIONS_SHOWTOOLBAR, this.ShowToolbar);
			storage.WriteString(SECTION_PREVIEWSTYLE, KEY_PREVIEWSTYLE_STYLEDESCRIPTOR, this.PreviewStyle);
			storage.WriteInt32(SECTION_SERVEROPTIONS, KEY_SERVEROPTIONS_PORT, this.ServerPort);
			storage.UpdateStorage();
		}
	}
}
