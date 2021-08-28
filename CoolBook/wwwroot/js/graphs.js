

books_amount_graph()
best_books_graph()
watched_books_graph()

function books_amount_graph() {
    var width = 960,
        height = 500,
        radius = Math.min(width, height) / 2;
    var arc = d3.arc()
        .outerRadius(radius - 10)
        .innerRadius(0);

    // defines wedge size
    var pie = d3.pie()
        .sort(null)
        .value(function (d) { return d.count; });

    var svg = d3.select("#books-amount").append("svg")
        .attr("width", width)
        .attr("height", height)
        .append("g")
        .attr("transform", "translate(" + width / 2 + "," + height / 2 + ")");

    d3.json("GetCatAmounts", function (data) {
        // Create array of objects of search results to be used by D3
        var parsed_data = [];
        for (var d in data) {
            parsed_data.push({
                count: data[d].count,
                name: data[d].name
            });
        }
        var color = d3.scaleOrdinal().domain([0, parsed_data.length])
            .range(d3.schemeSet3);
        var g = svg.selectAll(".arc")
            .data(pie(parsed_data))
            .enter().append("g")
            .attr("class", "arc");

        g.append("path")
            .attr("d", arc)
            .style("fill", function (d) { return color(d.data.name); });

        g.append("text")
            .attr("transform", function (d) { return "translate(" + arc.centroid(d) + ")"; })
            .attr("dy", ".35em")
            .style("text-anchor", "middle")
            .text(function (d) { return d.data.name; });
    });
}



function best_books_graph() {
    // set the dimensions and margins of the graph
    var margin = { top: 20, right: 20, bottom: 30, left: 40 },
        width = 960 - margin.left - margin.right,
        height = 500 - margin.top - margin.bottom;

    // set the ranges
    var x = d3.scaleBand()
        .range([0, width])
        .padding(0.1);
    var y = d3.scaleLinear()
        .range([height, 0]);

    // append the svg object to the right div
    var svg = d3.select("#best-books").append("svg")
        .attr("width", width + margin.left + margin.right)
        .attr("height", height + margin.top + margin.bottom)
        .append("g")
        .attr("transform",
            "translate(" + margin.left + "," + margin.top + ")");

    // get the data
    d3.json("GetBestBooks", function (data) {
        // Scale the range of the data in the domains
        x.domain(data.map(function (d) { return d.name; }));
        y.domain([0, d3.max(data, function (d) { return d.rate; })]);

        var color = d3.scaleOrdinal().domain([0, data.length])
            .range(d3.schemeSet3);

        // append the rectangles for the bar chart
        svg.selectAll(".bar")
            .data(data)
            .enter().append("rect")
            .attr("class", "bar")
            .attr("x", function (d) { return x(d.name); })
            .attr("width", x.bandwidth())
            .attr("y", function (d) { return y(d.rate); })
            .style("fill", function (d) { return color(d.name); })
            .attr("height", function (d) { return height - y(d.rate); });

        // add the x Axis
        svg.append("g")
            .attr("transform", "translate(0," + height + ")")
            .call(d3.axisBottom(x));

        // add the y Axis
        svg.append("g")
            .call(d3.axisLeft(y));
    });
}

function watched_books_graph() {
    // set the dimensions and margins of the graph
    var margin = { top: 10, right: 30, bottom: 40, left: 100 },
        width = 460 - margin.left - margin.right,
        height = 500 - margin.top - margin.bottom;

    // append the svg object to the body of the page
    var svg = d3.select("#watched-books")
        .append("svg")
        .attr("width", width + margin.left + margin.right)
        .attr("height", height + margin.top + margin.bottom)
        .append("g")
        .attr("transform",
            "translate(" + margin.left + "," + margin.top + ")");

    // Parse the Data
    d3.json("GetWatchedBooks", function (data) {
        // Add X axis
        var x = d3.scaleLinear()
            .domain([0, 1.3 * d3.max(data, function (d) { return d.views; })])
            .range([0, width]);
        svg.append("g")
            .attr("transform", "translate(0," + height + ")")
            .call(d3.axisBottom(x))
            .selectAll("text")
            .attr("transform", "translate(-10,0)rotate(-45)")
            .style("text-anchor", "end");

        // Y axis
        var y = d3.scaleBand()
            .range([0, height])
            .domain(data.map(function (d) { return d.name; }))
            .padding(1);
        svg.append("g")
            .call(d3.axisLeft(y))


        // Lines
        svg.selectAll("myline")
            .data(data)
            .enter()
            .append("line")
            .attr("x1", function (d) { return x(d.views); })
            .attr("x2", x(0))
            .attr("y1", function (d) { return y(d.name); })
            .attr("y2", function (d) { return y(d.name); })
            .attr("stroke", "grey")

        // Circles
        svg.selectAll("mycircle")
            .data(data)
            .enter()
            .append("circle")
            .attr("cx", function (d) { return x(d.views); })
            .attr("cy", function (d) { return y(d.name); })
            .attr("r", "4")
            .style("fill", "#69b3a2")
            .attr("stroke", "black")
    })

}