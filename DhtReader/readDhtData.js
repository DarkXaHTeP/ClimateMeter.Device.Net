var sensor = require('node-dht-sensor');

function readData(callback, pinNumber, retryNumber) {
    sensor.read(11, 4, function(err, temp, hum) {
        if (!err) {
             callback({
                 Temperature: temp.toFixed(1),
                 Humidity: hum.toFixed(1)
             });
        } else {
            if (retryNumber < 5) {
                readData(callback, pinNumber, retryNumber + 1);
            } else {
                callback(err);
            }
        }
    });
}

module.exports = function (callback, pinNumber) {
    readData(callback, pinNumber, 0);
};