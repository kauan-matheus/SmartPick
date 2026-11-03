FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY WarehouseManagementService.Api/*.csproj WarehouseManagementService.Api/
COPY WarehouseManagementService.Application/*.csproj WarehouseManagementService.Application/
COPY WarehouseManagementService.Communication/*.csproj WarehouseManagementService.Communication/
COPY WarehouseManagementService.Domain/*.csproj WarehouseManagementService.Domain/
COPY WarehouseManagementService.Infra.Data/*.csproj WarehouseManagementService.Infra.Data/
COPY WarehouseManagementService.Infra.IOC/*.csproj WarehouseManagementService.Infra.IOC/
RUN dotnet restore WarehouseManagementService.Api

COPY . .
RUN dotnet publish WarehouseManagementService.Api -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine
WORKDIR /app
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1
COPY --from=build /app .

USER app
EXPOSE 8080
HEALTHCHECK --interval=15s --timeout=3s --start-period=20s \
  CMD wget -q --spider http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "WarehouseManagementService.Api.dll"]
