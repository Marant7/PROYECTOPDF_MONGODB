FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar los archivos csproj y restaurar dependencias
COPY ["PROYECTOPDF/PROYECTOPDF.csproj", "PROYECTOPDF/"]
COPY ["NegocioPDF/NegocioPDF.csproj", "NegocioPDF/"]
RUN dotnet restore "PROYECTOPDF/PROYECTOPDF.csproj"

# Copiar todo el código fuente
COPY ["PROYECTOPDF/", "PROYECTOPDF/"]
COPY ["NegocioPDF/", "NegocioPDF/"]

# Compilar y publicar
WORKDIR "/src/PROYECTOPDF"
RUN dotnet build "PROYECTOPDF.csproj" -c Release -o /app/build
RUN dotnet publish "PROYECTOPDF.csproj" -c Release -o /app/publish

# Imagen final
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "PROYECTOPDF.dll"]