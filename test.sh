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
     dotnet test -c Release -p:DefineConstants=DOTNET_CORE
     dotnet test -c Debug -p:DefineConstants=DOTNET_CORE
 fi
