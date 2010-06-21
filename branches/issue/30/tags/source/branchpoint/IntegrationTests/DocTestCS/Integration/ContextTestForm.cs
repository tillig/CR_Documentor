using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DocTestCS.Integration
{
	/// <summary>
	/// Test form for checking contexts - like whether a command is available
	/// in the designer or in the source editor.
	/// </summary>
	public partial class ContextTestForm : Form
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ContextTestForm"/> class.
		/// </summary>
		public ContextTestForm()
		{
			InitializeComponent();
		}
	}
}
