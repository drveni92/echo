(function() {
    angular
        .module("Billing")
        .factory('DashboardFactory', function() {
            var regions = BillingConfig.regions;
            var months = BillingConfig.months;

            return {
                regionsYear: function(result) {
                    data = [];
                    for (var i = result.length - 1; i >= 0; i--) {
                        var temp = { key: result[i].label, values: [] };
                        for (var j = 0; j < result[i].sales.length; j++) {
                            var reg = months[j];
                            temp.values.push({ x: reg, y: result[i].sales[j] });
                        }
                        data.push(temp);
                    }
                    return {
                        data: data,
                        options: {
                            chart: {
                                type: 'multiBarChart',
                                height: 650,
                                margin: {
                                    top: 20,
                                    right: 100,
                                    bottom: 100,
                                    left: 100
                                },
                                x: function(d) {
                                    return d.x;
                                },
                                valueFormat: function(d) {
                                    return d3.format('$,.2f')(d);
                                },
                                clipEdge: true,
                                duration: 500,
                                stacked: false,
                                xAxis: {
                                    rotateLabels: 45
                                }
                            },
                            title: {
                                enable: true,
                                text: 'Region Sales Year'
                            }
                        }
                    };
                },
                products: function(result) {
                    var data = [];
                    for (var i = result.length - 1; i >= 0; i--) {
                        data.push({ key: result[i].product, value: result[i] });
                    }
                    return {
                        data: data,
                        options: {
                            chart: {
                                type: 'pieChart',
                                height: 650,
                                x: function(d) {
                                    return d.key;
                                },
                                y: function(d) {
                                    return d.value.revenue;
                                },
                                tooltip: {
                                    contentGenerator: function(e) {
                                        var obj = e.data.value;
                                        var content = "<p><strong>Debit: </strong>" + d3.format('$,.2f')(obj.revenue.toFixed(2)) + "</strong></p>";
                                        content += "<p><strong>Quantity: </strong>" + (obj.quantity) + "</strong></p>";
                                        return content;
                                    }
                                },
                                showLabels: true,
                                duration: 500,
                                labelThreshold: 0.01,
                                labelSunbeamLayout: true,
                                legend: {
                                    margin: {
                                        top: 5,
                                        right: 35,
                                        bottom: 5,
                                        left: 0
                                    }
                                }
                            },
                            title: {
                                enable: true,
                                text: 'Top 5 Products'
                            }
                        }
                    }
                },
                invoices: function(result) {
                    var data = [];
                    for (var i = result.length - 1; i >= 0; i--) {
                        data.push({ key: result[i].status.split(/(?=[A-Z])/).join(" "), value: result[i].count });
                    }
                    return {
                        data: data,
                        options: {
                            chart: {
                                type: 'pieChart',
                                height: 650,
                                x: function(d) {
                                    return d.key;
                                },
                                y: function(d) {
                                    return d.value;
                                },
                                valueFormat: function(d) {
                                    return Math.floor(d);
                                },
                                showLabels: true,
                                duration: 500,
                                labelThreshold: 0.01,
                                labelSunbeamLayout: true,
                                legend: {
                                    margin: {
                                        top: 5,
                                        right: 35,
                                        bottom: 5,
                                        left: 0
                                    }
                                }
                            },
                            title: {
                                enable: true,
                                text: 'Invoices Status'
                            }
                        }
                    }
                },

                customers: function(result) {
                    var data = [];
                    for (var i = result.length - 1; i >= 0; i--) {
                        data.push({ key: result[i].name, y: result[i] });
                    }
                    return {
                        data: data,
                        options: {
                            chart: {
                                type: 'pieChart',
                                height: 650,
                                x: function(d) {
                                    return d.key;
                                },
                                y: function(d) {
                                    return d.y.debit;
                                },
                                tooltip: {
                                    contentGenerator: function(e) {
                                        var obj = e.data.y;
                                        var content = "<p><strong>Credit: </strong>" + d3.format('$,.2f')(obj.credit.toFixed(2)) + "</strong></p>";
                                        content += "<p><strong>Debit: </strong>" + d3.format('$,.2f')(obj.debit.toFixed(2)) + "</strong></p>";
                                        return content;
                                    }
                                },
                                showLabels: true,
                                duration: 500,
                                labelThreshold: 0.01,
                                labelSunbeamLayout: true,
                                legend: {
                                    margin: {
                                        top: 5,
                                        right: 35,
                                        bottom: 5,
                                        left: 0
                                    }
                                }
                            },
                            title: {
                                enable: true,
                                text: 'Top Customers'
                            }
                        }
                    }
                },
                categoriesyear: function(result) {
                    data = [];
                    for (var i = result.length - 1; i >= 0; i--) {
                        var temp = { key: result[i].label, values: [] };
                        for (var j = 0; j < result[i].sales.length; j++) {
                            var reg = months[j];
                            temp.values.push({ x: reg, y: result[i].sales[j] });
                        }
                        data.push(temp);
                    }
                    return {
                        data: data,
                        options: {
                            chart: {
                                type: 'multiBarChart',
                                height: 650,
                                margin: {
                                    top: 20,
                                    right: 100,
                                    bottom: 100,
                                    left: 100
                                },
                                x: function(d) {
                                    return d.x;
                                },
                                valueFormat: function(d) {
                                    return d3.format('$,.2f')(d);
                                },
                                clipEdge: true,
                                duration: 500,
                                stacked: false,
                                xAxis: {
                                    rotateLabels: 45
                                }
                            },
                            title: {
                                enable: true,
                                text: 'Category Sales Year'
                            }
                        }
                    };
                },
                burningitems: function(result) {
                    var data = [];
                    for (var i = result.length - 1; i >= 0; i--) {
                        data.push({ key: result[i].name, y: result[i] });
                    }
                    return {
                        data: data,
                        options: {
                            chart: {
                                type: 'pieChart',
                                height: 650,
                                x: function(d) {
                                    return d.key;
                                },
                                y: function(d) {
                                    return d.y.sold;
                                },
                                tooltip: {
                                    contentGenerator: function(e) {
                                        var obj = e.data.y;
                                        var content = "<p><strong>Ordered: </strong>" + obj.ordered + "</strong></p>";
                                        content += "<p><strong>Sold: </strong>" + obj.sold + "</strong></p>";
                                        content += "<p><strong>Stock: </strong>" + obj.stock + "</strong></p>";
                                        return content;
                                    }
                                },
                                showLabels: true,
                                duration: 500,
                                labelThreshold: 0.01,
                                labelSunbeamLayout: true,
                                legend: {
                                    margin: {
                                        top: 5,
                                        right: 35,
                                        bottom: 5,
                                        left: 0
                                    }
                                }
                            },
                            title: {
                                enable: true,
                                text: 'Burning Items'
                            }
                        }
                    }
                },
                agentsales: function(result) {
                    data = [];
                    for (var i = result.length - 1; i >= 0; i--) {
                        var temp = { key: result[i].label, values: [] };
                        for (var j = result[i].sales.length - 1; j >= 0; j--) {
                            var reg = regions[j];
                            temp.values.push({ x: reg, y: result[i].sales[j] });
                        }
                        data.push(temp);
                    }
                    return {
                        data: data,
                        options: {
                            chart: {
                                type: 'multiBarChart',
                                height: 650,
                                margin: {
                                    top: 20,
                                    right: 100,
                                    bottom: 100,
                                    left: 100
                                },
                                x: function(d) {
                                    return d.x;
                                },
                                valueFormat: function(d) {
                                    return d3.format('$,.2f')(d);
                                },
                                clipEdge: true,
                                duration: 500,
                                stacked: false,
                                xAxis: {
                                    rotateLabels: 30
                                },
                                yAxis: {
                                    axisLabelDistance: 50
                                }
                            },
                            title: {
                                enable: true,
                                text: 'Agents Sales'
                            }
                        }
                    };
                }
            };
        })
}());
