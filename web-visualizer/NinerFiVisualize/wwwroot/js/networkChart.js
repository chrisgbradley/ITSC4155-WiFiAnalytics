const ctx = document.getElementById('networkChart');
const cty = document.getElementById('doughnutChart');

//use D3.js to load data
d3.csv('csv/testdata.csv').then(data => {
    const dataObj = [];

    for (let i = 0; i < data.length; i++) {
        dataObj.push({
            day: data[i].Day,
            hostname: data[i].Hostname,
            numLogs: data[i].log_entries
        });
    };

    //group data by hostname
    const groupedData = d3.group(dataObj, d => d.hostname);

    //use d3.rollup method to compute the total num of logs for each hostname
    //using d3.sum to find total logs per hostname
    const summedData = d3.rollup(dataObj, v => d3.sum(v, d => d.numLogs), d => d.hostname);

    const totalLogs = d3.sum(dataObj, d => d.numLogs);

    //get percent of logs out of total
    function percentage(key) {
        return ((summedData.get(key) / totalLogs) * 100).toFixed(2);
    }

    //create doughnut chart
    const doughnutChart = new Chart(cty, {
        type: 'doughnut',
        data: {
            labels: ["Grig", "Rees", "Foun", "Denn"],
            datasets: [
                {
                    label: 'Percent',
                    data: [percentage("Grig"), percentage("Rees"), percentage("Foun"), percentage("Denn")],
                    backgroundColor: [
                        'rgba(0, 255, 144, 0.5)',
                        'rgba(0, 7, 255, 0.5)',
                        'rgba(230, 114, 0, 0.5)',
                        'rgba(255, 17, 0, 0.5)'
                    ]
                },
            ],
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    position: 'top',
                },
                title: {
                    display: true,
                    text: 'Total Number of Logs: ' + totalLogs,
                    align: 'start',
                }
            }
        }
    });

    //get number of access points from grouping unique entries in hostname column
    const accessPoints = (dataObj.map(d => d.hostname)).size;

    //create line chart
    const networkChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: dataObj.map(d => d.hostname),
            datasets: [{
                label: 'Number of Log Entries',
                data: dataObj.map(d => d.numLogs),
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
                        text: 'Number of Entries'
                    }
                }
            },
            radius: 3,
            hitRadius: 30,
            hoverRadius: 10
        }
    });

    //hide bootstrap spinner after data loads
    d3.select('.spinner-border').style('display', 'none');

}).catch(error => console.log(error));