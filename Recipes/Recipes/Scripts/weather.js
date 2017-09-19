//weather.js

var weatherCallback = function (data) {
    var wind = data.query.results.channel.wind;
    var item = data.query.results.channel.item;
    $("#temperatureDiv h4").text("Weather: " + item.title);
    var text = "Wind - chill: " + wind.chill + ", speed: " + wind.speed +
        "</br> Temperature: " + item.condition.temp + " °C";
    $("#temperatureDiv p").html(text);

    if (item.condition.temp <= 20)
    {
        $("#temperatureDiv").append("<p>It's seems to be pretty cold out there! stay home, we have great soup recipes.</p>");
    }
    else if (item.condition.temp > 20 && item.condition.temp < 28) {
        $("#temperatureDiv").append("<p>The weather is great! take your cooking skills outdoor and test some of our outdoor recipes!</p>");
    } else {
        $("#temperatureDiv").append("<p>It's seems to be very hot out there! make yourself a cool drink! </p>");
    }
};