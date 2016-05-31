FROM microsoft/dotnet:1.0.0-preview1
RUN mkdir -p /IdSvrHost
WORKDIR /IdSvrHost
ONBUILD COPY . /IdSvrHost
ONBUILD RUN dotnet restore
EXPOSE 22530/tcp
ENTRYPOINT ["dotnet", "run"]