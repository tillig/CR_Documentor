// ---------------------------------------------------------
// Lutz Roeder's .NET Reflector, October 2000.
// Copyright (C) 2000-2003 Lutz Roeder. All rights reserved.
// http://www.aisto.com/roeder/dotnet
// roeder@aisto.com
// ---------------------------------------------------------
// Contains modifications (primarily removals of unused
// items) by Travis Illig.
// ---------------------------------------------------------

using System;

namespace CR_Documentor.Reflector
{
	/// <summary>
	/// Helps in determining the member naming information used in rendering.
	/// </summary>
	public sealed class MemberKey
	{
		private string _prefix = "T:";
		private string _namespaceName = string.Empty;
		private string _type = string.Empty;
		private string _name = string.Empty;
		private string _parameters = string.Empty;

		/// <summary>
		/// Initializes a new instance of the <see cref="CR_Documentor.Reflector.MemberKey" /> class.
		/// </summary>
		/// <param name="memberKey">
		/// A <see cref="System.String"/> containing the member key to parse into components.
		/// </param>
		/// <remarks>
		/// <para>
		/// Member keys are things that show up in "cref" attributes, like
		/// "MyNamespace.MyTypeName" or "M:Foo.Bar.Baz" or "Alpha.Beta.Gamma(string, int)".
		/// </para>
		/// </remarks>
		private MemberKey(string memberKey)
		{
			if ((memberKey.Length >= 2) && (memberKey[1] == ':'))
			{
				this._prefix = memberKey.Substring(0, 2);
				memberKey = memberKey.Substring(2);
			}
			else if (memberKey.IndexOf(':') >= 0)
			{
				memberKey = memberKey.Substring(memberKey.IndexOf(':') + 1);
			}

			int bracket = memberKey.IndexOf('(');
			if (bracket != -1)
			{
				this._parameters = memberKey.Substring(bracket).Replace('@', '&');
				memberKey = memberKey.Substring(0, bracket);
			}

			this._namespaceName = memberKey;
			if (this._prefix != "N:")
			{
				string str1 = memberKey;
				int dot1 = memberKey.LastIndexOf('.');
				if (dot1 != -1)
				{
					this._namespaceName = memberKey.Substring(0, dot1);
					str1 = memberKey.Substring(dot1 + 1);
				}
				else
				{
					this._namespaceName = string.Empty;
				}

				if (this._prefix == "T:")
				{
					this._type = str1;
				}
				else
				{
					this._name = str1.Replace('#', '.');

					int dot2 = this._namespaceName.LastIndexOf('.');
					if (dot2 != -1)
					{
						this._type = this._namespaceName.Substring(dot2 + 1);
						this._namespaceName = this._namespaceName.Substring(0, dot2);
					}
				}
			}
		}

		/// <summary>
		/// Gets a member name based on the provided key.
		/// </summary>
		/// <param name="memberKey">The key to extract the name from.</param>
		/// <returns>A member name string (i.e., <c>MyNamespace.MyClass.MyMethod</c>) for the member key.</returns>
		public static string GetName(string memberKey)
		{
			MemberKey description = new MemberKey(memberKey);
			switch (description._prefix)
			{
				case "N:":
					return description._namespaceName;
				case "T:":
					return description._type;
				default:
					return description._name;
			}
		}

		/// <summary>
		/// Gets a member full name based on the provided short name.
		/// </summary>
		/// <param name="memberName">The key to extract the name from.</param>
		/// <returns>A member name string (i.e., <c>MyNamespace.MyClass.MyMethod</c>) for the member key.</returns>
		public static string GetFullName(string memberName)
		{
			MemberKey memberKey = new MemberKey(memberName);

			if (memberKey._prefix == "N:")
			{
				return memberKey._namespaceName;
			}

			string type = (memberKey._namespaceName.Length > 0) ? (memberKey._namespaceName + "." + memberKey._type) : memberKey._type;

			switch (memberKey._prefix)
			{
				case "T:":
					return type;

				case "M:":
					return type + "." + memberKey._name + ((memberKey._parameters.Length == 0) ? "()" : memberKey._parameters);

				case "P:":
					return type + "." + memberKey._name + memberKey._parameters;

				case "E:":
				case "F:":
					return type + "." + memberKey._name;
			}

			return string.Empty;
		}
	}
}
