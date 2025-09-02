# Base image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

# Build image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src


COPY ["back/LoginPage.sln", "back/"]
COPY ["back/LoginPage/LoginPage.csproj", "back/LoginPage/"]
RUN dotnet restore "back/LoginPage.sln"
COPY ["back/", "back/"]
WORKDIR "/src/back/LoginPage"
RUN dotnet build "LoginPage.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish image
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "LoginPage.csproj" -c $BUILD_CONFIGURATION -o /app/publish

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LoginPage.dll"]
