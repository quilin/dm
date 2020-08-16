FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY . ./
RUN dotnet publish Web/DM.Web.API -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

RUN adduser --disabled-password dmuser
USER dmuser
CMD dotnet DM.Web.API.dll