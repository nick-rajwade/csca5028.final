#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Development
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["sales-web-app/sales-web-app.csproj", "sales-web-app/"]
RUN dotnet restore "sales-web-app/sales-web-app.csproj"
COPY . .
WORKDIR "/src/sales-web-app"
RUN dotnet build "sales-web-app.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "sales-web-app.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "sales-web-app.dll"]