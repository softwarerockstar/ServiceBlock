[T4Scaffolding.Scaffolder(Description = "Generates service layer for all entities.")][CmdletBinding()]
param(        
    [string]$Project,
	[string]$CodeLanguage,
	[string[]]$TemplateFolders,
	[switch]$Force = $false,
	[switch]$Config = $false
)

# Load global variables
$scriptFolder = Split-Path -Parent $MyInvocation.MyCommand.Path
$includesFolder  = Join-Path -Path $scriptFolder -ChildPath ..\..\..\Includes
. "$includesFolder\Globals.ps1"

ForEach ($entity in $domainClasses.GetEnumerator()) 
{
	$entityName = $entity.Key
	$entityNamePlural = $entity.Value

	# Add entity service interface
	Add-ProjectItemViaTemplate ("I" + $entityName + "Service") -Template IEntityService `
		-Model @{ EntityName = $entityName; EntityNamespace = $modelsNamespace; MessagesNamespace = $messagesNamespace; ServiceNamespace = $serviceNamespace; } `
		-SuccessMessage "Added service interface at {0}" `
		-TemplateFolders $TemplateFolders -Project $serviceProjectName -CodeLanguage $CodeLanguage -Force:$Force

	# Add entity service implementation
	Add-ProjectItemViaTemplate ($entityName + "Service") -Template EntityService `
		-Model @{ EntityName = $entityName; EntityNamespace = $modelsNamespace; MessagesNamespace = $messagesNamespace; ServiceNamespace = $serviceNamespace; } `
		-SuccessMessage "Added service implementation at {0}" `
		-TemplateFolders $TemplateFolders -Project $serviceProjectName -CodeLanguage $CodeLanguage -Force:$Force

	# Add service metadata service
	Add-ProjectItemViaTemplate ($entityName + "Service") -Template EntityServiceMetadata `
		-Model @{ EntityName = $entityName; ServiceNamespace = $serviceNamespace; } `
		-SuccessMessage "Added service metada at {0}" `
		-TemplateFolders $TemplateFolders -Project $serviceProjectName -CodeLanguage $CodeLanguage -Force:$Force
}

# Add service Web.config
if ($Config.IsPresent)
{
	Add-ProjectItemViaTemplate ("Web") -Template Web `
		-Model @{ ServiceNamespace = $serviceNamespace; DomainNamespace = $domainProjectName; EntityNamespace = $modelsNamespace; WorkflowNamespace = $workflowNamespace; UnitOfWorkClassName = $unitOfWorkClassName; WorkflowAssemblyName = $workflowProjectName; DomainAssemblyName = $domainProjectName; DbName = $solutionName } `
		-SuccessMessage "Added config at {0}" `
		-TemplateFolders $TemplateFolders -Project $serviceProjectName -CodeLanguage "config" -Force:$Force
}