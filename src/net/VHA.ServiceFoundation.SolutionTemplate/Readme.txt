PURPOSE:
Create a solution template that includes various layers of a solution built on VHA.ServiceFoundation framework.  

DETAILS:
The TemplateInstaller project builds a VSIX package that can be shared with all VHA developers from a network share etc.  When developers install this package, 
a new item appears in their Visual Studio 2012 solution templates called "VHA Service Foundation Application".  When a developer creates a solution using this 
template, all projects with correct references and code generation templates are added to the solution by default.

USAGE:

Creating the solution:
1. Install the VSIX package.
2. Close and re-open Visual Studio 2012 (if it was already open).
3. Create a new project.
4. In the "Search Installed Templates" textbox, type in "VHA" and hit enter.
5. Solution template called "VHA Service Foundation Application" will appear as a choice.
6. Click on "VHA Service Foundation Application" option, type in a solution name, then click OK.
7. Solution with several projects will be created.

Enabling NuGet package management:
1. If you do not already have NuGet installed:
	1a. Install NuGet from http://www.nuget.org/.  
	1b. Restart Visual Studio and reload your project.
2. If you have never added VHA Package Repository to NuGet:
	2a. Click on Tool|Library Package Manager|Package Manager Settings|Package Sources
	2b. Add a package:
		Name: NuGet official package source
		Source: https://nuget.org/api/v2/
3. Right-click on the solution node in Solution Explorer, then click on "Enabl NuGet Package Restore".
4. Click on Tool|Library Package Manager|Package Manager Console
5. Select "VHA.CodeGen.ServiceFoundation" as the Default project.
6. At the PM> prompt, type in "Install-Package T4Scaffolding".  Wait for package to be installed.

Generating Code:
1. In the Domain project, create your domain classes under the Models folder.  Use following attributes to decorate class and properties:
	
	[DataContract(Namespace = "VHA.[Domain Name].Services")]
	[DataMember]
	[Key]
	[ForeignKey("[ID Property]")]

	See sample code for how these attributes are used, or check out MSDN documentation for these attributes.

2. Go to Package Manager Console and at the PM> Prompt type:

	Scaffold EnterpriseApp

	Note: EnterpriseApp scaffold can have following optional parameters:

	-Config:	Generate service configuration file (web.config).  Note that if this option is used, web.config in Service project will be overwritten if already exists.
	-UI:		Generate a sample MVC app
	-Layout:	Generate a layout (ala. master page) for the UI.  If already exists it will be overwritten.
	-Force:		By default all class files that already exist are skipped.  With force option everything is regenerated.

	If you want to generate only a specific layer, you can use the specific scaffold for that layer only, e.g.

	Scaffold Domain
	Scaffold DataActivities
	Scaffold Orchestrations
	Scaffold Services
	Scaffold WebUI

	All scaffolders and code generation templates are part of your solution.  Feel free to modify code generation as needed as long as the general architecture of the 
	solution remains intact.
3. Once code is generated, build the solution once, and then add service references to the WebUI project:
	3a. Right click on WebUI project node in solution explorer, select Add Service Reference
	3b. Click on discover.  This will show services within current solution, one per entity.
	3c. Select a service.  Type in entity name followed by Svc (e.g. ProductSvc) in the namespace textbox.  This is a convention that must be used at all times, also generated code depends upon this convention.
	3d. Click Advanced, then select System.Collections.Generic.List for Collection Types option.
	3e. Click OK.  This will generate your service proxy.
	3f. Repeat above steps for each service in the solution.
4. Build, run, enjoy the show!


