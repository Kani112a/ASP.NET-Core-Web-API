# =========================
# Build stage
# =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["Cityinfo.API/Cityinfo.API.csproj", "Cityinfo.API/"]

RUN dotnet restore "Cityinfo.API/Cityinfo.API.csproj"

COPY . .

WORKDIR "/src/Cityinfo.API"

RUN dotnet publish "Cityinfo.API.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false


# =========================
# Runtime stage
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "Cityinfo.API.dll"]