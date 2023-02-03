FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /source

ENV PATH $PATH:/root/.dotnet/tools
RUN dotnet tool install --global dotnet-ef --version 6.* && dotnet tool update --global dotnet-ef

# copy csproj and restore as distinct layers
COPY . ./
RUN dotnet restore Services/DM.Services.DataAccess && dotnet build Services/DM.Services.DataAccess

CMD dotnet ef database update --project Services/DM.Services.DataAccess