using System;

namespace DocTest.Syntax
{
	/// <summary>
	/// Class used in testing syntax generation.
	/// </summary>
	[Serializable]
	public class TestClass
	{
		# region Fields

		/// <summary>
		/// Constant.
		/// </summary>
		public const string ConstantString = "Constant";

		/// <summary>
		/// Static read-only.
		/// </summary>
		protected static readonly string StaticString = "Static";

		#endregion

		#region Events and Delegates

		/// <summary>
		/// Delegate that takes parameters.
		/// </summary>
		public delegate int DelegateWithParameters(string param1);

		/// <summary>
		/// Delegate that doesn't take parameters.
		/// </summary>
		public delegate int DelegateWithoutParameters();

		/// <summary>
		/// Delegate that has no return value.
		/// </summary>
		public delegate void DelegateWithoutReturnValue();

		/// <summary>
		/// Event using a generic eventhandler signature.
		/// </summary>
		public event EventHandler<EventArgs> EventGeneric;

		/// <summary>
		/// Event using a non-generic eventhandler signature.
		/// </summary>
		public event EventHandler EventRegular;

		// Used to consume events so we don't get build warnings.
		private void EventConsumer()
		{
			this.EventGeneric(null, null);
			this.EventRegular(null, null);
		}

		#endregion

		#region Constructors and Destructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public TestClass() { }

		/// <summary>
		/// Overloaded constructor with one parameter.
		/// </summary>
		/// <param name="param1">First parameter.</param>
		public TestClass(string param1) : this() { }

		/// <summary>
		/// Overloaded constructor with two parameters.
		/// </summary>
		/// <param name="param1">First parameter.</param>
		/// <param name="param2">Second parameter.</param>
		public TestClass(string param1, int param2) : this(param1) { }

		/// <summary>
		/// Destructor.
		/// </summary>
		~TestClass() { }

		#endregion

		#region Properties

		/// <summary>
		/// Indexed property
		/// </summary>
		/// <param name="index">Index parameter.</param>
		public string this[int index]
		{
			get
			{
				return "";
			}
			set
            {
            }
		}

		/// <summary>
		/// Read-only property.
		/// </summary>
		public string PropertyReadOnly
		{
			get
			{
				return "";
			}
		}

		/// <summary>
		/// Write-only property.
		/// </summary>
		public string PropertyWriteOnly
		{
			set
            {
            }
		}

		/// <summary>
		/// Read/write property.
		/// </summary>
		public string PropertyReadWrite
		{
			get { return null; }
			set { }
		}

		#endregion

		#region Methods

		/// <summary>
		/// Method marked as both "protected" and "static."
		/// </summary>
		protected static void MethodProtectedStatic()
		{
		}

		/// <summary>
		/// Method marked with the "virtual" keyword.
		/// </summary>
		public virtual void MethodVirtual()
		{
		}

		/// <summary>
		/// Method with no parameters.
		/// </summary>
		public void MethodWithNoParams()
		{
		}

		/// <summary>
		/// Method with optional parameters. Uses "params" array.
		/// </summary>
		/// <param name="required">Required parameter.</param>
		/// <param name="notRequired">Optional parameter (array).</param>
		public void MethodWithOptionalParams(string required, params object[] notRequired)
		{
		}

		/// <summary>
		/// Method that returns a string.
		/// </summary>
		/// <returns>Always returns emtpy string.</returns>
		public string MethodWithReturnValue()
		{
			return "";
		}

		#endregion

		#region Operators

		/// <summary>
		/// Overloaded multiplication operator.
		/// </summary>
		/// <param name="a">First parameter.</param>
		/// <param name="b">Second parameter.</param>
		/// <returns>Always returns <see langword="null" />.</returns>
		public static TestClass operator *(TestClass a, TestClass b)
		{
			return null;
		}

		/// <summary>
		/// Overloaded unary negation operator.
		/// </summary>
		/// <param name="a">First parameter.</param>
		/// <returns>Always returns <see langword="null" />.</returns>
		public static TestClass operator -(TestClass a)
		{
			return null;
		}

		/// <summary>
		/// Overloaded implicit conversion to double.
		/// </summary>
		/// <param name="a">First parameter.</param>
		/// <returns>Always returns <see langword="null" />.</returns>
		public static implicit operator double(TestClass a)
		{
			return 0;
		}

		/// <summary>
		/// Overloaded explicit conversion to long.
		/// </summary>
		/// <param name="a">First parameter.</param>
		/// <returns>Always returns <see langword="null" />.</returns>
		public static explicit operator long(TestClass a)
		{
			return 0;
		}

		#endregion

	}
}
