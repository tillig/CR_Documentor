using System;
using System.IO;

namespace CR_Documentor
{
	/// <summary>
	/// Service class for reading embedded resources.
	/// </summary>
	public static class EmbeddedResource
	{
		/// <summary>
		/// Reads an embedded resource string from an assembly.
		/// </summary>
		/// <param name="asm">The <see cref="System.Reflection.Assembly"/> to read the resource from.</param>
		/// <param name="resourcePath">The path to the embedded resource.</param>
		/// <returns>The <see cref="System.String"/> contents of the embedded resource.</returns>
		/// <remarks>
		/// <para>
		/// This method is handy in implementations of <see cref="CR_Documentor.Transformation.TransformEngine.GetHtmlPageTemplate"/>.
		/// </para>
		/// </remarks>
		/// <exception cref="System.ArgumentNullException">
		/// Thrown if <paramref name="asm" /> or <paramref name="resourcePath" /> are <see langword="null" />.
		/// </exception>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// Thrown if <paramref name="resourcePath" /> is <see cref="System.String.Empty"/>.
		/// </exception>
		public static string ReadEmbeddedResourceString(System.Reflection.Assembly asm, string resourcePath)
		{
			if (asm == null)
			{
				throw new ArgumentNullException("asm", "Assembly to read resource from may not be null.");
			}
			if (resourcePath == null)
			{
				throw new ArgumentNullException("resourcePath", "Path to embedded resource to read may not be null.");
			}
			if (resourcePath == "")
			{
				throw new ArgumentOutOfRangeException("resourcePath", resourcePath, "Path to embedded resource to read may not be null.");
			}

			using (Stream stm = asm.GetManifestResourceStream(resourcePath))
			using (StreamReader sreader = new StreamReader(stm))
			{
				string retVal = sreader.ReadToEnd();
				return retVal;
			}
		}
	}
}
