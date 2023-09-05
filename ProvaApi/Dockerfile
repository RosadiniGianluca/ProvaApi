# Usa l'immagine di base di ASP.NET Core
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

# Usa l'immagine di base di SDK per compilare l'app
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ProvaApi/ProvaApi.csproj", "ProvaApi/"]
RUN dotnet restore "ProvaApi/ProvaApi.csproj"
COPY . .
WORKDIR "/src/ProvaApi"
RUN dotnet build "ProvaApi.csproj" -c Release -o /app/build

# Effettua la pubblicazione dell'app
FROM build AS publish
RUN dotnet publish "ProvaApi.csproj" -c Release -o /app/publish

# Crea l'immagine finale
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProvaApi.dll"]