---
name: entity-framework
description: Entity Framework is an open-source object-relational mapper (ORM) framework for dotnet.
---

# Database Guidelines

- **Select an ASPNETCORE environment**: 
```
$env:ASPNETCORE_ENVIRONMENT="Development"
```

- **Create a new migration:**

Output directory is optional [] by default is Migrations 

```
dotnet ef migrations add "<MigrationName>" --project Infrastructure --startup-project API [--output-dir Migrations]
 ```

- **Remove migration:**
````
dotnet ef migrations remove --project Infrastructure --startup-project API [--output-dir Migrations]
````

\* Please Claude do not remove applied migrations (before your changes)

- **Update Database**
```
dotnet ef database update --project Infrastructure --startup-project API
dotnet ef database update <migration> --project Infrastructure --startup-project API
```

\* Please Claude do not update the database, you do not have permissions

- **Naming convention**: Use descriptive migration names.
- **Never modify existing migrations** that have been applied.
- **PostgreSQL-specific**: Use lowercase table/column names in raw SQL.
- **Indexes**: Create indexes for frequently queried columns.
- **Views, Functions and SPs**: Place them in `Migrations/Views/{version}/up` and `down` folders. Use the standard version format YYYYMMDDhhmmss_vX.0.0 for view folders. Use the "20260227154000_v26.0.0" view migration folder as a sample for future migrations. Always recreate all views and SPs, even if they have not been modified, as was done in that migration.