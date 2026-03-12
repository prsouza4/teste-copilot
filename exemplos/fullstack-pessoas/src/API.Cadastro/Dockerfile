FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5002

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/API.Cadastro/API.Cadastro.csproj", "src/API.Cadastro/"]
RUN dotnet restore "src/API.Cadastro/API.Cadastro.csproj"
COPY src/API.Cadastro/ src/API.Cadastro/
WORKDIR "/src/src/API.Cadastro"
RUN dotnet build "API.Cadastro.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "API.Cadastro.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.Cadastro.dll"]
