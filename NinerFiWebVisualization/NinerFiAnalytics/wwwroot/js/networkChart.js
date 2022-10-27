const ctx = document.getElementById('networkChart');

createNetworkChart();

async function createNetworkChart() {
    const data = await getData();

    const networkChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: data.xs,
            datasets: [{
                label: 'Count of Entries by Hostname',
                data: data.ys,
                fill: false,
                backgroundColor: 'rgba(1, 80, 53, 1)',
                borderColor: 'rgba(1, 80, 53, 0.3)',
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
                    text: 'Total Number of Access Points: ' + data.accessPoints,
                    align: 'start',
                }
            },
            scales: {
                x: {
                    title: {
                        display: true,
                        text: 'Hostname'
                    }
                },
                y: {
                    title: {
                        display: true,
                        text: 'Count of Entries'
                    }
                }
            }
        }
    });
}

async function getData() {
    const xs = [];
    const ys = [];

    const response = await fetch('csv/data_b.csv');
    const data = await response.text();

    const table = data.split('\n').slice(1);
    table.forEach(row => {
        const columns = row.split(',');
        //const year = columns[0];
        //const day = columns[1];
        //const month = columns[2];
        const hostname = columns[3];
        xs.push(hostname);
        const countOfEntries = columns[4];
        ys.push(countOfEntries);
    });

    //get num of access points from num of unique hostnames
    let accessPoints = 0;
    accessPoints = (xs.filter((value, index, self) => self.indexOf(value) === index)).length;

    return { xs, ys, accessPoints };
}