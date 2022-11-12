const ctx = document.getElementById('logsChart');

//use D3.js to load data
d3.csv('csv/data_a.csv').then(data => {
    const rows = [];
    const xs = [];
    const ys = [];

    for (let i = 0; i < data.length; i++) {
        let objData = {
            year: data[i].Year,
            month: data[i].Month,
            day: data[i].Day,
            hour: data[i].Hour,
            minute: data[i].Minute,
            numLogs: data[i]["Number of Log Entries"]
        };
        rows.push(objData);
    }

    for (let i = 0; i < rows.length; i++) {
        xs.push(rows[i].minute);
        ys.push(rows[i].numLogs);
    }

    //create chart
    const logsChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: xs,
            datasets: [
                {
                    label: 'Number of Log Entries',
                    data: rows,
                    fill: false,
                    backgroundColor: 'rgba(1, 80, 53, 1)',
                    borderColor: 'rgba(1, 80, 53, 0.3)',
                    borderWidth: 1
                }
            ],
        },
        options: {
            responsive: true,
            layout: {
                padding: 10
            },
            parsing: {
                xAxisKey: 'minute',
                yAxisKey: 'numLogs'
            },
            plugins: {
                title: {
                    display: true,
                    text: 'January 1st - 10th, 2021',
                    align: 'start',
                },
                tooltip: {
                    callbacks: {
                        title: function (context) {
                            const item = context[0].raw;
                            return "Datetime: " + item.year + "-" + item.month + "-" +
                                item.day + " " + item.hour + ":" + item.minute;
                        },
                        body: (context) => {
                            context.parsed.x + ' log entries';
                        }
                    }
                }
            },
            scales: {
                x: {
                    title: {
                        display: true,
                        text: 'Minute'
                    }
                },
                y: {
                    title: {
                        display: true,
                        text: 'Number of Entries'
                    }
                }
            }
        }
    })
    //hide bootstrap spinner after data loads
    d3.select('.spinner-border').style('display', 'none');

}).catch(error => console.log(error));