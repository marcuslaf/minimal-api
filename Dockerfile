FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Api/minimal-api.csproj", "Api/"]
COPY ["Test/Test.csproj", "Test/"]
RUN dotnet restore "Api/minimal-api.csproj"

COPY Api/ Api/
COPY Test/ Test/
WORKDIR "/src/Api"
RUN dotnet build "minimal-api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "minimal-api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production

CMD dotnet minimal-api.dll --urls "http://+:${PORT:-8080}"
