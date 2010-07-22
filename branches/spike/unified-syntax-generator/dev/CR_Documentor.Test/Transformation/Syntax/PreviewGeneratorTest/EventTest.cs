using System;
using CR_Documentor.Test.Transformation.Syntax.PreviewGeneratorTest.Proxies;
using CR_Documentor.Transformation.Syntax;
using DevExpress.CodeRush.StructuralParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeMock.ArrangeActAssert;

namespace CR_Documentor.Test.Transformation.Syntax.PreviewGeneratorTest
{
	[TestClass]
	[Isolated]
	public class EventTest
	{
		[TestMethod]
		public void Parameterized_Basic()
		{
			EventProxy info = new EventProxy("TestEvent")
			{
				MemberType = null,
			};
			info.Parameters.Add(new ParamProxy("a") { MemberType = "string" }.CreateFakeParam());
			ClassProxy parent = new ClassProxy("TestClass");
			info.__SetParentClassInterfaceStructOrModule(parent.CreateFakeClass());
			var element = info.CreateFakeEvent();

			string expected =
@"<div class=""code vb"">
<div class=""member"">
<span class=""keyword"">Public</span> <span class=""keyword"">Event</span> <span class=""identifier"">TestEvent</span>&nbsp;<span class=""keyword"">As</span> <a href=""#"">TestClass.TestEventEventHandler</a>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic, true);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Simple_Basic()
		{
			EventProxy info = new EventProxy("TestEvent")
			{
				MemberType = "EventHandler"
			};
			var element = info.CreateFakeEvent();

			string expected =
@"<div class=""code vb"">
<div class=""member"">
<span class=""keyword"">Public</span> <span class=""keyword"">Event</span> <span class=""identifier"">TestEvent</span>&nbsp;<span class=""keyword"">As</span> <a href=""#"">EventHandler</a>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic, true);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Simple_CSharp()
		{
			EventProxy info = new EventProxy("TestEvent")
			{
				MemberType = "EventHandler"
			};
			var element = info.CreateFakeEvent();

			string expected =
@"<div class=""code cs"">
<div class=""member"">
<span class=""keyword"">public</span> <span class=""keyword"">event</span> <a href=""#"">EventHandler</a> <span class=""identifier"">TestEvent</span>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.CSharp, true);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}
	}
}
