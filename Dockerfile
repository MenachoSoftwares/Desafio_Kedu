# Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/Kedu.Domain/Kedu.Domain.csproj src/Kedu.Domain/
COPY src/Kedu.Application/Kedu.Application.csproj src/Kedu.Application/
COPY src/Kedu.Infrastructure/Kedu.Infrastructure.csproj src/Kedu.Infrastructure/
COPY src/Kedu.API/Kedu.API.csproj src/Kedu.API/
RUN dotnet restore src/Kedu.API/Kedu.API.csproj

COPY src/ src/
RUN dotnet publish src/Kedu.API/Kedu.API.csproj -c Release -o /app/publish --no-restore

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "Kedu.API.dll"]
