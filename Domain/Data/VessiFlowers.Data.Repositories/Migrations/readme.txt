1. To add new migration:
PM> Add-Migration NewMigrationName
2. To generate migration script:
PM> Update-Database -Script -SourceMigration:0
3. Remove *.cs generated files in current folder