#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Development
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["point-of-sale-app/point-of-sale-app.csproj", "point-of-sale-app/"]
RUN dotnet restore "point-of-sale-app/point-of-sale-app.csproj"
COPY . .
WORKDIR "/src/point-of-sale-app"
RUN dotnet build "point-of-sale-app.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "point-of-sale-app.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "point-of-sale-app.dll"]