Code-first migrations allow database schema to be updated without losing data when changes are made to model entities.

To enable migrations, from the Package Manager Console:

1. Enter command:
	Enable-Migrations -ProjectName Domain -StartupProjectName Services -EnableAutomaticMigrations 

2. Add initial migration using the following command:
	Add-Migration Initial -ProjectName Domain -StartupProjectName Services

3. When you make any changes to models that require DB changes, use the following command to update the database and migrate existing data:
	Update-Database -verbose -ProjectName Domain -StartupProjectName Services

For more information refer to:
http://msdn.microsoft.com/en-US/data/jj554735


