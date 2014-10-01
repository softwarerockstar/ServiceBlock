GETTING STARTED:
1. Right-click Solution in Solution Explorer.
2. Click on Enable NuGet Package Restore.  This will take a minute.
3. In the Package Manager Console window a button will appear to Download missing packages. Click that button.
4.  In the Package Manager Console window select VHA.CodeGen.ServiceFoundation as the default project.
5. Update packages by issuing following commands on PM>:
	
	a. Update-Package T4Scaffolding -Reinstall
	b. Update-Package VHA.ServiceFoundation
	c. Update-Package VHA.ServiceFoundation.DataProviders

ENABLING ENTERPRISE LIBRARY CONFIGURATION CONSOLE:

1. Right click on Solution in Solution Explorer.
2. Hit F4 to display solution properties.
3. In the Enterprise Library Binaries Path enter the following:
	packages\Unity.2.1.505.2\lib\NET35;packages\Unity.Interception.2.1.505.2\lib\NET35;packages\EnterpriseLibrary.Common.5.0.505.0\lib\NET35;packages\EnterpriseLibrary.ExceptionHandling.5.0.505.0\lib\NET35;packages\EnterpriseLibrary.Logging.5.0.505.1\lib\NET35;packages\EnterpriseLibrary.ExceptionHandling.Logging.5.0.505.0\lib\NET35;packages\EnterpriseLibrary.ExceptionHandling.WCF.5.0.505.0\lib\NET35;packages\EnterpriseLibrary.Validation.5.0.505.0\lib\NET35

GENERATING CODE:
1. Add one or more classes to Models folder in the Domain project.
2. In the Package Manager Console window type in:
	
	For SQL Server:
		Scaffold EnterpriseApp -UI -Layout -Config -Force

	For Cassandra:
		Scaffold EnterpriseApp -UI -Layout -Config -Force -Cassandra

3. Compile and run Services project.
4. Add service references to services in the WebUI project
5. Run solution.

