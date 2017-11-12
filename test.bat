cd src
dotnet restore
cd BuildIndicatron.Tests
dotnet test
cd ..
cd BuildIndicatron.Server.Tests
dotnet test
cd ..
cd ..
pause
