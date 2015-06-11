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
	/// Retrieves information about a <see cref="System.Type"/>.
	/// </summary>
	public static class TypeInformation
	{
		/// <summary>
		/// Retrieves the resolution scope for a given <see cref="System.Type"/>.
		/// </summary>
		/// <param name="type">The <see cref="System.Type"/> to inspect.</param>
		/// <returns>
		/// For nested types, the name of the parent type; for non-nested types,
		/// the namespace of the type.
		/// </returns>
		public static string GetResolutionScope(Type type)
		{
			if (type == null)
			{
				return String.Empty;
			}
			if (type.DeclaringType != null)
			{
				string resolutionScope = GetResolutionScope(type.DeclaringType);
				if (resolutionScope.Length != 0)
				{
					resolutionScope += ".";
				}
				return resolutionScope + type.DeclaringType.Name;
			}
			return type.Namespace ?? string.Empty;
		}
	}
}
