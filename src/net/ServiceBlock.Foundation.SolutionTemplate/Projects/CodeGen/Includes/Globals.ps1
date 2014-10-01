$domainProjectName = "Domain"
$domainClassesFolder = "Models"
$modelNamespace = $domainProjectName + "." + $domainClassesFolder 

$dataProjectName = "DataActivities"
$activityClassesFolder = "Activities"
$unitOfWorkInterfaceName = "IUnitOfWorkEx"
$dataNamespace = $dataProjectName + "." +  $activityClassesFolder  #DataActivities.Activities

$domainProjectName = "Domain"
$domainClassesFolder = "Models"
$unitOfWorkClassName = "DefaultUnitOfWork"
$efConnectionName = "DefaultEntityFrameworkDataContext"
$cassandraConnectionName = "DefaultCassandraDataContext"

$messagesSubFolderName = "Messages"
$modelsNamespace = $domainProjectName + "." +  $domainClassesFolder  # "Domain.Models"
$messagesNamespace = $domainProjectName + "." + $messagesSubFolderName

$workflowProjectName = "Orchestrations"
$workflowFolder = "Workflows"
$workflowNamespace = $workflowProjectName + "." + $workflowFolder

$serviceFolder = ""
$serviceProjectName = "Services"
$serviceNamespace = $serviceProjectName

$webProjectName = "WebUI"
$webNamespace = (Get-Project $webProjectName).Properties.Item("DefaultNamespace").Value


$solutionName = [System.IO.Path]::GetFileNameWithoutExtension($dte.Solution.FullName)

# List of all domain classes. Get all top level files/folders in the project | drill down to Models folder | Enumerate ProjectItems | Where Name ends with .cs | Select name truncating .cs, pluralized name
$domainClasses = New-Object "System.Collections.Generic.Dictionary``2[System.String,System.String]"
(Get-Project $domainProjectName).ProjectItems | Where { $_.Name -eq $domainClassesFolder } | ForEach { $_.ProjectItems } | Where { $_.Name.EndsWith('.cs') } | ForEach { $domainClasses.Add($_.Name.SubString(0,$_.Name.Length - 3), (Get-PluralizedWord $_.Name.SubString(0,$_.Name.Length - 3)) ) }
