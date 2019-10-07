# Setup .NET boilerplate; pass any arg to also run tests
rm Runtime/Runtime.csproj Tests/Tests.csproj Main.sln
set -euo pipefail
dotnet new solution --name "Main"
dotnet new classlib --name "Runtime" --force && rm Runtime/Class1.cs
dotnet new nunit    --name "Tests"   --force && rm Tests/UnitTest1.cs
dotnet add "Tests/Tests.csproj" reference "Runtime/Runtime.csproj"
dotnet sln add "Runtime/Runtime.csproj" "Tests/Tests.csproj"
if [ "$#" -eq  "0" ]
   then
     exit 0
 else
     dotnet test -c Release
     dotnet test -c Debug
     rm Main.sln
     rm Runtime/Runtime.csproj
     rm -rf Runtime/obj
     rm -rf Runtime/bin
     rm Tests/Tests.csproj
     rm -rf Tests/obj
     rm -rf Tests/bin
 fi
