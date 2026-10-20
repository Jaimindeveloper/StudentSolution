# 01-pin-pkcs-package: Pin the secure PKCS package version

Add an explicit `System.Security.Cryptography.Pkcs` package reference at the latest stable version supported by the project so NuGet resolves it instead of the vulnerable transitive 9.0.11 version brought by `Microsoft.Data.SqlClient`.

## Research findings

- Affected project: `ProductSolutions/ProductSolutions.csproj`, an SDK-style .NET 10 Razor Pages project.
- Package management mode: standard `PackageReference`; no Central Package Management was reported.
- Existing direct packages are `Microsoft.AspNetCore.Identity.EntityFrameworkCore`, `Microsoft.AspNetCore.Identity.UI`, `Microsoft.EntityFrameworkCore`, `Microsoft.EntityFrameworkCore.SqlServer`, and `Microsoft.EntityFrameworkCore.Tools`, all at 10.0.13.
- `ProductSolutions/obj/project.assets.json` shows `Microsoft.Data.SqlClient 6.1.7` depends on `System.Security.Cryptography.Pkcs 9.0.11`.
- The package assessment did not find a direct reference because the vulnerable package is transitive. `get_supported_package_version` resolved stable version `10.0.13` for `net10.0`.
- No source API changes were identified by the quick assessment.

## Execution

Add a direct `PackageReference` for `System.Security.Cryptography.Pkcs` version `10.0.13` to override the transitive dependency, then restore and build with the SDK-style `dotnet` workflow.

**Done when**: Restore resolves `System.Security.Cryptography.Pkcs` to 10.0.13 or newer supported stable version, NU1902 is absent, and no package API changes require source fixes.
