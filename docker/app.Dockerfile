FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ARG PROJECT_NAME

WORKDIR /app

# copy csproj and restore as distinct layers
COPY . ./
RUN dotnet publish src/${PROJECT_NAME} -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

ARG PROJECT_NAME

WORKDIR /app
COPY --from=build /app/out ./

RUN adduser --disabled-password dmuser
USER dmuser

ENV RUNTIME_PROJECT ${PROJECT_NAME}.dll
CMD dotnet ${RUNTIME_PROJECT}