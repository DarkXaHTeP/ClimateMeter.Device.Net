FROM microsoft/dotnet:2.0-sdk AS build-env
WORKDIR /app

COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -r linux-arm -o out

FROM darkxahtep/dht:latest-jessie as dht-env

FROM darkxahtep/dotnet:2.0-runtime-deps-jessie-arm32v7
WORKDIR /app

COPY --from=dht-env /usr/lib/libdht11.so /usr/lib/libdht11.so

COPY --from=build-env /app/out ./
ENV ASPNETCORE_URLS http://0.0.0.0:5000
EXPOSE 5000

ENTRYPOINT ["./ClimateMeter.Device.Net"]