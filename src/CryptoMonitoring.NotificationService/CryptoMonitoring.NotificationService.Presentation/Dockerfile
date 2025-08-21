FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/CryptoMonitoring.Models/CryptoMonitoring.Models.csproj", "src/CryptoMonitoring.Models/"]
COPY ["src/CryptoMonitoring.Common/CryptoMonitoring.Common.csproj", "src/CryptoMonitoring.Common/"]
COPY ["src/CryptoMonitoring.NotificationService/CryptoMonitoring.NotificationService.Business/CryptoMonitoring.NotificationService.Business.csproj", "src/CryptoMonitoring.NotificationService/CryptoMonitoring.NotificationService.Business/"]
COPY ["src/CryptoMonitoring.NotificationService/CryptoMonitoring.NotificationService.Presentation/CryptoMonitoring.NotificationService.Presentation.csproj", "src/CryptoMonitoring.NotificationService/CryptoMonitoring.NotificationService.Presentation/"]
RUN dotnet restore "./src/CryptoMonitoring.NotificationService/CryptoMonitoring.NotificationService.Presentation/CryptoMonitoring.NotificationService.Presentation.csproj"

COPY ["src/CryptoMonitoring.Models/", "src/CryptoMonitoring.Models/"]
COPY ["src/CryptoMonitoring.Common/", "src/CryptoMonitoring.Common/"]
COPY ["src/CryptoMonitoring.NotificationService/CryptoMonitoring.NotificationService.Business/", "src/CryptoMonitoring.NotificationService/CryptoMonitoring.NotificationService.Business/"]
COPY ["src/CryptoMonitoring.NotificationService/CryptoMonitoring.NotificationService.Presentation/", "src/CryptoMonitoring.NotificationService/CryptoMonitoring.NotificationService.Presentation/"]

RUN dotnet publish "./src/CryptoMonitoring.NotificationService/CryptoMonitoring.NotificationService.Presentation/CryptoMonitoring.NotificationService.Presentation.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .
COPY .env .env

ENTRYPOINT ["dotnet", "CryptoMonitoring.NotificationService.Presentation.dll"]