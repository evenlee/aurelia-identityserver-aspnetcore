# aurelia-identityserver-aspnetcore
## Basic Sample
### introduction
This sample shows aurelia in combination with a web api and identityserver 4. The aurelia plugin aurelia-auth is used to integrate the identityserver oauth and openId connect exchange.
### how to install?
  * cd src/AureliaAspNetApp
    * npm install
    * dotnet restore
  
  * cd src/AureliaWebsite
    * npm install
  
  * cd src/IdSvrHost
    * dotnet restore
  
  * cd src/WebApi
    * dotnet restore

### how to run?
The root of the solution contains a .cmd file which will start all projects. 
We use dotnet watch, so file changes will result in an immediate recompilation of the sources.

### WebPack
The webprojects AureliaAspNetApp and AureliaWebsite don't use jspm any longer but I migrated things to webpack/NPM.

### which editor/IDE should i use?
You are completely free to choose whatever you like. I like the simplicity and speed of visual studio code.
Currently it's not possible to debug c# code with visual studio code. So, if you need debugging, use visual studio.

1. Visual studio
  * You can either run StartDotNet.cmd or simply press F5
     
2. Visual studio Code or any other editor like SublimeText
  * run StartDotNet.cmd

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

### Dockerfiles and docker-compose

The individual projects contain a Dockerfile. In the root src folder you will find a docker-compose.yml file.
Make sure you have a linux box at your disposal (DigitalOcean, Azure, VirtualBox, ...) and install docker
and docker-compose:

* step 1: install docker: follow instructions from https://docs.docker.com/engine/installation/linux/ubuntulinux/
* step 2: install docker-compose:
```
    sudo apt-get -y install python-pip
    sudo pip install docker-compose
```
* step 3: git clone this repository on the linux box:
```
git clone https://github.com/paulvanbladel/aurelia-identityserver-aspnetcore.git
```
* step 4: run docker-compose
```
    cd aurelia-identityserver-aspnetcore/basicSetup/src/
    sudo sh  start-docker-compose.sh
```
* step 5: take a coffee or two and wait a bit

### Understanding the docker-compose approach

A common difficulty in software deployment is parameter configuration management. In this sample app we have configuration both on the level of the various
.NET projects, but also on the level of the SPA's (i.e. the Aurelia single page applications).
So, also with docker we need to deal with parameters. The most important param to deal with in this sample, is the IP address of the docker host.

The .NET projects all have a class containing settings: AppSettings.cs . The Ip address setting is stored in a property BaseUri which is received via an environment variable.

The docker-compose file creates this environment variable called Aurelia_Sample_BaseURI. Obviously, we could hard code an IP Address in the docker-compose file, but we tried to be
more flexible. the bash script start-docker-compose.sh applies a neat trick to retrieve the IP Address and inject it in the docker-compose.yml file.
```
#!/bin/bash

# get ip address from this docker host
HOSTIP=$(ip -f inet -o addr show eth0|cut -d\  -f 7 | cut -d/ -f 1)
# run docker-compose with patched docker-compose file  containing the docker host ip address instead of the placeholder
sed -e "s/REPLACE_WITH_DOCKERHOSTIP/$HOSTIP/g" docker-compose.yml | docker-compose --file - up -dpaul@ubuntu:~/aurelia-identityserver-aspnetcore/BasicSetup/src$
```

A more challenging problem is to inject the baseURI in the SPA. We have a two step approach here.

The docker-compose file for the SPA uses an ARGS section:
```
 aureliaaspnetapp:
    build:
      context: ./AureliaAspNetApp
      args:
        hostip: REPLACE_WITH_DOCKERHOSTIP
 ```
 so a variable hostip (containing the IP address of the docker host) will be passed to the dockerfile of AureliaAspNetApp.
 
 When the dockerfile receives this hostip variable it's used to make text file replacement on the file ./src/authConfig.js
 
 ```
 RUN sed -i s/docker-provided-apiServerBaseAddress/$hostip/g ./src/authConfig.js

 ```
 
 Indeed, some magic but it works.






