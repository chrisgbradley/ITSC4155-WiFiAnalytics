const ctx = document.getElementById('logsChart');

createLogsChart();

async function createLogsChart() {
    //wait till data is done before making chart
    const data = await getData();

    const logsChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: data.xs,
            datasets: [{
                label: 'Number of Log Entries by Minute',
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
            plugins: {
                title: {
                    display: true,
                    text: 'January 1st - 10th, 2021',
                    align: 'start',
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
    });
}

async function getData() {
    const xs = [];
    const ys = [];

    const response = await fetch('csv/data_a.csv');
    //Body.text() - raw text
    const data = await response.text();

    //Need to split up rows and columns
    //Slicing - deleting 0 element/index and keeping index 2 to the end
    const table = data.split('\n').slice(1);
    table.forEach(row => {
        const columns = row.split(',');
        //const year = columns[0];
        //const month = columns[1];
        //const day = columns[2];
        //const hour = columns[3];
        const minute = columns[4];
        xs.push(minute);
        const numLogs = columns[5];
        ys.push(numLogs);
    });
    return { xs, ys };
}