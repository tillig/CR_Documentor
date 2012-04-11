using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Reflection;
using CR_Documentor.Diagnostics;
using CR_Documentor.Resources;

namespace CR_Documentor.Server
{
	/// <summary>
	/// Handler for enumerating and serving embedded files.
	/// </summary>
	public class EmbeddedFileHandler
	{
		/// <summary>
		/// Log entry handler.
		/// </summary>
		private static readonly ILog Log = LogManager.GetLogger(typeof(EmbeddedFileHandler));

		/// <summary>
		/// Gets the cache of the attributes available in the assembly, keyed by the filename.
		/// </summary>
		/// <value>
		/// A <see cref="System.Collections.Generic.IDictionary{K, V}"/>
		/// with the list of attributes on the <see cref="CR_Documentor.Server.EmbeddedFileHandler.Assembly"/>,
		/// keyed by unique file name.
		/// </value>
		public IDictionary<string, EmbeddedFileAttribute> AttributeCache { get; private set; }

		/// <summary>
		/// Gets the assembly for which files will be served.
		/// </summary>
		/// <value>
		/// A <see cref="System.Reflection.Assembly"/> containing embedded resource
		/// files to be served.
		/// </value>
		public Assembly Assembly { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="CR_Documentor.Server.EmbeddedFileHandler" /> class.
		/// </summary>
		/// <param name="assembly">
		/// The assembly containing the embedded files to serve.
		/// </param>
		/// <exception cref="System.InvalidOperationException">
		/// Thrown if the <paramref name="assembly" /> has an <see cref="CR_Documentor.Resources.EmbeddedFileAttribute"/>
		/// that points to a resource not found in the assembly or has a duplicate
		/// filename on an attribute.
		/// </exception>
		public EmbeddedFileHandler(Assembly assembly)
		{
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}
			this.Assembly = assembly;
			this.AttributeCache = new Dictionary<string, EmbeddedFileAttribute>();
			this.EnumerateEmbeddedFileAttributes();
		}

		/// <summary>
		/// Enumerates the embedded file attributes in the assembly and checks
		/// for any duplicates before caching them.
		/// </summary>
		private void EnumerateEmbeddedFileAttributes()
		{
			var attributes = (EmbeddedFileAttribute[])this.Assembly.GetCustomAttributes(typeof(EmbeddedFileAttribute), true);
			var resourceNames = new List<String>(this.Assembly.GetManifestResourceNames());
			foreach (var attribute in attributes)
			{
				if (!resourceNames.Contains(attribute.ResourcePath))
				{
					throw new InvalidOperationException(String.Format(CultureInfo.InvariantCulture, "Found embedded file attribute specifying resource '{0}' that does not exist in assembly '{1}.", attribute.ResourcePath, this.Assembly.FullName));
				}
				if (this.AttributeCache.ContainsKey(attribute.FileName))
				{
					throw new InvalidOperationException(String.Format(CultureInfo.InvariantCulture, "Found duplicate embedded file attribute with filename '{0}' in assembly '{1}.", attribute.FileName, this.Assembly.FullName));
				}
				this.AttributeCache.Add(attribute.FileName, attribute);
			}
		}

		/// <summary>
		/// Writes an embedded file to a listener context.
		/// </summary>
		/// <param name="context">The context to which the file should be written.</param>
		/// <param name="fileName">The short unique filename corresponding to the file to write.</param>
		/// <exception cref="System.ArgumentNullException">
		/// Thrown if <paramref name="context" /> or <paramref name="fileName" />
		/// is <see langword="null" />.
		/// </exception>
		/// <exception cref="System.ArgumentException">
		/// Thrown if <paramref name="fileName" /> is empty.
		/// </exception>
		public void WriteFile(HttpListenerContext context, string fileName)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			if (fileName.Length == 0)
			{
				throw new ArgumentException("fileName may not be empty.", "fileName");
			}
			if (!this.AttributeCache.ContainsKey(fileName))
			{
				context.Response.StatusCode = (int)HttpStatusCode.NotFound;
				context.Response.StatusDescription = "Not Found";
				context.Response.Close();
				Log.Write(LogLevel.Warn, String.Format(CultureInfo.InvariantCulture, "Issuing 404 for requested file '{0}'.", fileName));
				return;
			}
			var attribute = this.AttributeCache[fileName];
			ResponseWriter.WriteResource(context, this.Assembly, attribute.ResourcePath, attribute.MimeType);
		}
	}
}
