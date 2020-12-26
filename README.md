# Template-NET-Core-React

# How to build and run

1. Install node.js to make react works
2. Install Visual Studio 2019 - [Required for .NET Core SDK version 3.1](https://docs.microsoft.com/en-us/dotnet/core/install/windows?tabs=netcore31#install-with-visual-studio)
3. Restore database using .bak or .sql to SQLServer
4. Try build and run in IIS Express

## How to host on IIS

1. Install [ASP.NET CORE Hosting Bundle](https://dotnet.microsoft.com/download/dotnet-core/3.1)
2. Add domain in C:\Windows\System32\drivers\etc\hosts
3. Add website on IIS using https binding
4. Change .NET CLR Version to "No Manage Code"
5. Publish the project into website root

## Note

1. To enable logs, set stdoutLogEnabled to "true" in web.config
2. To enable development mode, put environmentVariables inside aspNetCore tag in web.config
```xml
<aspNetCore processPath="..." arguments="..." stdoutLogEnabled="..." stdoutLogFile="..." hostingModel="...">
  <environmentVariables>
    <environmentVariable name="ASPNETCORE_ENVIRONMENT" value="Development" />
  </environmentVariables>
</aspNetCore>
```
3. To generate database type "Scaffold-DbContext Name=DefaultConnection Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models/Entity -Context SampleContext -ContextDir Models -Force" in Package Manager Console (set default project Pharmacovigilance.Business)
4. To make entity model compatible with repository, create mod class under "mod/" folder using "IEntity" base class