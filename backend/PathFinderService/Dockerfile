FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY PathFinderService.Grpc/*.csproj PathFinderService.Grpc/
COPY PathFinderService.Domain/*.csproj PathFinderService.Domain/
COPY PathFinderService.Communication/*.csproj PathFinderService.Communication/
RUN dotnet restore PathFinderService.Grpc

COPY . .
RUN dotnet publish PathFinderService.Grpc -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine
WORKDIR /app
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1
COPY --from=build /app .

USER app
EXPOSE 8080
HEALTHCHECK --interval=15s --timeout=3s --start-period=10s \
  CMD nc -z localhost 8080 || exit 1

ENTRYPOINT ["dotnet", "PathFinderService.Grpc.dll"]
