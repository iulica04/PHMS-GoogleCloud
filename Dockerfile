# Etapa de bază
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

# Copiază fișierele proiectului
COPY ["PHMS/PHMS.csproj", "PHMS/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]

# Restabilirea dependențelor
RUN dotnet restore "PHMS/PHMS.csproj"

# Copiază restul fișierelor din proiect
COPY . .

# Publică aplicația
RUN dotnet publish "PHMS/PHMS.csproj" -c Release -o /app/publish

# Etapa de rulare
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://0.0.0.0:8080

# Copiază fișierele publicate în container
COPY --from=build /app/publish .


# Lansează aplicația
ENTRYPOINT ["dotnet", "PHMS.dll"]
