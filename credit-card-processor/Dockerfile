#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Development
EXPOSE 80


FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["./credit-card-processor.csproj", "credit-card-processor/"]
COPY ["../csca5028.lib/csca5028.csproj","csca5028.lib/"]
RUN dotnet restore "credit-card-processor/credit-card-processor.csproj"
WORKDIR "/src/credit-card-processor"
COPY . .

RUN dotnet build "credit-card-processor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "credit-card-processor.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "credit-card-processor.dll"]