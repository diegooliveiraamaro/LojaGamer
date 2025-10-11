# Etapa 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia o arquivo de projeto e restaura dependências
COPY ["LojaGamer/LojaGamer.csproj", "LojaGamer/"]
RUN dotnet restore "LojaGamer/LojaGamer.csproj"

# Copia todo o código e publica em modo release
COPY . .
WORKDIR /src/LojaGamer
RUN dotnet publish "LojaGamer.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Define a porta padrão usada pelo container
EXPOSE 80

ENTRYPOINT ["dotnet", "LojaGamer.dll"]
