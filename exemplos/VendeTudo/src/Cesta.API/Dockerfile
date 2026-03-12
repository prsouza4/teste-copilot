FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /app

COPY VendeTudo.sln .
COPY Directory.Build.props .
COPY Directory.Packages.props .
COPY src/VendeTudo.Compartilhado/ src/VendeTudo.Compartilhado/
COPY src/VendeTudo.PadroeServico/ src/VendeTudo.PadroeServico/
COPY src/BarramentoEventos/ src/BarramentoEventos/
COPY src/BarramentoEventosRabbitMQ/ src/BarramentoEventosRabbitMQ/
COPY src/Cesta.API/ src/Cesta.API/

RUN dotnet restore src/Cesta.API/Cesta.API.csproj
RUN dotnet publish src/Cesta.API/Cesta.API.csproj -c Release -o /publicado --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview
WORKDIR /app
COPY --from=build /publicado .
EXPOSE 5003
ENTRYPOINT ["dotnet", "Cesta.API.dll"]
