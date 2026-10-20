# NuGet Package Upgrade Plan

## Overview

**Target**: Override the vulnerable transitive `System.Security.Cryptography.Pkcs` 9.0.11 dependency with the latest stable `.NET 10`-compatible version.
**Scope**: One .NET 10 Razor Pages project (`ProductSolutions/ProductSolutions.csproj`).

## Tasks

### 01-pin-pkcs-package: Pin the secure PKCS package version

Add an explicit `System.Security.Cryptography.Pkcs` package reference at the latest stable version supported by the project so NuGet resolves it instead of the vulnerable transitive 9.0.11 version brought by `Microsoft.Data.SqlClient`.

**Done when**: Restore resolves `System.Security.Cryptography.Pkcs` to 10.0.13 or newer supported stable version, NU1902 is absent, and no package API changes require source fixes.

---

### 02-validate-add-migration: Validate Entity Framework tooling

Restore and build the Razor Pages project, then validate that the EF Core tooling can inspect the `ApplicationDbContext` for migration operations without the vulnerability warning or new build errors.

**Done when**: The project builds warning-free and the requested Add-Migration workflow can run successfully or an equivalent EF tooling validation confirms the design-time context loads.
