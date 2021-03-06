﻿using System;
using CR_Documentor.Test.Transformation.Syntax.Proxies;
using CR_Documentor.Transformation.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeMock.ArrangeActAssert;

namespace CR_Documentor.Test.Transformation.Syntax.PreviewGeneratorTest
{
	[TestClass]
	[Isolated]
	public class FieldTest
	{
		// const and static readonly tests are in ContractTest.

		[TestMethod]
		public void Simple_Basic()
		{
			BaseVariableProxy info = new BaseVariableProxy("Field")
			{
				MemberType = "String"
			};
			var element = info.CreateFakeField();

			string expected =
@"<div class=""code vb"">
<div class=""member"">
<span class=""keyword"">Public</span> <span class=""identifier"">Field</span>&nbsp;<span class=""keyword"">As</span> <a href=""#"">String</a>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic, true);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Simple_CSharp()
		{
			BaseVariableProxy info = new BaseVariableProxy("Field")
			{
				MemberType = "string"
			};
			var element = info.CreateFakeField();

			string expected =
@"<div class=""code cs"">
<div class=""member"">
<span class=""keyword"">public</span> <a href=""#"">string</a> <span class=""identifier"">Field</span>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.CSharp, true);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}
	}
}
