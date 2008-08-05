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
	internal static class TypeInformation
	{
		public static string GetResolutionScope(Type type)
		{
			if (type.DeclaringType != null)
			{
				string resolutionScope = GetResolutionScope(type.DeclaringType);
				if (resolutionScope.Length != 0)
				{
					resolutionScope += ".";
				}
				return resolutionScope + type.DeclaringType.Name;
			}

			return (type.Namespace != null) ? type.Namespace : string.Empty;
		}
	}
}
