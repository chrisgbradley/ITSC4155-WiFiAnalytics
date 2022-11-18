const ctx = document.getElementById('errorChart').getContext('2d');

//use D3.js to load data
d3.csv('csv/testData.csv').then(data => {
    const dataObj = [];

    for (let i = 0; i < data.length; i++) {
        dataObj.push({
            year: data[i].Year,
            month: data[i].Month,
            day: data[i].Day,
            hour: data[i].Hour,
            minute: data[i].Minute,
            types: data[i].TypeName,
            numLogs: data[i].log_entries
        });
    }

    //group data by error type
    const groupedData = d3.group(dataObj, d => d.types);

    //map each error type's data to an object 
    //that chart.js can put in a bubble chart
    function getBubbleData(error) {
        return d3.map(groupedData.get(error), (d) => {
            return {
                x: d.day,
                y: d.month,
                r: d.numLogs
            };
        });
    }

    const errorChart = new Chart(ctx, {
        type: 'bubble',
        data: {
            datasets: [
                {
                    label: "NOTI",
                    data: getBubbleData("NOTI"),
                    backgroundColor: 'rgba(0, 255, 144, 0.5)',
                    borderColor: 'rgba(0, 204, 116, 1)'
                },
                {
                    label: "WARN",
                    data: getBubbleData("WARN"),
                    backgroundColor: 'rgba(230, 114, 0, 0.5)',
                    borderColor: 'rgba(252, 92, 13, 1)'
                },
                {
                    label: "ERRS",
                    data: getBubbleData("ERRS"),
                    backgroundColor: 'rgba(255, 17, 0, 0.5)',
                    borderColor: 'rgba(204, 13, 0, 1)'
                },
                {
                    label: "INFO",
                    data: getBubbleData("INFO"),
                    backgroundColor: 'rgba(0, 7, 255, 0.5)',
                    borderColor: 'rgba(0, 7, 204, 1)'
                }
                ]
        },
        options: {
            aspectRatio: 1,
            scales: {
                x: {
                    title: {
                        display: true,
                        text: 'Day'
                    }
                },
                y: {
                    title: {
                        display: true,
                        text: 'Month'
                    }
                }
            },
            plugins: {
                tooltip: false
            }
        }
    })
    //hide bootstrap spinner after data loads
    d3.select('.spinner-border').style('display', 'none');

}).catch(error => console.log(error));