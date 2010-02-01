using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using CR_Documentor.ProductModule;
using CR_Documentor.Properties;
using DevExpress.CodeRush.Common;

[assembly: DXCoreAssembly(DXCoreAssemblyType.PlugIn, ProductConstants.PlugInName, PlugInLoadType.StartUp)]
[assembly: DXCoreProduct(typeof(CR_DocumentorProductModule))]
[assembly: DXCoreAuthorizedAssembly(LoadAuthorization.AllProducts)]

[assembly: ComVisible(false)]
[assembly: SecurityPermission(SecurityAction.RequestMinimum)]

[assembly: AssemblyTitle(ProductConstants.PlugInName)]
[assembly: AssemblyDescription("DXCore plugin that provides the ability to preview how XML documentation comments will render.")]
