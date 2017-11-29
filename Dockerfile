FROM microsoft/dotnet:2.0-sdk AS build-env
WORKDIR /app

COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM microsoft/dotnet:2.0.0-runtime-stretch-arm32v7
WORKDIR /app

COPY --from=build-env /app/out ./
ENV ASPNETCORE_URLS http://0.0.0.0:5000
EXPOSE 5000

ENTRYPOINT ["dotnet", "ClimateMeter.Device.Net.dll"]