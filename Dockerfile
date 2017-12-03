#FROM microsoft/dotnet:2.0-sdk AS build-env
#WORKDIR /app

#COPY . ./
#RUN dotnet restore
#RUN dotnet publish -c Release -r linux-arm -o out

FROM microsoft/dotnet:2.0-runtime-deps-stretch-arm32v7
WORKDIR /app
COPY tmp/qemu-arm-static /usr/bin/qemu-arm-static

RUN apt-get -qq update && apt-get install -qq -y curl ca-certificates build-essential
RUN mkdir bcm2835 && cd bcm2835 \
    && curl http://www.airspayce.com/mikem/bcm2835/bcm2835-1.52.tar.gz -o bcm2835.tar.gz \
    && tar zxvf bcm2835.tar.gz -C ./ --strip-components=1 \
    && ./configure && make && make install

RUN curl -o- https://raw.githubusercontent.com/creationix/nvm/v0.33.6/install.sh | bash && source ~/.bashrc && nvm install v8.9.1 && npm install
#COPY --from=build-env /app/out ./
COPY ./DhtReader/readDhtData.js .
#ENTRYPOINT ["./ClimateMeter.Device.Net"]