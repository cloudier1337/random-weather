FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AteaWeatherGraphs/AteaWeatherGraphs.csproj", "AteaWeatherGraphs/"]
RUN dotnet restore "AteaWeatherGraphs/AteaWeatherGraphs.csproj"
COPY . .
WORKDIR "/src/AteaWeatherGraphs"
RUN dotnet build "AteaWeatherGraphs.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AteaWeatherGraphs.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AteaWeatherGraphs.dll"]
