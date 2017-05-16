(function () {
    angular
        .module('Billing')
        .controller('ReportsMapController', ['$scope', 'DataFactory', '$timeout', function ($scope, DataFactory, $timeout) {

            var svg = null;

            $scope.result = [];
            $scope.currentTown = null;
            $scope.selectedItem = null;

            $scope.$watch('isMenuOpened', function () {
                $timeout(function () {
                    $("#map").empty();
                    $scope.loadMap();
                }, 800);
            });

            $scope.loadMap = function loadMap() {
                var width = parseInt(d3.select("#map").style("width")),
                    height = parseInt(d3.select("#map").style("height")),
                    active = d3.select(null),
                    centered;

                var path;

                var tooltip = d3.select('#map').append('div').attr('class', 'popup_hidden tooltip1');

                svg = d3.select("#map").append("svg")
                    .attr("width", width)
                    .attr("height", height);

                svg.append("rect")
                    .attr("class", "map_background")
                    .attr("width", width)
                    .attr("height", height);

                var g = svg.append("g").style("stroke-width", "1.5px");

                d3.json('assets/js/maps/municipalities.json', function (error, json) {
                    var center = d3.geo.centroid(json);
                    var scale = 6500;
                    var offset = [width / 2, height / 2.5];
                    var projection = d3.geo.mercator().center(center).scale(scale).translate(offset);
                    path = d3.geo.path().projection(projection);

                    g.selectAll("path")
                        .data(json.features)
                        .enter().append("path")
                        .attr("d", path)
                        .attr("class", function (d) {
                            var classes = "region";
                            if (d.properties.name == null && d.properties.name_2 == null)
                                classes = "disabled";
                            return classes;
                        })
                        .on("click", clicked)
                        .on('mousemove', function (d) {
                            var name = d.properties.name;
                            if (name == null) {
                                name = d.properties.name_2;
                                if (name == null) {
                                    name = d.properties.name_1;
                                }
                            }
                            var mouse = d3.mouse(svg.node()).map(function (d) {
                                return parseInt(d);
                            });
                            tooltip.classed('popup_hidden', false)
                                .attr('style', 'left:' + (mouse[0] + 15) + 'px; top:' + (mouse[1] - 35) + 'px')
                                .html(name);
                        })
                        .on('mouseout', function () {
                            tooltip.classed('popup_hidden', true);
                        });
                });

                function clicked(d) {
                    var x, y, k;
                    if (d && centered !== d) {
                        var centroid = path.centroid(d);
                        x = centroid[0];
                        y = centroid[1];
                        k = 4;
                        centered = d;
                        $scope.updateMapBasic(d.properties.name);
                    } else {

                        x = width / 2;
                        y = height / 2;
                        k = 1;
                        centered = null;
                    }

                    g.selectAll("path")
                        .classed("active", centered && function (d) { return d === centered; });

                    g.transition()
                        .duration(750)
                        .attr("transform", "translate(" + width / 2 + "," + height / 2 + ")scale(" + k + ")translate(" + -x + "," + -y + ")")
                        .style("stroke-width", 1.5 / k + "px");
                }
            }


            $scope.getTowns = function (value) {
                var url = 'towns/' + value;
                return DataFactory.promise(url)
                    .then(function (response) {
                        return response.data.list;
                    });
            }

            $scope.updateMapBasic = function updateMapBasic(town) {
                if (town) {
                    DataFactory.insert("salesbytown", { "name": town }, function (data) {
                        $scope.result = data;
                        $scope.currentTown = data.town;
                        $scope.currentRegion = data.region;
                        $scope.selectedItem = {
                            name: data.town
                        };
                    });
                }
            }


            $scope.updateMap = function updateMap(town) {
                if ($scope.selectedItem) {
                    var town = $scope.selectedItem.name;
                    var selected = false;
                    svg.selectAll('path').each(function (d, i) {
                        if (angular.lowercase(d.properties.name).indexOf(angular.lowercase(town)) !== -1 && !selected) {
                            var onClickFunc = d3.select(this).on("click");
                            onClickFunc.apply(this, [d, i]);
                            selected = true;
                        }
                    });
                }
            }

        }]);
}());