using System;

namespace DocTest.Tags.Inline {
	/// <summary>
	/// This is a block of code in a class summary:
	/// <code>
	/// This is the code.
	/// </code>
	/// </summary>
	public class Code {
		/// <summary>
		/// This is a block of code in a method summary with an 'escaped' attribute:
		/// <code escaped="true">
		/// This is the code.  It has an "<element />" in it.
		/// </code>
		/// </summary>
		public void CodeEscaped(){
		}

		/// <summary>
		/// This is a block of code in a method summary with a 'lang' attribute:
		/// <code lang="C#">
		/// This is the code.
		/// </code>
		/// </summary>
		public void CodeLang(){
		}

		/// <summary>
		/// This is a block of code in a method summary with both 'escaped' and 'lang' attributes:
		/// <code lang="C#">
		/// This is the code.  It has an "<element />" in it.
		/// </code>
		/// </summary>
		public void CodeEscapedLang(){
		}
	}
}
