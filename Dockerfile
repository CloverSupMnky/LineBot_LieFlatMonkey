#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["6.WebHost/LineBot_LieFlatMonkey.WebHost/LineBot_LieFlatMonkey.WebHost.csproj", "6.WebHost/LineBot_LieFlatMonkey.WebHost/"]
COPY ["3.Entities/LineBot_LieFlatMonkey.Entities/LineBot_LieFlatMonkey.Entities.csproj", "3.Entities/LineBot_LieFlatMonkey.Entities/"]
COPY ["5.Modules/LineBot_LieFlatMonkey.Modules/LineBot_LieFlatMonkey.Modules.csproj", "5.Modules/LineBot_LieFlatMonkey.Modules/"]
COPY ["1.Plugins/LineBot_LieFlatMonkey.Plugins/LineBot_LieFlatMonkey.Plugins.csproj", "1.Plugins/LineBot_LieFlatMonkey.Plugins/"]
COPY ["2.Assets/LineBot_LieFlatMonkey.Assets/LineBot_LieFlatMonkey.Assets.csproj", "2.Assets/LineBot_LieFlatMonkey.Assets/"]
RUN dotnet restore "6.WebHost/LineBot_LieFlatMonkey.WebHost/LineBot_LieFlatMonkey.WebHost.csproj"
COPY . .
WORKDIR "/src/6.WebHost/LineBot_LieFlatMonkey.WebHost"
RUN dotnet build "LineBot_LieFlatMonkey.WebHost.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LineBot_LieFlatMonkey.WebHost.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet LineBot_LieFlatMonkey.WebHost.dll