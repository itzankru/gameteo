FROM  mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
# COPY ["GameteoConsole/GameteoConsole.csproj", "GameteoConsole/"]
# RUN dotnet restore "GameteoConsole/GameteoConsole.csproj"

COPY ["GameteoWebApi/GameteoWebApi.csproj", "GameteoWebApi/"]
RUN dotnet restore "GameteoWebApi/GameteoWebApi.csproj"

COPY . .

# WORKDIR "/src/GameteoConsole"
# RUN dotnet build "GameteoConsole.csproj" -c Release -o /app/gameteo-console

WORKDIR "/src/GameteoWebApi"
RUN dotnet build "GameteoWebApi.csproj" -c Release -o /app/gameteo-web

FROM build AS publish
# RUN dotnet publish "GameteoConsole.csproj" -c Release -o /app/gameteo-console
RUN dotnet publish "GameteoWebApi.csproj" -c Release -o /app/gameteo-web

FROM base AS final
WORKDIR /app
# COPY --from=publish /app/gameteo-console .
COPY --from=publish /app/gameteo-web .

COPY currency.db .
COPY GameteoWebApi/appsettings.json .
COPY currencies_en.json .

EXPOSE 8080
ENTRYPOINT ["dotnet", "GameteoWebApi.dll"]
