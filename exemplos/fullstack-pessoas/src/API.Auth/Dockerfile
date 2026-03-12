FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/API.Auth/API.Auth.csproj", "src/API.Auth/"]
RUN dotnet restore "src/API.Auth/API.Auth.csproj"
COPY src/API.Auth/ src/API.Auth/
WORKDIR "/src/src/API.Auth"
RUN dotnet build "API.Auth.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "API.Auth.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.Auth.dll"]
