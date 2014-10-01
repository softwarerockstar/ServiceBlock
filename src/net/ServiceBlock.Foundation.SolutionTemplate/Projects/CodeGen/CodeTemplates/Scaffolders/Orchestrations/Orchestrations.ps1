[T4Scaffolding.Scaffolder(Description = "Generates orchestration workflows for all entities.")][CmdletBinding()]
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

	$primaryKeyName = [string](Get-PrimaryKey "$modelsNamespace.$entityName" -Project $domainProjectName)
	
	$entityCodeType = Get-ProjectType $EntityName -Project $domainProjectName -BlockUi
	if ($entityCodeType) { $relatedEntities = [Array](Get-RelatedEntities $entityCodeType.FullName -Project $domainProjectName) }
	if (!$relatedEntities) { $relatedEntities = @() }

	# Add AddOrUpdate orchestration
	Add-ProjectItemViaTemplate ($workflowFolder + "\" + $entityName + "\AddOrUpdate") -Template AddOrUpdate `
		-Model @{ EntityName = $entityName; EntityNamespace = $modelsNamespace; WorkflowNamespace = $workflowNamespace; DataNamespace = $dataNamespace; DomainNamespace = $domainProjectName; MessagesNamespace = $messagesNamespace; DataAssemblyName = $dataProjectName; WorkflowAssemblyName = $workflowProjectName; UoWInterfaceName = $unitOfWorkInterfaceName; } `
		-SuccessMessage "Added AddOrUpdate at {0}" `
		-TemplateFolders $TemplateFolders -Project $workflowProjectName -CodeLanguage 'xaml' -Force:$Force
	
	# Add Delete orchestration
	Add-ProjectItemViaTemplate ($workflowFolder + "\" + $entityName + "\Delete") -Template Delete `
		-Model @{ EntityName = $entityName; EntityNamespace = $modelsNamespace; WorkflowNamespace = $workflowNamespace; DataNamespace = $dataNamespace; DomainNamespace = $domainProjectName; MessagesNamespace = $messagesNamespace; DataAssemblyName = $dataProjectName; WorkflowAssemblyName = $workflowProjectName; UoWInterfaceName = $unitOfWorkInterfaceName; } `
		-SuccessMessage "Added Delete at {0}" `
		-TemplateFolders $TemplateFolders -Project $workflowProjectName -CodeLanguage 'xaml' -Force:$Force

	# Add GetAll orchestration
	Add-ProjectItemViaTemplate ($workflowFolder + "\" + $entityName + "\GetAll") -Template GetAll `
		-Model @{ EntityName = $entityName; EntityNamespace = $modelsNamespace; WorkflowNamespace = $workflowNamespace; DataNamespace = $dataNamespace; DomainNamespace = $domainProjectName; MessagesNamespace = $messagesNamespace; DataAssemblyName = $dataProjectName; WorkflowAssemblyName = $workflowProjectName; UoWInterfaceName = $unitOfWorkInterfaceName; } `
		-SuccessMessage "Added GetAll at {0}" `
		-TemplateFolders $TemplateFolders -Project $workflowProjectName -CodeLanguage 'xaml' -Force:$Force

	# Add GetById orchestration
	Add-ProjectItemViaTemplate ($workflowFolder + "\" + $entityName + "\GetById") -Template GetById `
		-Model @{ EntityName = $entityName; Entity = $entityCodeType; PrimaryKeyName = $primaryKeyName; RelatedEntities = $relatedEntities; EntityNamespace = $modelsNamespace; WorkflowNamespace = $workflowNamespace; DataNamespace = $dataNamespace; DomainNamespace = $domainProjectName; MessagesNamespace = $messagesNamespace; DataAssemblyName = $dataProjectName; WorkflowAssemblyName = $workflowProjectName; UoWInterfaceName = $unitOfWorkInterfaceName; } `
		-SuccessMessage "Added GetById at {0}" `
		-TemplateFolders $TemplateFolders -Project $workflowProjectName -CodeLanguage 'xaml' -Force:$Force

	# Add GetWithCriteria orchestration
	Add-ProjectItemViaTemplate ($workflowFolder + "\" + $entityName + "\GetWithCriteria") -Template GetWithCriteria `
		-Model @{ EntityName = $entityName; Entity = $entityCodeType; PrimaryKeyName = $primaryKeyName; RelatedEntities = $relatedEntities; EntityNamespace = $modelsNamespace; WorkflowNamespace = $workflowNamespace; DataNamespace = $dataNamespace; DomainNamespace = $domainProjectName; MessagesNamespace = $messagesNamespace; DataAssemblyName = $dataProjectName; WorkflowAssemblyName = $workflowProjectName; UoWInterfaceName = $unitOfWorkInterfaceName; } `
		-SuccessMessage "Added GetWithCriteria at {0}" `
		-TemplateFolders $TemplateFolders -Project $workflowProjectName -CodeLanguage 'xaml' -Force:$Force

}

