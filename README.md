# aurelia-identityserver-aspnetcore
## Basic Sample
### introduction
This sample shows aurelia in combination with a web api and identityserver 4. The aurelia plugin aurelia-auth is used to integrate the identityserver oauth and openId connect exchange.
### how to install?
  * cd src/AureliaAspNetApp
    * npm install
    * dnu restore
  
  * cd src/AureliaWebsite
    * npm install
  
  * cd src/IdSvrHost
    * dnu restore
  
  * cd src/IdSvrConstants
    * dnu restore
  
  * cd src/WebApi
    * dnu restore
    
### how to run?
The root of the solution contains a .cmd file which will start all projects. 
We use dnx-watch, so file changes will result in an immediate recompilation of the sources.

### WebPack
The webprojects AureliaAspNetApp and AureliaWebsite don't use jspm any longer but I migrated things to webpack/NPM.

### which editor/IDE should i use?
You are completely free to choose whatever you like. I like the simplicity and speed of visual studio code.
Currently it's not possible to debug c# code with visual studio code. So, if you need debugging, use visual studio.

1. Visual studio
  * You can either run StartDnx.cmd or simply press F5
     
2. Visual studio Code or any other editor like SublimeText
  * run StartDnx.cmd

### components overview
#### IdSvrHost
An asp.net core project containing Thinktecture's IdentityServer version 4. We use simple in memory Clients (for AureliaAspNetApp and AureliaWebsite) , Scopes and Users. 
#### WebApi
An asp.net core project containing 2 'protected' controllers: CustomersController and IdentityController. 
An application (client) that want to make use of the api needs the 'crm' scope. 
The purpose of the IdentityController is to visualize the user's claims through the eyes of the web api.
#### AureliaAspNetApp
An asp.net core project which is basically an mvc project with only two controllers and one view. The only purpose is to 'serve' an aurelia application. 
But, since we have a 'backend', we can use the oauth AUTHORIZATION CODE FLOW. The Home controller 'serves' the aurelia application 
and the TokenController allows the aurelia app to exchange an authorization code received with the users consent, for an access token.
#### AureliaWebsite
A simple website running the aurelia app as static asset. So the aurelia application has no real backend here (as it has in AureliaAspNetApp). 
This implies that we can NOT use the oauth AUTHORIZATION CODE FLOW here. Instead we use the IMPLICIT flow.
