using System;
using CR_Documentor.Test.Transformation.Syntax.Proxies;
using CR_Documentor.Transformation.Syntax;
using DevExpress.CodeRush.StructuralParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeMock.ArrangeActAssert;

namespace CR_Documentor.Test.Transformation.Syntax.PreviewGeneratorTest
{
	[TestClass]
	[Isolated]
	public class PropertyTest
	{
		[TestMethod]
		public void GetOnly_Basic()
		{
			PropertyProxy info = new PropertyProxy("TestProperty")
			{
				MemberType = "String",
				HasGetter = true,
				HasSetter = false,
				IsReadOnly = true // ReadOnly properties in VB parse with IsReadOnly set true
			};
			var element = info.CreateFakeProperty();

			string expected =
@"<div class=""code vb"">
<div class=""member"">
<span class=""keyword"">Public</span> <span class=""keyword"">ReadOnly</span> <span class=""keyword"">Property</span> <span class=""identifier"">TestProperty</span>&nbsp;<span class=""keyword"">As</span> <a href=""#"">String</a><div class=""getset"">
<span class=""keyword"">Get</span>
</div>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic, true);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetOnly_CSharp()
		{
			PropertyProxy info = new PropertyProxy("TestProperty")
			{
				MemberType = "string",
				HasGetter = true,
				HasSetter = false
			};
			var element = info.CreateFakeProperty();

			string expected =
@"<div class=""code cs"">
<div class=""member"">
<span class=""keyword"">public</span> <a href=""#"">string</a> <span class=""identifier"">TestProperty</span> {<div class=""getset"">
<span class=""keyword"">get</span>;
</div>}
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.CSharp, true);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetSet_Basic()
		{
			PropertyProxy info = new PropertyProxy("TestProperty")
			{
				MemberType = "String",
				HasGetter = true,
				HasSetter = true
			};
			var element = info.CreateFakeProperty();

			string expected =
@"<div class=""code vb"">
<div class=""member"">
<span class=""keyword"">Public</span> <span class=""keyword"">Property</span> <span class=""identifier"">TestProperty</span>&nbsp;<span class=""keyword"">As</span> <a href=""#"">String</a><div class=""getset"">
<span class=""keyword"">Get</span>
</div><div class=""getset"">
<span class=""keyword"">Set</span>
</div>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic, true);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetSet_CSharp()
		{
			PropertyProxy info = new PropertyProxy("TestProperty")
			{
				MemberType = "string",
				HasGetter = true,
				HasSetter = true
			};
			var element = info.CreateFakeProperty();

			string expected =
@"<div class=""code cs"">
<div class=""member"">
<span class=""keyword"">public</span> <a href=""#"">string</a> <span class=""identifier"">TestProperty</span> {<div class=""getset"">
<span class=""keyword"">get</span>;
</div><div class=""getset"">
<span class=""keyword"">set</span>;
</div>}
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.CSharp, true);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Indexer_Basic()
		{
			PropertyProxy info = new PropertyProxy("Item")
			{
				MemberType = "String",
				HasGetter = true,
				HasSetter = true
			};
			ParamProxy param = new ParamProxy("index")
			{
				ParamType = "Integer"
			};
			info.Parameters.Add(param.CreateFakeParam());
			var element = info.CreateFakeProperty();

			string expected =
@"<div class=""code vb"">
<div class=""member"">
<span class=""keyword"">Public</span> <span class=""keyword"">Default</span> <span class=""keyword"">Property</span> <span class=""identifier"">Item</span><div class=""parameters"">
( _<div class=""parameter"">
<span class=""identifier"">index</span> <span class=""keyword"">As</span> <a href=""#"">Integer</a> _
</div>)
</div>&nbsp;<span class=""keyword"">As</span> <a href=""#"">String</a><div class=""getset"">
<span class=""keyword"">Get</span>
</div><div class=""getset"">
<span class=""keyword"">Set</span>
</div>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic, true);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Indexer_CSharp()
		{
			PropertyProxy info = new PropertyProxy("Item")
			{
				MemberType = "string",
				HasGetter = true,
				HasSetter = true
			};
			ParamProxy param = new ParamProxy("index")
			{
				ParamType = "int"
			};
			info.Parameters.Add(param.CreateFakeParam());
			var element = info.CreateFakeProperty();

			string expected =
@"<div class=""code cs"">
<div class=""member"">
<span class=""keyword"">public</span> <a href=""#"">string</a> <span class=""keyword"">this</span><div class=""parameters"">
[<div class=""parameter"">
<a href=""#"">int</a> <span class=""identifier"">index</span>
</div>]
</div> {<div class=""getset"">
<span class=""keyword"">get</span>;
</div><div class=""getset"">
<span class=""keyword"">set</span>;
</div>}
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.CSharp, true);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}
		[TestMethod]
		public void SetOnly_Basic()
		{
			PropertyProxy info = new PropertyProxy("TestProperty")
			{
				MemberType = "String",
				HasGetter = false,
				HasSetter = true
			};
			var element = info.CreateFakeProperty();

			string expected =
@"<div class=""code vb"">
<div class=""member"">
<span class=""keyword"">Public</span> <span class=""keyword"">WriteOnly</span> <span class=""keyword"">Property</span> <span class=""identifier"">TestProperty</span>&nbsp;<span class=""keyword"">As</span> <a href=""#"">String</a><div class=""getset"">
<span class=""keyword"">Set</span>
</div>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic, true);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void SetOnly_CSharp()
		{
			PropertyProxy info = new PropertyProxy("TestProperty")
			{
				MemberType = "string",
				HasGetter = false,
				HasSetter = true
			};
			var element = info.CreateFakeProperty();

			string expected =
@"<div class=""code cs"">
<div class=""member"">
<span class=""keyword"">public</span> <a href=""#"">string</a> <span class=""identifier"">TestProperty</span> {<div class=""getset"">
<span class=""keyword"">set</span>;
</div>}
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.CSharp, true);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}
	}
}
