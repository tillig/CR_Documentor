using System;

namespace DocTestCS.Integration
{
	/// <example>
	/// Example stuff.
	/// <code>
	/// public Class1 myInstance;
	/// </code>
	/// More example
	/// <code lang="C#">
	/// This is C# code.
	/// </code>
	/// </example>
	/// <preliminary />
	/// <remarks>
    /// <para>
    /// Remarks go here. First CREF to string: <see cref="System.String"/>.
    /// Second CREF to string: <see cref="System.String"/>.
    /// </para>
	/// <block type="note">This is an ECMA 'note' block.</block>
	/// <block type="example">This is an ECMA 'example' block.</block>
	/// <block type="behaviors">This is an ECMA 'behaviors' block.</block>
	/// <block type="overrides">This is an ECMA 'overrides' block.</block>
	/// <block type="usage">This is an ECMA 'usage' block.</block>
	/// <block type="default">This is an ECMA 'default' block.</block>
	/// <block type="invalidtype">This is an ECMA 'invalidtype' block.</block>
	/// <block>This is an ECMA block with NO type.</block>
	/// </remarks>
	/// <summary>
	/// Class1 is used to test documentation tags.  Third CREF to string: <see cref="System.String"/>.
	/// </summary>
	/// <threadsafety static="true" instance="false" />
	public class Class1
	{
		/// <summary>
		/// Private field. Should NOT show up in the class summary.
		/// </summary>
		private int _privateField = 0;

		/// <summary>
		/// Internal field. Should show up in the class summary.
		/// </summary>
		internal int _internalField = 0;

		/// <summary>
		/// Protected field. Should show up in the class summary.
		/// </summary>
		protected int _protectedField = 0;

		/// <summary>
		/// Public field. Should show up in the class summary.
		/// </summary>
		public int _publicField = 0;

		#region Class1 Events

		/// <example>
		/// Example description text
		/// </example>
		/// <exception cref="System.ApplicationException">
		/// ApplicationException description
		/// </exception>
		/// <overloads>
		/// This is a summary-only overload line.
		/// </overloads>
		/// <permission cref="System.Security.PermissionSet">Everyone can access this method.</permission>
		/// <preliminary />
		/// <remarks>
		/// This is a remarks line.
		/// </remarks>
		/// <seealso cref="Property" />
		/// <seealso href="http://www.paraesthesia.com" />
		/// <summary>
		/// Event description text
		/// </summary>
		public event EventHandler MyTestEvent;

		#endregion



		#region Class1 Properties

		/// <example>
		/// This example information line is not in para tags.  There is no code.
		/// </example>
		/// <exception cref="System.ApplicationException">
		/// <para>
		/// ApplicationException description para 1
		/// </para>
		/// <para>
		/// ApplicationException description para 2
		/// </para>
		/// </exception>
		/// <overloads>
		/// <summary>Summary for overload</summary>
		/// <remarks>Remarks for overload</remarks>
		/// <example>Example for overload</example>
		/// </overloads>
		/// <permission cref="System.Security.PermissionSet">Everyone can access this method.</permission>
		/// <preliminary>This is a specific preliminary warning.</preliminary>
		/// <remarks>
		/// This is a remarks line.
		/// </remarks>
		/// <seealso cref="Property">See Property!</seealso>
		/// <seealso href="http://www.paraesthesia.com">See Paraesthesia!</seealso>
		/// <summary>
		/// This first line is not in para tags.
		/// <para>This second line IS in para tags.</para>
		/// </summary>
		public string Property
		{
			get
			{
				// Get the private field so the compiler doesn't complain
				// about the unused field.
				return this._privateField.ToString();
			}
		}

		/// <summary>
		/// Indexed property.
		/// </summary>
		/// <value>
		/// No real value.
		/// </value>
		public string this[int index]
		{
			get
			{
				return "foo";
			}
			set
			{
				// Nothing to do
			}
		}

		#endregion



		#region Class1 Implementation

		#region Constructors

		/// <summary>
		/// Summary for Class1 constructor. There's <c>inline code</c>, too.
		/// </summary>
		/// <remarks>
		/// Remarks for Class1 constructor.  These remarks are not in para tags.
		/// </remarks>
		/// <example>
		/// This example information line is not in para tags.
		/// <para>This second line IS in para tags.</para>
		/// <code>
		/// Here's some code.  There's no lang attribute
		/// Here's another line of code.
		/// </code>
		/// This line separates the code.
		/// <code lang="C#">
		/// This is some C# code.
		///		This code has a single tab in front of it.
		///			This code has two tabs.
		/// </code>
		/// Here's another separator line.
		/// <code lang="XML" escaped="true">
		/// <root>
		///  <xmlElement>Text in the xmlElement</xmlElement>
		/// </root>
		/// </code>
		/// </example>
		public Class1()
		{
		}

		#endregion

		#region Methods

		/// <event cref="MyTestEvent">Event description</event>
		/// <example>
		/// This example information line is not in para tags.  There is no code.
		/// </example>
		/// <exception cref="System.ApplicationException">
		/// <para>
		/// ApplicationException description para 1
		/// </para>
		/// <para>
		/// ApplicationException description para 2
		/// </para>
		/// </exception>
		/// <overloads>
		/// <summary>Summary for overload</summary>
		/// <remarks>Remarks for overload</remarks>
		/// <example>Example for overload</example>
		/// </overloads>
		/// <param name="input">One line param description</param>
		/// <param name="otherInput">
		/// <para>Two lines</para>
		/// <para>Describe the param</para>
		/// <para lang="C#">This para has a lang of C#.</para>
		/// </param>
		/// <permission cref="System.Security.Permissions.ReflectionPermission">when invoked through late-bound mechanisms.</permission>
		/// <permission cref="System.Security.Permissions.SecurityPermission">because this is a really secure thing.</permission>
		/// <preliminary>This is a specific preliminary warning.</preliminary>
		/// <remarks>
		/// This is a remarks line.
		/// <note>Note with no type</note>
		/// Note separator
		/// <note type="caution">Caution note</note>
		/// Note separator
		/// <note type="inheritinfo">Inheritinfo note</note>
		/// Note separator
		/// <note type="implementnotes">Implementnotes note</note>
		/// <list type="bullet">
		/// <listheader><term>listheader term</term><description>listheader description</description></listheader>
		/// <listheader><term>listheader term only</term></listheader>
		/// <listheader><description>listheader description only</description></listheader>
		/// <item><term>item term</term><description>item description</description></item>
		/// <item><term>item term only</term></item>
		/// <item><description>item description only</description></item>
		/// <item><term>cref:  <see cref="String.Empty"/></term></item>
		/// <item><term>cref with desc:  <see cref="String.Empty">Empty String</see></term></item>
		/// <item><term>href:  <see href="http://www.paraesthesia.com"/></term></item>
		/// <item><term>href with desc:  <see href="http://www.paraesthesia.com">Paraesthesia</see></term></item>
		/// <item><term>null:  <see langword="null"/></term></item>
		/// <item><term>sealed:  <see langword="sealed"/></term></item>
		/// <item><term>static:  <see langword="static"/></term></item>
		/// <item><term>abstract:  <see langword="abstract"/></term></item>
		/// <item><term>virtual:  <see langword="virtual"/></term></item>
		/// <item><term>true:  <see langword="true"/></term></item>
		/// <item><term>false:  <see langword="false"/></term></item>
		/// <item>
		/// <term>Nested list</term>
		/// <description>
		/// <list type="bullet">
		/// <item>
		/// <term>Nested term</term>
		/// <description>Nested description</description>
		/// </item>
		/// </list>
		/// </description>
		/// </item>
		/// </list>
		/// <list type="number">
		/// <listheader><term>listheader term</term><description>listheader description</description></listheader>
		/// <listheader><term>listheader term only</term></listheader>
		/// <listheader><description>listheader description only</description></listheader>
		/// <item><term>item term</term><description>item description</description></item>
		/// <item><term>item term only</term></item>
		/// <item><description>item description only</description></item>
		/// <item>
		/// <term>Nested list</term>
		/// <description>
		/// <list type="bullet">
		/// <item>
		/// <term>Nested term</term>
		/// <description>Nested description</description>
		/// </item>
		/// </list>
		/// </description>
		/// </item>
		/// </list>
		/// <list type="table">
		/// <listheader><term>listheader term</term><description>listheader description</description></listheader>
		/// <listheader><term>listheader term only</term></listheader>
		/// <listheader><description>listheader description only</description></listheader>
		/// <item><term>item term</term><description>item description</description></item>
		/// <item><term>item term only</term></item>
		/// <item><description>item description only</description></item>
		/// <item>
		/// <term>Nested list</term>
		/// <description>
		/// <list type="bullet">
		/// <item>
		/// <term>Nested term</term>
		/// <description>Nested description</description>
		/// </item>
		/// </list>
		/// </description>
		/// </item>
		/// </list>
		/// <list type="definition">
		/// <listheader><term>listheader term</term><description>listheader description</description></listheader>
		/// <listheader><term>listheader term only</term></listheader>
		/// <listheader><description>listheader description only</description></listheader>
		/// <item><term>item term</term><description>item description</description></item>
		/// <item><term>item term only</term></item>
		/// <item><description>item description only</description></item>
		/// <item>
		/// <term>Nested list</term>
		/// <description>
		/// <list type="bullet">
		/// <item>
		/// <term>Nested term</term>
		/// <description>Nested description</description>
		/// </item>
		/// </list>
		/// </description>
		/// </item>
		/// </list>
		/// </remarks>
		/// <returns>
		/// Always returns <see cref="System.String.Empty"/>.
		/// </returns>
		/// <seealso cref="Property">See Property!</seealso>
		/// <seealso href="http://www.paraesthesia.com">See Paraesthesia!</seealso>
		/// <summary>
		/// This first line is not in para tags.  There are also <b>bold</b> and <i>italic</i>
		/// and <u>underlined</u> words.  Also a <paramref name="input" /> paramref.
		/// </summary>
		public string MyMethod(bool input, bool otherInput)
		{
			if (MyTestEvent != null)
			{
				MyTestEvent(null, null);
			}
			return String.Empty;
		}


		/// <summary>
		/// This method has a lot of HTML tags in the remarks.
		/// </summary>
		/// <remarks>
		/// <p>This is in a 'p' tag.</p>
		/// <div>This is in a 'div' tag.</div>
		/// <ul>
		/// <li>This is the first item in a 'ul'</li>
		/// </ul>
		/// <ol>
		/// <li>This is the first item in an 'ol'</li>
		/// </ol>
		/// <table>
		/// <tr><th>This is a table header.</th></tr>
		/// <tr><td>This is a table cell.</td></tr>
		/// </table>
		/// <para>This text <span style="font-weight: bold">has some spanned bold text</span>.</para>
		/// <para>Standard <b>bold</b>, <i>italic</i>, and <u>underlined</u> text.</para>
		/// <para>This is a <a href="http://www.paraesthesia.com">standard a href link</a>.</para>
		/// <h1>Header 1</h1>
		/// <h2>Header 2</h2>
		/// <h3>Header 3</h3>
		/// </remarks>
		public void MyHtmlDescriptionMethod()
		{
		}

		/// <!--
		/// Includes are added inclusive - so the node that is specified
		/// in XPath is copied in with all of its children.  Generally this
		/// means you specify multiple nodes in XPath like this:
		/// path='doc/members/member[@name="M:Member.Info.Here"]/*'
		/// -->
		///
		/// <!--
		/// Relative include - working folder is wherever CSC is executed from.
		/// In Visual Studio, this is the IDE folder:
		/// c:\Program Files\Microsoft Visual Studio .NET 2003\Common7\IDE
		/// 
		/// Commented out to avoid build warnings. Uncomment when testing.
		/// -->
		/// <!-- include file='.\include.xml' path='doc/members/member[@name="M:Relative.Include"]/*'/ -->
		/// <!-- include file='C:\Documents and Settings\tillig\My Documents\Visual Studio 2008\Projects\CR_Documentor\dev\DocTestCS\Integration\include.xml' path='doc/members/member[@name="M:Absolute.Include"]/*'/ -->
		/// <summary>
		/// <list type="bullet">
		/// <item>
		/// <term>Here's an included bit of doc embedded in the description:</term>
		/// <description>
		/// <!-- include file='C:\Documents and Settings\tillig\My Documents\Visual Studio 2008\Projects\CR_Documentor\dev\DocTestCS\Integration\include.xml' path='doc/members/member[@name="M:Absolute.Include"]/remarks/text()[position() = 1]'/ -->
		/// </description>
		/// </item>
		/// </list>
		/// </summary>
		public void IncludeTest()
		{
		}

		#endregion

		#endregion

	}
}
