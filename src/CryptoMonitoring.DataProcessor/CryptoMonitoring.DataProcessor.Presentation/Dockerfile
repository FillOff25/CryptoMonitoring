FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/CryptoMonitoring.Models/CryptoMonitoring.Models.csproj", "src/CryptoMonitoring.Models/"]
COPY ["src/CryptoMonitoring.Common/CryptoMonitoring.Common.csproj", "src/CryptoMonitoring.Common/"]
COPY ["src/CryptoMonitoring.DataProcessor/CryptoMonitoring.DataProcessor.Business/CryptoMonitoring.DataProcessor.Business.csproj", "src/CryptoMonitoring.DataProcessor/CryptoMonitoring.DataProcessor.Business/"]
COPY ["src/CryptoMonitoring.DataProcessor/CryptoMonitoring.DataProcessor.Presentation/CryptoMonitoring.DataProcessor.Presentation.csproj", "src/CryptoMonitoring.DataProcessor/CryptoMonitoring.DataProcessor.Presentation/"]
RUN dotnet restore "./src/CryptoMonitoring.DataProcessor/CryptoMonitoring.DataProcessor.Presentation/CryptoMonitoring.DataProcessor.Presentation.csproj"

COPY ["src/CryptoMonitoring.Models/", "src/CryptoMonitoring.Models/"]
COPY ["src/CryptoMonitoring.Common/", "src/CryptoMonitoring.Common/"]
COPY ["src/CryptoMonitoring.DataProcessor/CryptoMonitoring.DataProcessor.Business/", "src/CryptoMonitoring.DataProcessor/CryptoMonitoring.DataProcessor.Business/"]
COPY ["src/CryptoMonitoring.DataProcessor/CryptoMonitoring.DataProcessor.Presentation/", "src/CryptoMonitoring.DataProcessor/CryptoMonitoring.DataProcessor.Presentation/"]

RUN dotnet publish "./src/CryptoMonitoring.DataProcessor/CryptoMonitoring.DataProcessor.Presentation/CryptoMonitoring.DataProcessor.Presentation.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .
COPY .env .env

ENTRYPOINT ["dotnet", "CryptoMonitoring.DataProcessor.Presentation.dll"]