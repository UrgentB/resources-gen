FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5226

ENV ASPNETCORE_URLS=http://+:5226

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["GuildForge.csproj", "./"]
RUN dotnet restore "GuildForge.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "GuildForge.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "GuildForge.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GuildForge.dll"]
