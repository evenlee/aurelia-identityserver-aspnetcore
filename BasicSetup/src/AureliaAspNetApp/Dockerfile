FROM microsoft/dotnet:1.0.0-preview1
ARG hostip

RUN curl -sL https://deb.nodesource.com/setup_5.x |  bash -
RUN apt-get install -y nodejs



RUN mkdir -p /AureliaAspNetApp
WORKDIR /AureliaAspNetApp
ONBUILD COPY . /AureliaAspNetApp
ONBUILD RUN dotnet restore
RUN ["npm","install"]
#the docker compose environment var is injected here in the SPA javascript file authConfig.js 
#for magic see following line

RUN sed -i s/docker-provided-apiServerBaseAddress/$hostip/g ./src/authConfig.js

RUN ["npm", "run","build"]


EXPOSE 49849/tcp
ENTRYPOINT ["dotnet", "run"]