using System;

namespace CR_Documentor.Resources
{
	/// <summary>
	/// Attribute for marking an embedded resource as a file that can be served
	/// by the preview server.
	/// </summary>
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
	public class EmbeddedFileAttribute : Attribute
	{
		/// <summary>
		/// Gets the file name.
		/// </summary>
		/// <value>
		/// A <see cref="System.String"/> that is a short, unique filename and
		/// will be used in the URL to the resource.
		/// </value>
		public string FileName { get; private set; }

		/// <summary>
		/// Gets the mime type.
		/// </summary>
		/// <value>
		/// A <see cref="System.String"/> that indicates the mime type of the resource
		/// to be served.
		/// </value>
		public string MimeType { get; private set; }

		/// <summary>
		/// Gets the embedded resource path.
		/// </summary>
		/// <value>
		/// A <see cref="System.String"/> that contains the full path to the
		/// embedded resource in the assembly.
		/// </value>
		public string ResourcePath { get; private set; }


		/// <summary>
		/// Initializes a new instance of the <see cref="CR_Documentor.Resources.EmbeddedFileAttribute" /> class.
		/// </summary>
		/// <param name="fileName">The short, unique file name.</param>
		/// <param name="mimeType">The resource mime type.</param>
		/// <param name="resourcePath">The path to the embedded resource in the assembly.</param>
		/// <exception cref="System.ArgumentNullException">
		/// Thrown if any parameter is <see langword="null" />.
		/// </exception>
		/// <exception cref="System.ArgumentException">
		/// Thrown if any parameter is empty.
		/// </exception>
		public EmbeddedFileAttribute(string resourcePath, string fileName, string mimeType)
		{
			if (resourcePath == null)
			{
				throw new ArgumentNullException("resourcePath");
			}
			if (resourcePath.Length == 0)
			{
				throw new ArgumentException("resourcePath may not be empty.", "resourcePath");
			}
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			if (fileName.Length == 0)
			{
				throw new ArgumentException("fileName may not be empty.", "fileName");
			}
			if (mimeType == null)
			{
				throw new ArgumentNullException("mimeType");
			}
			if (mimeType.Length == 0)
			{
				throw new ArgumentException("mimeType may not be empty.", "mimeType");
			}
			this.ResourcePath = resourcePath;
			this.MimeType = mimeType;
			this.FileName = fileName;
		}
	}
}
