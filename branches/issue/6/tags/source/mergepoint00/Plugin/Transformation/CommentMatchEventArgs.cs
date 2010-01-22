using System;
using System.Xml;

namespace CR_Documentor.Transformation
{
	/// <summary>
	/// Event arguments for matching an XML comment node.
	/// </summary>
	public sealed class CommentMatchEventArgs : EventArgs
	{
		/// <summary>
		/// Gets the XML element matched in the comments.
		/// </summary>
		/// <value>
		/// A <see cref="System.Xml.XmlElement"/> from a doc comment.
		/// </value>
		public XmlElement Element { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="CR_Documentor.Transformation.CommentMatchEventArgs"/> class.
		/// </summary>
		/// <param name="element">The matched XML element.</param>
		/// <exception cref="System.ArgumentNullException">
		/// Thrown if <paramref name="element" /> is <see langword="null" />.
		/// </exception>
		public CommentMatchEventArgs(XmlElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			this.Element = element;
		}
	}
}
