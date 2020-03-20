#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Scheduler/Scheduler.csproj", "Scheduler/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Core/Core.csproj", "Core/"]
RUN dotnet restore "Scheduler/Scheduler.csproj"
COPY . .
WORKDIR "/src/Scheduler"
RUN dotnet build "Scheduler.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Scheduler.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Scheduler.dll"]