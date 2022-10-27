const ctx = document.getElementById('myChart');
const ctm = document.getElementById('networkChart');


const myChart = new Chart(ctx, {
    type: 'line',
    data: {
        labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August'],
        datasets: [{
            label: 'Number of Log Entries at Different Times',
            data: [10, 5, 7, 2, 8, 20, 30, 45],
            backgroundColor: 'rgb(255, 99, 132)',
            borderColor: 'rgb(255, 99 132)'
        }]
    },
    options: {
        scales: {
            y: {
                beginAtZero: true
            }
        }
    }
});


const mixedChart = new Chart(ctm, {
    data: {
        datasets: [{
            type: 'line',
            label: 'Traffic Statistics',
            data: [0, 5, 3, 9],
            fill: false,
            borderColor: 'rgb(1, 80, 53)'
        }, {
            type: 'bar',
            label: 'Number of Access Points',
            data: [10, 15, 5, 9],
            backgroundColor: 'rgb(255, 195, 0)'
        }, {
            type: 'bar',
            label: 'Number of Hostnames',
            data: [7, 11, 3, 4],
            backgroundColor: 'rgb(114, 193, 166)'
        }],
        labels: ['January', 'February', 'March', 'April'],

    },
    options: {
        animations: {
            tension: {
                duration: 2000,
                easing: 'linear',
                from: 1,
                to: 0,
                loop: true
            }
        },
        scales: {
            y: {
                min: 0,
                max: 20
            }
        }
    }
});