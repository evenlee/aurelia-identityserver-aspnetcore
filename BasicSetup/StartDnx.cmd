rem call dnvm use 1.0.0-rc1-update1 -r coreclr
set ASPNET_ENV=Development
set Aurelia_Sample_BaseURI=localhost
set baseDir=%~dp0\src

rem start "aurelia asp.net"  dnx-watch --project %baseDir%\AureliaAspNetApp --dnx-args web --server.urls=http://localhost:49849/ 


rem start cmd.exe /K "cd %baseDir%\AureliaWebsite\ && npm run dev"
rem replacing the web api project with RC2
rem start "web api"  dnx-watch --project %baseDir%\WebApi --dnx-args web --server.urls=http://localhost:57391/ 
rem dnx-watch --project %baseDir%\IdSvrHost --dnx-args web --server.urls=http://localhost:22530/
start cmd.exe /K "cd %baseDir%\IdSvrHost\ && dotnet run" 


start cmd.exe /K "cd %baseDir%\AureliaAspNetApp\ && dotnet run" 
start cmd.exe /K "cd %baseDir%\AureliaAspNetApp\ && npm run build"

