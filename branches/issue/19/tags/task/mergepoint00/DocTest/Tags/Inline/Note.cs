using System;

namespace DocTest.Tags.Inline {
	/// <summary>
	/// This class tests the 'note' tag.
	/// </summary>
	public class Note {
		/// <summary>
		/// This is a 'caution' note:
		/// <note type=" caution">
		/// Caution note text.
		/// </note>
		/// </summary>
		public void Caution(){
		}

		/// <summary>
		/// This is an 'implementnotes' note:
		/// <note type=" implementnotes">
		/// Implementation note text.
		/// </note>
		/// </summary>
		public void ImplementNotes(){
		}

		/// <summary>
		/// This is an 'inheritinfo' note:
		/// <note type=" inheritinfo">
		/// Inherit info note text.
		/// </note>
		/// </summary>
		public void InheritInfo(){
		}
	}
}
