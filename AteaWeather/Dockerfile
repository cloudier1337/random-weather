FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AteaWeather/AteaWeather.csproj", "AteaWeather/"]
RUN dotnet restore "AteaWeather/AteaWeather.csproj"
COPY . .
WORKDIR "/src/AteaWeather"
RUN dotnet build "AteaWeather.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AteaWeather.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AteaWeather.dll"]
