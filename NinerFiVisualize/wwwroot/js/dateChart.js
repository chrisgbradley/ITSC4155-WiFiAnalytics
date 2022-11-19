const logs = ViewData["logs"];

const ctx = document.getElementById('chart').getContext('2d');

const hours = logs.map(log => log.hour);

let delayed;

//Gradient fill
let gradient = ctx.createLinearGradient(0, 0, 0, 400);
gradient.addColorStop(0, 'rgba(58, 123, 213, 1');
gradient.addColorStop(1, 'rgba(0, 210, 255, 0.3)');

const testChart = new Chart(ctx, {
    type: 'line',
    data: {
        labels: hours,
        datasets: [{
            label: 'Number of Log Entries',
            data: logs,
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
            xAxisKey: 'hour',
            yAxisKey: 'number_of_logs'
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
});
//hide bootstrap spinner after data loads
