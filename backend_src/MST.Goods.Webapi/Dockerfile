FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MST.Goods.Webapi/MST.Goods.Webapi.csproj", "MST.Goods.Webapi/"]
RUN dotnet restore "MST.Goods.Webapi/MST.Goods.Webapi.csproj"
COPY . .
WORKDIR "/src/MST.Goods.Webapi"
RUN dotnet build "MST.Goods.Webapi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MST.Goods.Webapi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MST.Goods.Webapi.dll"]
