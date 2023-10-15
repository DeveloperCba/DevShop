FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["./building blocks/DevShop.Core/DevShop.Core.csproj", "building blocks/DevShop.Core/"]
COPY ["./building blocks/DevShop.WebAPI.Core/DevShop.WebAPI.Core.csproj", "building blocks/DevShop.WebAPI.Core/"]
COPY ["./building blocks/DevShop.MessageBus/DevShop.MessageBus.csproj", "building blocks/DevShop.MessageBus/"]

COPY ["./services/DevShop.Identity.Domain/DevShop.Identity.Domain.csproj", "services/DevShop.Identity.Domain/"]
COPY ["./services/DevShop.Identity.Infrastructure/DevShop.Identity.Infrastructure.csproj", "services/DevShop.Identity.Infrastructure/"]
COPY ["./services/DevShop.Identity.Application/DevShop.Identity.Application.csproj", "services/DevShop.Identity.Application/"]
COPY ["./services/DevShop.Identity.API/DevShop.Identity.API.csproj", "services/DevShop.Identity.API/"]
 

RUN dotnet restore "services/DevShop.Identity.API/DevShop.Identity.API.csproj"

COPY . .
WORKDIR "services/DevShop.Identity.API"
RUN dotnet build "DevShop.Identity.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DevShop.Identity.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DevShop.Identity.API.dll"]