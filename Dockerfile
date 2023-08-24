# Build stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source
COPY . .
RUN dotnet restore "./MetaExchange.API/MetaExchange.API.csproj" --disable-parallel
RUN dotnet publish "./MetaExchange.API/MetaExchange.API.csproj" -c Debug -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app ./

EXPOSE 80
ENTRYPOINT ["dotnet", "MetaExchange.API.dll"]