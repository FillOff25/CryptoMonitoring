FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/CryptoMonitoring.Models/CryptoMonitoring.Models.csproj", "src/CryptoMonitoring.Models/"]
COPY ["src/CryptoMonitoring.Common/CryptoMonitoring.Common.csproj", "src/CryptoMonitoring.Common/"]
COPY ["src/CryptoMonitoring.ReportGenerator/CryptoMonitoring.ReportGenerator.Business/CryptoMonitoring.ReportGenerator.Business.csproj", "src/CryptoMonitoring.ReportGenerator/CryptoMonitoring.ReportGenerator.Business/"]
COPY ["src/CryptoMonitoring.ReportGenerator/CryptoMonitoring.ReportGenerator.Presentation/CryptoMonitoring.ReportGenerator.Presentation.csproj", "src/CryptoMonitoring.ReportGenerator/CryptoMonitoring.ReportGenerator.Presentation/"]
RUN dotnet restore "./src/CryptoMonitoring.ReportGenerator/CryptoMonitoring.ReportGenerator.Presentation/CryptoMonitoring.ReportGenerator.Presentation.csproj"

COPY ["src/CryptoMonitoring.Models/", "src/CryptoMonitoring.Models/"]
COPY ["src/CryptoMonitoring.Common/", "src/CryptoMonitoring.Common/"]
COPY ["src/CryptoMonitoring.ReportGenerator/CryptoMonitoring.ReportGenerator.Business/", "src/CryptoMonitoring.ReportGenerator/CryptoMonitoring.ReportGenerator.Business/"]
COPY ["src/CryptoMonitoring.ReportGenerator/CryptoMonitoring.ReportGenerator.Presentation/", "src/CryptoMonitoring.ReportGenerator/CryptoMonitoring.ReportGenerator.Presentation/"]

RUN dotnet publish "./src/CryptoMonitoring.ReportGenerator/CryptoMonitoring.ReportGenerator.Presentation/CryptoMonitoring.ReportGenerator.Presentation.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .
COPY .env .env

ENTRYPOINT ["dotnet", "CryptoMonitoring.ReportGenerator.Presentation.dll"]