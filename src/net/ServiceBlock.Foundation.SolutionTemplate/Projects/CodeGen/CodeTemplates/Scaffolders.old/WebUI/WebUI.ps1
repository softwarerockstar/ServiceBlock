[T4Scaffolding.Scaffolder(Description = "Generates sample web layer for all entities.")][CmdletBinding()]
param(        
    [string]$Project,
	[string]$CodeLanguage,
	[string[]]$TemplateFolders,
	[switch]$Force = $false,
	[switch]$Layout = $false
)

# Load global variables
$scriptFolder = Split-Path -Parent $MyInvocation.MyCommand.Path
$includesFolder  = Join-Path -Path $scriptFolder -ChildPath ..\..\..\Includes
. "$includesFolder\Globals.ps1"

ForEach ($entity in $domainClasses.GetEnumerator()) 
{
	$entityName = $entity.Key
	$entityNamePlural = $entity.Value

	$primaryKeyName = [string](Get-PrimaryKey $entityName -Project $domainProjectName)
	
	$entityCodeType = Get-ProjectType $EntityName -Project $domainProjectName -BlockUi
	if ($entityCodeType) { $relatedEntities = [Array](Get-RelatedEntities $entityCodeType.FullName -Project $domainProjectName) }
	if (!$relatedEntities) { $relatedEntities = @() }

	# Add MVC controller
	Add-ProjectItemViaTemplate ("Controllers\" + $entityName + "Controller") -Template EntityController `
		-Model @{ Namespace = $webNamespace; EntityName = $entityName; Entity = $entityCodeType; } `
		-SuccessMessage "Added controller at {0}" `
		-TemplateFolders $TemplateFolders -Project $webProjectName -CodeLanguage $CodeLanguage -Force:$Force

	# Add MVC View
	Add-ProjectItemViaTemplate ("Views\" + $entityName + "\Index") -Template EntityView `
		-Model @{ EntityName = $entityName; EntityNamePlural = $entityNamePlural; Entity = $entityCodeType; PrimaryKeyName = $primaryKeyName; RelatedEntities = $relatedEntities } `
		-SuccessMessage "Added view at {0}" `
		-TemplateFolders $TemplateFolders -Project $webProjectName -CodeLanguage $CodeLanguage -Force:$Force
}

if ($Layout.IsPresent)
{
	# Add shared layout
	Add-ProjectItemViaTemplate ("Views\Shared\_Layout") -Template Layout `
		-Model @{ Entities = $domainClasses } `
		-SuccessMessage "Added layout at {0}" `
		-TemplateFolders $TemplateFolders -Project $webProjectName -CodeLanguage $CodeLanguage -Force:$Force

}

