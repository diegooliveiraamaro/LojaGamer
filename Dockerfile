# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia o arquivo de projeto e restaura dependências
COPY ["LojaGamer/LojaGamer.csproj", "LojaGamer/"]
RUN dotnet restore "LojaGamer/LojaGamer.csproj"

# Copia todo o restante do código
COPY . .

# Compila e publica o projeto
WORKDIR "/src/LojaGamer"
RUN dotnet publish "LojaGamer.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Expõe a porta que o ECS vai usar
EXPOSE 80
EXPOSE 443

# Copia o resultado da build
COPY --from=build /app/publish .

# Define a variável de ambiente ASPNETCORE_URLS
# Isso força o servidor Kestrel a escutar na porta 80 (obrigatório no Fargate)
ENV ASPNETCORE_URLS=http://+:80

# Entry point da aplicação
ENTRYPOINT ["dotnet", "LojaGamer.dll"]
