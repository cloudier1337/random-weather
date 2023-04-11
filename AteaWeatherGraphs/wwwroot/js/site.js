$(document).ready(function () {
    // event listener for button click
    $("#getDataButton").click(function () {
        // get selected city and country
        const selectedCity = $("#citySelect").val();
        const selectedCountry = $("#countrySelect").val();

        // make API call to retrieve weather data
        $.get(`/Weather/GetData?city=${selectedCity}&country=${selectedCountry}`, function (data) {
            // clear previous chart data
            $("#minTempChart").empty();
            $("#windSpeedChart").empty();

            // parse data from API response
            const cityName = data.name;
            const countryName = data.sys.country;
            const temperature = (data.main.temp - 273.15).toFixed(2);
            const clouds = data.clouds.all;
            const windSpeed = data.wind.speed;

            // update UI with current weather data
            $("#currentWeatherData").html(`
        <h2>Current Weather Data for ${cityName}, ${countryName}</h2>
        <p>Temperature: ${temperature}&deg;C</p>
        <p>Cloud Coverage: ${clouds}%</p>
        <p>Wind Speed: ${windSpeed} m/s</p>
      `);

            // prepare chart data for min temperature
            const tempData = data.main.temp_history.map((temp) => ({
                x: new Date(temp.dt * 1000),
                y: (temp.temp_min - 273.15).toFixed(2),
            }));

            // prepare chart data for wind speed
            const windData = data.wind_history.map((wind) => ({
                x: new Date(wind.dt * 1000),
                y: wind.speed,
            }));

            // create chart for min temperature
            new Chart($("#minTempChart"), {
                type: "line",
                data: {
                    datasets: [
                        {
                            label: `Min Temperature History for ${cityName}, ${countryName}`,
                            data: tempData,
                            backgroundColor: "rgba(255, 99, 132, 0.2)",
                            borderColor: "rgba(255, 99, 132, 1)",
                            borderWidth: 1,
                        },
                    ],
                },
                options: {
                    scales: {
                        x: {
                            type: "time",
                            time: {
                                unit: "hour",
                            },
                        },
                        y: {
                            ticks: {
                                callback: function (value) {
                                    return value + "°C";
                                },
                            },
                        },
                    },
                },
            });

            // create chart for wind speed
            new Chart($("#windSpeedChart"), {
                type: "line",
                data: {
                    datasets: [
                        {
                            label: `Wind Speed History for ${cityName}, ${countryName}`,
                            data: windData,
                            backgroundColor: "rgba(54, 162, 235, 0.2)",
                            borderColor: "rgba(54, 162, 235, 1)",
                            borderWidth: 1,
                        },
                    ],
                },
                options: {
                    scales: {
                        x: {
                            type: "time",
                            time: {
                                unit: "hour",
                            },
                        },
                        y: {
                            ticks: {
                                callback: function (value) {
                                    return value + " m/s";
                                },
                            },
                        },
                    },
                },
            });
        });
    });
});