using System;
using CR_Documentor.ContextMenu;
using CR_Documentor.ContextMenu.Button;
using CR_Documentor.ContextMenu.Popup;
using CR_Documentor.Diagnostics;
using CR_Documentor.Server;
using CR_Documentor.Transformation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CR_Documentor.Test.Diagnostics
{
	[TestClass]
	public class LogManagerTest
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetLogger_NullOwner()
		{
			LogManager.GetLogger(null);
		}

		[TestMethod]
		public void GetLogger_MenuOwner()
		{
			Type[] menuTypes = new Type[]
			{
				typeof(ContextMenuItem),
				typeof(DocumentorVisibilityToggleButton),
				typeof(ContextMenuPopup)
			};
			foreach (Type menuType in menuTypes)
			{
				ILog logger = LogManager.GetLogger(menuType);
				Assert.IsInstanceOfType(logger, typeof(MenuLogger), "The logger for " + menuType.FullName + " was not a menu logger.");
			}
		}

		[TestMethod]
		public void GetLogger_ToolWindowOwner()
		{
			Type[] windowTypes = new Type[]
			{
				typeof(DocumentorWindow),
				typeof(ResponseWriter),
				typeof(TransformEngine)
			};
			foreach (Type windowType in windowTypes)
			{
				ILog logger = LogManager.GetLogger(windowType);
				Assert.IsInstanceOfType(logger, typeof(ToolWindowLogger), "The logger for " + windowType.FullName + " was not a tool window logger.");
			}
		}
	}
}
