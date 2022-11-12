const ctx = document.getElementById('errorChart').getContext('2d');

//use D3.js to load data
d3.csv('csv/errortest.csv').then(data => {
    const dataObj = [];
    const xs = [];

    for (let i = 0; i < data.length; i++) {
        dataObj.push({
            year: data[i].Year,
            month: data[i].Month,
            day: data[i].Day,
            hour: data[i].Hour,
            minute: data[i].Minute,
            numLogs: data[i]["Number of Log Entries"],
            types: data[i].Types
        });
    }

    //get unique log code types
    //types = types.filter((value, index, self) => self.indexOf(value) === index);

    //for each log code type, create a dataset

    //group data by type
    const groupedData = d3.group(dataObj, d => d.types);
    let x = d3.map(groupedData.get("NOTI"), d => d.month);
    let y = d3.map(groupedData.get("NOTI"), d => d.day);
    let r = d3.map(groupedData.get("NOTI"), d => d.numLogs);
    const bubbleData = [{ x: x, y: y, r: r }];
    console.log(groupedData);

    const errorChart = new Chart(ctx, {
        type: 'bubble',
        data: {
            labels: xs,
            datasets: [
                {
                    label: "NOTI",
                    data: dataObj,
                    backgroundColor: 'rgba(255, 79, 66, 0.5)',
                    borderColor: 'rgba(255, 41, 29, 1)'
                }
                ]
        },
        options: {
            responsive: true,
            aspectRatio: 1,
            scales: {
                x: {
                    title: {
                        display: true,
                        text: 'Months'
                    }
                },
                y: {
                    title: {
                        display: true,
                        text: 'Days'
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