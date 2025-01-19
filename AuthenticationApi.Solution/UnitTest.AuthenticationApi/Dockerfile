# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar los archivos de proyecto necesarios para restaurar dependencias
COPY AuthenticationApi.Solution/UnitTest.AuthenticationApi/UnitTest.AuthenticationApi.csproj UnitTest.AuthenticationApi/
COPY AuthenticationApi.Solution/AuthenticationApi.Application/AuthenticationApi.Application.csproj AuthenticationApi.Application/
COPY AuthenticationApi.Solution/AuthenticationApi.Domain/AuthenticationApi.Domain.csproj AuthenticationApi.Domain/
COPY AuthenticationApi.Solution/AuthenticationApi.Infrastructure/AuthenticationApi.Infrastructure.csproj AuthenticationApi.Infrastructure/
COPY AuthenticationApi.Solution/AuthenticationApi.Presentation/AuthenticationApi.Presentation.csproj AuthenticationApi.Presentation/

# Restaurar dependencias
RUN dotnet restore UnitTest.AuthenticationApi/UnitTest.AuthenticationApi.csproj

# Copiar el código fuente completo
COPY AuthenticationApi.Solution/ ./AuthenticationApi.Solution/

# Cambiar al directorio del proyecto de pruebas
WORKDIR /src/AuthenticationApi.Solution/UnitTest.AuthenticationApi

# Ejecutar las pruebas unitarias
CMD ["dotnet", "test", "--logger:trx"]
