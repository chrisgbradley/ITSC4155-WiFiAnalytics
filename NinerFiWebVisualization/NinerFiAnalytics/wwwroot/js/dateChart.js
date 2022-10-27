const ctx = document.getElementById('testChart');

createChart();

async function createChart() {
    const data = await getData();

    const testChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: data.xs,
            datasets: [{
                label: 'Number of Log Entries',
                data: data.ys,
                fill: false,
                backgroundColor: 'rgba(58, 123, 213, 1)',
                borderColor: 'rgba(0, 210, 255, 0.3)',
                borderWidth: 1,
            }],
        },
        options: {
            responsive: true,
            layout: {
                padding: 10
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
    });
}

async function getData(x, y) {
    const xs = [];
    const ys = [];

    const response = await fetch('csv/testdata.csv');
    const data = await response.text();

    const table = data.split('\n').slice(1);
    table.forEach(row => {
        const columns = row.split(',');
        const year = columns[0];
        const month = columns[1];
        const day = columns[2];
        xs.push(day);
        const hour = columns[3];
        const minute = columns[4];
        const numLogs = columns[5];
        ys.push(numLogs);
    });
    return { xs, ys };
}