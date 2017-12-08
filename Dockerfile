FROM microsoft/dotnet:2.0-sdk AS build-env
WORKDIR /app

COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -r linux-arm -o out

FROM darkxahtep/node-bcm2835:latest-arm32v7 AS node-env
WORKDIR /app
COPY ./package.json ./package-lock.json ./
RUN npm install

FROM microsoft/dotnet:2.0-runtime-deps-stretch-arm32v7
WORKDIR /app
COPY tmp/qemu-arm-static /usr/bin/qemu-arm-static

RUN apt-get -qq update && apt-get install -qq -y curl gnupg
RUN curl -sL https://deb.nodesource.com/setup_8.x | bash
RUN apt-get install -qq -y nodejs

COPY --from=build-env /app/out ./
COPY --from=node-env /app/node_modules ./node_modules/
ENTRYPOINT ["./ClimateMeter.Device.Net"]