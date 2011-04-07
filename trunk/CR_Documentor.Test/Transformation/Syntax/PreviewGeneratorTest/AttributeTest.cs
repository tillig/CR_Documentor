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
	public class AttributeTest
	{
		[TestMethod]
		public void Class_Single_Basic()
		{
			ClassProxy info = new ClassProxy("TestClass");
			AttributeProxy attrib = new AttributeProxy("SerializableAttribute");
			info.Attributes.Add(attrib.CreateFakeAttribute());
			var element = info.CreateFakeClass();

			string expected =
@"<div class=""code vb"">
<div class=""attributes"">
<div class=""attribute"">
&lt;<a href=""#"">SerializableAttribute</a>&gt; _
</div>
</div><div class=""member"">
<span class=""keyword"">Public</span> <span class=""keyword"">Class</span> <span class=""identifier"">TestClass</span>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic, true);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Class_Single_CSharp()
		{
			ClassProxy info = new ClassProxy("TestClass");
			AttributeProxy attrib = new AttributeProxy("SerializableAttribute");
			info.Attributes.Add(attrib.CreateFakeAttribute());
			var element = info.CreateFakeClass();

			string expected =
@"<div class=""code cs"">
<div class=""attributes"">
<div class=""attribute"">
[<a href=""#"">SerializableAttribute</a>]
</div>
</div><div class=""member"">
<span class=""keyword"">public</span> <span class=""keyword"">class</span> <span class=""identifier"">TestClass</span>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.CSharp, true);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Class_Multiple_Basic()
		{
			ClassProxy info = new ClassProxy("TestClass");
			AttributeProxy attrib = new AttributeProxy("SerializableAttribute");
			info.Attributes.Add(attrib.CreateFakeAttribute());
			attrib = new AttributeProxy("XmlRootAttribute");
			attrib.Arguments.Add(new ExpressionProxy("\"root\"").CreateFakeExpression());
			attrib.Arguments.Add(new BinaryOperatorExpressionProxy("DataType", "\"string\"").CreateFakeExpression());
			info.Attributes.Add(attrib.CreateFakeAttribute());
			var element = info.CreateFakeClass();

			string expected =
@"<div class=""code vb"">
<div class=""attributes"">
<div class=""attribute"">
&lt;<a href=""#"">SerializableAttribute</a>&gt; _
</div><div class=""attribute"">
&lt;<a href=""#"">XmlRootAttribute</a><div class=""parameters"">
(<div class=""parameter"">
<span class=""literal"">&quot;root&quot;</span>, 
</div><div class=""parameter"">
<span class=""identifier"">DataType</span> := <span class=""literal"">&quot;string&quot;</span>
</div>)
</div>&gt; _
</div>
</div><div class=""member"">
<span class=""keyword"">Public</span> <span class=""keyword"">Class</span> <span class=""identifier"">TestClass</span>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic, true);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Class_Multiple_CSharp()
		{
			ClassProxy info = new ClassProxy("TestClass");
			AttributeProxy attrib = new AttributeProxy("SerializableAttribute");
			info.Attributes.Add(attrib.CreateFakeAttribute());
			attrib = new AttributeProxy("XmlRootAttribute");
			attrib.Arguments.Add(new ExpressionProxy("\"root\"").CreateFakeExpression());
			attrib.Arguments.Add(new BinaryOperatorExpressionProxy("DataType", "\"string\"").CreateFakeExpression());
			info.Attributes.Add(attrib.CreateFakeAttribute());
			var element = info.CreateFakeClass();

			string expected =
@"<div class=""code cs"">
<div class=""attributes"">
<div class=""attribute"">
[<a href=""#"">SerializableAttribute</a>]
</div><div class=""attribute"">
[<a href=""#"">XmlRootAttribute</a><div class=""parameters"">
(<div class=""parameter"">
<span class=""literal"">&quot;root&quot;</span>, 
</div><div class=""parameter"">
<span class=""identifier"">DataType</span> = <span class=""literal"">&quot;string&quot;</span>
</div>)
</div>]
</div>
</div><div class=""member"">
<span class=""keyword"">public</span> <span class=""keyword"">class</span> <span class=""identifier"">TestClass</span>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.CSharp, true);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}
	}
}
