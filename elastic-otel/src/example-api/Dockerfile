FROM mcr.microsoft.com/dotnet/sdk:9.0 as build
WORKDIR /App

COPY ./ ./

RUN ls -la
RUN dotnet restore ./Example.Api.csproj --configfile ./Nuget.Config
RUN dotnet publish ./Example.Api.csproj -c Release -o ./out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
EXPOSE 8080
EXPOSE 8081

WORKDIR /App
COPY --from=build /App/out .
ENTRYPOINT ["dotnet", "Example.Api.dll"]
