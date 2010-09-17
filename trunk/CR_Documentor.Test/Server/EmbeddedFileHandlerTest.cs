using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using CR_Documentor.Resources;
using CR_Documentor.Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeMock.ArrangeActAssert;

namespace CR_Documentor.Test.Server
{
	[TestClass]
	[Isolated]
	public class EmbeddedFileHandlerTest
	{
		[TestMethod]
		public void Ctor_AttributesEnumerated()
		{
			var asm = CreateTestAssembly(
				"EmbeddedFileHandlerTest.Ctor_AttributesEnumerated",
				new EmbeddedFileInfo()
				{
					FileName = "file1.txt",
					ResourcePath = "EmbeddedFileHandlerTest.Ctor_AttributesEnumerated.file1.txt",
					MimeType = "text/plain",
					FileContents = "file1"
				},
				new EmbeddedFileInfo()
				{
					FileName = "file2.txt",
					ResourcePath = "EmbeddedFileHandlerTest.Ctor_AttributesEnumerated.file2.txt",
					MimeType = "text/plain",
					FileContents = "file2"
				});
			var handler = new EmbeddedFileHandler(asm);
			Assert.AreEqual(2, handler.AttributeCache.Count);
			Assert.IsTrue(handler.AttributeCache.ContainsKey("file1.txt"));
			Assert.IsTrue(handler.AttributeCache.ContainsKey("file2.txt"));
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void Ctor_DuplicateFileName()
		{
			var asm = CreateTestAssembly(
				"EmbeddedFileHandlerTest.Ctor_MissingResource",
				new EmbeddedFileInfo()
				{
					FileName = "file.txt",
					ResourcePath = "EmbeddedFileHandlerTest.Ctor_DuplicateFileName.file1.txt",
					MimeType = "text/plain",
					FileContents = "file1"
				},
				new EmbeddedFileInfo()
				{
					FileName = "file.txt",
					ResourcePath = "EmbeddedFileHandlerTest.Ctor_DuplicateFileName.file2.txt",
					MimeType = "text/plain",
					FileContents = "file2"
				});
			new EmbeddedFileHandler(asm);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void Ctor_MissingResource()
		{
			var asm = CreateTestAssembly(
				"EmbeddedFileHandlerTest.Ctor_MissingResource",
				new EmbeddedFileInfo()
				{
					FileName = "file.txt",
					ResourcePath = "EmbeddedFileHandlerTest.Ctor_MissingResource.file.txt",
					MimeType = "text/plain"
				});
			new EmbeddedFileHandler(asm);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Ctor_NullAssembly()
		{
			new EmbeddedFileHandler(null);
		}

		[TestMethod]
		public void Ctor_SetsProperties()
		{
			var asm = Assembly.GetExecutingAssembly();
			var handler = new EmbeddedFileHandler(asm);
			Assert.AreSame(asm, handler.Assembly);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void WriteFile_EmptyFileName()
		{
			var handler = new EmbeddedFileHandler(Assembly.GetExecutingAssembly());
			var context = Isolate.Fake.Instance<HttpListenerContext>();
			handler.WriteFile(context, "");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void WriteFile_NullContext()
		{
			var handler = new EmbeddedFileHandler(Assembly.GetExecutingAssembly());
			handler.WriteFile(null, "filename");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void WriteFile_NullFileName()
		{
			var handler = new EmbeddedFileHandler(Assembly.GetExecutingAssembly());
			var context = Isolate.Fake.Instance<HttpListenerContext>();
			handler.WriteFile(context, null);
		}

		[TestMethod]
		public void WriteFile_FileNotFound()
		{
			var handler = new EmbeddedFileHandler(Assembly.GetExecutingAssembly());
			var context = Isolate.Fake.Instance<HttpListenerContext>(Members.ReturnRecursiveFakes);
			handler.WriteFile(context, "not-found.txt");
			Isolate.Verify.WasCalledWithExactArguments(() => context.Response.StatusCode = 404);
			Isolate.Verify.WasCalledWithExactArguments(() => context.Response.StatusDescription = "Not Found");
			Isolate.Verify.WasCalledWithExactArguments(() => context.Response.Close());
		}

		[TestMethod]
		public void WriteFile_FileFound()
		{
			var asm = CreateTestAssembly(
				"EmbeddedFileHandlerTest.WriteFile_FileFound",
				new EmbeddedFileInfo()
				{
					FileName = "file.txt",
					ResourcePath = "EmbeddedFileHandlerTest.WriteFile_FileFound.file.txt",
					MimeType = "text/plain",
					FileContents = "content"
				});
			var handler = new EmbeddedFileHandler(asm);
			var context = Isolate.Fake.Instance<HttpListenerContext>(Members.ReturnRecursiveFakes);
			Isolate.WhenCalled(() => ResponseWriter.WriteResource(null, null, null, null)).IgnoreCall();
			handler.WriteFile(context, "file.txt");
			Isolate.Verify.WasCalledWithArguments(() => ResponseWriter.WriteResource(null, null, null, null))
				.Matching(args =>
					{
						return
						args[0] == context &&
						(Assembly)args[1] == asm &&
						(string)args[2] == "EmbeddedFileHandlerTest.WriteFile_FileFound.file.txt" &&
						(string)args[3] == "text/plain";
					});
		}

		private class EmbeddedFileInfo
		{
			public string FileName { get; set; }
			public string MimeType { get; set; }
			public string ResourcePath { get; set; }
			public string FileContents { get; set; }
		}

		/// <summary>
		/// Creates a temporary in-memory assembly. DO NOT MAKE A LOT OF THESE.
		/// They will not be released from memory until the entire test AppDomain
		/// is released.
		/// </summary>
		/// <param name="assemblyName">The full name of the assembly, minus the .dll extension. Make this unique!</param>
		/// <param name="attributes">
		/// Definitions for the embedded resource attributes that should be included
		/// on the assembly.
		/// </param>
		/// <returns>
		/// An in-memory assembly with the given characteristics.
		/// </returns>
		private static Assembly CreateTestAssembly(string assemblyName, params EmbeddedFileInfo[] attributes)
		{
			CompilerParameters cp = new CompilerParameters();
			cp.TempFiles = new TempFileCollection();
			cp.TempFiles.KeepFiles = false;

			try
			{
				if (!Directory.Exists(cp.TempFiles.BasePath))
				{
					Directory.CreateDirectory(cp.TempFiles.BasePath);
				}
				cp.GenerateExecutable = false;
				cp.OutputAssembly = assemblyName + ".dll";
				cp.IncludeDebugInformation = false;
				cp.ReferencedAssemblies.Add("System.dll");
				cp.ReferencedAssemblies.Add("CR_Documentor.dll");
				cp.GenerateInMemory = true;
				cp.WarningLevel = 3;
				cp.TreatWarningsAsErrors = false;
				cp.CompilerOptions = "/optimize";
				var provider = CodeDomProvider.CreateProvider("cs");
				CodeCompileUnit assemblyAttributeFile = new CodeCompileUnit();
				foreach (var attribute in attributes)
				{
					assemblyAttributeFile.AssemblyCustomAttributes.Add(CreateEmbeddedResourceFileAttribute(attribute.ResourcePath, attribute.FileName, attribute.MimeType));
					if (attribute.FileContents != null)
					{
						string filePath = CreateTemporaryEmbeddedResource(attribute.ResourcePath, attribute.FileContents, cp.TempFiles);
						cp.EmbeddedResources.Add(filePath);
					}
				}

				CompilerResults cr = provider.CompileAssemblyFromDom(cp, assemblyAttributeFile);

				if (cr.Errors.Count > 0)
				{
					Console.WriteLine("Errors creating temporary assembly.");
					foreach (CompilerError ce in cr.Errors)
					{
						Console.WriteLine("  {0}", ce.ToString());
					}
					Assert.Fail("Compiler error while creating temporary assembly.");
				}
				return cr.CompiledAssembly;
			}
			finally
			{
				if (Directory.Exists(cp.TempFiles.BasePath))
				{
					Directory.Delete(cp.TempFiles.BasePath, true);
				}
				cp.TempFiles.Delete();
			}
		}

		private static CodeAttributeDeclaration CreateEmbeddedResourceFileAttribute(string resourcePath, string fileName, string mimeType)
		{
			CodeAttributeArgument resourceArg = new CodeAttributeArgument(new CodePrimitiveExpression(resourcePath));
			CodeAttributeArgument mimeArg = new CodeAttributeArgument(new CodePrimitiveExpression(mimeType));
			CodeAttributeArgument fileArg = new CodeAttributeArgument(new CodePrimitiveExpression(fileName));
			return new CodeAttributeDeclaration(new CodeTypeReference(typeof(EmbeddedFileAttribute)), resourceArg, fileArg, mimeArg);
		}

		private static string CreateTemporaryEmbeddedResource(string embeddedResourcePath, string fileContents, TempFileCollection tempFiles)
		{
			string path = Path.Combine(tempFiles.BasePath, embeddedResourcePath);
			using (var stream = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write))
			{
				byte[] byteContents = Encoding.Default.GetBytes(fileContents);
				stream.Write(byteContents, 0, byteContents.Length);
				stream.Flush();
				stream.Close();
			}
			tempFiles.AddFile(path, false);
			return path;
		}
	}
}
