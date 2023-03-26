FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["CiCdBot.Run/CiCdBot.Run.csproj", "CiCdBot.Run/"]
RUN dotnet restore "CiCdBot.Run/CiCdBot.Run.csproj"
COPY . .
WORKDIR "/src/CiCdBot.Run"
RUN dotnet build "CiCdBot.Run.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CiCdBot.Run.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CiCdBot.Run.dll"]
