#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0.0-preview.2-bullseye-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0.100-preview.2-bullseye-slim AS build
RUN apt-get update -yq \
    && apt-get install curl gnupg -yq \
    && curl -sL https://deb.nodesource.com/setup_14.x | bash \
    && apt-get install nodejs -yq \
    && npm install pnpm -g

WORKDIR /src
COPY MyHome.sln /src/
COPY src/Domain/*.csproj /src/Domain/
COPY src/UseCases/Application/*.csproj /src/UseCases/Application/
COPY src/Infrastructure/Infrastructure/*.csproj /src/Infrastructure/Infrastructure/
COPY src/Entrypoints/HttpService/*.csproj /src/Entrypoints/HttpService/

WORKDIR /src/Entrypoints/HttpService
RUN dotnet restore MyHome.HttpService.csproj

WORKDIR /src
COPY src /src/
WORKDIR /src/Entrypoints/HttpService
RUN dotnet build MyHome.HttpService.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish MyHome.HttpService.csproj -c Release -o /app/publish
WORKDIR /src/Entrypoints/Dashboard
RUN npm install && npx ng build --prod --output-path /app/publish/ClientApp/dist

FROM base AS final
RUN apt-get update -yq \
    && apt-get install libc6-dev  libgdiplus -yq 
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyHome.HttpService.dll"]
