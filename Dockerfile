FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-alpine AS builder

WORKDIR /src
COPY ./Sources/Fanex.Data.Services/*.csproj ./Fanex.Data.Services/

RUN dotnet nuget add source https://nuget.nexdev.net/api/v2
RUN --mount=type=cache,target=/root/.nuget/packages \ 
	dotnet restore Fanex.Data.Services/Fanex.Data.Services.csproj 

COPY ./Sources/Fanex.Data.Services/. ./Fanex.Data.Services

WORKDIR /src/Fanex.Data.Services
RUN --mount=type=cache,target=/root/.nuget/packages \
	dotnet publish -c Release -o /app

FROM hub.nexdev.net/base/dotnet/core/aspnet:8.0-alpine AS base

WORKDIR /app
COPY --from=builder /app .

ENV TZ="America/La_Paz"
ENV COMPlus_EnableDiagnostics=0

# Don t run as root
RUN adduser --disabled-password \
  --home /app \
  --gecos '' dotnetuser && chown -R dotnetuser /app && chown -R dotnetuser /tmp
USER dotnetuser
#

ENTRYPOINT ["dotnet", "Fanex.Data.Services.dll"]
