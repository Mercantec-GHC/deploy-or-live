FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["API/API.csproj", "API/"]
COPY ["DomainModels/DomainModels.csproj", "DomainModels/"]

RUN dotnet restore "API/API.csproj"

COPY . .

WORKDIR "/src/API"

RUN dotnet publish "API.csproj" -c Release -o /app/publish

FROM runtime AS final
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "API.dll"]
