FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

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

RUN apt-get update \
    && apt-get install -y curl \
    && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "API.dll"]
