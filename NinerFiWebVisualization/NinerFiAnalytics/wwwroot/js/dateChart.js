const ctx = document.getElementById('testChart').getContext('2d');

//use D3.js to load data
d3.csv('csv/testdata.csv').then(data => {
    const rows = [];
    const xs = [];
    const ys = [];
    
    for (let i = 0; i < data.length; i++) {
        rows.push({
            year: data[i].Year,
            month: data[i].Month,
            day: data[i].Day,
            hour: data[i].Hour,
            minute: data[i].Minute,
            numLogs: data[i]["Number of Log Entries"]
        });
    }

    for (let i = 0; i < rows.length; i++) {
        xs.push(rows[i].minute);
        ys.push(rows[i].numLogs);
    }

    let delayed;

    //Gradient fill
    let gradient = ctx.createLinearGradient(0, 0, 0, 400);
    gradient.addColorStop(0, 'rgba(58, 123, 213, 1');
    gradient.addColorStop(1, 'rgba(0, 210, 255, 0.3)');

    const testChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: xs,
            datasets: [{
                label: 'Number of Log Entries',
                data: rows,
                fill: true,
                backgroundColor: gradient,
                borderColor: '#fff',
                pointBackgroundColor: 'rgb(189, 195, 199)'
            }],
        },
        options: {
            responsive: true,
            layout: {
                padding: 40
            },
            parsing: {
                xAxisKey: 'minute',
                yAxisKey: 'numLogs'
            },
            plugins: {
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
            radius: 5,
            hitRadius: 30,
            hoverRadius: 10,
            animation: {
                onComplete: () => {
                    delayed = true;
                },
                delay: (context) => {
                    let delay = 0;
                    if (context.type === 'data' && context.mode === 'default' && !delayed) {
                        delay = context.dataIndex * 300 + context.datasetIndex * 100;
                    }
                    return delay;
                }
            }
        }
    })
    //hide bootstrap spinner after data loads
    d3.select('.spinner-border').style('display', 'none');

}).catch(error => console.log(error));