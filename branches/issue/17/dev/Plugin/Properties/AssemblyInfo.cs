using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using DevExpress.CodeRush.Common;

[assembly: DXCoreAssembly(DXCoreAssemblyType.PlugIn, "CR_Documentor")]
[assembly: DXCoreProduct(typeof(CR_Documentor.CR_DocumentorProductModule))]
[assembly: DXCoreAuthorizedAssembly(LoadAuthorization.AllProducts)]

[assembly: ComVisible(false)]
[assembly: SecurityPermission(SecurityAction.RequestMinimum)]

[assembly: AssemblyTitle("CR_Documentor")]
[assembly: AssemblyDescription("CR_Documentor provides the ability to preview how XML documentation comments will render.")]
