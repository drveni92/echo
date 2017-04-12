angular
    .module("Billing")
    .controller('AgentsController', ['$scope', '$http', '$uibModal', 'DataFactory', '$rootScope','$timeout', function($scope, $http, $uibModal, DataFactory,$rootScope,$timeout) {
        $scope.regions= REGIONS;
        $scope.get = function(currentAgent){
            $scope.agent = currentAgent;
            $scope.showAgent = true;
        };
        function ListAgents(){

            DataFactory.list("agents", function(data){ $scope.agents = data;});
        }

        ListAgents();

        $scope.new = function() {
            var modalInstance = $uibModal.open({
                animation: true,
                focus : false,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'app/components/agents/templates/new.html',
                controller: 'ModalInstanceController',
                controllerAs: '$modal',
                resolve:
                    {
                    data: function() {
                return { id: 0, name: '', towns : [] }
                },
                    options: function() {
                        return ["towns"]
                    }
                }
         });
            modalInstance.rendered.then(function () {
                $('.ui.dropdown').dropdown();
            });
            modalInstance.result.then(function(agent) {
                console.log($rootScope.workexps );
                DataFactory.insert("agents", agent, function(data) { ListAgents(); });
            }, function() {
            console.log('Modal dismissed at: ' + new Date());
        });
        };





        $scope.edit = function(agent) {
            var modalInstance = $uibModal.open(
                {
                animation: true,
                    focus : false,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'app/components/agents/templates/edit.html',
                controller: 'ModalInstanceController',
                controllerAs: '$modal',
                resolve: {
                    data: function() {
                        return agent
                    },
                    options: function() {
                        return ["towns"]
                    }
                }
            });

            modalInstance.rendered.then(function () {
               $timeout(getTowns, 1000);
            });
            modalInstance.result.then(function(agent) {
                DataFactory.update("agents", agent.id, agent, function(data) {
                    ListAgents();
                });
            }, function() {
                ListAgents();
            });



        }
        getTowns = function() {
            $('.ui.dropdown').dropdown();
        }
        $scope.delete = function(agent){
            DataFactory.delete("agents", agent.id, function(data){ ListAgents();});
        };

}]);


