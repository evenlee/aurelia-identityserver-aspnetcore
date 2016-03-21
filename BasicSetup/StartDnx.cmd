call dnvm use 1.0.0-rc1-update1 -r coreclr
set ASPNET_ENV=Development
set baseDir=%~dp0\src

start "aurelia asp.net"  dnx-watch --project %baseDir%\AureliaAspNetApp --dnx-args web --server.urls=http://localhost:49849/ 
start cmd.exe /K "cd %baseDir%\AureliaAspNetApp\ && npm run build"
start cmd.exe /K "cd %baseDir%\AureliaWebsite\ && npm run dev"
start "web api"  dnx-watch --project %baseDir%\WebApi --dnx-args web --server.urls=http://localhost:57391/ 
dnx-watch --project %baseDir%\IdSvrHost --dnx-args web --server.urls=http://localhost:22530/



