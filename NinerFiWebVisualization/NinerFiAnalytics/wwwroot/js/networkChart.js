const ctx = document.getElementById('networkChart');

//use D3.js to load data
d3.csv('csv/data_b.csv').then(data => {
    const xs = [];
    const ys = [];

    for (let i = 0; i < data.length; i++) {
        //const year = data[i].Year;
        //const day = data[i].Day;
        //const month = data[i].Month;
        const hostname = data[i].Hostname;
        xs.push(hostname);
        const countOfEntries = data[i]["Count of Entries"];
        ys.push(countOfEntries);
    }

    //get number of access points from grouping unique entries in hostname column
    let accessPoints = (data.map(d => d.Hostname)).keys();

    const networkChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: xs,
            datasets: [{
                label: 'Number of Log Entries',
                data: ys,
                fill: false,
                backgroundColor: 'rgba(1, 80, 53, 1)',
                borderColor: 'rgba(1, 80, 53, 0.3)',
                borderWidth: 1
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
                    text: 'Total Number of Access Points: ' + accessPoints,
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
    })
    //hide bootstrap spinner after data loads
    d3.select('.spinner-border').style('display', 'none');

}).catch(error => console.log(error));