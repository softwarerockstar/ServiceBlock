[T4Scaffolding.Scaffolder(Description = "Generates complete enterprise application.")][CmdletBinding()]
param(        
    [string]$Project,
	[string]$CodeLanguage,
	[string[]]$TemplateFolders,
	[switch]$Force = $false,		# rewrite existing files
	[switch]$Config = $false,		# generate service config
	[switch]$UI = $false,			# generate web layer
	[switch]$Layout = $false,		# generate shared layout in web layer
	[switch]$Cassandra = $false		# generate Cassandra context instead of EF
)

Write-Host "Generating Domain classes..." -foregroundcolor "DarkGreen"
Scaffold Domain -Project $Project -CodeLanguage $CodeLanguage -TemplateFolders $TemplateFolders -Cassandra:$Cassandra.IsPresent -Force:$Force.IsPresent

Write-Host "Generating Data activity classes..." -foregroundcolor "DarkGreen"
Scaffold DataActivities -Project $Project -CodeLanguage $CodeLanguage -TemplateFolders $TemplateFolders -Force:$Force.IsPresent

Write-Host "Generating orchestration classes..." -foregroundcolor "DarkGreen"
Scaffold Orchestrations -Project $Project -CodeLanguage $CodeLanguage -TemplateFolders $TemplateFolders -Force:$Force.IsPresent

Write-Host "Generating service classes..." -foregroundcolor "DarkGreen"
Scaffold Services -Project $Project -CodeLanguage $CodeLanguage -TemplateFolders $TemplateFolders -Cassandra:$Cassandra.IsPresent -Force:$Force.IsPresent -Config:$Config.IsPresent

if ($UI.IsPresent)
{
	Write-Host "Generating MVC UI..." -foregroundcolor "DarkGreen"
	Scaffold WebUI -Project $Project -CodeLanguage $CodeLanguage -TemplateFolders $TemplateFolders -Force:$Force.IsPresent -Layout:$Layout.IsPresent
}

Write-Host "----- Code generation completed. -----" -foregroundcolor "DarkGreen"

