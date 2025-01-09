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
COPY ["producer/producer.csproj", "producer/"]
RUN dotnet restore "producer/producer.csproj"
COPY . .
WORKDIR "/src/producer"
RUN dotnet build "producer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "producer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "producer.dll"]