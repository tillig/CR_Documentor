using System;

namespace DocTest.Tags.TopLevel
{
	/// <summary>
	/// This class tests the 'include' tag.
	/// </summary>
	public class IncludeClass
	{
		/// <summary>
		/// Absolute include:
		/// [
		/// <include file='C:\Documents and Settings\tillig\My Documents\Visual Studio 2008\Projects\CR_Documentor\dev\DocTest\Tags\TopLevel\include.xml' path='doc/members/member[@name="M:Absolute.Include"]/remarks/text()[position() = 1]'/>
		/// ]
		/// </summary>
		public void AbsoluteInclude()
		{
		}

		/// <summary>
		/// Relative include:
		/// [
		/// <include file='.\include.xml' path='doc/members/member[@name="M:Relative.Include"]/remarks/text()[position() = 1]'/>
		/// ]
		/// </summary>
		public void RelativeInclude()
		{
		}
	}
}
