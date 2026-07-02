# Manual Package Generation with dotnet pack

This repository uses a dotnet-only workflow for package generation.

## Prerequisites

- .NET SDK installed (`dotnet --version`)
- Run commands from the repository root

## 1. Restore dependencies

```powershell
dotnet restore src
```

## 2. Build projects in Release mode

```powershell
dotnet build src/StructureMap/StructureMap.csproj --configuration Release --no-restore
dotnet build src/StructureMap.AutoFactory/StructureMap.AutoFactory.csproj --configuration Release --no-restore
dotnet build src/StructureMap.DynamicInterception/StructureMap.DynamicInterception.csproj --configuration Release --no-restore
```

## 3. Run tests

```powershell
dotnet test src/StructureMap.Testing/StructureMap.Testing.csproj --configuration Release --no-build
dotnet test src/StructureMap.AutoFactory.Testing/StructureMap.AutoFactory.Testing.csproj --configuration Release --no-build
dotnet test src/StructureMap.DynamicInterception.Testing/StructureMap.DynamicInterception.Testing.csproj --configuration Release --no-build
```

## 4. Pack NuGet packages

```powershell
dotnet pack src/StructureMap/StructureMap.csproj --configuration Release --no-build --output artifacts
dotnet pack src/StructureMap.AutoFactory/StructureMap.AutoFactory.csproj --configuration Release --no-build --output artifacts
dotnet pack src/StructureMap.DynamicInterception/StructureMap.DynamicInterception.csproj --configuration Release --no-build --output artifacts
```

Generated packages are written to the `artifacts` folder.

## Optional: clean output folders first

If you want a fresh package output:

```powershell
if (Test-Path artifacts) { Remove-Item artifacts -Recurse -Force }
New-Item -Path artifacts -ItemType Directory | Out-Null
```
