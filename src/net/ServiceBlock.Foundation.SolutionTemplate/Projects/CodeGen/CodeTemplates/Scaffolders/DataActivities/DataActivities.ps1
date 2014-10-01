[T4Scaffolding.Scaffolder(Description = "Generates data activities for all entities.")][CmdletBinding()]
param(        
    [string]$Project,
	[string]$CodeLanguage,
	[string[]]$TemplateFolders,
	[switch]$Force = $false
)

# Load global variables
$scriptFolder = Split-Path -Parent $MyInvocation.MyCommand.Path
$includesFolder  = Join-Path -Path $scriptFolder -ChildPath ..\..\..\Includes
. "$includesFolder\Globals.ps1"

ForEach ($entity in $domainClasses.GetEnumerator()) 
{
	$entityName = $entity.Key
	$entityNamePlural = $entity.Value
	
	$typeName = $modelNamespace + "." + $entityName			
	$pk = Get-PrimaryKey -Project $domainProjectName -Type $typeName	
	$entityCodeType = Get-ProjectType $EntityName -Project $domainProjectName -BlockUi
	if ($entityCodeType) { $relatedEntities = [Array](Get-RelatedEntities $entityCodeType.FullName -Project $domainProjectName) }
	if (!$relatedEntities) { $relatedEntities = @() }

	# Add Delete activity
	Add-ProjectItemViaTemplate ($activityClassesFolder + "\" + $entityName + "\Delete") -Template Delete `
		-Model @{ Namespace = $dataNamespace; EntityNamespace = $modelNamespace; EntityName = $entityName; EntityNamePlural = $entityNamePlural; PrimaryKey = $pk; UoWInterfaceName = $unitOfWorkInterfaceName } `
		-SuccessMessage "Added Delete at {0}" `
		-TemplateFolders $TemplateFolders -Project $dataProjectName -CodeLanguage $CodeLanguage -Force:$Force

	# Add GetAll activity
	Add-ProjectItemViaTemplate ($activityClassesFolder + "\" + $entityName + "\GetAll") -Template GetAll `
		-Model @{ Namespace = $dataNamespace; EntityNamespace = $modelNamespace; EntityName = $entityName; EntityNamePlural = $entityNamePlural; PrimaryKey = $pk; UoWInterfaceName = $unitOfWorkInterfaceName } `
		-SuccessMessage "Added GetAll at {0}" `
		-TemplateFolders $TemplateFolders -Project $dataProjectName -CodeLanguage $CodeLanguage -Force:$Force

	# Add GetById activity
	Add-ProjectItemViaTemplate ($activityClassesFolder + "\" + $entityName + "\GetById") -Template GetById `
		-Model @{ Namespace = $dataNamespace; EntityNamespace = $modelNamespace; EntityName = $entityName; EntityNamePlural = $entityNamePlural; PrimaryKey = $pk; UoWInterfaceName = $unitOfWorkInterfaceName } `
		-SuccessMessage "Added GetById at {0}" `
		-TemplateFolders $TemplateFolders -Project $dataProjectName -CodeLanguage $CodeLanguage -Force:$Force

	# Add GetWithCriteria activity
	Add-ProjectItemViaTemplate ($activityClassesFolder + "\" + $entityName + "\GetWithCriteria") -Template GetWithCriteria `
		-Model @{ Namespace = $dataNamespace; EntityNamespace = $modelNamespace; EntityName = $entityName; EntityNamePlural = $entityNamePlural; PrimaryKey = $pk; UoWInterfaceName = $unitOfWorkInterfaceName } `
		-SuccessMessage "Added GetWithCriteria at {0}" `
		-TemplateFolders $TemplateFolders -Project $dataProjectName -CodeLanguage $CodeLanguage -Force:$Force

	# Add InsertOrUpdate activity
	Add-ProjectItemViaTemplate ($activityClassesFolder + "\" + $entityName + "\InsertOrUpdate") -Template InsertOrUpdate `
		-Model @{ Namespace = $dataNamespace; EntityNamespace = $modelNamespace; Entity = $entityCodeType; EntityName = $entityName; EntityNamePlural = $entityNamePlural; PrimaryKey = $pk; UoWInterfaceName = $unitOfWorkInterfaceName; RelatedEntities = $relatedEntities } `
		-SuccessMessage "Added InsertOrUpdate at {0}" `
		-TemplateFolders $TemplateFolders -Project $dataProjectName -CodeLanguage $CodeLanguage -Force:$Force

}