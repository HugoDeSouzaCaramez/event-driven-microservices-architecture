# Base runtime image (Corrigido: usar aspnet:7.0)
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

# Debug image
FROM base as mydebug
RUN echo "This is my debug image" > /tmp/image_type

# Build and publish image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["producer.csproj", "./"]
RUN dotnet restore "producer.csproj"
COPY . .
RUN dotnet publish "producer.csproj" -c Release -o /app/publish

# Final image (Corrigido: usar aspnet:7.0)
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "producer.dll"]
