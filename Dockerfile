# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["LojaGamer/LojaGamer.csproj", "LojaGamer/"]
RUN dotnet restore "LojaGamer/LojaGamer.csproj"

COPY . .

WORKDIR "/src/LojaGamer"
RUN dotnet publish "LojaGamer.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_URLS=http://+:80

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "LojaGamer.dll"]
