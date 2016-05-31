set baseDir=%~dp0\src

set ASPNETCORE_ENVIRONMENT=Development
set Aurelia_Sample_BaseURI=localhost
set Aurelia_Sample_API_URL=http://%Aurelia_Sample_BaseURI%:57391
set Aurelia_Sample_MVC_URL=http://%Aurelia_Sample_BaseURI%:49849
set Aurelia_Sample_STS_URL=http://%Aurelia_Sample_BaseURI%:22530

start cmd.exe /K "cd %baseDir%\IdSvrHost\ && dotnet run" 
start cmd.exe /K "cd %baseDir%\WebApi\ && dotnet run" 
start cmd.exe /K "cd %baseDir%\AureliaAspNetApp\ && dotnet run" 
start cmd.exe /K "cd %baseDir%\AureliaAspNetApp\ && npm run build"
start cmd.exe /K "cd %baseDir%\AureliaWebsite\ && npm run dev"
