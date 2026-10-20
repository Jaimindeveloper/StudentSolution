# 02-validate-add-migration: Validate Entity Framework tooling

Restore and build the Razor Pages project, then validate that the EF Core tooling can inspect the `ApplicationDbContext` for migration operations without the vulnerability warning or new build errors.

## Research findings

- The project is SDK-style and targets only `net10.0`, so `dotnet build` is the appropriate validation tool.
- `dotnet build ProductSolutions/ProductSolutions.csproj` already succeeds with no warnings or errors after the explicit PKCS reference and import fixes.
- `dotton ef migrations list` successfully loaded the design-time context and listed the existing migrations `20260927065616_ProductMigration` and `20261005175637_AddCategories`.
- EF tooling reports that the installed tools version `10.0.8` is older than the runtime `10.0.13`; this is an informational compatibility notice, not the NU1902 vulnerability and not a blocking failure.
- The package list resolves `System.Security.Cryptography.Pkcs` to `10.0.13`, with no NU1902 output.

## Validation

Re-run restore/build, confirm the project output and warning state, run available tests, and validate EF migration design-time loading without creating or changing a migration.

**Done when**: The project builds warning-free and the requested Add-Migration workflow can run successfully or an equivalent EF tooling validation confirms the design-time context loads.
