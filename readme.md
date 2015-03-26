#CR_Documentor

Writing [XML documentation comments](http://msdn.microsoft.com/en-us/library/b2s063f7.aspx) in .NET can be troublesome sometimes because you can't really see what it's going to look like - you have to compile the code, then take the extracted XML doc and feed it to a rendering engine like [Sandcastle](http://www.codeplex.com/Sandcastle), then wait for the compiled help to come out so you can read it.

**CR_Documentor is a plugin for [DXCore](http://www.devexpress.com/Products/Visual_Studio_Add-in/DXCore/) that allows you to preview what the documentation will look like when it's rendered - in a tool window inside Visual Studio.**

##License

Copyright 2004 CR_Documentor Contributors

Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.

##Requirements

This product requires **DXCore for Visual Studio .NET 11.2.8 or later.**

DXCore is a free download from Developer Express, Inc.: http://www.devexpress.com/Products/Visual_Studio_Add-in/DXCore/

##Installation

[Install the add-in via the Visual Studio extension gallery.](https://visualstudiogallery.msdn.microsoft.com/668a65b5-2468-4afa-b78d-8c369850e2b2)

##Usage

1. Open an instance of Visual Studio.
2. From the DevExpress menu, select "Tool Windows," then "Documentor."  This will show the Documentor window.
3. When working in a source file with XML documentation, watch the Documentor window as you edit your XML comments.  It will contain a preview of what your comments will look like when rendered into end-user documentation.

While viewing a source code file, CR_Documentor provides an additional context menu.  Different options appear based on whether you are inside an XML doc comment or not; and whether you have text selected or not.