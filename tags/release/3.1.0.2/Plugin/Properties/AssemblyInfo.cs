using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using CR_Documentor.ProductModule;
using CR_Documentor.Properties;
using DevExpress.CodeRush.Common;

[assembly: DXCoreAssembly(DXCoreAssemblyType.PlugIn, ProductConstants.PlugInName, PlugInLoadType.StartUp)]
[assembly: DXCoreProduct(typeof(CR_DocumentorProductModule))]
[assembly: DXCoreAuthorizedAssembly(LoadAuthorization.AllProducts)]

[assembly: ComVisible(false)]
[assembly: SecurityTransparent]
[assembly: SecurityRules(SecurityRuleSet.Level2)]

[assembly: AssemblyTitle(ProductConstants.PlugInName)]
[assembly: AssemblyDescription("DXCore plugin that provides the ability to preview how XML documentation comments will render.")]
