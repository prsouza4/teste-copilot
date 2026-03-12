FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /app

COPY VendeTudo.sln .
COPY Directory.Build.props .
COPY Directory.Packages.props .
COPY src/VendeTudo.Compartilhado/ src/VendeTudo.Compartilhado/
COPY src/BarramentoEventos/ src/BarramentoEventos/
COPY src/BarramentoEventosRabbitMQ/ src/BarramentoEventosRabbitMQ/
COPY src/Pedidos.Dominio/ src/Pedidos.Dominio/
COPY src/Pedidos.Infraestrutura/ src/Pedidos.Infraestrutura/
COPY src/ProcessadorPagamentos/ src/ProcessadorPagamentos/

RUN dotnet restore src/ProcessadorPagamentos/ProcessadorPagamentos.csproj
RUN dotnet publish src/ProcessadorPagamentos/ProcessadorPagamentos.csproj -c Release -o /publicado --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview
WORKDIR /app
COPY --from=build /publicado .
ENTRYPOINT ["dotnet", "ProcessadorPagamentos.dll"]
