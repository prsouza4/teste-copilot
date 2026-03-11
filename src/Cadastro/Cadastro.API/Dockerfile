# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

# Copy solution and project files
COPY Cadastro.sln ./
COPY Cadastro.Domain/Cadastro.Domain.csproj ./Cadastro.Domain/
COPY Cadastro.Application/Cadastro.Application.csproj ./Cadastro.Application/
COPY Cadastro.Infrastructure/Cadastro.Infrastructure.csproj ./Cadastro.Infrastructure/
COPY Cadastro.API/Cadastro.API.csproj ./Cadastro.API/

# Restore dependencies
RUN dotnet restore Cadastro.API/Cadastro.API.csproj

# Copy source code
COPY . .

# Build
RUN dotnet build Cadastro.API/Cadastro.API.csproj -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish Cadastro.API/Cadastro.API.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS final
WORKDIR /app

# Create non-root user for security
RUN adduser --disabled-password --gecos '' appuser

COPY --from=publish /app/publish .

# Switch to non-root user
USER appuser

EXPOSE 8080

ENTRYPOINT ["dotnet", "Cadastro.API.dll"]
