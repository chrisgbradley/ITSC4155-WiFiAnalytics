d3.csv('csv/errortest.csv').then(data => {
    //get column headers
    let columns = Object.keys(data[0]);
    //add table to the page
    let table = d3.select('#tableContainer').append('table').attr('class', 'table table-striped table-dark table-bordered');
    let thead = table.append('thead');
    let tbody = table.append('tbody');

    //append a header row
    thead.append('tr')
        .selectAll('th')
        .data(columns)
        .enter()
        .append('th')
        .attr('scope', 'col')
        .text(column => column);

    //create a row for each object
    let rows = tbody.selectAll('tr')
        .data(data)
        .enter()
        .append('tr')

    //create a cell in each row and column
    let cells = rows.selectAll('td')
        .data(row => columns.map(function(column) {
            return {
                column: column,
                value: row[column]
            };
        }))
        .enter()
        .append('td')
        .text(d => d.value)
});