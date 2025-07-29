FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/CryptoMonitoring.Models/CryptoMonitoring.Models.csproj", "src/CryptoMonitoring.Models/"]
COPY ["src/CryptoMonitoring.Common/CryptoMonitoring.Common.csproj", "src/CryptoMonitoring.Common/"]
COPY ["src/CryptoMonitoring.DataGenerator/CryptoMonitoring.DataGenerator.Business/CryptoMonitoring.DataGenerator.Business.csproj", "src/CryptoMonitoring.DataGenerator/CryptoMonitoring.DataGenerator.Business/"]
COPY ["src/CryptoMonitoring.DataGenerator/CryptoMonitoring.DataGenerator.Persistence/CryptoMonitoring.DataGenerator.Persistence.csproj", "src/CryptoMonitoring.DataGenerator/CryptoMonitoring.DataGenerator.Persistence/"]
COPY ["src/CryptoMonitoring.DataGenerator/CryptoMonitoring.DataGenerator.Presentation/CryptoMonitoring.DataGenerator.Presentation.csproj", "src/CryptoMonitoring.DataGenerator/CryptoMonitoring.DataGenerator.Presentation/"]
RUN dotnet restore "./src/CryptoMonitoring.DataGenerator/CryptoMonitoring.DataGenerator.Presentation/CryptoMonitoring.DataGenerator.Presentation.csproj"

COPY ["src/CryptoMonitoring.Models/", "src/CryptoMonitoring.Models/"]
COPY ["src/CryptoMonitoring.Common/", "src/CryptoMonitoring.Common/"]
COPY ["src/CryptoMonitoring.DataGenerator/CryptoMonitoring.DataGenerator.Business/", "src/CryptoMonitoring.DataGenerator/CryptoMonitoring.DataGenerator.Business/"]
COPY ["src/CryptoMonitoring.DataGenerator/CryptoMonitoring.DataGenerator.Persistence/", "src/CryptoMonitoring.DataGenerator/CryptoMonitoring.DataGenerator.Persistence/"]
COPY ["src/CryptoMonitoring.DataGenerator/CryptoMonitoring.DataGenerator.Presentation/", "src/CryptoMonitoring.DataGenerator/CryptoMonitoring.DataGenerator.Presentation/"]

RUN dotnet publish "./src/CryptoMonitoring.DataGenerator/CryptoMonitoring.DataGenerator.Presentation/CryptoMonitoring.DataGenerator.Presentation.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .
COPY .env .env

ENTRYPOINT ["dotnet", "CryptoMonitoring.DataGenerator.Presentation.dll"]