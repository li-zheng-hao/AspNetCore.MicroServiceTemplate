FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MST.Auth.Webapi/MST.Auth.Webapi.csproj", "MST.Auth.Webapi/"]
RUN dotnet restore "MST.Auth.Webapi/MST.Auth.Webapi.csproj"
COPY . .
WORKDIR "/src/MST.Auth.Webapi"
RUN dotnet build "MST.Auth.Webapi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MST.Auth.Webapi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MST.Auth.Webapi.dll"]
