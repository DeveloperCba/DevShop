FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["./web/DevShop.Identity.WebApp/DevShop.Identity.WebApp.csproj", "web/DevShop.Identity.WebApp/"]

RUN dotnet restore "web/DevShop.Identity.WebApp/DevShop.Identity.WebApp.csproj"

COPY . .
WORKDIR "web/DevShop.Identity.WebApp"
RUN dotnet build "DevShop.Identity.WebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DevShop.Identity.WebApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DevShop.Identity.WebApp.dll"]
 