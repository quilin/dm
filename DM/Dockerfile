FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

ARG PROJECT_NAME
ARG PROJECT_DIR

WORKDIR /app

# copy csproj and restore as distinct layers
COPY . ./
RUN dotnet publish ${PROJECT_DIR}/${PROJECT_NAME} -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime

ARG PROJECT_NAME

WORKDIR /app
COPY --from=build /app/out ./

RUN adduser --disabled-password dmuser
USER dmuser

ENV RUNTIME_PROJECT ${PROJECT_NAME}.dll
CMD dotnet ${RUNTIME_PROJECT}