FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY . ./
RUN dotnet restore Web/DM.Web.API/*.csproj
RUN dotnet publish Web/DM.Web.API/*.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS runtime
WORKDIR /app
COPY --from=build /app/Web/DM.Web.API/out ./
CMD ASPNETCORE_URLS=http://*:$PORT dotnet DM.Web.API.dll