# 02-validate-add-migration: Validate Entity Framework tooling

Restore and build the Razor Pages project, then validate that the EF Core tooling can inspect the `ApplicationDbContext` for migration operations without the vulnerability warning or new build errors.

**Done when**: The project builds warning-free and the requested Add-Migration workflow can run successfully or an equivalent EF tooling validation confirms the design-time context loads.
