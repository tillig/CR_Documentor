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
		private string prefix;
		private string namespaceName;
		private string type;
		private string name;
		private string parameters;

		private MemberKey(string memberKey)
		{
			this.prefix = string.Empty;
			this.namespaceName = string.Empty;
			this.type = string.Empty;
			this.name = string.Empty;
			this.parameters = string.Empty;
	
			this.prefix = "T:";
			if ((memberKey.Length >= 2) && (memberKey[1] == ':'))
			{
				this.prefix = memberKey.Substring(0, 2);
				memberKey = memberKey.Substring(2);
			}
	
			int bracket = memberKey.IndexOf('(');
			if (bracket != -1)
			{
				this.parameters = memberKey.Substring(bracket).Replace('@', '&');
				memberKey = memberKey.Substring(0, bracket);
			}
	
			this.namespaceName = memberKey;
			if (this.prefix != "N:")
			{
				string str1 = memberKey;
				int dot1 = memberKey.LastIndexOf('.');  	
				if (dot1 != -1)
				{
					this.namespaceName = memberKey.Substring(0, dot1);
					str1 = memberKey.Substring(dot1 + 1);
				}
	
				if (this.prefix == "T:")
				{
					this.type = str1;
				}
				else
				{
					this.name = str1.Replace('#', '.');
	
					int dot2 = this.namespaceName.LastIndexOf('.');  	
					if (dot2 != -1)
					{
						this.type = this.namespaceName.Substring(dot2 + 1);
						this.namespaceName = this.namespaceName.Substring(0, dot2);
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

			if (description.prefix.Length == 0)
				return string.Empty;

			if (description.prefix == "N:") 
				return description.namespaceName;

			if (description.prefix == "T:") 
				return description.type;
			
			return description.name;
		}
	
		/// <summary>
		/// Gets a member full name based on the provided short name.
		/// </summary>
		/// <param name="memberName">The key to extract the name from.</param>
		/// <returns>A member name string (i.e., <c>MyNamespace.MyClass.MyMethod</c>) for the member key.</returns>
		public static string GetFullName(string memberName)
		{
			MemberKey memberKey = new MemberKey(memberName);
			
			if (memberKey.prefix == "N:") 
				return memberKey.namespaceName;
			
			string type = (memberKey.namespaceName.Length > 0) ? (memberKey.namespaceName + "." + memberKey.type) : memberKey.type;
			
			switch (memberKey.prefix)
			{
				case "T:":
					return type;

				case "M:":
					return type + "." + memberKey.name + ((memberKey.parameters.Length == 0) ? "()" : memberKey.parameters);
			
				case "P:":
					return type + "." + memberKey.name + memberKey.parameters;

				case "E:":
				case "F:":
					return type + "." + memberKey.name;
			}
			
			return string.Empty;
		}
	}
}
