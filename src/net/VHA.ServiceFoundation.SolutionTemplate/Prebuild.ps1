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

ZipFolder "..\..\..\Projects" "..\..\..\TemplateInstaller\ProjectTemplates\Template.zip"
