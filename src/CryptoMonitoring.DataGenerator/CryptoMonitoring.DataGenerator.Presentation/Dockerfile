FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["CryptoMonitoring.DataGenerator.Business/CryptoMonitoring.DataGenerator.Business.csproj", "CryptoMonitoring.DataGenerator.Business/"]
COPY ["CryptoMonitoring.DataGenerator.DataAccess/CryptoMonitoring.DataGenerator.DataAccess.csproj", "CryptoMonitoring.DataGenerator.DataAccess/"]
COPY ["CryptoMonitoring.DataGenerator.Presentation/CryptoMonitoring.DataGenerator.Presentation.csproj", "CryptoMonitoring.DataGenerator.Presentation/"]
RUN dotnet restore "./CryptoMonitoring.DataGenerator.Presentation/CryptoMonitoring.DataGenerator.Presentation.csproj"

COPY . ./
RUN dotnet publish "./CryptoMonitoring.DataGenerator.Presentation/CryptoMonitoring.DataGenerator.Presentation.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "CryptoMonitoring.DataGenerator.Presentation.dll"]