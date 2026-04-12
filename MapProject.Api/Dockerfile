FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["MapProject.Api/MapProject.Api.csproj", "MapProject.Api/"]
COPY ["MapProject.DtoLayer/MapProject.DtoLayer.csproj", "MapProject.DtoLayer/"]
RUN dotnet restore "MapProject.Api/MapProject.Api.csproj"

COPY . .
WORKDIR "/src/MapProject.Api"
RUN dotnet build "MapProject.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MapProject.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MapProject.Api.dll"]
