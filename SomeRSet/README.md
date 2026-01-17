# SomeRSet

Scaffolded solution for SomeRSet.

Structure created:

- src/SomeRSet: application project
- tests/SomeRSet.Tests: xUnit tests project

Run the following to finish setup:

```powershell
cd SomeRSet
dotnet new sln -n SomeRSet
dotnet sln add src/SomeRSet/SomeRSet.csproj
dotnet sln add tests/SomeRSet.Tests/SomeRSet.Tests.csproj
dotnet add tests/SomeRSet.Tests/SomeRSet.Tests.csproj reference src/SomeRSet/SomeRSet.csproj
```