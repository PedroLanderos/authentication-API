# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar los archivos del proyecto
COPY AuthenticationApi.Solution/AuthenticationApi.Presentation/AuthenticationApi.Presentation.csproj AuthenticationApi.Presentation/
COPY AuthenticationApi.Solution/AuthenticationApi.Application/AuthenticationApi.Application.csproj AuthenticationApi.Application/
COPY AuthenticationApi.Solution/AuthenticationApi.Domain/AuthenticationApi.Domain.csproj AuthenticationApi.Domain/
COPY AuthenticationApi.Solution/AuthenticationApi.Infrastructure/AuthenticationApi.Infrastructure.csproj AuthenticationApi.Infrastructure/

# Restaurar dependencias
RUN dotnet restore AuthenticationApi.Presentation/AuthenticationApi.Presentation.csproj

# Copiar el código fuente
COPY AuthenticationApi.Solution/ ./AuthenticationApi.Solution/

# Cambiar al directorio del proyecto principal
WORKDIR /src/AuthenticationApi.Solution/AuthenticationApi.Presentation

# Publicar la aplicación
RUN dotnet publish AuthenticationApi.Presentation.csproj -c Release -o /app/publish

# Etapa final: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Establecer la variable de entorno para usar el puerto 5000
ENV ASPNETCORE_URLS=http://+:5000

# Configurar el punto de entrada para aplicar migraciones y luego ejecutar la aplicación
ENTRYPOINT ["dotnet", "AuthenticationApi.Presentation.dll"]