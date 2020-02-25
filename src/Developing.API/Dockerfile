FROM mcr.microsoft.com/dotnet/core/sdk:3.1.102-alpine3.11 AS build
WORKDIR /app

COPY src/Developing.API/Developing.API.csproj ./
RUN dotnet restore

COPY src/Developing.API ./
RUN dotnet tool install --global dotnet-ef --version 3.1.2
RUN dotnet publish -c release -o dist
RUN export PATH="$PATH:$HOME/.dotnet/tools" && dotnet ef migrations script -i -o dist/migrate.sql

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.2-alpine3.11
WORKDIR /app
COPY --from=build /app/dist .
ENTRYPOINT ["dotnet", "Developing.API.dll"]
