# Base runtime image (para produção)
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

# Debug image (corrigido para permitir depuração)
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS debug
WORKDIR /app
COPY . . 
EXPOSE 4022
ENTRYPOINT ["dotnet", "watch", "run", "--no-launch-profile"]

# Build e publish image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["consumer.csproj", "./"]
RUN dotnet restore "consumer.csproj"
COPY . .
RUN dotnet publish "consumer.csproj" -c Release -o /app/publish

# Final image (para produção)
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "consumer.dll"]
