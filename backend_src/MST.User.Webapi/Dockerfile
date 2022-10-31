FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MST.User.Webapi/MST.User.Webapi.csproj", "MST.User.Webapi/"]
RUN dotnet restore "MST.User.Webapi/MST.User.Webapi.csproj"
COPY . .
WORKDIR "/src/MST.User.Webapi"
RUN dotnet build "MST.User.Webapi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MST.User.Webapi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MST.User.Webapi.dll"]
