# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Backend/Microservices/Kong/k0002/API.Northwind/API.Northwind.csproj", "src/Backend/Microservices/Kong/k0002/API.Northwind/"]
COPY ["src/Backend/Common/Common.Api/Common.Api.csproj", "src/Backend/Common/Common.Api/"]
COPY ["src/Backend/Common/Common.Notification/Common.Notification.csproj", "src/Backend/Common/Common.Notification/"]
COPY ["src/Backend/Common/Common.Domain/Common.Domain.csproj", "src/Backend/Common/Common.Domain/"]
COPY ["src/Backend/Common/Common.Logging/Common.Logging.csproj", "src/Backend/Common/Common.Logging/"]
COPY ["src/Backend/Services/Northwind/NorthwindService/NorthwindService.csproj", "src/Backend/Services/Northwind/NorthwindService/"]
COPY ["src/Backend/Common/Common.Extensions/Common.Extensions.csproj", "src/Backend/Common/Common.Extensions/"]
COPY ["src/Backend/Common/Common.UnitOfWork/Common.UnitOfWork.csproj", "src/Backend/Common/Common.UnitOfWork/"]
COPY ["src/Backend/Infra/Northwind/Northwind.csproj", "src/Backend/Infra/Northwind/"]
RUN dotnet restore "./src/Backend/Microservices/Kong/k0002/API.Northwind/API.Northwind.csproj"
COPY . .
WORKDIR "/src/src/Backend/Microservices/Kong/k0002/API.Northwind"
RUN dotnet build "./API.Northwind.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./API.Northwind.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.Northwind.dll"]