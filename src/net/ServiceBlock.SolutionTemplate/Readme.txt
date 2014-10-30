1. "Projects" folder contains the solution that will be used a model.
2. If you make any changes to any of the projects in the solution, export project template of the corresponding project to the "Templates" folder.
3. The pre-build event script for ServiceBlock.SolutionTemplate project zips all the files in the "Templates" folder into "ProjectTemplates" folder of the ServiceBlock.SolutionTemplate. These are the zip files that will be used in the VSIX.

NOTE:
Before prebuild.ps1 script can be executed from Visual Studio make sure to run the 
following commands in ps console. Replace paths where appropriate:

set-executionpolicy unrestricted
unblock-file "D:\Projects\ServiceBlock.SolutionTemplate\prebuild.ps1"
unblock-file "D:\Projects\ServiceBlock.SolutionTemplate\Ionic.Zip.dll"