using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CR_Documentor.Test.Server
{
	/// <summary>
	/// Handler for enumerating and serving embedded files.
	/// </summary>
	public class EmbeddedFileHandler
	{
		//ResponseWriter.WriteResource(e.RequestContext, Assembly.GetExecutingAssembly(), "CR_Documentor.Transformation.SandcastlePrototype.Resources.alert_caution.gif", "image/gif");

		// TODO: ISSUE 4 - Add a constructor that takes an assembly and enumerates the EmbeddedFileAttributes in the assembly.
		// TODO: ISSUE 4 - Store the embedded file references and assembly reference in a dictionary.
		// TODO: ISSUE 4 - Create a method that takes a short filename and uses ResponseWriter to write the result or log and 404 if not found.
	}
}
