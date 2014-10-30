<#
Before this script can be executed from Visual Studio make sure to run the 
following commands in ps console. Replace paths where appropriate:

set-executionpolicy unrestricted
unblock-file "D:\Projects\ServiceBlock.SolutionTemplate\prebuild.ps1"
unblock-file "D:\Projects\ServiceBlock.SolutionTemplate\Ionic.Zip.dll"

#>
function ZipFolder(
	[string]$directoryToZip,
	[string]$destinationArchive) 
{
    [System.Reflection.Assembly]::LoadFrom("..\..\..\Ionic.Zip.dll");    
    $zipfile = new-object Ionic.Zip.ZipFile
    $e = $zipfile.AddDirectory($directoryToZip)
    $zipfile.Save($destinationArchive)
    $zipfile.Dispose()
}

ZipFolder "..\..\..\Templates" "..\..\..\ServiceBlock.SolutionTemplate\ProjectTemplates\Template.zip"
