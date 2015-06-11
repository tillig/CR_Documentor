using System;

namespace DocTestCS.Tags.Inline
{
	/// <summary>
	/// This class tests the 'para' tag.
	/// </summary>
	public class Para
	{
		/// <summary>
		/// <para>
		/// This is the first paragraph in the summary.
		/// </para>
		/// <para>
		/// This is the second paragraph in the summary.
		/// </para>
		/// </summary>
		public void Standard()
		{
		}

		/// <summary>
		/// <para lang="C#">
		/// This is a C#-only paragraph.
		/// </para>
		/// <para lang="Visual Basic">
		/// This is a VB-only paragraph.
		/// </para>
		/// </summary>
		public void Lang()
		{
		}

		/// <summary>
		/// <para>
		/// This is the first paragraph in the summary.
		/// </para>
		/// <para>
		/// This is the second paragraph in the summary.
		/// </para>
		/// <para lang="C#">
		/// This is a C#-only paragraph.
		/// </para>
		/// </summary>
		public void Mixed()
		{
		}
	}
}
