[T4Scaffolding.Scaffolder(Description = "Generates service layer for all entities.")][CmdletBinding()]
param(        
    [string]$Project,
	[string]$CodeLanguage,
	[string[]]$TemplateFolders,
	[switch]$Force = $false,
	[switch]$Config = $false,
	[switch]$Cassandra = $false
)

# Load global variables
$scriptFolder = Split-Path -Parent $MyInvocation.MyCommand.Path
$includesFolder  = Join-Path -Path $scriptFolder -ChildPath ..\..\..\Includes
. "$includesFolder\Globals.ps1"

ForEach ($entity in $domainClasses.GetEnumerator()) 
{
	$entityName = $entity.Key
	$entityNamePlural = $entity.Value

	# Add SOAP service interface
	Add-ProjectItemViaTemplate ("Soap\ISoap" + $entityName + "Service") -Template ISoapService `
		-Model @{ EntityName = $entityName; EntityNamespace = $modelsNamespace; MessagesNamespace = $messagesNamespace; ServiceNamespace = $serviceNamespace; } `
		-SuccessMessage "Added SOAP service interface at {0}" `
		-TemplateFolders $TemplateFolders -Project $serviceProjectName -CodeLanguage $CodeLanguage -Force:$Force

	# Add SOAP service implementation
	Add-ProjectItemViaTemplate ("Soap\Soap" + $entityName + "Service") -Template SoapService `
		-Model @{ EntityName = $entityName; EntityNamespace = $modelsNamespace; MessagesNamespace = $messagesNamespace; ServiceNamespace = $serviceNamespace; } `
		-SuccessMessage "Added SOAP service implementation at {0}" `
		-TemplateFolders $TemplateFolders -Project $serviceProjectName -CodeLanguage $CodeLanguage -Force:$Force

	# Add REST service interface
	Add-ProjectItemViaTemplate ("Rest\IRest" + $entityName + "Service") -Template IRestService `
		-Model @{ EntityName = $entityName; EntityNamespace = $modelsNamespace; MessagesNamespace = $messagesNamespace; ServiceNamespace = $serviceNamespace; } `
		-SuccessMessage "Added REST service interface at {0}" `
		-TemplateFolders $TemplateFolders -Project $serviceProjectName -CodeLanguage $CodeLanguage -Force:$Force

	# Add REST service implementation
	Add-ProjectItemViaTemplate ("Rest\Rest" + $entityName + "Service") -Template RestService `
		-Model @{ EntityName = $entityName; EntityNamespace = $modelsNamespace; MessagesNamespace = $messagesNamespace; ServiceNamespace = $serviceNamespace; } `
		-SuccessMessage "Added REST service implementation at {0}" `
		-TemplateFolders $TemplateFolders -Project $serviceProjectName -CodeLanguage $CodeLanguage -Force:$Force

}

# Add SOAP routes
Add-ProjectItemViaTemplate ("Routing\SoapRoutes") -Template SoapRoutes `
	-Model @{ DomainClasses=$domainClasses; } `
	-SuccessMessage "Added SOAP routes at {0}" `
	-TemplateFolders $TemplateFolders -Project $serviceProjectName -CodeLanguage $CodeLanguage -Force:$Force
	
# Add REST routes
Add-ProjectItemViaTemplate ("Routing\RestRoutes") -Template RestRoutes `
	-Model @{ DomainClasses=$domainClasses; } `
	-SuccessMessage "Added REST routes at {0}" `
	-TemplateFolders $TemplateFolders -Project $serviceProjectName -CodeLanguage $CodeLanguage -Force:$Force

# Add service Web.config
if ($Config.IsPresent)
{
	Add-ProjectItemViaTemplate ("Web") -Template Web `
		-Model @{ ServiceNamespace = $serviceNamespace; DomainNamespace = $domainProjectName; EntityNamespace = $modelsNamespace; WorkflowNamespace = $workflowNamespace; UnitOfWorkClassName = $unitOfWorkClassName; WorkflowAssemblyName = $workflowProjectName; DomainAssemblyName = $domainProjectName; DbName = $solutionName; IsCassandra = $Cassandra.IsPresent; } `
		-SuccessMessage "Added config at {0}" `
		-TemplateFolders $TemplateFolders -Project $serviceProjectName -CodeLanguage "config" -Force:$Force

	Add-ProjectItemViaTemplate ("Global") -Template GlobalMetada `
		-Model @{ ServiceNamespace = $serviceNamespace; DomainNamespace = $domainProjectName; EntityNamespace = $modelsNamespace; WorkflowNamespace = $workflowNamespace; UnitOfWorkClassName = $unitOfWorkClassName; WorkflowAssemblyName = $workflowProjectName; DomainAssemblyName = $domainProjectName; DbName = $solutionName } `
		-SuccessMessage "Added Global.asax at {0}" `
		-TemplateFolders $TemplateFolders -Project $serviceProjectName -CodeLanguage $CodeLanguage -Force:$Force

	Add-ProjectItemViaTemplate ("Global") -Template GlobalCodeBehind `
		-Model @{ ServiceNamespace = $serviceNamespace; DomainNamespace = $domainProjectName; EntityNamespace = $modelsNamespace; WorkflowNamespace = $workflowNamespace; UnitOfWorkClassName = $unitOfWorkClassName; WorkflowAssemblyName = $workflowProjectName; DomainAssemblyName = $domainProjectName; DbName = $solutionName } `
		-SuccessMessage "Added Global code behind at {0}" `
		-TemplateFolders $TemplateFolders -Project $serviceProjectName -CodeLanguage $CodeLanguage -Force:$Force

}