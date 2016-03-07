# aurelia-identityserver-aspnetcore
## Basic Sample
### introduction
This sample shows aurelia in combination with a web api and identityserver 4. The aurelia plugin aurelia-auth is used to integrate the identityserver oauth and openId connect exchange.
### how to install
  cd src/AureliaAspNetApp
  npm install
  jspm install -y
  dnu restore
  
  cd src/AureliaWebsite
  npm install
  jspm install -y
  
  cd src/IdSvrHost
  dnu restore
  
  cd src/IdSvrConstants
  dnu restore
  
  cd src/WebApi
  dnu restore

### components overview
#### IdSvrHost
An asp.net core project containing Thinktecture's IdentityServer version 4. We use simple in memory Clients (for AureliaAspNetApp and AureliaWebsite) , Scopes and Users. 
#### IdSvrConstants
An asp.net core project containing a constants class for carrying the various Urls.
#### WebApi
An asp.net core project containing 2 'protected' controllers: CustomersController and IdentityController. An application (client) that want to make use of the api needs the 'crm' scope. The purpose of the IdentityController is to visualize the user's claims through the eyes of the web api.
#### AureliaAspNetApp
An asp.net core project which is basically an mvc project with only two controllers and one view. The only purpose is to 'serve' an aurelia application. But, since we have a 'backend', we can use the oauth AUTHORIZATION CODE FLOW. The Home controller 'serves' the aurelia application and the TokenController allows the aurelia app to exchange an authorization code received with the users consent, with an access token.
#### AureliaWebsite
A simple website running the aurelia app as static asset. So the aurelia application has no real backend here (as it has in AureliaAspNetApp). This implies that we can NOT use the oauth AUTHORIZATION CODE FLOW here. Instead we use the IMPLICIT flow.
