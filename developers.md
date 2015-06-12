# Developer Notes

## Debugging

* Set the CR\_Documentor project as the startup project.
* In the CR\_Documentor project properties, under Debug, set...
	- Startup action = program
	- Program = path to devenv.exe like `C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\devenv.exe`
	- Start arguments = `/rootsuffix Exp`

## CodeRush Cache Issue

As of VS2015 + CodeRush 14.2 or so, if you debug a VSIX-based add-in and then try to install it for real to test it, you may find that CodeRush appears corrupted - like only your add-in appears and none of the standard ones.

You can fix this by clearing your loader cache. Delete all the files in these folders:

* `%appdata%\CodeRush for VS .NET\1.1\Settings.xml\Loader`
* `%appdata%\CodeRush for VS .NET\1.1\Settings.xml\_Scheme_FrictionFree\Loader`

[See the issue here.](https://www.devexpress.com/Support/Center/Question/Details/T254485)