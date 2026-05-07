FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /WebAPI

COPY IncidentAPI_MayaraBouazra/*.sln IncidentAPI_MayaraBouazra/
COPY IncidentAPI_MayaraBouazra/*.csproj IncidentAPI_MayaraBouazra/
COPY AppTest/*.csproj AppTest/


RUN dotnet restore IncidentAPI_MayaraBouazra/IncidentAPI_MayaraBouazra.sln

COPY . .

RUN dotnet publish IncidentAPI_MayaraBouazra/IncidentAPI_MayaraBouazra.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

ENV ASPNETCORE_URLS=http://0.0.0.0:80
EXPOSE 80

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "IncidentAPI_MayaraBouazra.dll"]