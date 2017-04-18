(function() {
    angular
        .module("Billing")
        .factory('AgentsFactory', function() {
            return {
                empty: function() {
                    return {
                        id: 0,
                        name: '',
                        towns: []
                    }
                },
                agent: function(agent) {
                    var tempTowns = [];
                    for (var i = agent.towns.length - 1; i >= 0; i--) {
                        tempTowns.push({id: agent.towns[i].id, name: agent.towns[i].name});
                    }
                    return {
                        id: agent.id,
                        name: agent.name,
                        towns: tempTowns
                    }
                }
            };
        })
}());
