FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Blazor/Blazor.csproj", "Blazor/"]
COPY ["DomainModels/DomainModels.csproj", "DomainModels/"]

RUN dotnet restore "Blazor/Blazor.csproj"

COPY . .
WORKDIR "/src/Blazor"
RUN dotnet publish "Blazor.csproj" -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html

COPY --from=build /app/publish/wwwroot .

EXPOSE 80