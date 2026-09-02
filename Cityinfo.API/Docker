# -----------------------------
# Stage 1: Build
# -----------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copy project file
COPY ["Cityinfo.API.csproj", "./"]

# Restore NuGet packages
RUN dotnet restore "Cityinfo.API.csproj"

# Copy source code
COPY . .

# Build and publish
RUN dotnet publish "Cityinfo.API.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false


# -----------------------------
# Stage 2: Runtime
# -----------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

# Copy published application
COPY --from=build /app/publish .

# Application listens on port 8080
EXPOSE 8080

# Start application
ENTRYPOINT ["dotnet", "Cityinfo.API.dll"]