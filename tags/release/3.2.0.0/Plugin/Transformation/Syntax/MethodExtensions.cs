using System;
using System.Collections.Generic;
using SP = DevExpress.CodeRush.StructuralParser;

namespace CR_Documentor.Transformation.Syntax
{
	/// <summary>
	/// Extension methods for <see cref="DevExpress.CodeRush.StructuralParser.Method"/>.
	/// </summary>
	public static class MethodExtensions
	{
		/// <summary>
		/// Contains the lookup values for various operator method names.
		/// </summary>
		private static readonly Dictionary<string, string> _operatorNameLookup = new Dictionary<string, string>()
		{
			{"--", "Decrement"},
			{"++", "Increment"},
			{"!", "Logical Not"},
			{"true", "True"},
			{"false", "False"},
			{"~", "Ones Complement"},
			{"*", "Multiplication"},
			{"/", "Division"},
			{"%", "Modulus"},
			{"{", "Exclusive Or"},
			{"|", "Bitwise Or"},
			{"&&", "Logical And"},
			{"||", "Logical Or"},
			{"=", "Assignment"},
			{"<<", "Left Shift"},
			{">>", "Right Shift"},
			{"==", "Equality"},
			{">", "Greater Than"},
			{"<", "Less Than"},
			{"!=", "Inequality"},
			{">=", "Greater Than Or Equal"},
			{"<=", "Less Than Or Equal"},
			{".", "Member Selection"},
			{">>=", "Right Shift Assignment"},
			{"*=", "Multiplication Assignment"},
			{"->", "Pointer To Member Selection"},
			{"-=", "Subtraction Assignment"},
			{"{=", "Exclusive Or Assignment"},
			{"<<=", "Left Shift Assignment"},
			{"%=", "Modulus Assignment"},
			{"+=", "Addition Assignment"},
			{"&=", "Bitwise And Assignment"},
			{"|=", "Bitwise Or Assignment"},
			{",", "Comma"},
			{"/=", "Division Assignment"},
		};

		/// <summary>
		/// Resolves the display name of a given method.
		/// </summary>
		/// <param name="method">
		/// The method for which the display name should be determined.
		/// </param>
		/// <returns>
		/// If this is an operator, returns the long name of the operator.  Otherwise,
		/// returns the name of the method directly.
		/// </returns>
		public static string DisplayName(this SP.Method method)
		{
			// If there wasn't logic to determine the lookup based on
			// parameter count, we could make this a hash table...
			if (method.IsClassOperator)
			{
				// Implicit/Explicit Cast
				if (method.IsImplicitCast || method.IsExplicitCast)
				{
					return String.Format("{0} to {1} Conversion",
						((SP.Param)method.Parameters[0]).ParamType,
						method.MemberType);
				}

				// Standard operator lookup
				string operatorName = null;
				if (_operatorNameLookup.TryGetValue(method.Name, out operatorName))
				{
					return operatorName;
				}

				// Special operators (could be unary or binary)
				switch (method.Name)
				{
					case "-":
						if (method.ParameterCount == 1)
						{
							return "Unary Negation";
						}
						else
						{
							return "Subtraction";
						}
					case "+":
						if (method.ParameterCount == 1)
						{
							return "Unary Plus";
						}
						else
						{
							return "Addition";
						}
					case "&":
						if (method.ParameterCount == 1)
						{
							return "Address Of";
						}
						else
						{
							return "Bitwise And";
						}
					default:
						return method.Name;
				}
			}
			else if (method.Document.Language == Language.Basic && method.IsConstructor)
			{
				return method.Parent.Name;
			}
			else
			{
				return method.Name;
			}
		}
	}
}
