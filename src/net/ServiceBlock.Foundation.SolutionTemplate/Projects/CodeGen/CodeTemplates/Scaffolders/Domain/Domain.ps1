[T4Scaffolding.Scaffolder(Description = "Generates interface and implementation for default Unit of Work and all request/response messages.")][CmdletBinding()]
param(        
    [string]$Project,
	[string]$CodeLanguage,
	[string[]]$TemplateFolders,
	[switch]$Cassandra = $false,
	[switch]$Force = $false
)

# Load global variables
$scriptFolder = Split-Path -Parent $MyInvocation.MyCommand.Path
$includesFolder  = Join-Path -Path $scriptFolder -ChildPath ..\..\..\Includes
. "$includesFolder\Globals.ps1"

# List of all domain classes. Get all top level files/folders in the project | drill down to Models folder | Enumerate ProjectItems | Where Name ends with .cs | Select name truncating .cs, pluralized name
$domainClasses = New-Object "System.Collections.Generic.Dictionary``2[System.String,System.String]"
(Get-Project $domainProjectName).ProjectItems | Where { $_.Name -eq $domainClassesFolder } | ForEach { $_.ProjectItems } | Where { $_.Name.EndsWith('.cs') } | ForEach { $domainClasses.Add($_.Name.SubString(0,$_.Name.Length - 3), (Get-PluralizedWord $_.Name.SubString(0,$_.Name.Length - 3)) ) }

$classOutputPath = $unitOfWorkClassName


# Add UoW Implementation
if ($Cassandra.IsPresent)
{
	Add-ProjectItemViaTemplate $classOutputPath -Template CassandraDefaultUnitOfWork `
		-Model @{ Namespace = $modelsNamespace; ClassName = $unitOfWorkClassName; ConnectionName = $cassandraConnectionName;  DomainClasses=$domainClasses; } `
		-SuccessMessage "Added UoW Implementation at {0}" `
		-TemplateFolders $TemplateFolders -Project $DomainProjectName -CodeLanguage $CodeLanguage -Force:$Force
}
else
{
	Add-ProjectItemViaTemplate $classOutputPath -Template EfDefaultUnitOfWork `
		-Model @{ Namespace = $modelsNamespace; ClassName = $unitOfWorkClassName; ConnectionName = $efConnectionName;  DomainClasses=$domainClasses; } `
		-SuccessMessage "Added UoW Implementation at {0}" `
		-TemplateFolders $TemplateFolders -Project $DomainProjectName -CodeLanguage $CodeLanguage -Force:$Force
}

ForEach ($entity in $domainClasses.GetEnumerator()) 
{
	$entityName = $entity.Key

	# Add AddOrUpdateRequest Message
	Add-ProjectItemViaTemplate ($messagesSubFolderName + "\" + $entityName + "\" + $entityName + "AddOrUpdateRequest") -Template AddOrUpdateRequest `
		-Model @{ Namespace = $messagesNamespace; EntityName = $entityName } `
		-SuccessMessage "Added AddOrUpdateRequest at {0}" `
		-TemplateFolders $TemplateFolders -Project $DomainProjectName -CodeLanguage $CodeLanguage -Force:$Force

	# Add AddOrUpdateResponse Message
	Add-ProjectItemViaTemplate ($messagesSubFolderName + "\" + $entityName + "\" + $entityName + "AddOrUpdateResponse") -Template AddOrUpdateResponse `
		-Model @{ Namespace = $messagesNamespace; EntityName = $entityName } `
		-SuccessMessage "Added AddOrUpdateResponse at {0}" `
		-TemplateFolders $TemplateFolders -Project $DomainProjectName -CodeLanguage $CodeLanguage -Force:$Force

	# Add DeleteRequest Message
	Add-ProjectItemViaTemplate ($messagesSubFolderName + "\" + $entityName + "\" + $entityName + "DeleteRequest") -Template DeleteRequest `
		-Model @{ Namespace = $messagesNamespace; EntityName = $entityName } `
		-SuccessMessage "Added DeleteRequest at {0}" `
		-TemplateFolders $TemplateFolders -Project $DomainProjectName -CodeLanguage $CodeLanguage -Force:$Force

	# Add DeleteResponse Message
	Add-ProjectItemViaTemplate ($messagesSubFolderName + "\" + $entityName + "\" + $entityName + "DeleteResponse") -Template DeleteResponse `
		-Model @{ Namespace = $messagesNamespace; EntityName = $entityName } `
		-SuccessMessage "Added DeleteResponse at {0}" `
		-TemplateFolders $TemplateFolders -Project $DomainProjectName -CodeLanguage $CodeLanguage -Force:$Force

	# Add GetAllRequest Message
	Add-ProjectItemViaTemplate ($messagesSubFolderName + "\" + $entityName + "\" + $entityName + "GetAllRequest") -Template GetAllRequest `
		-Model @{ Namespace = $messagesNamespace; EntityName = $entityName } `
		-SuccessMessage "Added GetAllRequest at {0}" `
		-TemplateFolders $TemplateFolders -Project $DomainProjectName -CodeLanguage $CodeLanguage -Force:$Force

	# Add GetAllResponse Message
	Add-ProjectItemViaTemplate ($messagesSubFolderName + "\" + $entityName + "\" + $entityName + "GetAllResponse") -Template GetAllResponse `
		-Model @{ Namespace = $messagesNamespace; EntityName = $entityName } `
		-SuccessMessage "Added GetAllResponse at {0}" `
		-TemplateFolders $TemplateFolders -Project $DomainProjectName -CodeLanguage $CodeLanguage -Force:$Force

	# Add GetByIdRequest Message
	Add-ProjectItemViaTemplate ($messagesSubFolderName + "\" + $entityName + "\" + $entityName + "GetByIdRequest") -Template GetByIdRequest `
		-Model @{ Namespace = $messagesNamespace; EntityName = $entityName } `
		-SuccessMessage "Added GetByIdRequest at {0}" `
		-TemplateFolders $TemplateFolders -Project $DomainProjectName -CodeLanguage $CodeLanguage -Force:$Force

	# Add GetByIdResponse Message
	Add-ProjectItemViaTemplate ($messagesSubFolderName + "\" + $entityName + "\" + $entityName + "GetByIdResponse") -Template GetByIdResponse `
		-Model @{ Namespace = $messagesNamespace; EntityName = $entityName } `
		-SuccessMessage "Added GetByIdResponse at {0}" `
		-TemplateFolders $TemplateFolders -Project $DomainProjectName -CodeLanguage $CodeLanguage -Force:$Force
	
	# Add GetWithCriteriaRequest Message
	Add-ProjectItemViaTemplate ($messagesSubFolderName + "\" + $entityName + "\" + $entityName + "GetWithCriteriaRequest") -Template GetWithCriteriaRequest `
		-Model @{ Namespace = $messagesNamespace; EntityName = $entityName } `
		-SuccessMessage "Added GetWithCriteriaRequest at {0}" `
		-TemplateFolders $TemplateFolders -Project $DomainProjectName -CodeLanguage $CodeLanguage -Force:$Force

	# Add GetWithCriteriaResponse Message
	Add-ProjectItemViaTemplate ($messagesSubFolderName + "\" + $entityName + "\" + $entityName + "GetWithCriteriaResponse") -Template GetWithCriteriaResponse `
		-Model @{ Namespace = $messagesNamespace; EntityName = $entityName } `
		-SuccessMessage "Added GetWithCriteriaResponse at {0}" `
		-TemplateFolders $TemplateFolders -Project $DomainProjectName -CodeLanguage $CodeLanguage -Force:$Force

}

