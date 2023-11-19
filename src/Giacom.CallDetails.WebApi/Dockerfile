FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Giacom.CallDetails.WebApi/Giacom.CallDetails.WebApi.csproj", "Giacom.CallDetails.WebApi/"]
RUN dotnet restore "Giacom.CallDetails.WebApi/Giacom.CallDetails.WebApi.csproj"
COPY . .
WORKDIR "/src/Giacom.CallDetails.WebApi"
RUN dotnet build "Giacom.CallDetails.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Giacom.CallDetails.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Giacom.CallDetails.WebApi.dll"]
