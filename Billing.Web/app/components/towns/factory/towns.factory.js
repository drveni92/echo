(function() {
    angular
        .module("Billing")
        .factory('TownsFactory', function() {
            return {
                empty: function() {
                    return {
                        id: 0,
                        name: '',
                        zip: '',
                        region: null
                    }
                },
                town: function(town) {
                    return {
                        id: town.id,
                        name: town.name,
                        zip: town.zip,
                        region: town.region
                    }
                }
            };
        })
}());
