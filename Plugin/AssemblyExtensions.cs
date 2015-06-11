using System;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace CR_Documentor
{
	/// <summary>
	/// Extension methods for <see cref="System.Reflection.Assembly"/>.
	/// </summary>
	public static class AssemblyExtensions
	{
		/// <summary>
		/// Loads an embedded icon from 
		/// </summary>
		/// <param name="assembly">The assembly that contains the icon.</param>
		/// <param name="resourcePath">The path to the embedded icon resource.</param>
		/// <returns>
		/// The <see cref="System.Drawing.Icon"/> loaded from the 
		/// </returns>
		public static Icon ReadEmbeddedResourceIcon(this Assembly assembly, string resourcePath)
		{
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}
			if (resourcePath == null)
			{
				throw new ArgumentNullException("resourcePath");
			}
			if (resourcePath == "")
			{
				throw new ArgumentException("Path to embedded resource to read may not be empty.", "resourcePath");
			}

			using (Stream stm = assembly.GetManifestResourceStream(resourcePath))
			{
				if (stm == null)
				{
					throw new ArgumentException("Unable to load icon from embedded resources: " + resourcePath);
				}
				Icon icon = new Icon(stm);
				stm.Close();
				return icon;
			}
		}

		/// <summary>
		/// Reads an embedded resource string from an assembly.
		/// </summary>
		/// <param name="assembly">The <see cref="System.Reflection.Assembly"/> to read the resource from.</param>
		/// <param name="resourcePath">The path to the embedded resource.</param>
		/// <returns>The <see cref="System.String"/> contents of the embedded resource.</returns>
		/// <remarks>
		/// <para>
		/// This method is handy in implementations of <see cref="CR_Documentor.Transformation.TransformEngine.GetHtmlPageTemplate"/>.
		/// </para>
		/// </remarks>
		/// <exception cref="System.ArgumentNullException">
		/// Thrown if <paramref name="assembly" /> or <paramref name="resourcePath" /> are <see langword="null" />.
		/// </exception>
		/// <exception cref="System.ArgumentException">
		/// Thrown if <paramref name="resourcePath" /> is <see cref="System.String.Empty"/>
		/// or if it is not found in the assembly.
		/// </exception>
		public static string ReadEmbeddedResourceString(this Assembly assembly, string resourcePath)
		{
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}
			if (resourcePath == null)
			{
				throw new ArgumentNullException("resourcePath");
			}
			if (resourcePath == "")
			{
				throw new ArgumentException("Path to embedded resource to read may not be empty.", "resourcePath");
			}

			using (Stream stm = assembly.GetManifestResourceStream(resourcePath))
			{
				if (stm == null)
				{
					throw new ArgumentException("The resource path was not found in the assembly.", "resourcePath");
				}
				using (StreamReader sreader = new StreamReader(stm))
				{
					string retVal = sreader.ReadToEnd();
					sreader.Close();
					return retVal;
				}
			}
		}
	}
}
