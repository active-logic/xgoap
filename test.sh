#!/bin/bash
# Setup .NET boilerplate; pass any arg to also run tests
# For local coverage, this tool needs to be installed once
# dotnet tool install -g dotnet-reportgenerator-globaltool
rm Runtime/Runtime.csproj Tests/Tests.csproj Main.sln
set -euo pipefail
dotnet new solution --name "Main"
dotnet new classlib --name "Runtime" --force && rm Runtime/Class1.cs
dotnet new nunit    --name "Tests"   --force && rm Tests/UnitTest1.cs
dotnet add "Tests/Tests.csproj" reference "Runtime/Runtime.csproj"
dotnet add "Tests/Tests.csproj" package coverlet.msbuild
dotnet sln add "Runtime/Runtime.csproj" "Tests/Tests.csproj"
if [ "$#" -eq  "0" ]
   then
     exit 0
 else
     dotnet test -c Debug -p:CollectCoverage=true \
                          -p:CoverletOutputFormat=opencover
     reportgenerator -reports:Tests/coverage.opencover.xml \
                     -targetdir:../../CoverageReport
     ./clean.sh
     open ../../CoverageReport/index.htm
 fi
