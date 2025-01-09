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
COPY ["consumer/consumer.csproj", "consumer/"]
RUN dotnet restore "consumer/consumer.csproj"
COPY . .
WORKDIR "/src/consumer"
RUN dotnet build "consumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "consumer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "consumer.dll"]